using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public interface IEndDayTradeTransactionRepository
    {
        EndDayTradeTransaction GetSingle(int id);

        EndDayTradeTransaction GetSingle(int tradeId, int saleTransactionId);

        List<EndDayTradeTransaction> GetAll(int tradeId);

        List<EndDayTradeTransaction> GetAll(List<int> saleTransactionIds, DateTime lastApproval);

        EndDayTradeTransaction GetSingle(int saleTransactionId, DateTime lastApproval);

        bool Add(EndDayTradeTransaction addData);

        bool Delete(int id);
    }
}