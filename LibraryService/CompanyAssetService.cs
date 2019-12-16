using System;
using System.Collections.Generic;
using System.Linq;
using LibraryData;
using LibraryData.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryService
{
    public class CompanyAssetService : ICompanyAsset
    {
        private LibraryContext _context;
        public CompanyAssetService(LibraryContext context)
        {
            _context = context;
        }
        public void Add(CompanyAsset newAsset)
        {
            _context.Add(newAsset);
            _context.SaveChanges();
        }

        public IEnumerable<CompanyAsset> GetAll()
        {
            return _context.CompanyAssets
            .Include(asset => asset.Status);
        }

        public CompanyAsset GetById(int id)
        {
            return
            GetAll()
           .FirstOrDefault(asset => asset.Id == id);
        }

        public string GetDescription(int id)
        {
            return _context.CompanyAssets
           .FirstOrDefault(a => a.Id == id)
           .Description;
        }

        public int GetQuantity(int id)
        {
            return _context.CompanyAssets
           .FirstOrDefault(a => a.Id == id)
           .Quatity;
        }
    }
}

