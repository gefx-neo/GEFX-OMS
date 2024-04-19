using DataAccess;
using DataAccess.POCO;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private DataAccess.GreatEastForex db;

        public AuditLogRepository()
        {
            db = new DataAccess.GreatEastForex();
        }

        public IQueryable<AuditLog> Select()
        {
            var result = from a in db.AuditLogs select a;

            return result;
        }

        public IList<AuditLog> GetAll()
        {
            try
            {
                IQueryable<AuditLog> records = Select();

                return records.OrderByDescending(e => e.Timestamp).ToList();
            }
            catch
            {
                throw;
            }
        }

        public IList<AuditLog> GetAll(string tableName, string startDate, string endDate)
        {
            try
            {
                IQueryable<AuditLog> records = Select();

                if (!string.IsNullOrEmpty(tableName))
                {
                    records = records.Where(e => e.TableAffected == tableName);
                }

                if (!string.IsNullOrEmpty(startDate))
                {
                    DateTime start = Convert.ToDateTime(startDate + " 00:00:00");

                    records = records.Where(e => e.Timestamp >= start);
                }

                if (!string.IsNullOrEmpty(endDate))
                {
                    DateTime end = Convert.ToDateTime(endDate + " 23:59:59");

                    records = records.Where(e => e.Timestamp <= end);
                }

                return records.OrderByDescending(e => e.Timestamp).ToList();
            }
            catch
            {
                throw;
            }
        }

        public IPagedList<AuditLog> GetPaged(string tableName, string startDate, string endDate, int page, int size)
        {
            try
            {
                IQueryable<AuditLog> records = Select();

                if (!string.IsNullOrEmpty(tableName))
                {
                    records = records.Where(e => e.TableAffected == tableName);
                }

                if (!string.IsNullOrEmpty(startDate))
                {
                    DateTime start = Convert.ToDateTime(startDate + " 00:00:00");

                    records = records.Where(e => e.Timestamp >= start);
                }

                if (!string.IsNullOrEmpty(endDate))
                {
                    DateTime end = Convert.ToDateTime(endDate + " 23:59:59");

                    records = records.Where(e => e.Timestamp <= end);
                }

                return records.OrderByDescending(e => e.Timestamp).ToPagedList(page, size);
            }
            catch
            {
                throw;
            }
        }

        public IList<AuditLog> GetUniqueTable()
        {
            try
            {
                IQueryable<AuditLog> records = Select();

                records = records.GroupBy(g => g.TableAffected).Select(group => group.FirstOrDefault());

                return records.OrderBy(e => e.TableAffected).ToList();
            }
            catch
            {
                throw;
            }
        }

        public bool Add(AuditLog addData)
        {
            try
            {
                addData.Timestamp = DateTime.Now;

                db.AuditLogs.Add(addData);

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