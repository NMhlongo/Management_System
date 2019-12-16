using System;
namespace SystemManagement.Models
{
    public class AssetIndexListingModel
    {
        public int Id { get; set; }
        public string CurrentLocation { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
    }
}
