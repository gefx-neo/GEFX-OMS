using DataAccess.POCO;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public interface ISaleRepository
    {
        IList<Sale> GetAll();

        IList<Sale> GetAllSales(string from, string to);

        IList<Sale> GetTodaySales(string status = "");

        IList<Sale> GetWeeklySales(string from, string to);

        Sale GetSingle(int id);

		Sale GetSingleJuniorDealer(int id, int userid);

		Sale GetSingle2(int id);

        Sale GetSingle(string memoID);

        Sale GetLastRecord();

        IList<Sale> GetUserSales(List<string> status, int userId, string isOpsExec = null);

		//get First week record
		IList<Sale> GetOneWeekRecord(string firstDate, string keyword, string status, string date = "");

        int GetUserSalesCount(List<string> status, int userId);

		int GetUserSalesCount2(List<string> status, int userId, string isOpsExec = null, string isOpsManager = null);

		IPagedList<Sale> GetPaged(string keyword, string status, int page, int size, string date = "");

        IList<Sale> GetAllSales2(string keyword, string status, string date = "");

        IPagedList<Sale> GetPaged(string keyword, string status, string fromDate, string toDate, int page, int size);

        IPagedList<Sale> GetSaleDataPaged(string from, string to, int page, int size);

        IPagedList<Sale> GetSalesByDatePaged(string fromDate, string toDate, List<string> exceptionStatus, List<string> acceptStatus, int page, int pageSize, List<int> productIds, int cid = 0, string transactionType = null);

        decimal[] GetReportGrandTotal(string fromDate, string toDate, List<string> exceptionStatus, List<string> acceptStatus, List<int> productIds, int cid = 0, string transactionType = null);

        IList<Sale> GetSalesByYear(int year);

        IList<Sale> GetSaleDataByDate(string fromDate, string toDate, List<string> exceptionStatus, List<string> acceptStatus, List<int> productIds, int cid = 0, string transactionType = null);

        IList<Sale> GetSalesByDateRange(string fromDate, string toDate);

        List<Sale> GetPendingDeleteSales(DateTime from, DateTime to, string transactionType);

        bool Add(Sale addData);

        bool Update(int id, Sale updateData);

        bool UpdateStatus(int id, string status, int deliveryManId);

        bool UpdateDeleteStatus(int id, string status, int deliveryManId);

        bool UpdateStatus(int id, string status);

        bool DisapproveSale(int id, Sale updateData);

        bool Delete(int id);

        Sale GetEarliestSales(List<string> acceptStatus);

        int GetPageCount(string keyword, string status);

		int GetPageCount2(string keyword, string status, bool isMultipleRole, int userid);
		IEnumerable<Sale> GetCustomItems(int page, string keyword, string status);

		IEnumerable<Sale> GetCustomItems2(int page, string keyword, string status, bool isMultipleRole, int userid);

		IPagedList<Sale> GetSaleLastApprovedDataPaged(string from, string to, int page, int size);

        IList<Sale> GetSaleLastApprovedDataByDate(string fromDate, string toDate, List<string> exceptionStatus, List<string> acceptStatus, List<int> productIds, int cid = 0, string transactionType = null);
        IList<Sale> GetSalesSummaryList(string fromDate, string toDate, List<int> productIds, List<string> transactionType, List<int> CustomerList);

        IPagedList<Sale> GetLastApprovedSalesByDatePaged(string fromDate, string toDate, List<string> exceptionStatus, List<string> acceptStatus, int page, int pageSize, List<int> productIds, int cid = 0, string transactionType = null);

        decimal[] GetLastApprovedReportGrandTotal(string fromDate, string toDate, List<string> exceptionStatus, List<string> acceptStatus, List<int> productIds, int cid = 0, string transactionType = null);

    }
}