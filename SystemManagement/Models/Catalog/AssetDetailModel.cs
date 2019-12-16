using System;
using System.Collections.Generic;
using LibraryData.Models;

namespace SystemManagement.Models.Catalog
{
    public class AssetDetailModel
    {

        public int AssetId { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string CustomerName { get; set; }
        public int Quantity { get; set; }
        public int Year { get; set; }
        public decimal Cost { get; set; }
        public Checkout LatestCheckout { get; set; }

        public IEnumerable<CheckoutHistory> CheckoutHistory { get; set; }
        public IEnumerable<AssetHoldModel> CurrentHolds { get; set; }

        public class AssetHoldModel
        {
            public string CustomerName { get; set; }
            public DateTime HoldPlaced { get; set; }

        }
        public AssetDetailModel()
        {
        }
    }
}
