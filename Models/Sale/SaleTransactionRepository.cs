using DataAccess;
using DataAccess.POCO;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public class SaleTransactionRepository : ISaleTransactionRepository
    {
        private DataAccess.GreatEastForex db;

        public SaleTransactionRepository()
        {
            db = new DataAccess.GreatEastForex();
        }

        public IQueryable<SaleTransaction> Select()
        {
            var result = from s in db.SaleTransactions orderby s.ID select s;

            return result;
        }

        public IList<SaleTransaction> GetSaleTransactions(int saleid)
        {
            try
            {
                IQueryable<SaleTransaction> records = Select();

                return records.Where(e => e.SaleId == saleid).OrderBy(e => e.TransactionID).ToList();
            }
            catch
            {
                throw;
            }
        }

        public int GetProductTransactions(int productId, string date, string transactionType, List<string> exceptionStatus)
        {
            try
            {
                var records = db.SaleTransactions.Where(e => e.CurrencyId == productId).Select(n => new {
                    n.CurrencyId
                }).ToList();
                //IQueryable<SaleTransaction> records = Select().Where(e => e.CurrencyId == productId);

                //if (!string.IsNullOrEmpty(date))
                //{
                //    DateTime startDate = Convert.ToDateTime(date + " 00:00:00");
                //    DateTime endDate = Convert.ToDateTime(date + " 23:59:59.9999999");
                //    records = records.Where(e => e.Sales.IssueDate >= startDate && e.Sales.IssueDate <= endDate).ToList();
                //}

                //if (exceptionStatus.Count > 0)
                //{
                //    records = records.Where(e => !exceptionStatus.Contains(e.Sales.Status)).ToList();
                //}

                //return records;
                return records.Count;
            }
            catch
            {
                throw;
            }
        }

        public IList<SaleTransaction> GetProductTransactions(int productId, string fromDate, string toDate, string transactionType, List<string> exceptionStatus)
        {
            try
            {
                IQueryable<SaleTransaction> records = Select().Where(e => e.CurrencyId == productId);

                if (!string.IsNullOrEmpty(fromDate))
                {
                    DateTime startDate = Convert.ToDateTime(fromDate + " 00:00:00");

                    records = records.Where(e => e.Sales.IssueDate >= startDate);
                }

                if (!string.IsNullOrEmpty(toDate))
                {
                    DateTime endDate = Convert.ToDateTime(toDate + " 23:59:59.9999999");

                    records = records.Where(e => e.Sales.IssueDate <= endDate);
                }

                if (exceptionStatus.Count > 0)
                {
                    records = records.Where(e => !exceptionStatus.Contains(e.Sales.Status));
                }

                return records.ToList();
            }
            catch
            {
                throw;
            }
        }

        public IPagedList<SaleTransaction> GetTransactionsPaged(string date, List<string> exceptionStatus, int page, int pageSize)
        {
            try
            {
                IQueryable<SaleTransaction> records = Select();

                if (!string.IsNullOrEmpty(date))
                {
                    DateTime startDate = Convert.ToDateTime(date + " 00:00:00");
                    DateTime endDate = Convert.ToDateTime(date + " 23:59:59.9999999");

                    records = records.Where(e => e.Sales.IssueDate >= startDate && e.Sales.IssueDate <= endDate);
                }

                if (exceptionStatus.Count > 0)
                {
                    records = records.Where(e => !exceptionStatus.Contains(e.Sales.Status));
                }

                return records.Where(e => e.Sales.IsDeleted == "N").OrderByDescending(e => e.Sales.CreatedOn).ToPagedList(page, pageSize);
            }
            catch
            {
                throw;
            }
        }

        public SaleTransaction GetSingle(int id)
        {
            try
            {
                IQueryable<SaleTransaction> records = Select();

                return records.Where(e => e.ID == id).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public bool Add(SaleTransaction addData)
        {
            try
            {
                db.SaleTransactions.Add(addData);

                db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Update(int id, SaleTransaction updateData)
        {
            try
            {
                SaleTransaction data = db.SaleTransactions.Find(id);

                data.PaymentMode = updateData.PaymentMode;
                data.ChequeNo = updateData.ChequeNo;
                data.BankTransferNo = updateData.BankTransferNo;

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
                SaleTransaction data = db.SaleTransactions.Find(id);

                db.SaleTransactions.Remove(data);

                db.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool Delete2(int id)
        {
            try
            {
                var record = db.SaleTransactions.Where(e => e.SaleId == id).ToList();


                if(record.Count > 0)
                {
                    db.SaleTransactions.RemoveRange(record);
                    db.SaveChanges();
                }
                //SaleTransaction data = db.SaleTransactions.Find(id);
                //db.SaleTransactions.Remove(SaleTransaction.(s => s. == 1));

                return true;
            }
            catch
            {
                throw;
            }
        }

        public IList<SaleTransaction> GetEndDayTradeTransactions(int productId, DateTime from, DateTime to, string transactionType, List<string> acceptStatus)
        {
            IQueryable<SaleTransaction> records = Select().Where(e => e.CurrencyId == productId);

            if (!string.IsNullOrEmpty(transactionType))
            {
                records = records.Where(e => e.TransactionType == transactionType);
            }

            if (acceptStatus.Count > 0)
            {
                records = records.Where(e => acceptStatus.Contains(e.Sales.Status));
            }

            return records.Where(e => e.Sales.LastApprovalOn >= from && e.Sales.LastApprovalOn <= to).ToList();
        }
    }
}