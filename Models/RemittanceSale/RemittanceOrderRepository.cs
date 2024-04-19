using DataAccess;
using DataAccess.POCO;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public class RemittanceOrderRepository : IRemittanceOrderRepository
    {
        private DataAccess.GreatEastForex db;

        public RemittanceOrderRepository()
        {
            db = new DataAccess.GreatEastForex();
        }

        public IQueryable<RemittanceOrders> Select()
        {
            var result = from s in db.RemittanceOrders orderby s.ID select s;

            return result;
        }

        public IList<RemittanceOrders> GetSaleTransactions(int saleid)
        {
            try
            {
                IQueryable<RemittanceOrders> records = Select();

                return records.Where(e => e.RemittanceId == saleid).OrderBy(e => e.ID).ToList();
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
                var records = db.RemittanceOrders.Where(e => e.GetCurrency == productId).Select(n => new {
                    n.GetCurrency
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

        public IList<RemittanceOrders> GetProductTransactions(int productId, string fromDate, string toDate, string transactionType, List<string> exceptionStatus)
        {
            try
            {
                IQueryable<RemittanceOrders> records = Select().Where(e => e.GetCurrency == productId);

                if (!string.IsNullOrEmpty(fromDate))
                {
                    DateTime startDate = Convert.ToDateTime(fromDate + " 00:00:00");

                    records = records.Where(e => e.Remittances.IssueDate >= startDate);
                }

                if (!string.IsNullOrEmpty(toDate))
                {
                    DateTime endDate = Convert.ToDateTime(toDate + " 23:59:59.9999999");

                    records = records.Where(e => e.Remittances.IssueDate <= endDate);
                }

                if (exceptionStatus.Count > 0)
                {
                    records = records.Where(e => !exceptionStatus.Contains(e.Remittances.Status));
                }

                return records.ToList();
            }
            catch
            {
                throw;
            }
        }

        public IPagedList<RemittanceOrders> GetTransactionsPaged(string date, List<string> exceptionStatus, int page, int pageSize)
        {
            try
            {
                IQueryable<RemittanceOrders> records = Select();

                if (!string.IsNullOrEmpty(date))
                {
                    DateTime startDate = Convert.ToDateTime(date + " 00:00:00");
                    DateTime endDate = Convert.ToDateTime(date + " 23:59:59.9999999");

                    records = records.Where(e => e.Remittances.IssueDate >= startDate && e.Remittances.IssueDate <= endDate);
                }

                if (exceptionStatus.Count > 0)
                {
                    records = records.Where(e => !exceptionStatus.Contains(e.Remittances.Status));
                }

                return records.Where(e => e.Remittances.IsDeleted == "N").OrderByDescending(e => e.Remittances.CreatedOn).ToPagedList(page, pageSize);
            }
            catch
            {
                throw;
            }
        }

        public RemittanceOrders GetSingle(int id)
        {
            try
            {
                IQueryable<RemittanceOrders> records = Select();

                return records.Where(e => e.ID == id).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public bool Add(RemittanceOrders addData)
        {
            try
            {
                db.RemittanceOrders.Add(addData);

                db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Update(int id, RemittanceOrders updateData)
        {
            try
            {
                RemittanceOrders data = db.RemittanceOrders.Find(id);

                data.Fee = updateData.Fee;
                data.GetAmount = updateData.GetAmount;
                data.GetCurrency = updateData.GetCurrency;
                data.GetPaymentType = updateData.GetPaymentType;
                data.PayAmount = updateData.PayAmount;
                data.PayCurrency = updateData.PayCurrency;
                data.PayDepositAccount = updateData.PayDepositAccount;
                data.PayPaymentType = updateData.PayPaymentType;
                data.Rate = updateData.Rate;
                data.BankTransferNo = updateData.BankTransferNo;
                data.ChequeNo = updateData.ChequeNo;
                data.BeneficiaryBankAccountNo = updateData.BeneficiaryBankAccountNo;
                data.BeneficiaryBankAddress = updateData.BeneficiaryBankAddress;
                data.BeneficiaryBankCode = updateData.BeneficiaryBankCode;
                data.BeneficiaryBankCountry = updateData.BeneficiaryBankCountry;
                data.BeneficiaryCategoryOfBusiness = updateData.BeneficiaryCategoryOfBusiness;
                data.BeneficiaryCompanyContactNo = updateData.BeneficiaryCompanyContactNo;
                data.BeneficiaryCompanyRegistrationNo = updateData.BeneficiaryCompanyRegistrationNo;
                data.BeneficiaryFullName = updateData.BeneficiaryFullName;
                data.BeneficiaryNationality = updateData.BeneficiaryNationality;
                data.BeneficiaryPaymentDetails = updateData.BeneficiaryPaymentDetails;
                data.BeneficiaryPurposeOfPayment = updateData.BeneficiaryPurposeOfPayment;
                data.BeneficiarySourceOfPayment = updateData.BeneficiarySourceOfPayment;
                data.BeneficiaryType = updateData.BeneficiaryType;
                data.BeneficiaryUploadSupportingFile = updateData.BeneficiaryUploadSupportingFile;
                data.BeneficiaryUploadSupportingType = updateData.BeneficiaryUploadSupportingType;

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
                RemittanceOrders data = db.RemittanceOrders.Find(id);

                db.RemittanceOrders.Remove(data);

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
                var record = db.RemittanceOrders.Where(e => e.RemittanceId == id).ToList();


                if(record.Count > 0)
                {
                    db.RemittanceOrders.RemoveRange(record);
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

        public IList<RemittanceOrders> GetEndDayTradeTransactions(int productId, DateTime from, DateTime to, string transactionType, List<string> acceptStatus)
        {
            IQueryable<RemittanceOrders> records = Select().Where(e => e.GetCurrency == productId);

            //if (!string.IsNullOrEmpty(transactionType))
            //{
            //    records = records.Where(e => e.TransactionType == transactionType);
            //}

            if (acceptStatus.Count > 0)
            {
                records = records.Where(e => acceptStatus.Contains(e.Remittances.Status));
            }

            return records.Where(e => e.Remittances.LastApprovalOn >= from && e.Remittances.LastApprovalOn <= to).ToList();
        }
    }
}