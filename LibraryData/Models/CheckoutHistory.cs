using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryData.Models
{
    public class CheckoutHistory
    {

        public int Id { get; set; }

        [Required]
        public CompanyAsset CompanyAssets { get; set; }

        [Required]
        public RentCard RentCards { get; set; }

        [Required]
        public DateTime CheckedOut { get; set; }
        public DateTime? CheckedIn { get; set; }
        
    }
}
