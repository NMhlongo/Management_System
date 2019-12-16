using System.Linq;
using LibraryData;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemManagement.Models;
using SystemManagement.Models.Catalog;
using static SystemManagement.Models.Catalog.AssetDetailModel;
using SystemManagement.Models.CheckoutModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SystemManagement.Controllers
{
    public class CatalogController : Controller
    {
        private ICompanyAsset _assets;
        private ICheckout _checkouts;


        public CatalogController(ICompanyAsset assets, ICheckout checkouts)
        {
            _assets = assets;
            _checkouts = checkouts;

        }


        public IActionResult Index()
        {

            var assetModels = _assets.GetAll();
            var listingResult = assetModels
                .Select(result => new AssetIndexListingModel
                {
                    Id = result.Id,
                    Quantity = result.Quatity,
                    ImageUrl = result.ImageUrl,
                    Description = result.Description

                });
            var model = new AssetIndexModel
            {
                Assets = listingResult
            };

            return View(model);

        }


        public IActionResult Detail(int id)
        {
            var currentHolds = _checkouts.GetCurrentHold(id)
                .Select(a => new AssetHoldModel
                {

                    HoldPlaced = _checkouts.GetCurrentHoldPlaced(a.Id),
                    CustomerName = _checkouts.GetCurrentHoldCustomerName(a.Id),
                });
            var asset = _assets.GetById(id);
            var model = new AssetDetailModel
            {
                AssetId = id,
                Description = asset.Description,
                ImageUrl = asset.ImageUrl,
                Cost = asset.Cost,
                Year = asset.Year,
                Quantity = asset.Quatity,
                Status = asset.Status.Name,
                CheckoutHistory = _checkouts.GetCheckOutHistory(id),
                LatestCheckout = _checkouts.GetlatestCheckOut(id),
                CustomerName = _checkouts.GetCurrentCheckoutCustomer(id),
                CurrentHolds = currentHolds
            };

            return View(model);

        }

        public IActionResult Checkout(int id)
        {
            var asset = _assets.GetById(id);

            var model = new CheckoutModel
            {


                AssetId = id,
                Description = asset.Description,
                ImageUrl = asset.ImageUrl,
                RentCardId = "",
                IsCheckedOut = _checkouts.IsCheckedOut(id)

            };

            return View(model);
        }

        public IActionResult Hold(int id)
        {
            var asset = _assets.GetById(id);

            var model = new CheckoutModel
            {


                AssetId = id,
                Description = asset.Description,
                ImageUrl = asset.ImageUrl,
                RentCardId = "",
                IsCheckedOut = _checkouts.IsCheckedOut(id),
                HoldCount = _checkouts.GetCurrentHold(id).Count()

            };

            return View(model);
        }

        [HttpPost]
        public IActionResult PlaceCheckout(int assetId, int RentCardId)
        {
            _checkouts.CheckOutItem(assetId, RentCardId);
            return RedirectToAction("Detail", new { id = assetId });
        }

        [HttpPost]
        public IActionResult PlaceHold(int assetId, int RentCardId)
        {
            _checkouts.PlaceHold(assetId, RentCardId);
            return RedirectToAction("Detail", new { id = assetId });
        }

        public IActionResult MarkLost(int assetId)
        {
            _checkouts.MarkLost(assetId);
            return RedirectToAction("Detail", new { id = assetId });

        }

        public IActionResult MarkFound(int assetId)
        {
            _checkouts.MarkFound(assetId);
            return RedirectToAction("Detail", new { id = assetId });

        }

    }
}


