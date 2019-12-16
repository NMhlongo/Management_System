using System;
using System.Collections.Generic;
using LibraryData.Models;

namespace LibraryData
{
    public interface ICheckout
    {

        IEnumerable<CheckoutHistory> GetCheckOutHistory(int id);

        void PlaceHold(int assetId, int RentCardId);
        string GetCurrentHoldCustomerName(int holdid);
        DateTime GetCurrentHoldPlaced(int holdid);
        bool IsCheckedOut(int assetId);

        IEnumerable<Hold> GetCurrentHold(int id);
        Checkout GetlatestCheckOut(int assetId);
        void MarkLost(int assetId);
        void MarkFound(int assetId);

        IEnumerable<Checkout> GetAll();
        Checkout GetById(int checkoutId);
        void Add(Checkout newCheckOut);
        void CheckOutItem(int assetId, int RentCardId);
        void CheckInItem(int assetId, int RentCardId);
        string GetCurrentCheckoutCustomer(int assetId);
    }
}
