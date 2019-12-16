using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryData.Models
{
    public class Checkout
    {

        public readonly object Since;

        public int Id { get; set; }

        [Required]
        public CompanyAsset CompanyAsserts { get; set; }
        public RentCard RentCard { get; set; }
        public DateTime From { get; set; }
        public DateTime Until { get; set; }

       
    }
}
