using DataAccess;
using DataAccess.POCO;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public class ProductRepository : IProductRepository
    {
        private DataAccess.GreatEastForex db;

        public ProductRepository()
        {
            db = new DataAccess.GreatEastForex();
        }

        public IQueryable<Product> Select()
        {
            var result = from p in db.Products select p;

            return result.Where(e => e.IsDeleted == "N");
        }

        public IQueryable<Product> SelectAll()
        {
            var result = from p in db.Products select p;

            return result;
        }

        public IList<Product> GetAll()
        {
            try
            {
                var records = db.Products.Where(e => e.IsDeleted.Equals("N"));
                //IQueryable<Product> records = Select();

                return records.OrderBy(e => e.CurrencyCode).ToList();
            }
            catch
            {
                throw;
            }
        }

        public IList<Product> GetAll(string transactionType)
        {
            try
            {
                IQueryable<Product> records = Select();

                return records.Where(e => e.TransactionTypeAllowed.Contains(transactionType)).OrderBy(e => e.CurrencyCode).ToList();
            }
            catch
            {
                throw;
            }
        }

        public IList<Product> GetAll2(string transactionType)
        {
            try
            {
                //IQueryable<Product> records = Select();
                if (!string.IsNullOrEmpty(transactionType))
                {
                    var records = db.Products.Where(e => e.TransactionTypeAllowed.Contains(transactionType) && e.Status == "Active" && e.CurrencyCode != "SGD" && e.IsDeleted == "N").OrderBy(e => e.CurrencyCode).ToList();
                    return records;
                }
                else
                {
                    var records = db.Products.Where(e => e.Status == "Active" && e.CurrencyCode != "SGD" && e.IsDeleted == "N").OrderBy(e => e.CurrencyCode).ToList();
                    return records;
                }
                //return records.Where(e => e.TransactionTypeAllowed.Contains(transactionType)).OrderBy(e => e.CurrencyCode).ToList();
            }
            catch
            {
                throw;
            }
        }

        public IList<Product> GetAll(List<string> transactionType)
        {
            try
            {
                IQueryable<Product> records = Select();

                if (transactionType.Count > 0)
                {
                    switch (transactionType.Count)
                    {
                        case 1:
                            string a1 = transactionType[0];
                            records = records.Where(e => e.TransactionTypeAllowed.Contains(a1));
                            break;
                        case 2:
                            string a2 = transactionType[0];
                            string b2 = transactionType[1];
                            records = records.Where(e => e.TransactionTypeAllowed.Contains(a2) || e.TransactionTypeAllowed.Contains(b2));
                            break;
                        case 3:
                            string a3 = transactionType[0];
                            string b3 = transactionType[1];
                            string c3 = transactionType[2];
                            records = records.Where(e => e.TransactionTypeAllowed.Contains(a3) || e.TransactionTypeAllowed.Contains(b3) || e.TransactionTypeAllowed.Contains(c3));
                            break;
                        case 4:
                            string a4 = transactionType[0];
                            string b4 = transactionType[1];
                            string c4 = transactionType[2];
                            string d4 = transactionType[3];
                            records = records.Where(e => e.TransactionTypeAllowed.Contains(a4) || e.TransactionTypeAllowed.Contains(b4) || e.TransactionTypeAllowed.Contains(c4) || e.TransactionTypeAllowed.Contains(d4));
                            break;
                        case 5:
                            string a5 = transactionType[0];
                            string b5 = transactionType[1];
                            string c5 = transactionType[2];
                            string d5 = transactionType[3];
                            string e5 = transactionType[4];
                            records = records.Where(e => e.TransactionTypeAllowed.Contains(a5) || e.TransactionTypeAllowed.Contains(b5) || e.TransactionTypeAllowed.Contains(c5) || e.TransactionTypeAllowed.Contains(d5) || e.TransactionTypeAllowed.Contains(e5));
                            break;
                        default:
                            break;
                    }
                }
                return records.OrderBy(e => e.CurrencyCode).ToList();
            }
            catch
            {
                throw;
            }
        }

        public IList<Product> GetAllProducts(string from, string to)
        {
            try
            {
                IQueryable<Product> records = SelectAll();

                if (!string.IsNullOrEmpty(from))
                {
                    DateTime fromDate = Convert.ToDateTime(from + " 00:00:00");
                    records = records.Where(e => e.CreatedOn >= fromDate);
                }

                if (!string.IsNullOrEmpty(to))
                {
                    DateTime toDate = Convert.ToDateTime(to + " 23:59:59");
                    records = records.Where(e => e.CreatedOn <= toDate);
                }

                return records.OrderBy(e => e.CurrencyCode).ToList();
            }
            catch
            {
                throw;
            }
        }

        public Product GetSingle(int id)
        {
            try
            {
                //var result = from p in db.Products select p;

                //return result.Where(e => e.IsDeleted == "N");
                var records = db.Products.Where(e => e.IsDeleted == "N").ToList();
                //IQueryable<Product> records = Select();

                return records.Where(e => e.ID == id).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public IPagedList<Product> GetPaged(string keyword, int page, int size)
        {
            try
            {
                IQueryable<Product> records = Select();

                if (!string.IsNullOrEmpty(keyword))
                {
                    records = records.Where(e => e.CurrencyCode.Contains(keyword) || e.CurrencyName.Contains(keyword));
                }

                return records.OrderBy(e => e.CurrencyCode).ToPagedList(page, size);
            }
            catch
            {
                throw;
            }
        }

        public IPagedList<Product> GetProductDataPaged(string from, string to, int page, int size)
        {
            try
            {
                IQueryable<Product> records = Select();

                if (!string.IsNullOrEmpty(from))
                {
                    DateTime fromDate = Convert.ToDateTime(from + " 00:00:00");
                    records = records.Where(e => e.CreatedOn >= fromDate);
                }

                if (!string.IsNullOrEmpty(to))
                {
                    DateTime toDate = Convert.ToDateTime(to + " 23:59:59");
                    records = records.Where(e => e.CreatedOn <= toDate);
                }

                return records.OrderBy(e => e.CurrencyCode).ToPagedList(page, size);
            }
            catch
            {
                throw;
            }
        }

        public Product FindCurrencyCode(string code)
        {
            try
            {
                IQueryable<Product> records = Select();

                return records.Where(e => e.CurrencyCode == code).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public bool Add(Product addData)
        {
            try
            {
                addData.CreatedOn = DateTime.Now;
                addData.UpdatedOn = DateTime.Now;
                addData.IsDeleted = "N";

                db.Products.Add(addData);

                db.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool Update(int id, Product updateData)
        {
            try
            {
                Product data = db.Products.Find(id);

                data.CurrencyCode = updateData.CurrencyCode;
                data.CurrencyName = updateData.CurrencyName;
                data.BuyRate = updateData.BuyRate;
                data.SellRate = updateData.SellRate;
                data.EncashmentRate = updateData.EncashmentRate;
                data.Decimal = updateData.Decimal;
                data.Symbol = updateData.Symbol;
                data.AcceptableRange = updateData.AcceptableRange;
                data.Unit = updateData.Unit;
                data.PaymentModeAllowed = updateData.PaymentModeAllowed;
                data.TransactionTypeAllowed = updateData.TransactionTypeAllowed;
				data.MaxAmount = updateData.MaxAmount;
				data.GuaranteeRates = updateData.GuaranteeRates;
				data.PopularCurrencies = updateData.PopularCurrencies;
				data.BuyRateAdjustment = updateData.BuyRateAdjustment;
				data.SellRateAdjustment = updateData.SellRateAdjustment;
				data.TransactionTypeAllowedCustomer = updateData.TransactionTypeAllowedCustomer;
				data.EncashmentMapping = updateData.EncashmentMapping;
                data.Status = updateData.Status;
                data.UpdatedOn = DateTime.Now;

				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();
				db.Configuration.ValidateOnSaveEnabled = true;

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
                Product data = db.Products.Find(id);

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