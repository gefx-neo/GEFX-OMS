using DataAccess;
using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public class CustomerCustomRateRepository : ICustomerCustomRateRepository
    {
        private DataAccess.GreatEastForex db;

        public CustomerCustomRateRepository()
        {
            db = new DataAccess.GreatEastForex();
        }

        public IQueryable<CustomerCustomRate> Select()
        {
            var result = from c in db.CustomerCustomRates select c;

            return result;
        }

        public CustomerCustomRate GetSingle(int id)
        {
            try
            {
                IQueryable<CustomerCustomRate> records = Select();

                return records.Where(e => e.ID == id).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public IList<CustomerCustomRate> GetCustomRates(int CustomerParticularId)
        {
            try
            {
                IQueryable<CustomerCustomRate> records = Select();

                return records.Where(e => e.CustomerParticularId == CustomerParticularId).ToList();
            }
            catch
            {
                throw;
            }
        }

        public CustomerCustomRate GetCustomerProductRate(int CustomerParticularId, int productid)
        {
            try
            {
                IQueryable<CustomerCustomRate> records = Select();

                return records.Where(e => e.CustomerParticularId == CustomerParticularId && e.ProductId == productid).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public bool Add(CustomerCustomRate addData)
        {
            try
            {
                db.CustomerCustomRates.Add(addData);

                db.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

		public bool AddReject(IList<Temp_CustomerCustomRates> addData)
		{
			try
			{
				CustomerCustomRate rate = new CustomerCustomRate();
				List<CustomerCustomRate> ratelist = new List<CustomerCustomRate>();

				foreach (Temp_CustomerCustomRates temp in addData)
				{
					rate.BuyRate = temp.BuyRate;
					rate.CustomerParticularId = temp.CustomerParticularId;
					rate.EncashmentRate = temp.EncashmentRate;
					rate.ProductId = temp.ProductId;
					rate.SellRate = temp.SellRate;

					ratelist.Add(rate);
					rate = new CustomerCustomRate();
				}

				db.CustomerCustomRates.AddRange(ratelist);

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
                CustomerCustomRate data = db.CustomerCustomRates.Find(id);

                db.CustomerCustomRates.Remove(data);

                db.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

		public bool DeleteAll(int id)
		{
			try
			{
				db.CustomerCustomRates.Where(e => e.CustomerParticularId == id).ToList().ForEach(ccr => db.CustomerCustomRates.Remove(ccr));
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