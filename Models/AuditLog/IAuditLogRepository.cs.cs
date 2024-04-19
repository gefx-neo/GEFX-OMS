using DataAccess.POCO;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public interface IAuditLogRepository
    {
        IList<AuditLog> GetAll();

        IList<AuditLog> GetAll(string tableName, string startDate, string endDate);

        IPagedList<AuditLog> GetPaged(string tableName, string startDate, string endDate, int page, int size);

        IList<AuditLog> GetUniqueTable();

        bool Add(AuditLog addData);
    }
}