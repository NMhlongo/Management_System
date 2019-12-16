using System;
namespace LibraryData.Models
{
    public class Hold
    {

        public int Id { get; set; }
        public virtual CompanyAsset CompanyAsserts { get; set; }
        public virtual RentCard RentCards { get; set; }
        public DateTime HoldPlaced { get; set; }
        
    }
}
