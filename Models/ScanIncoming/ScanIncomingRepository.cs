using DataAccess;
using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public class ScanIncomingRepository : IScanIncomingRepository
    {
        private DataAccess.GreatEastForex db;

        public ScanIncomingRepository()
        {
            db = new DataAccess.GreatEastForex();
        }

        public IQueryable<ScanIncoming> Select()
        {
            var result = from s in db.ScanIncomings select s;

            return result.Where(e => e.IsDeleted == "N");
        }

        public IList<ScanIncoming> GetAll()
        {
            try
            {
                IQueryable<ScanIncoming> records = Select();

                return records.Where(e => e.Status != "Confirmed").OrderByDescending(e => e.CreatedOn).ToList();
            }
            catch
            {
                throw;
            }
        }

        public IList<ScanIncoming> GetAllScanBy(int userid)
        {
            try
            {
                IQueryable<ScanIncoming> records = Select();

                return records.Where(e => e.Status != "Confirmed" && e.ScanById == userid).OrderByDescending(e => e.CreatedOn).ToList();
            }
            catch
            {
                throw;
            }
        }
        public ScanIncoming GetSingle(int id)
        {
            try
            {
                IQueryable<ScanIncoming> records = Select();

                return records.Where(e => e.ID == id).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public ScanIncoming GetSingleScanBy(int id, int userid)
        {
            try
            {
                IQueryable<ScanIncoming> records = Select();

                return records.Where(e => e.ID == id && e.ScanById == userid).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public ScanIncoming FindMemoID(string memoID)
        {
            try
            {
                IQueryable<ScanIncoming> records = Select();

                return records.Where(e => e.Sales.MemoID == memoID).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public bool Add(ScanIncoming addData)
        {
            try
            {
                addData.CreatedOn = DateTime.Now;
                addData.IsDeleted = "N";

                db.ScanIncomings.Add(addData);

                db.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool ConfirmIncoming(int id)
        {
            try
            {
                ScanIncoming data = db.ScanIncomings.Find(id);

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
                ScanIncoming data = db.ScanIncomings.Find(id);

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