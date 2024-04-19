using DataAccess;
using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public class EndDayTradeRepository : IEndDayTradeRepository
    {
        private DataAccess.GreatEastForex db;

        public EndDayTradeRepository()
        {
            db = new DataAccess.GreatEastForex();
        }

        public IQueryable<EndDayTrade> Select()
        {
            var result = from t in db.EndDayTrades where t.IsDeleted == "N" select t;

            return result;
        }

        public IQueryable<EndDayTrade> SelectAll()
        {
            var result = from t in db.EndDayTrades select t;

            return result;
        }

        public IList<EndDayTrade> GetAll(string activationTime)
        {
            try
            {
                IQueryable<EndDayTrade> records = Select();

                if (!string.IsNullOrEmpty(activationTime))
                {
                    DateTime start = Convert.ToDateTime(activationTime + " 00:00:00");
                    DateTime end = Convert.ToDateTime(activationTime + " 23:59:59.9999999");

                    records = records.Where(e => e.CurrentActivationTime >= start && e.CurrentActivationTime <= end);
                }

                return records.ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<EndDayTrade> GetAll(DateTime lastApproval)
        {
            try
            {
                IQueryable<EndDayTrade> records = Select();

                return records.Where(e => e.LastActivationTime >= lastApproval && e.CurrentActivationTime <= lastApproval).ToList();
            }
            catch
            {
                throw;
            }
        }

        public IList<EndDayTrade> GetAllDAT(string fromDate, string toDate)
        {
            try
            {
                IQueryable<EndDayTrade> records = Select();

                DateTime start = Convert.ToDateTime(fromDate + " 00:00:00.0000000");
                DateTime end = Convert.ToDateTime(toDate + " 23:59:59.99999999");

                return records.Where(e => e.CurrentActivationTime >= start && e.CurrentActivationTime <= end).ToList();
            }
            catch
            {
                throw;
            }
        }

        public EndDayTrade GetProductTrade(int productId, string activationTime = null)
        {
            try
            {
                IQueryable<EndDayTrade> records = Select();

                if (!string.IsNullOrEmpty(activationTime))
                {
                    DateTime start = Convert.ToDateTime(activationTime + " 00:00:00");
                    DateTime end = Convert.ToDateTime(activationTime + " 23:59:59.99999999");

                    records = records.Where(e => e.CurrentActivationTime >= start && e.CurrentActivationTime <= end);
                }

                return records.Where(e => e.CurrencyId == productId).OrderByDescending(e => e.CurrentActivationTime).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public EndDayTrade GetLastRecord()
        {
            try
            {
                IQueryable<EndDayTrade> records = Select();

                return records.OrderByDescending(e => e.CurrentActivationTime).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public EndDayTrade GetProductPreviousTrade(int productId, DateTime lastActivationTime)
        {
            try
            {
                var records = Select();

                return records.Where(e => e.CurrencyId == productId && e.CurrentActivationTime <= lastActivationTime).OrderByDescending(e => e.CurrentActivationTime).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public EndDayTrade GetProductLastTrade(int productId, string activationDate = null)
        {
            try
            {
                var records = Select();

                if (!string.IsNullOrEmpty(activationDate))
                {
                    DateTime a = Convert.ToDateTime(activationDate);

                    records = records.Where(e => e.CurrentActivationTime < a);
                }

                return records.Where(e => e.CurrencyId == productId).OrderByDescending(e => e.CurrentActivationTime).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public EndDayTrade GetProductNextTrade(int productId, DateTime currentActivation)
        {
            try
            {
                var records = Select();

                return records.Where(e => e.CurrencyId == productId && e.LastActivationTime == currentActivation).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public EndDayTrade GetProductCurrentTrade(int productId, DateTime lastApproval)
        {
            try
            {
                var records = Select();

                return records.Where(e => e.CurrencyId == productId && e.LastActivationTime <= lastApproval && e.CurrentActivationTime >= lastApproval).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public EndDayTrade GetProductFutureTrade(int productId, DateTime currentActivation)
        {
            try
            {
                //var records = Select();
                var records = db.EndDayTrades.Where(e => e.IsDeleted == "N");
                return records.Where(e => e.CurrencyId == productId && e.LastActivationTime >= currentActivation).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public bool Add(EndDayTrade addData)
        {
            try
            {
                addData.IsDeleted = "N";

                db.EndDayTrades.Add(addData);

                db.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool Update(int id, EndDayTrade updateData)
        {
            try
            {
                EndDayTrade data = db.EndDayTrades.Find(id);

                data.OpeningBankAmount = updateData.OpeningBankAmount;
                data.OpeningCashAmount = updateData.OpeningCashAmount;
                data.OpeningForeignCurrencyBalance = updateData.OpeningForeignCurrencyBalance;
                data.OpeningAveragePurchaseCost = updateData.OpeningAveragePurchaseCost;
                data.OpeningBalanceAtAveragePurchase = updateData.OpeningBalanceAtAveragePurchase;
                data.OpeningProfitAmount = updateData.OpeningProfitAmount;
                data.ClosingBankAmount = updateData.ClosingBankAmount;
                data.ClosingCashAmount = updateData.ClosingCashAmount;
                data.ClosingForeignCurrencyBalance = updateData.ClosingForeignCurrencyBalance;
                data.ClosingAveragePurchaseCost = updateData.ClosingAveragePurchaseCost;
                data.ClosingBalanceAtAveragePurchase = updateData.ClosingBalanceAtAveragePurchase;
                data.ClosingProfitAmount = updateData.ClosingProfitAmount;
                data.Description = updateData.Description;

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
                EndDayTrade data = db.EndDayTrades.Find(id);

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