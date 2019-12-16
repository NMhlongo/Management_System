using System;
namespace LibraryData.Models
{
    public class Customer
    {
        public readonly object RentCard;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string Cellphone { get; set; }
        public string IdNo { get; set; }
        public int RentcardId { get; set; }

    }
}
