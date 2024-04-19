using DataAccess.Report;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public class CurrencyClosingBalanceRepository : ICurrencyClosingBalanceRepository
    {
        public IPagedList<CurrencyClosingBalance> GetPaged(List<CurrencyClosingBalance> list, int page, int pageSize)
        {
            try
            {
                return list.OrderBy(e => e.Code).ToPagedList(page, pageSize);
            }
            catch
            {
                throw;
            }
        }

        public IList<CurrencyClosingBalance> GetAll(List<CurrencyClosingBalance> list)
        {
            try
            {
                return list.OrderBy(e => e.Code).ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}