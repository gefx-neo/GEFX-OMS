using DataAccess.POCO;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public interface IRemittanceSaleRepository
    {
        IList<Remittances> GetAll();

        IList<Remittances> GetAllSales(string from, string to);

        IList<Remittances> GetTodaySales(string status = "");

        IList<Remittances> GetWeeklySales(string from, string to);

        Remittances GetSingle(int id);

        Remittances GetSingleJuniorDealer(int id, int userid);

        Remittances GetSingle2(int id);

        Remittances GetSingle(string memoID);

        Remittances GetLastRecord();

        IList<Remittances> GetUserSales(List<string> status, int userId, string isOpsExec = null);

		//get First week record
		IList<Remittances> GetOneWeekRecord(string firstDate, string keyword, string status, string date = "");

        int GetUserSalesCount(List<string> status, int userId);

		int GetUserSalesCount2(List<string> status, int userId, string isOpsExec = null, string isOpsManager = null);

		IPagedList<Remittances> GetPaged(string keyword, string status, int page, int size, string date = "");

        IList<Remittances> GetAllSales2(string keyword, string status, string date = "");

        IPagedList<Remittances> GetPaged(string keyword, string status, string fromDate, string toDate, int page, int size);

        IPagedList<Remittances> GetSaleDataPaged(string from, string to, int page, int size);

        IPagedList<Remittances> GetSalesByDatePaged(string fromDate, string toDate, List<string> exceptionStatus, List<string> acceptStatus, int page, int pageSize, List<int> productIds, int cid = 0, string transactionType = null);

        IList<Remittances> GetSalesSummaryList(string fromDate, string toDate, List<int> productIds, List<string> transactionType, List<int> CustomerList);

        IList<Remittances> GetSalesSummaryList2(string fromDate, string toDate, List<int> productIds, List<string> transactionType, List<int> CustomerList, string reportStatus);
        IList<Remittances> GetSalesSummaryGetCurrencyList(string fromDate, string toDate, List<int> productIds, List<string> transactionType, List<int> CustomerList, string reportStatus);
        IList<Remittances> GetSalesSummaryList3(string fromDate, string toDate, List<int> productIds, List<string> transactionType, List<int> CustomerList, string reportStatus, List<int> CountriesList, string businessType, List<int> AgentList);

        //decimal[] GetReportGrandTotal(string fromDate, string toDate, List<string> exceptionStatus, List<string> acceptStatus, List<int> productIds, int cid = 0, string transactionType = null);

        IList<Remittances> GetSalesByYear(int year);

        IList<Remittances> GetSaleDataByDate(string fromDate, string toDate, List<string> exceptionStatus, List<string> acceptStatus, List<int> productIds, int cid = 0, string transactionType = null);

        IList<Remittances> GetSalesByDateRange(string fromDate, string toDate);

        List<Remittances> GetPendingDeleteSales(DateTime from, DateTime to, string transactionType);

        bool Add(Remittances addData);

        bool Update(int id, Remittances updateData);

        bool UpdateStatus(int id, string status, int deliveryManId);

        bool UpdateDeleteStatus(int id, string status, int deliveryManId);

        bool UpdateStatus(int id, string status);

        bool DisapproveSale(int id, Remittances updateData);

        bool Delete(int id);

        Remittances GetEarliestSales(List<string> acceptStatus);

        int GetPageCount(string keyword, string status);

		int GetPageCount2(string keyword, string status, bool isMultipleRole, int userid);
		IEnumerable<Remittances> GetCustomItems(int page, string keyword, string status);

		IEnumerable<Remittances> GetCustomItems2(int page, string keyword, string status, bool isMultipleRole, int userid);

		IPagedList<Remittances> GetSaleLastApprovedDataPaged(string from, string to, int page, int size);

        IList<Remittances> GetSaleLastApprovedDataByDate(string fromDate, string toDate, List<string> exceptionStatus, List<string> acceptStatus, List<int> productIds, int cid = 0, string transactionType = null);

        IPagedList<Remittances> GetLastApprovedSalesByDatePaged(string fromDate, string toDate, List<string> exceptionStatus, List<string> acceptStatus, int page, int pageSize, List<int> productIds, int cid = 0, string transactionType = null);

        //decimal[] GetLastApprovedReportGrandTotal(string fromDate, string toDate, List<string> exceptionStatus, List<string> acceptStatus, List<int> productIds, int cid = 0, string transactionType = null);

    }
}