using DataAccess;
using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public class SaleTransactionDenominationRepository : ISaleTransactionDenominationRepository
    {
        private DataAccess.GreatEastForex db;

        public SaleTransactionDenominationRepository()
        {
            db = new DataAccess.GreatEastForex();
        }

        public IQueryable<SaleTransactionDenomination> Select()
        {
            var result = from s in db.SaleTransactionDenominations orderby s.ID select s;

            return result;
        }

        public IList<SaleTransactionDenomination> GetTransactionDenominations(int transactionid)
        {
            try
            {
                IQueryable<SaleTransactionDenomination> records = Select();

                return records.Where(e => e.SaleTransactionId == transactionid).OrderByDescending(e => e.Denomination == 100).ThenBy(e => e.Denomination).ToList().ToList();
            }
            catch
            {
                throw;
            }
        }

        public SaleTransactionDenomination GetSingle(int id)
        {
            try
            {
                IQueryable<SaleTransactionDenomination> records = Select();

                return records.Where(e => e.ID == id).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public bool Add(SaleTransactionDenomination addData)
        {
            try
            {
                db.SaleTransactionDenominations.Add(addData);

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
                SaleTransactionDenomination data = db.SaleTransactionDenominations.Find(id);

                db.SaleTransactionDenominations.Remove(data);

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