using DataAccess;
using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public class ProductInventoryRepository : IProductInventoryRepository
    {
        private DataAccess.GreatEastForex db;

        public ProductInventoryRepository()
        {
            db = new DataAccess.GreatEastForex();
        }

        public IQueryable<ProductInventory> Select()
        {
            var result = from p in db.ProductInventories select p;

            return result.Where(e => e.IsDeleted == "N");
        }

        public ProductInventory GetSingle(int id)
        {
            try
            {
                IQueryable<ProductInventory> records = Select();

                return records.Where(e => e.ID == id).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public ProductInventory GetProductInventory(int productid)
        {
            try
            {
                IQueryable<ProductInventory> records = Select();

                return records.Where(e => e.ProductId == productid).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public bool Add(ProductInventory addData)
        {
            try
            {
                addData.CreatedOn = DateTime.Now;
                addData.UpdatedOn = DateTime.Now;
                addData.IsDeleted = "N";

                db.ProductInventories.Add(addData);

                db.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool Update(int id, ProductInventory updateData)
        {
            try
            {
                ProductInventory data = db.ProductInventories.Find(id);

                data.TotalInAccount = updateData.TotalInAccount;
                data.UpdatedOn = DateTime.Now;

                db.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

		public bool Update2(int id, decimal amount, string transactiontype)
		{
			try
			{
				ProductInventory data = db.ProductInventories.Find(id);

				if (transactiontype == "plus")
				{
					data.TotalInAccount += amount;
					data.UpdatedOn = DateTime.Now;
				}
				else if (transactiontype == "minus")
				{
					data.TotalInAccount -= amount;
					data.UpdatedOn = DateTime.Now;
				}
				else
				{
					return false;
				}
				//data.TotalInAccount = amount;

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
                ProductInventory data = db.ProductInventories.Find(id);

                data.IsDeleted = "Y";
                data.UpdatedOn = DateTime.Now;

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