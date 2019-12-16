using System;
using System.Collections.Generic;
using System.Linq;
using LibraryData;
using LibraryData.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryService
{
    public class CheckoutService : ICheckout
    {
        private LibraryContext _context;

        public CheckoutService(LibraryContext context)
        {
            _context = context;
        }
        public void Add(Checkout newCheckOut)
        {
            _context.Add(newCheckOut);
            _context.SaveChanges();
        }

        public void CheckInItem(int assetId, int RentCardId)
        {
            var now = DateTime.Now;
            var item = _context.CompanyAssets
              .FirstOrDefault(a => a.Id == assetId);


            RemoveExistingCheckOuts(assetId);
            CloseExistingHistory(assetId, now);

            var currentHolds = _context.Holds
             .Include(h => h.CompanyAsserts)
             .Include(h => h.RentCards)
             .Where(h => h.CompanyAsserts.Id == assetId);

            if (currentHolds.Any())
            {
                CheckoutToEarliestHold(assetId, currentHolds);

            }

            UpdateStatus(assetId, "Available");
            _context.SaveChanges();
        }

        public void CheckOutItem(int assetId, int RentCardId)
        {
            var now = DateTime.Now;
            if (IsCheckedOut(assetId))

                return;
            var item = _context.CompanyAssets
             .FirstOrDefault(a => a.Id == assetId);


            UpdateStatus(assetId, "Checkout");
            var Rentcard = _context.RentCards
                .Include(card => card.Checkouts)
                .FirstOrDefault(card => card.Id == RentCardId);

            var checkout = new Checkout
            {
                CompanyAsserts = item,
                RentCard = Rentcard,
                From = now,
                Until = GetCheckOutTime(now)

            };

            _context.Add(checkout);

            var checkoutHistory = new CheckoutHistory
            {
                CheckedOut = now,
                CompanyAssets = item,
                RentCards = Rentcard

            };

            _context.Add(checkoutHistory);
            _context.SaveChanges();
        }

        public bool IsCheckedOut(int assetId)
        {
            return _context.Checkouts
                .Where(co => co.CompanyAsserts.Id == assetId)
                .Any();
        }


        public IEnumerable<Checkout> GetAll()
        {
            return _context.Checkouts;
        }

        public Checkout GetById(int checkoutId)
        {
            return
            GetAll()
            .FirstOrDefault(checkout => checkout.Id == checkoutId);
        }

        public IEnumerable<CheckoutHistory> GetCheckOutHistory(int id)
        {
            return _context.CheckoutHistories
            .Include(h => h.CompanyAssets)
            .Include(h => h.RentCards)
            .Where(h => h.CompanyAssets.Id == id);
        }

        string ICheckout.GetCurrentCheckoutCustomer(int assetId)
        {
            var checkout = GetCheckoutbyAssetId(assetId);
            if (checkout == null)
            {
                return "";

            }

            var cardId = checkout.RentCard.Id;
            var customer = _context.Customers
                 .Include(p => p.RentCard)
                 .FirstOrDefault(p => p.RentcardId == cardId);
            return customer?.Name + " " + customer?.Surname;

        }

        private Checkout GetCheckoutbyAssetId(int assetId)
        {
            return _context.Checkouts
                .Include(co => co.CompanyAsserts)
                .Include(co => co.RentCard)
                .FirstOrDefault(co => co.CompanyAsserts.Id == assetId);
        }

        public IEnumerable<Hold> GetCurrentHold(int id)
        {
            return _context.Holds
            .Include(h => h.CompanyAsserts)
            .Where(h => h.CompanyAsserts.Id == id);
        }

        public string GetCurrentHoldCustomerName(int holdid)
        {
            var hold = _context.Holds
            .Include(h => h.CompanyAsserts)
            .Include(h => h.RentCards)
            .FirstOrDefault(h => h.Id == holdid);

            var cardId = hold?.RentCards.Id;

            var customer = _context.Customers
                .Include(p => p.RentcardId)
                .FirstOrDefault(p => p.RentcardId == cardId);

            return customer?.Name + " " + customer?.Surname;
        }

        public DateTime GetCurrentHoldPlaced(int holdid)
        {
            return

               _context.Holds
               .Include(h => h.CompanyAsserts)
               .Include(h => h.RentCards)
               .FirstOrDefault(h => h.Id == holdid)
               .HoldPlaced;
        }

        public Checkout GetlatestCheckOut(int assetId)
        {
            return _context.Checkouts
            .Where(c => c.CompanyAsserts.Id == assetId)
            .OrderByDescending(c => c.From)
            .FirstOrDefault();
        }

        public void MarkFound(int assetId)
        {
            var now = DateTime.Now;

            UpdateStatus(assetId, "Available");
            RemoveExistingCheckOuts(assetId);
            CloseExistingHistory(assetId, now);

            _context.SaveChanges();
        }

        public void MarkLost(int assetId)
        {
            UpdateStatus(assetId, "Lost");
            _context.SaveChanges();
        }

        public void PlaceHold(int assetId, int RentCardId)
        {
            var now = DateTime.Now;

            var asset = _context.CompanyAssets
             .FirstOrDefault(a => a.Id == assetId);

            var card = _context.RentCards
                .FirstOrDefault(c => c.Id == RentCardId);

            if (asset.Status.Name == "Available")
            {
                UpdateStatus(assetId, "On Hold");
            }

            var hold = new Hold
            {
                HoldPlaced = now,
                CompanyAsserts = asset,
                RentCards = card

            };

            _context.Add(hold);
            _context.SaveChanges();
        }

        private DateTime GetCheckOutTime(DateTime now)
        {
            return now.AddDays(30);
        }

        private void UpdateStatus(int assetId, string v)
        {
            var item = _context.CompanyAssets
           .FirstOrDefault(a => a.Id == assetId);
            _context.Update(item);

            item.Status = _context.Statuses
                .FirstOrDefault(status => status.Name == "Available");
        }

        private void CloseExistingHistory(int assetId, DateTime now)
        {
            var history = _context.CheckoutHistories
               .FirstOrDefault(h => h.CompanyAssets.Id == assetId & h.CheckedIn == null);

            if (history != null)
            {
                _context.Update(history);
                history.CheckedIn = now;
            }
        }

        private void RemoveExistingCheckOuts(int assetId)
        {

            var checkout = _context.CheckoutHistories
                .FirstOrDefault(co => co.Id == assetId);

            if (checkout != null)
            {
                _context.Remove(checkout);
            }
        }

        private void CheckoutToEarliestHold(int assetId, IQueryable<Hold> currentHolds)
        {
            var earliestHold = currentHolds
                .OrderBy(holds => holds.HoldPlaced)
                .FirstOrDefault();

            var card = earliestHold.RentCards;

            _context.Remove(earliestHold);
            _context.SaveChanges();

            CheckOutItem(assetId, card.Id);


        }



    }
}
