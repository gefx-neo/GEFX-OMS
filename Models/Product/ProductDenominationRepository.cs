using DataAccess;
using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public class ProductDenominationRepository : IProductDenominationRepository
    {
        private DataAccess.GreatEastForex db;

        public ProductDenominationRepository()
        {
            db = new DataAccess.GreatEastForex();
        }

        public IQueryable<ProductDenomination> Select()
        {
            var result = from p in db.ProductDenominations select p;

            return result;
        }

        public ProductDenomination GetSingle(int id)
        {
            try
            {
                IQueryable<ProductDenomination> records = Select();

                return records.Where(e => e.ID == id).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public IList<ProductDenomination> GetProductDenomination(int productid)
        {
            try
            {
                IQueryable<ProductDenomination> records = Select();

                return records.Where(e => e.ProductId == productid).OrderByDescending(e => e.DenominationValue).ToList();
            }
            catch
            {
                throw;
            }
        }

        public bool Add(ProductDenomination addData)
        {
            try
            {
                db.ProductDenominations.Add(addData);

                db.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                ProductDenomination data = db.ProductDenominations.Find(id);

                db.ProductDenominations.Remove(data);

                db.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}