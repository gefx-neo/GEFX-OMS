using DataAccess;
using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public class ScanOutgoingRepository : IScanOutgoingRepository
    {
        private DataAccess.GreatEastForex db;

        public ScanOutgoingRepository()
        {
            db = new DataAccess.GreatEastForex();
        }

        public IQueryable<ScanOutgoing> Select()
        {
            var result = from s in db.ScanOutgoings select s;

            return result.Where(e => e.IsDeleted == "N");
        }

        public IList<ScanOutgoing> GetAll()
        {
            try
            {
                IQueryable<ScanOutgoing> records = Select();

                return records.Where(e => e.Status != "Confirmed").OrderByDescending(e => e.CreatedOn).ToList();
            }
            catch
            {
                throw;
            }
        }

        public IList<ScanOutgoing> GetAllScanBy(int userid)
        {
            try
            {
                IQueryable<ScanOutgoing> records = Select();

                return records.Where(e => e.Status != "Confirmed" && e.ScanById == userid).OrderByDescending(e => e.CreatedOn).ToList();
            }
            catch
            {
                throw;
            }
        }

        public ScanOutgoing GetSingle(int id)
        {
            try
            {
                IQueryable<ScanOutgoing> records = Select();

                return records.Where(e => e.ID == id).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public ScanOutgoing GetSingleScanBy(int id, int userid)
        {
            try
            {
                IQueryable<ScanOutgoing> records = Select();

                return records.Where(e => e.SaleId == id && e.ScanById == userid).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public ScanOutgoing FindMemoID(string memoID)
        {
            try
            {
                IQueryable<ScanOutgoing> records = Select();

                return records.Where(e => e.Sales.MemoID == memoID).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public bool Add(ScanOutgoing addData)
        {
            try
            {
                addData.CreatedOn = DateTime.Now;
                addData.IsDeleted = "N";

                db.ScanOutgoings.Add(addData);

                db.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool ConfirmOutgoing(int id)
        {
            try
            {
                ScanOutgoing data = db.ScanOutgoings.Find(id);

                data.Status = "Confirmed";
                data.ConfirmedOn = DateTime.Now;

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
                ScanOutgoing data = db.ScanOutgoings.Find(id);

                data.DeletedOn = DateTime.Now;
                data.IsDeleted = "Y";

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