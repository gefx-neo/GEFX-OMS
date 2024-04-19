using DataAccess;
using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public class InventoryRepository : IInventoryRepository
    {
        private DataAccess.GreatEastForex db;

        public InventoryRepository()
        {
            db = new DataAccess.GreatEastForex();
        }

        public IQueryable<Inventory> Select()
        {
            var result = from i in db.Inventories select i;

            return result.Where(e => e.IsDeleted == "N");
        }

        public IList<Inventory> GetAll(int productid)
        {
            try
            {
                IQueryable<Inventory> records = Select();

                return records.Where(e => e.ProductId == productid).OrderByDescending(e => e.CreatedOn).ToList();
            }
            catch
            {
                throw;
            }
        }

        public bool Add(Inventory addData)
        {
            try
            {
                addData.CreatedOn = DateTime.Now;
                addData.UpdatedOn = DateTime.Now;
                addData.IsDeleted = "N";

                db.Inventories.Add(addData);

                db.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool Update(int id, Inventory updateData)
        {
            try
            {
                Inventory data = db.Inventories.Find(id);

                data.Type = updateData.Type;
                data.Amount = updateData.Amount;
                data.Description = updateData.Description;
                data.UpdatedOn = DateTime.Now;

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
                Inventory data = db.Inventories.Find(id);

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

        public IEnumerable<Inventory> ViewlogPagination(int page, int id)
        {
            try
            {
                int getpage = page;
                int fixTotalItems = 50;
                int minusPerpage = 50;
                int getMaxItems = getpage * fixTotalItems;
                int getRange = getMaxItems - minusPerpage;

                var rec = db.Inventories.Where(e => e.ProductId == id && e.IsDeleted == "N").ToList();
                var getrecords = rec.OrderByDescending(e => e.CreatedOn);
                return getrecords.Skip(getRange).Take(fixTotalItems);

            }
            catch
            {
                throw;
            }
        }

        public int GetTotalCount(int id)
        {
            try
            {
                var records = db.Inventories.Where(e => e.ProductId == id).Count();
                int pageSize = 50;

                double countpage = (records + (pageSize - 1)) / pageSize;

                int getpage = Convert.ToInt32(countpage);

                return getpage;
            }
            catch
            {
                throw;
            }
        }


    }
}