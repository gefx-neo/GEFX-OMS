using DataAccess;
using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public class EndDayTradeTransactionRepository : IEndDayTradeTransactionRepository
    {
        private DataAccess.GreatEastForex db;

        public EndDayTradeTransactionRepository()
        {
            db = new DataAccess.GreatEastForex();
        }

        public IQueryable<EndDayTradeTransaction> Select()
        {
            var result = from a in db.EndDayTradeTransactions
                         where a.EndDayTrade.IsDeleted == "N"
                         select a;

            return result;
        }

        public EndDayTradeTransaction GetSingle(int id)
        {
            try
            {
                var records = Select();

                return records.Where(e => e.ID == id).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public EndDayTradeTransaction GetSingle(int tradeId, int saleTransactionId)
        {
            try
            {
                var records = Select();

                return records.Where(e => e.TradeId == tradeId && e.SaleTransactionId == saleTransactionId).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public List<EndDayTradeTransaction> GetAll(int tradeId)
        {
            try
            {
                var records = Select();

                return records.Where(e => e.TradeId == tradeId).OrderBy(e => e.ID).ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<EndDayTradeTransaction> GetAll(List<int> saleTransactionIds, DateTime lastApproval)
        {
            try
            {
                var records = Select();

                return records.Where(e => saleTransactionIds.Contains(e.SaleTransactionId) && e.EndDayTrade.LastActivationTime <= lastApproval && e.EndDayTrade.CurrentActivationTime >= lastApproval).ToList();
            }
            catch
            {
                throw;
            }
        }

        public EndDayTradeTransaction GetSingle(int saleTransactionId, DateTime lastApproval)
        {
            try
            {
                var records = Select();

                return records.Where(e => e.SaleTransactionId == saleTransactionId && e.EndDayTrade.LastActivationTime <= lastApproval && e.EndDayTrade.CurrentActivationTime >= lastApproval).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public bool Add(EndDayTradeTransaction addData)
        {
            try
            {
                db.EndDayTradeTransactions.Add(addData);

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
                EndDayTradeTransaction data = db.EndDayTradeTransactions.Find(id);

                db.EndDayTradeTransactions.Remove(data);

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