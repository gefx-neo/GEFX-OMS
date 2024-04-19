using DataAccess;
using DataAccess.POCO;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public class RemittanceProductRepository : IRemittanceProductRepository
	{
		private DataAccess.GreatEastForex db;

		public RemittanceProductRepository()
		{
			db = new DataAccess.GreatEastForex();
		}

		public IQueryable<RemittanceProducts> Select()
		{
			var result = from u in db.RemittanceProducts select u;

			return result.Where(e => e.IsDeleted == "N");
		}

		public IQueryable<RemittanceProducts> SelectAll()
		{
			var result = from u in db.RemittanceProducts select u;

			return result;
		}

		public IList<RemittanceProducts> GetAll()
		{
			try
			{
				IQueryable<RemittanceProducts> records = Select();

				return records.OrderByDescending(e => e.CreatedOn).ToList();
			}
			catch
			{
				throw;
			}
		}

		public RemittanceProducts GetSingle(int id)
		{
			try
			{
				IQueryable<RemittanceProducts> records = Select();

				return records.Where(e => e.ID == id).FirstOrDefault();
			}
			catch
			{
				throw;
			}
		}

		public RemittanceProducts FindCurrencyCode(string CurrencyCode)
		{
			try
			{
				IQueryable<RemittanceProducts> records = Select();

				return records.Where(e => e.CurrencyCode == CurrencyCode).FirstOrDefault();
			}
			catch
			{
				throw;
			}
		}

		public RemittanceProducts FindCurrencyCodeNotOwnSelf(int id, string CurrencyCode)
		{
			try
			{
				IQueryable<RemittanceProducts> records = Select();

				return records.Where(e => e.CurrencyCode == CurrencyCode && e.ID != id).FirstOrDefault();
			}
			catch
			{
				throw;
			}
		}

		public IPagedList<RemittanceProducts> GetPaged(string keyword, int page, int size)
		{
			try
			{
				IQueryable<RemittanceProducts> records = Select();

				if (!string.IsNullOrEmpty(keyword))
				{
					records = records.Where(e => e.CurrencyCode.Contains(keyword) || e.CurrencyName.Contains(keyword));
				}

				return records.OrderByDescending(e => e.CreatedOn).ToPagedList(page, size);
			}
			catch
			{
				throw;
			}
		}

		public bool Add(RemittanceProducts addData)
		{
			try
			{
				addData.CreatedOn = DateTime.Now;
				addData.UpdatedOn = DateTime.Now;
				addData.IsDeleted = "N";

				db.RemittanceProducts.Add(addData);

				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool Update(int id, RemittanceProducts updateData)
		{
			try
			{
				RemittanceProducts data = db.RemittanceProducts.Find(id);

				data.AcceptableRange = updateData.AcceptableRange;
				data.CurrencyCode = updateData.CurrencyCode;
				data.CurrencyName = updateData.CurrencyName;
				data.GetRate = updateData.GetRate;
				data.GuaranteeRates = updateData.GuaranteeRates;
				data.MaxAmount = updateData.MaxAmount;
				data.PaymentModeAllowed = updateData.PaymentModeAllowed;
				data.PayRate = updateData.PayRate;
				data.PopularCurrencies = updateData.PopularCurrencies;
				data.ProductDecimal = updateData.ProductDecimal;
				data.ProductSymbol = updateData.ProductSymbol;
				data.TransactionFee = updateData.TransactionFee;
				data.BuyRateAdjustment = updateData.BuyRateAdjustment;
				data.SellRateAdjustment = updateData.SellRateAdjustment;
				data.TransactionTypeAllowed = updateData.TransactionTypeAllowed;
				data.Status = updateData.Status;
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
				RemittanceProducts data = db.RemittanceProducts.Find(id);

				if (data.IsBaseProduct == 0)
				{
					data.IsDeleted = "Y";
					data.UpdatedOn = DateTime.Now;

					db.SaveChanges();
					return true;
				}
				else
				{
					return false;
				}
			}
			catch
			{
				throw;
			}
		}
	}
}