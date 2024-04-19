using DataAccess.Report;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public interface ICurrencyClosingBalanceRepository
    {
        IPagedList<CurrencyClosingBalance> GetPaged(List<CurrencyClosingBalance> list, int page, int pageSize);

        IList<CurrencyClosingBalance> GetAll(List<CurrencyClosingBalance> list);
    }
}