using DataAccess;
using DataAccess.POCO;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public class EmailLogRepository : IEmailLogRepository
    {
        private DataAccess.GreatEastForex db;

        public EmailLogRepository()
        {
            db = new DataAccess.GreatEastForex();
        }

        public IQueryable<EmailLog> Select()
        {
            var results = from a in db.EmailLogs
                          orderby a.Timestamp descending
                          select a;

            return results;
        }

        public IList<EmailLog> GetAll(List<string> emailTypes)
        {
            try
            {
                var records = Select();

                return records.Where(e => emailTypes.Contains(e.EmailType)).ToList();
            }
            catch
            {
                throw;
            }
        }

        public IPagedList<EmailLog> GetPaged(List<string> emailTypes, string keyword, int page, int pageSize)
        {
            try
            {
                var records = Select();

                if (!string.IsNullOrEmpty(keyword))
                {
                    records = records.Where(e => e.ReceiverEmail.Contains(keyword) || e.Subject.Contains(keyword));
                }

                return records.Where(e => emailTypes.Contains(e.EmailType)).ToPagedList(page, pageSize);
            }
            catch
            {
                throw;
            }
        }

        public IList<EmailLog> GetAll(List<string> emailTypes, int saleId)
        {
            try
            {
                var records = Select();

                return records.Where(e => emailTypes.Contains(e.EmailType) && e.SaleId == saleId).ToList();
            }
            catch
            {
                throw;
            }
        }

        public IPagedList<EmailLog> GetPaged(List<string> emailTypes, string keyword, int saleId, int page, int pageSize)
        {
            try
            {
                var records = Select();

                if (!string.IsNullOrEmpty(keyword))
                {
                    records = records.Where(e => e.ReceiverEmail.Contains(keyword) || e.Subject.Contains(keyword));
                }

                return records.Where(e => emailTypes.Contains(e.EmailType) && e.SaleId == saleId).ToPagedList(page, pageSize);
            }
            catch
            {
                throw;
            }
        }

        public bool Add(EmailLog addData)
        {
            try
            {
                db.EmailLogs.Add(addData);

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