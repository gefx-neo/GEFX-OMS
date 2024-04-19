using DataAccess.POCO;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public interface IRemittanceOrderRepository
    {
        IList<RemittanceOrders> GetSaleTransactions(int saleid);

        int GetProductTransactions(int productId, string date, string transactionType, List<string> exceptionStatus);

        IList<RemittanceOrders> GetProductTransactions(int productId, string fromDate, string toDate, string transactionType, List<string> exceptionStatus);

        IPagedList<RemittanceOrders> GetTransactionsPaged(string date, List<string> exceptionStatus, int page, int pageSize);

        RemittanceOrders GetSingle(int id);

        bool Add(RemittanceOrders addData);

        bool Update(int id, RemittanceOrders updateData);

        bool Delete(int id);

        bool Delete2(int id);

        IList<RemittanceOrders> GetEndDayTradeTransactions(int productId, DateTime from, DateTime to, string transactionType, List<string> acceptStatus);
    }
}