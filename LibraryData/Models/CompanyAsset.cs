using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryData.Models
{
    public class CompanyAsset
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public Status Status { get; set; }

        [Required]
        public decimal Cost { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public int Quatity { get; set; }
    }
}
