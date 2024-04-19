using DataAccess.POCO;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public interface ISaleTransactionRepository
    {
        IList<SaleTransaction> GetSaleTransactions(int saleid);

        int GetProductTransactions(int productId, string date, string transactionType, List<string> exceptionStatus);

        IList<SaleTransaction> GetProductTransactions(int productId, string fromDate, string toDate, string transactionType, List<string> exceptionStatus);

        IPagedList<SaleTransaction> GetTransactionsPaged(string date, List<string> exceptionStatus, int page, int pageSize);

        SaleTransaction GetSingle(int id);

        bool Add(SaleTransaction addData);

        bool Update(int id, SaleTransaction updateData);

        bool Delete(int id);

        bool Delete2(int id);

        IList<SaleTransaction> GetEndDayTradeTransactions(int productId, DateTime from, DateTime to, string transactionType, List<string> acceptStatus);
    }
}