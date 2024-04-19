using DataAccess;
using DataAccess.POCO;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public class SaleRepository : ISaleRepository
    {
        private DataAccess.GreatEastForex db;

        public SaleRepository()
        {
            db = new DataAccess.GreatEastForex();
        }

        public IQueryable<Sale> Select()
        {
            var result = from s in db.Sales select s;

            return result.Where(e => e.IsDeleted == "N");
        }

        public IQueryable<Sale> SelectAll()
        {
            var result = from s in db.Sales select s;

            return result;
        }

        public IList<Sale> GetAll()
        {
            try
            {
                IQueryable<Sale> records = Select();

                return records.OrderByDescending(e => e.CreatedOn).ToList();
            }
            catch
            {
                throw;
            }
        }

        public IList<Sale> GetAllSales(string from, string to)
        {
            try
            {
                IQueryable<Sale> records = SelectAll();

                if (!string.IsNullOrEmpty(from))
                {
                    DateTime fromDate = Convert.ToDateTime(from + " 00:00:00");
                    records = records.Where(e => e.IssueDate >= fromDate);
                }

                if (!string.IsNullOrEmpty(to))
                {
                    DateTime toDate = Convert.ToDateTime(to + " 23:59:59");
                    records = records.Where(e => e.IssueDate <= toDate);
                }

                return records.OrderByDescending(e => e.CreatedOn).ToList();
            }
            catch
            {
                throw;
            }
        }

        //Get First week record
        public IList<Sale> GetOneWeekRecord(string firstDate, string keyword, string status, string date = "")
        {
            try
            {
                IQueryable<Sale> records = Select();

                if (!string.IsNullOrEmpty(firstDate))
                {
                    DateTime getfirstDate = Convert.ToDateTime(firstDate + " 00:00:00");
                    records = records.Where(e => e.CreatedOn >= getfirstDate);
                }

                if (!string.IsNullOrEmpty(keyword))
                {

                    //rec = rec.Where(e => e.CustomerParticulars.Company_RegisteredName != null).ToList();
                    //rec = rec.Where(e => e.CustomerParticulars.Natural_Name != null).ToList();
                    records = records.Where(e => e.MemoID.Contains(keyword) || e.CustomerParticulars.Company_RegisteredName.Contains(keyword) || e.CustomerParticulars.Natural_Name.Contains(keyword));
                }

                if (!string.IsNullOrEmpty(status))
                {
                    if (status == "Pending GM Approval")
                    {
                        records = records.Where(e => e.Status == "Pending GM Approval");
                    }
                    else
                    {
                        records = records.Where(e => e.Status.Contains(status));
                    }

                }

                if (!string.IsNullOrEmpty(date))
                {
                    DateTime start = Convert.ToDateTime(date + " 00:00:00");
                    DateTime end = Convert.ToDateTime(date + " 23:59:59.9999999");

                    records = records.Where(e => e.IssueDate >= start && e.IssueDate <= end);
                }

                //if (!string.IsNullOrEmpty(lastDate))
                //{
                //    DateTime getlastDate = Convert.ToDateTime(lastDate + " 23:59:59");
                //    records = records.Where(e => e.CreatedOn <= getlastDate);
                //}

                return records.OrderByDescending(e => e.CreatedOn).ToList();
            }
            catch
            {
                throw;
            }
        }

        public IList<Sale> GetAllSales2(string keyword, string status, string date = "")
        {
            try
            {

                var rec = db.Sales.Where(e => e.IsDeleted == "N");

                //rec = rec.Where(e => e.Status == "Completed").ToList();
                if (!string.IsNullOrEmpty(keyword))
                {

                    //rec = rec.Where(e => e.CustomerParticulars.Company_RegisteredName != null).ToList();
                    //rec = rec.Where(e => e.CustomerParticulars.Natural_Name != null).ToList();
                    rec = rec.Where(e => e.MemoID.Contains(keyword) || e.CustomerParticulars.Company_RegisteredName.Contains(keyword) || e.CustomerParticulars.Natural_Name.Contains(keyword));
                }

                if (!string.IsNullOrEmpty(status))
                {
                    if (status == "Pending GM Approval")
                    {
                        rec = rec.Where(e => e.Status == "Pending GM Approval");
                    }
                    else
                    {
                        rec = rec.Where(e => e.Status.Contains(status));
                    }

                }

                if (!string.IsNullOrEmpty(date))
                {
                    DateTime start = Convert.ToDateTime(date + " 00:00:00");
                    DateTime end = Convert.ToDateTime(date + " 23:59:59.9999999");

                    rec = rec.Where(e => e.IssueDate >= start && e.IssueDate <= end);
                }

                var getrecords = rec.OrderByDescending(e => e.CreatedOn).ToList();
                return getrecords;

                //var records = Select();

                //if (!string.IsNullOrEmpty(keyword))
                //{
                //    records = records.Where(e => e.MemoID.Contains(keyword) || e.CustomerParticulars.Company_RegisteredName.Contains(keyword) || e.CustomerParticulars.Natural_Name.Contains(keyword));
                //}

                //if (!string.IsNullOrEmpty(status))
                //{
                //    if (status == "Pending GM Approval")
                //    {
                //        records = records.Where(e => e.Status == "Pending GM Approval");
                //    }
                //    else
                //    {
                //        records = records.Where(e => e.Status.Contains(status));
                //    }

                //}

                //if (!string.IsNullOrEmpty(date))
                //{
                //    DateTime start = Convert.ToDateTime(date + " 00:00:00");
                //    DateTime end = Convert.ToDateTime(date + " 23:59:59.9999999");

                //    records = records.Where(e => e.IssueDate >= start && e.IssueDate <= end);
                //}

                //var getrecords = records.OrderByDescending(e => e.CreatedOn).ToList();
                //return getrecords;
            }
            catch
            {
                throw;
            }
        }

        public IList<Sale> GetTodaySales(string status = "")
        {
            try
            {
                IQueryable<Sale> records = Select();

                if (!string.IsNullOrEmpty(status))
                {
                    records = records.Where(e => e.Status.Contains(status));
                }

                string today = DateTime.Now.ToString("dd/MM/yyyy");
                DateTime start = Convert.ToDateTime(today + " 00:00:00");
                DateTime end = Convert.ToDateTime(today + " 23:59:59.9999999");

                return records.Where(e => e.IssueDate >= start && e.IssueDate <= end && !(e.Status == "Rejected" || e.Status == "Disapproved" || e.Status == "Cancelled")).OrderByDescending(e => e.CreatedOn).ToList();
            }
            catch
            {
                throw;
            }
        }

        public IList<Sale> GetWeeklySales(string from, string to)
        {
            try
            {
                IQueryable<Sale> records = Select();

                if (!string.IsNullOrEmpty(from))
                {
                    DateTime fromDate = Convert.ToDateTime(from + " 00:00:00.0000000");
                    records = records.Where(e => e.IssueDate >= fromDate);
                }

                if (!string.IsNullOrEmpty(to))
                {
                    DateTime toDate = Convert.ToDateTime(to + " 23:59:59.9999999");
                    records = records.Where(e => e.IssueDate <= toDate);
                }

                return records.Where(e => !(e.Status == "Rejected" || e.Status == "Disapproved" || e.Status == "Cancelled")).OrderByDescending(e => e.CreatedOn).ToList();
            }
            catch
            {
                throw;
            }
        }

        public Sale GetSingle(int id)
        {
            try
            {
                IQueryable<Sale> records = Select();

                return records.Where(e => e.ID == id).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public Sale GetSingleJuniorDealer(int id, int userid)
        {
            try
            {
                IQueryable<Sale> records = Select();

                return records.Where(e => e.ID == id && e.CreatedBy == userid).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public Sale GetSingle2(int id)
        {
            try
            {
                var records = db.Sales.Where(e => e.IsDeleted == "N").ToList();
                return records.Where(e => e.ID == id).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public Sale GetSingle(string memoID)
        {
            try
            {
                IQueryable<Sale> records = Select();

                return records.Where(e => e.MemoID == memoID).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public Sale GetLastRecord()
        {
            try
            {
                int year = DateTime.Now.Year;

                IQueryable<Sale> records = Select();

                return records.Where(e => e.CreatedOn.Year == year).OrderByDescending(e => e.ID).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public IList<Sale> GetUserSales(List<string> status, int userId, string isOpsExec = null)
        {
            try
            {
                IQueryable<Sale> records = Select();

                string s1 = status.Where(e => e.Contains("Pending Delivery by ")).FirstOrDefault();
                string s2 = status.Where(e => e.Contains("Pending Incoming Delivery by ")).FirstOrDefault();

                return records.Where(e => status.Contains(e.Status) || (e.Status.Contains(s1) && e.PendingDeliveryById == userId) || (e.Status.Contains(s2) && e.PendingDeliveryById == userId)).OrderByDescending(e => e.CreatedOn).ToList();
            }
            catch
            {
                throw;
            }
        }

        public int GetUserSalesCount(List<string> status, int userId)
        {
            try
            {
                IQueryable<Sale> records = Select();

                string s1 = status.Where(e => e.Contains("Pending Delivery by ")).FirstOrDefault();
                string s2 = status.Where(e => e.Contains("Pending Incoming Delivery by ")).FirstOrDefault();

                return records.Where(e => status.Contains(e.Status) || (e.Status.Contains(s1) && e.PendingDeliveryById == userId) || (e.Status.Contains(s2) && e.PendingDeliveryById == userId)).OrderByDescending(e => e.CreatedOn).Count();
            }
            catch
            {
                throw;
            }
        }

        public int GetUserSalesCount2(List<string> status, int userId, string isOpsExec = null, string isOpsManager = null)
        {
            try
            {
                IQueryable<Sale> records = Select();

                string s1 = status.Where(e => e.Contains("Pending Delivery by ")).FirstOrDefault();
                string s2 = status.Where(e => e.Contains("Pending Incoming Delivery by ")).FirstOrDefault();

                return records.Where(e => status.Contains(e.Status) || (e.Status.Contains(s1) && e.PendingDeliveryById == userId) || (e.Status.Contains(s2) && e.PendingDeliveryById == userId)).OrderByDescending(e => e.CreatedOn).Distinct().Count();
            }
            catch
            {
                throw;
            }
        }

        public IPagedList<Sale> GetPaged(string keyword, string status, int page, int size, string date = "")
        {
            try
            {
                IQueryable<Sale> records = Select();

                if (!string.IsNullOrEmpty(keyword))
                {
                    records = records.Where(e => e.MemoID.Contains(keyword) || e.CustomerParticulars.Company_RegisteredName.Contains(keyword) || e.CustomerParticulars.Natural_Name.Contains(keyword));
                }

                if (!string.IsNullOrEmpty(status))
                {
                    if (status == "Pending GM Approval")
                    {
                        records = records.Where(e => e.Status == "Pending GM Approval");
                    }
                    else
                    {
                        records = records.Where(e => e.Status.Contains(status));
                    }

                }

                if (!string.IsNullOrEmpty(date))
                {
                    DateTime start = Convert.ToDateTime(date + " 00:00:00");
                    DateTime end = Convert.ToDateTime(date + " 23:59:59.9999999");

                    records = records.Where(e => e.IssueDate >= start && e.IssueDate <= end);
                }

                //return records.OrderBy(e => e.CollectionDate).ThenByDescending(e => e.CollectionTime == "9am to 10am").ThenByDescending(e => e.CollectionTime == "10am to 12pm").ThenByDescending(e => e.CollectionTime == "2pm to 3pm").ThenByDescending(e => e.CollectionTime == "3pm to 5pm").ToPagedList(page, size);
                return records.OrderByDescending(e => e.CreatedOn).ToPagedList(page, size);
            }
            catch
            {
                throw;
            }
        }

        public IPagedList<Sale> GetPaged(string keyword, string status, string fromDate, string toDate, int page, int size)
        {
            try
            {
                IQueryable<Sale> records = Select();

                if (!string.IsNullOrEmpty(keyword))
                {
                    records = records.Where(e => e.MemoID.Contains(keyword) || e.CustomerParticulars.Company_RegisteredName.Contains(keyword) || e.CustomerParticulars.Natural_Name.Contains(keyword));
                }

                if (!string.IsNullOrEmpty(status))
                {
                    records = records.Where(e => e.Status.Contains(status));
                }

                if (!string.IsNullOrEmpty(fromDate))
                {
                    DateTime startDate = Convert.ToDateTime(fromDate + " 00:00:00");

                    records = records.Where(e => e.LastApprovalOn >= startDate);
                }

                if (!string.IsNullOrEmpty(toDate))
                {
                    DateTime endDate = Convert.ToDateTime(toDate + " 23:59:59.9999999");

                    records = records.Where(e => e.LastApprovalOn <= endDate);
                }

                //return records.OrderBy(e => e.CollectionDate).ThenByDescending(e => e.CollectionTime == "9am to 10am").ThenByDescending(e => e.CollectionTime == "10am to 12pm").ThenByDescending(e => e.CollectionTime == "2pm to 3pm").ThenByDescending(e => e.CollectionTime == "3pm to 5pm").ToPagedList(page, size);
                return records.OrderByDescending(e => e.CreatedOn).ToPagedList(page, size);
            }
            catch
            {
                throw;
            }
        }

        public IPagedList<Sale> GetSaleDataPaged(string from, string to, int page, int size)
        {
            try
            {
                IQueryable<Sale> records = Select();

                if (!string.IsNullOrEmpty(from))
                {
                    DateTime fromDate = Convert.ToDateTime(from + " 00:00:00");
                    records = records.Where(e => e.IssueDate >= fromDate);
                }

                if (!string.IsNullOrEmpty(to))
                {
                    DateTime toDate = Convert.ToDateTime(to + " 23:59:59.999999");
                    records = records.Where(e => e.IssueDate <= toDate);
                }

                return records.OrderByDescending(e => e.CreatedOn).ToPagedList(page, size);
            }
            catch
            {
                throw;
            }
        }

        public IPagedList<Sale> GetSaleLastApprovedDataPaged(string from, string to, int page, int size)
        {
            try
            {
                IQueryable<Sale> records = Select();

                if (!string.IsNullOrEmpty(from))
                {
                    DateTime fromDate = Convert.ToDateTime(from + " 00:00:00");
                    records = records.Where(e => e.LastApprovalOn >= fromDate);
                }

                if (!string.IsNullOrEmpty(to))
                {
                    DateTime toDate = Convert.ToDateTime(to + " 23:59:59.999999");
                    records = records.Where(e => e.LastApprovalOn <= toDate);
                }

                return records.OrderByDescending(e => e.CreatedOn).ToPagedList(page, size);
            }
            catch
            {
                throw;
            }
        }

        public IPagedList<Sale> GetSalesByDatePaged(string fromDate, string toDate, List<string> exceptionStatus, List<string> acceptStatus, int page, int pageSize, List<int> productIds, int cid = 0, string transactionType = null)
        {
            try
            {
                IQueryable<Sale> records = Select();

                if (!string.IsNullOrEmpty(fromDate))
                {
                    DateTime startDate = Convert.ToDateTime(fromDate + " 00:00:00");

                    records = records.Where(e => e.IssueDate >= startDate);
                }

                if (!string.IsNullOrEmpty(toDate))
                {
                    DateTime endDate = Convert.ToDateTime(toDate + " 23:59:59.9999999");

                    records = records.Where(e => e.IssueDate <= endDate);
                }

                if (exceptionStatus.Count > 0)
                {
                    records = records.Where(e => !exceptionStatus.Any(f => f.Contains(e.Status)));
                }

                if (acceptStatus.Count > 0)
                {
                    records = records.Where(e => acceptStatus.Where(f => e.Status.Contains(f)).Count() > 0);
                }

                if (cid > 0)
                {
                    records = records.Where(e => e.CustomerParticularId == cid);
                }

                if (productIds.Count > 0)
                {
                    records = records.Where(e => e.SaleTransactions.Where(f => productIds.Contains(f.CurrencyId) && f.TransactionType == transactionType).Count() > 0);
                }

                if (!string.IsNullOrEmpty(transactionType))
                {
                    records = records.Where(e => e.SaleTransactions.Where(f => f.TransactionType == transactionType).Count() > 0);
                }

                return records.OrderByDescending(e => e.CreatedOn).ToPagedList(page, pageSize);
            }
            catch
            {
                throw;
            }
        }

        public IPagedList<Sale> GetLastApprovedSalesByDatePaged(string fromDate, string toDate, List<string> exceptionStatus, List<string> acceptStatus, int page, int pageSize, List<int> productIds, int cid = 0, string transactionType = null)
        {
            try
            {
                IQueryable<Sale> records = Select();

                if (!string.IsNullOrEmpty(fromDate))
                {
                    DateTime startDate = Convert.ToDateTime(fromDate + " 00:00:00");

                    records = records.Where(e => e.LastApprovalOn >= startDate);
                }

                if (!string.IsNullOrEmpty(toDate))
                {
                    DateTime endDate = Convert.ToDateTime(toDate + " 23:59:59.9999999");

                    records = records.Where(e => e.LastApprovalOn <= endDate);
                }

                if (exceptionStatus.Count > 0)
                {
                    records = records.Where(e => !exceptionStatus.Any(f => f.Contains(e.Status)));
                }

                if (acceptStatus.Count > 0)
                {
                    records = records.Where(e => acceptStatus.Where(f => e.Status.Contains(f)).Count() > 0);
                }

                if (cid > 0)
                {
                    records = records.Where(e => e.CustomerParticularId == cid);
                }

                if (productIds.Count > 0)
                {
                    records = records.Where(e => e.SaleTransactions.Where(f => productIds.Contains(f.CurrencyId) && f.TransactionType == transactionType).Count() > 0);
                }

                if (!string.IsNullOrEmpty(transactionType))
                {
                    records = records.Where(e => e.SaleTransactions.Where(f => f.TransactionType == transactionType).Count() > 0);
                }

                return records.OrderByDescending(e => e.CreatedOn).ToPagedList(page, pageSize);
            }
            catch
            {
                throw;
            }
        }

        public decimal[] GetReportGrandTotal(string fromDate, string toDate, List<string> exceptionStatus, List<string> acceptStatus, List<int> productIds, int cid = 0, string transactionType = null)
        {
            try
            {
                IQueryable<Sale> records = Select();

                if (!string.IsNullOrEmpty(fromDate))
                {
                    DateTime startDate = Convert.ToDateTime(fromDate + " 00:00:00");

                    records = records.Where(e => e.IssueDate >= startDate);
                    //records = records.Where(e => e.LastApprovalOn >= startDate);
                }

                if (!string.IsNullOrEmpty(toDate))
                {
                    DateTime endDate = Convert.ToDateTime(toDate + " 23:59:59.9999999");

                    records = records.Where(e => e.IssueDate <= endDate);
                    //records = records.Where(e => e.LastApprovalOn <= endDate);
                }

                if (exceptionStatus.Count > 0)
                {
                    records = records.Where(e => !exceptionStatus.Any(f => f.Contains(e.Status)));
                }

                if (acceptStatus.Count > 0)
                {
                    records = records.Where(e => acceptStatus.Where(f => e.Status.Contains(f)).Count() > 0);
                }

                if (cid > 0)
                {
                    records = records.Where(e => e.CustomerParticularId == cid);
                }

                if (productIds.Count > 0)
                {
                    records = records.Where(e => e.SaleTransactions.Where(f => productIds.Contains(f.CurrencyId)).Count() > 0);
                }

                if (!string.IsNullOrEmpty(transactionType))
                {
                    records = records.Where(e => e.SaleTransactions.Where(f => f.TransactionType == transactionType).Count() > 0);
                }

                decimal totalForeign = -1;
                List<int> distinctProducts = records.SelectMany(e => e.SaleTransactions.Where(f => f.TransactionType == transactionType && (productIds.Contains(f.CurrencyId) || productIds.Count == 0)).Select(f => f.CurrencyId)).Distinct().ToList();

                if (distinctProducts.Count == 1)
                {
                    totalForeign = records.Sum(e => e.SaleTransactions.Where(f => f.TransactionType == transactionType && (productIds.Contains(f.CurrencyId) || productIds.Count == 0)).Sum(f => f.AmountForeign));
                }
                decimal totalLocal = records.Sum(e => e.SaleTransactions.Where(f => f.TransactionType == transactionType && (productIds.Contains(f.CurrencyId) || productIds.Count == 0)).Sum(f => f.AmountLocal));

                return new decimal[2] { totalForeign, totalLocal };
            }
            catch
            {
                throw;
            }
        }

        public decimal[] GetLastApprovedReportGrandTotal(string fromDate, string toDate, List<string> exceptionStatus, List<string> acceptStatus, List<int> productIds, int cid = 0, string transactionType = null)
        {
            try
            {
                IQueryable<Sale> records = Select();

                if (!string.IsNullOrEmpty(fromDate))
                {
                    DateTime startDate = Convert.ToDateTime(fromDate + " 00:00:00");

                    records = records.Where(e => e.LastApprovalOn >= startDate);
                    //records = records.Where(e => e.LastApprovalOn >= startDate);
                }

                if (!string.IsNullOrEmpty(toDate))
                {
                    DateTime endDate = Convert.ToDateTime(toDate + " 23:59:59.9999999");

                    records = records.Where(e => e.LastApprovalOn <= endDate);
                    //records = records.Where(e => e.LastApprovalOn <= endDate);
                }

                if (exceptionStatus.Count > 0)
                {
                    records = records.Where(e => !exceptionStatus.Any(f => f.Contains(e.Status)));
                }

                if (acceptStatus.Count > 0)
                {
                    records = records.Where(e => acceptStatus.Where(f => e.Status.Contains(f)).Count() > 0);
                }

                if (cid > 0)
                {
                    records = records.Where(e => e.CustomerParticularId == cid);
                }

                if (productIds.Count > 0)
                {
                    records = records.Where(e => e.SaleTransactions.Where(f => productIds.Contains(f.CurrencyId)).Count() > 0);
                }

                if (!string.IsNullOrEmpty(transactionType))
                {
                    records = records.Where(e => e.SaleTransactions.Where(f => f.TransactionType == transactionType).Count() > 0);
                }

                decimal totalForeign = -1;
                List<int> distinctProducts = records.SelectMany(e => e.SaleTransactions.Where(f => f.TransactionType == transactionType && (productIds.Contains(f.CurrencyId) || productIds.Count == 0)).Select(f => f.CurrencyId)).Distinct().ToList();

                if (distinctProducts.Count == 1)
                {
                    totalForeign = records.Sum(e => e.SaleTransactions.Where(f => f.TransactionType == transactionType && (productIds.Contains(f.CurrencyId) || productIds.Count == 0)).Sum(f => f.AmountForeign));
                }
                decimal totalLocal = records.Sum(e => e.SaleTransactions.Where(f => f.TransactionType == transactionType && (productIds.Contains(f.CurrencyId) || productIds.Count == 0)).Sum(f => f.AmountLocal));

                return new decimal[2] { totalForeign, totalLocal };
            }
            catch
            {
                throw;
            }
        }

        public IList<Sale> GetSalesByYear(int year)
        {
            try
            {
                IQueryable<Sale> records = SelectAll();

                return records.Where(e => e.CreatedOn.Year == year).OrderByDescending(e => e.CreatedOn).ToList();
            }
            catch
            {
                throw;
            }
        }

        public IList<Sale> GetSaleDataByDate(string fromDate, string toDate, List<string> exceptionStatus, List<string> acceptStatus, List<int> productIds, int cid = 0, string transactionType = null)
        {
            try
            {
                IQueryable<Sale> records = Select();

                if (!string.IsNullOrEmpty(fromDate))
                {
                    DateTime startDate = Convert.ToDateTime(fromDate + " 00:00:00");

                    records = records.Where(e => e.IssueDate >= startDate);
                }

                if (!string.IsNullOrEmpty(toDate))
                {
                    DateTime endDate = Convert.ToDateTime(toDate + " 23:59:59.9999999");

                    records = records.Where(e => e.IssueDate <= endDate);
                }

                if (exceptionStatus.Count > 0)
                {
                    records = records.Where(e => !exceptionStatus.Any(f => f.Contains(e.Status)));
                }

                if (acceptStatus.Count > 0)
                {
                    records = records.Where(e => acceptStatus.Where(f => e.Status.Contains(f)).Count() > 0);
                }

                if (cid > 0)
                {
                    records = records.Where(e => e.CustomerParticularId == cid);
                }

                if (productIds.Count > 0)
                {
                    records = records.Where(e => e.SaleTransactions.Where(f => productIds.Contains(f.CurrencyId) && f.TransactionType == transactionType).Count() > 0);
                }

                if (!string.IsNullOrEmpty(transactionType))
                {
                    records = records.Where(e => e.SaleTransactions.Where(f => f.TransactionType == transactionType).Count() > 0);
                }

                return records.OrderByDescending(e => e.CreatedOn).ToList();
            }
            catch
            {
                throw;
            }
        }

        public IList<Sale> GetSaleLastApprovedDataByDate(string fromDate, string toDate, List<string> exceptionStatus, List<string> acceptStatus, List<int> productIds, int cid = 0, string transactionType = null)
        {
            try
            {
                IQueryable<Sale> records = Select();

                if (!string.IsNullOrEmpty(fromDate))
                {
                    DateTime startDate = Convert.ToDateTime(fromDate + " 00:00:00");

                    records = records.Where(e => e.LastApprovalOn >= startDate);
                }

                if (!string.IsNullOrEmpty(toDate))
                {
                    DateTime endDate = Convert.ToDateTime(toDate + " 23:59:59.9999999");

                    records = records.Where(e => e.LastApprovalOn <= endDate);
                }

                if (exceptionStatus.Count > 0)
                {
                    records = records.Where(e => !exceptionStatus.Any(f => f.Contains(e.Status)));
                }

                if (acceptStatus.Count > 0)
                {
                    records = records.Where(e => acceptStatus.Where(f => e.Status.Contains(f)).Count() > 0);
                }

                if (cid > 0)
                {
                    records = records.Where(e => e.CustomerParticularId == cid);
                }

                if (productIds.Count > 0)
                {
                    records = records.Where(e => e.SaleTransactions.Where(f => productIds.Contains(f.CurrencyId) && f.TransactionType == transactionType).Count() > 0);
                }

                if (!string.IsNullOrEmpty(transactionType))
                {
                    records = records.Where(e => e.SaleTransactions.Where(f => f.TransactionType == transactionType).Count() > 0);
                }

                return records.OrderByDescending(e => e.CreatedOn).ToList();
            }
            catch
            {
                throw;
            }
        }

        public IList<Sale> GetSalesByDateRange(string fromDate, string toDate)
        {
            try
            {
                var records = Select();

                DateTime from = Convert.ToDateTime(fromDate + " 00:00:00.0000000");
                DateTime to = Convert.ToDateTime(toDate + " 23:59:59.9999999");

                return records.Where(e => e.LastApprovalOn >= from && e.LastApprovalOn <= to && (e.Status == "Completed" || e.Status == "Pending Delete GM Approval")).ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<Sale> GetPendingDeleteSales(DateTime from, DateTime to, string transactionType)
        {
            try
            {
                var records = Select();

                if (!string.IsNullOrEmpty(transactionType))
                {
                    records = records.Where(e => e.SaleTransactions.Where(f => f.TransactionType == transactionType).Count() > 0);
                }

                return records.Where(e => e.LastApprovalOn >= from && e.LastApprovalOn <= to && e.Status == "Pending Delete GM Approval").OrderBy(e => e.MemoID).ToList();
            }
            catch
            {
                throw;
            }
        }

        public bool Add(Sale addData)
        {
            try
            {
                addData.LastApprovalOn = DateTime.Now;
                addData.CreatedOn = DateTime.Now;
                addData.UpdatedOn = DateTime.Now;
                addData.IsDeleted = "N";

                db.Sales.Add(addData);

                db.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool Update(int id, Sale updateData)
        {
            try
            {
                Sale data = db.Sales.Find(id);

                data.CustomerParticularId = updateData.CustomerParticularId;
                data.CollectionDate = updateData.CollectionDate;
                data.CollectionTime = updateData.CollectionTime;
                data.Urgent = updateData.Urgent;
                data.RequireDelivery = updateData.RequireDelivery;
                data.BagNo = updateData.BagNo;
                data.Remarks = updateData.Remarks;
                data.TransactionType = updateData.TransactionType;
                data.LocalPaymentMode = updateData.LocalPaymentMode;
                data.CashAmount = updateData.CashAmount;
                data.CashBank = updateData.CashBank;
                data.Cheque1No = updateData.Cheque1No;
                data.Cheque1Amount = updateData.Cheque1Amount;
                data.Cheque1Bank = updateData.Cheque1Bank;
                data.Cheque2No = updateData.Cheque2No;
                data.Cheque2Amount = updateData.Cheque2Amount;
                data.Cheque2Bank = updateData.Cheque2Bank;
                data.Cheque3No = updateData.Cheque3No;
                data.Cheque3Amount = updateData.Cheque3Amount;
                data.Cheque3Bank = updateData.Cheque3Bank;
                data.BankTransferNo = updateData.BankTransferNo;
                data.BankTransferAmount = updateData.BankTransferAmount;
                data.BankTransferBank = updateData.BankTransferBank;
                data.MemoBalance = updateData.MemoBalance;
                data.TotalAmountLocal = updateData.TotalAmountLocal;
                data.TotalAmountForeign = updateData.TotalAmountForeign;
                data.Status = updateData.Status;
                data.LastApprovalOn = updateData.LastApprovalOn;
                data.DeliveryConfirmation = updateData.DeliveryConfirmation;
                data.Sale_ContactPerson = updateData.Sale_ContactPerson;
                data.Sale_DeliveryRemarks = updateData.Sale_DeliveryRemarks;
                data.UpdatedOn = DateTime.Now;

                db.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool UpdateStatus(int id, string status)
        {
            try
            {
                Sale data = db.Sales.Find(id);

                data.Status = status;
                data.LastApprovalOn = DateTime.Now;
                data.UpdatedOn = DateTime.Now;

                db.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool UpdateStatus(int id, string status, int deliveryManId)
        {
            try
            {
                Sale data = db.Sales.Find(id);

                data.Status = status;
                data.PendingDeliveryById = deliveryManId;
                data.LastApprovalOn = DateTime.Now;
                data.UpdatedOn = DateTime.Now;

                db.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool UpdateDeleteStatus(int id, string status, int deliveryManId)
        {
            try
            {
                Sale data = db.Sales.Find(id);

                data.Status = status;
                data.PendingDeliveryById = deliveryManId;
                data.UpdatedOn = DateTime.Now;

                db.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool DisapproveSale(int id, Sale updateData)
        {
            try
            {
                Sale data = db.Sales.Find(id);

                data.Status = "Pending GM Approval (Rejected)";
                data.DisapprovedReason = updateData.DisapprovedReason;
                data.Remarks = updateData.Remarks;
                data.LastApprovalOn = DateTime.Now;
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
                Sale data = db.Sales.Find(id);

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

        public Sale GetEarliestSales(List<string> acceptStatus)
        {
            try
            {
                var records = Select();

                if (acceptStatus.Count > 0)
                {
                    records = records.Where(e => acceptStatus.Contains(e.Status));
                }

                return records.OrderBy(e => e.LastApprovalOn).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }
        public int GetPageCount(string keyword, string status)
        {
            try
            {
                var records = db.Sales.Where(e => e.IsDeleted == "N").Count();
                int pageSize = 50;

                if (records < 0)
                {
                    records = 0;
                }

                if (!string.IsNullOrEmpty(keyword))
                {

                    records = db.Sales.Where(e => e.IsDeleted == "N" && (e.MemoID.Contains(keyword) || e.CustomerParticulars.Company_RegisteredName.Contains(keyword) || e.CustomerParticulars.Natural_Name.Contains(keyword))).Count();
                }

                if (!string.IsNullOrEmpty(status))
                {
                    if (status == "Pending GM Approval")
                    {
                        records = db.Sales.Where(e => e.Status == "Pending GM Approval" && e.IsDeleted == "N").Count();
                    }
                    else
                    {
                        if (status == "Deliveries")
                        {
                            //Pending Assign Delivery, Pending Delivery, Pending Incoming Delivery
                            records = db.Sales.Where(e => e.Status.Contains("Pending Assign Delivery") || e.Status.Contains("Pending Delivery") || e.Status.Contains("Pending Incoming Delivery") && e.IsDeleted == "N").Count();
                        }
                        else
                        {
                            records = db.Sales.Where(e => e.Status.Contains(status) && e.IsDeleted == "N").Count();
                        }

                    }

                }

                double countpage = (records + (pageSize - 1)) / pageSize;

                int getpage = Convert.ToInt32(countpage);

                return getpage;
            }
            catch
            {
                throw;
            }
        }

        public int GetPageCount2(string keyword, string status, bool isMultipleRole, int userid)
        {
            try
            {
                var records = db.Sales.Where(e => e.IsDeleted == "N").Count();
                int pageSize = 50;

                if (records < 0)
                {
                    records = 0;
                }

                if (!isMultipleRole)
                {
                    //only is junior dealer

                    if (!string.IsNullOrEmpty(keyword))
                    {
                        records = db.Sales.Where(e => e.CreatedBy == userid && e.IsDeleted == "N" && (e.MemoID.Contains(keyword) || e.CustomerParticulars.Company_RegisteredName.Contains(keyword) || e.CustomerParticulars.Natural_Name.Contains(keyword))).Count();
                    }

                    if (!string.IsNullOrEmpty(status))
                    {
                        if (status == "Pending GM Approval")
                        {
                            records = db.Sales.Where(e => e.CreatedBy == userid && e.Status == "Pending GM Approval" && e.IsDeleted == "N").Count();
                        }
                        else
                        {
                            if (status == "Deliveries")
                            {
                                //Pending Assign Delivery, Pending Delivery, Pending Incoming Delivery
                                records = db.Sales.Where(e => e.CreatedBy == userid && e.Status.Contains("Pending Assign Delivery") || e.Status.Contains("Pending Delivery") || e.Status.Contains("Pending Incoming Delivery") && e.IsDeleted == "N").Count();
                            }
                            else
                            {
                                records = db.Sales.Where(e => e.CreatedBy == userid && e.Status.Contains(status) && e.IsDeleted == "N").Count();
                            }
                        }

                    }

                    if (string.IsNullOrEmpty(keyword) && string.IsNullOrEmpty(status))
                    {
                        //this is mean when keyword and status is empty, then only get the junior dealer own item.
                        records = db.Sales.Where(e => e.CreatedBy == userid && e.IsDeleted == "N").Count();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(keyword))
                    {
                        records = db.Sales.Where(e => e.IsDeleted == "N" && (e.MemoID.Contains(keyword) || e.CustomerParticulars.Company_RegisteredName.Contains(keyword) || e.CustomerParticulars.Natural_Name.Contains(keyword))).Count();
                    }

                    if (!string.IsNullOrEmpty(status))
                    {
                        if (status == "Pending GM Approval")
                        {
                            records = db.Sales.Where(e => e.Status == "Pending GM Approval" && e.IsDeleted == "N").Count();
                        }
                        else
                        {
                            if (status == "Deliveries")
                            {
                                //Pending Assign Delivery, Pending Delivery, Pending Incoming Delivery
                                records = db.Sales.Where(e => e.Status.Contains("Pending Assign Delivery") || e.Status.Contains("Pending Delivery") || e.Status.Contains("Pending Incoming Delivery") && e.IsDeleted == "N").Count();
                            }
                            else
                            {
                                records = db.Sales.Where(e => e.Status.Contains(status) && e.IsDeleted == "N").Count();
                            }
                        }

                    }
                }



                double countpage = (records + (pageSize - 1)) / pageSize;

                int getpage = Convert.ToInt32(countpage);

                return getpage;
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<Sale> GetCustomItems(int page, string keyword, string status)
        {
            try
            {
                int getpage = page;
                int fixTotalItems = 50;
                int minusPerpage = 50;
                int getMaxItems = getpage * fixTotalItems;
                int getRange = getMaxItems - minusPerpage;


                if (!string.IsNullOrEmpty(keyword))
                {
                    var rec = db.Sales.Where(e => e.IsDeleted == "N" && (e.MemoID.Contains(keyword) || e.CustomerParticulars.Company_RegisteredName.Contains(keyword) || e.CustomerParticulars.Natural_Name.Contains(keyword))).ToList();
                    var getrecords = rec.OrderByDescending(e => e.CreatedOn);
                    return getrecords.Skip(getRange).Take(fixTotalItems);
                }

                else if (!string.IsNullOrEmpty(status))
                {
                    if (status == "Pending GM Approval")
                    {
                        var rec = db.Sales.Where(e => e.IsDeleted == "N").ToList();
                        rec = rec.Where(e => e.Status == "Pending GM Approval").ToList();
                        var getrecords = rec.OrderByDescending(e => e.CreatedOn);
                        return getrecords.Skip(getRange).Take(fixTotalItems);
                    }
                    else
                    {
                        var rec = db.Sales.Where(e => e.IsDeleted == "N").ToList();

                        if (status == "Deliveries")
                        {
                            //Pending Assign Delivery, Pending Delivery, Pending Incoming Delivery
                            rec = rec.Where(e => e.Status.Contains("Pending Assign Delivery") || e.Status.Contains("Pending Delivery") || e.Status.Contains("Pending Incoming Delivery")).ToList();
                        }
                        else
                        {
                            rec = rec.Where(e => e.Status.Contains(status)).ToList();
                        }

                        var getrecords = rec.OrderByDescending(e => e.CreatedOn);
                        return getrecords.Skip(getRange).Take(fixTotalItems);
                    }
                }

                else
                {
                    var rec = db.Sales.Where(e => e.IsDeleted == "N").ToList();
                    var getrecords = rec.OrderByDescending(e => e.CreatedOn);
                    return getrecords.Skip(getRange).Take(fixTotalItems);
                }
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<Sale> GetCustomItems2(int page, string keyword, string status, bool isMultipleRole, int userid)
        {
            try
            {
                int getpage = page;
                int fixTotalItems = 50;
                int minusPerpage = 50;
                int getMaxItems = getpage * fixTotalItems;
                int getRange = getMaxItems - minusPerpage;

                if (!string.IsNullOrEmpty(keyword))
                {

                    if (!isMultipleRole)
                    {
                        //only is junior dealer
                        var rec = db.Sales.Where(e => e.CreatedBy == userid && e.IsDeleted == "N" && (e.MemoID.Contains(keyword) || e.CustomerParticulars.Company_RegisteredName.Contains(keyword) || e.CustomerParticulars.Natural_Name.Contains(keyword)));
                        var getrecords = rec.OrderByDescending(e => e.CreatedOn);
                        return getrecords.Skip(getRange).Take(fixTotalItems).ToList();
                    }
                    else
                    {
                        var rec = db.Sales.Where(e => e.IsDeleted == "N" && (e.MemoID.Contains(keyword) || e.CustomerParticulars.Company_RegisteredName.Contains(keyword) || e.CustomerParticulars.Natural_Name.Contains(keyword)));
                        var getrecords = rec.OrderByDescending(e => e.CreatedOn);
                        return getrecords.Skip(getRange).Take(fixTotalItems).ToList();
                    }

                }
                else if (!string.IsNullOrEmpty(status))
                {
                    if (status == "Pending GM Approval")
                    {
                        if (!isMultipleRole)
                        {
                            var rec = db.Sales.Where(e => e.CreatedBy == userid && e.IsDeleted == "N");
                            rec = rec.Where(e => e.Status == "Pending GM Approval");
                            var getrecords = rec.OrderByDescending(e => e.CreatedOn);
                            return getrecords.Skip(getRange).Take(fixTotalItems).ToList();
                        }
                        else
                        {
                            var rec = db.Sales.Where(e => e.IsDeleted == "N");
                            rec = rec.Where(e => e.Status == "Pending GM Approval");
                            var getrecords = rec.OrderByDescending(e => e.CreatedOn);
                            return getrecords.Skip(getRange).Take(fixTotalItems).ToList();
                        }
                    }
                    else
                    {
                        var rec = db.Sales.Where(e => e.IsDeleted == "N");

                        if (status == "Deliveries")
                        {
                            if (!isMultipleRole)
                            {
                                //Pending Assign Delivery, Pending Delivery, Pending Incoming Delivery
                                rec = rec.Where(e => e.CreatedBy == userid && e.Status.Contains("Pending Assign Delivery") || e.Status.Contains("Pending Delivery") || e.Status.Contains("Pending Incoming Delivery"));
                            }
                            else
                            {
                                //Pending Assign Delivery, Pending Delivery, Pending Incoming Delivery
                                rec = rec.Where(e => e.Status.Contains("Pending Assign Delivery") || e.Status.Contains("Pending Delivery") || e.Status.Contains("Pending Incoming Delivery"));
                            }
                        }
                        else
                        {
                            if (!isMultipleRole)
                            {
                                rec = rec.Where(e => e.CreatedBy == userid && e.Status.Contains(status));
                            }
                            else
                            {
                                rec = rec.Where(e => e.Status.Contains(status));
                            }

                        }

                        var getrecords = rec.OrderByDescending(e => e.CreatedOn);
                        return getrecords.Skip(getRange).Take(fixTotalItems).ToList();
                    }
                }
                else
                {
                    if (!isMultipleRole)
                    {
                        var rec = db.Sales.Where(e => e.CreatedBy == userid && e.IsDeleted == "N");
                        var getrecords = rec.OrderByDescending(e => e.CreatedOn);
                        return getrecords.Skip(getRange).Take(fixTotalItems).ToList();
                    }
                    else
                    {
                        var rec = db.Sales.Where(e => e.IsDeleted == "N");
                        var getrecords = rec.OrderByDescending(e => e.CreatedOn);
                        return getrecords.Skip(getRange).Take(fixTotalItems).ToList();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public IList<Sale> GetSalesSummaryList(string fromDate, string toDate, List<int> productIds, List<string> transactionList, List<int> CustomerList)
        {
            try
            {
                IQueryable<Sale> records = Select();
                records = records.Where(e => e.Status != "Cancelled");
                if (!string.IsNullOrEmpty(fromDate))
                {
                    DateTime startDate = Convert.ToDateTime(fromDate + " 00:00:00");

                    records = records.Where(e => e.LastApprovalOn >= startDate);
                }

                if (!string.IsNullOrEmpty(toDate))
                {
                    DateTime endDate = Convert.ToDateTime(toDate + " 23:59:59.9999999");

                    records = records.Where(e => e.LastApprovalOn <= endDate);
                }
                if (!CustomerList.Contains(0))
                {
                    records = records.Where(e => CustomerList.Contains(e.CustomerParticularId));
                }
                if (transactionList.Count > 0)
                {
                    records = records.Where(e => transactionList.Contains(e.TransactionType));
                }
                if (productIds.Count > 0)
                {
                    records = records.Where(e => e.SaleTransactions.Where(f => productIds.Contains(f.CurrencyId)).Count() > 0);
                }
                return records.OrderByDescending(e => e.CreatedOn).ToList();
            }
            catch
            {
                throw;
            }
        }

    }
}