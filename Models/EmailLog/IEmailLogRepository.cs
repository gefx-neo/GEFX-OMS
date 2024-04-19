using DataAccess.POCO;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public interface IEmailLogRepository
    {
        IList<EmailLog> GetAll(List<string> emailTypes);

        IPagedList<EmailLog> GetPaged(List<string> emailTypes, string keyword, int page, int pageSize);

        IList<EmailLog> GetAll(List<string> emailTypes, int saleId);

        IPagedList<EmailLog> GetPaged(List<string> emailTypes, string keyword, int saleId, int page, int pageSize);

        bool Add(EmailLog addData);
    }
}