using System;
using System.Collections.Generic;
using LibraryData.Models;

namespace LibraryData
{
    public interface ICompanyAsset
    {
        IEnumerable<CompanyAsset> GetAll();
        CompanyAsset GetById(int id);
        void Add(CompanyAsset newAsset);
        string GetDescription(int id);
        int GetQuantity(int id);
    }
}