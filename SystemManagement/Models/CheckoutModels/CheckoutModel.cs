using System;
namespace SystemManagement.Models.CheckoutModels
{
    public class CheckoutModel
    {

        public string RentCardId { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int AssetId { get; set; }
        public int HoldCount { get; set; }
        public bool IsCheckedOut { get; set; }

    }
}
