using DataAccess.POCO;
using GreatEastForex.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Helper
{
    public class AuditLogHelper
    {
        public static bool WriteAuditLog(int userTriggering, string tableAffected, string description)
        {
            AuditLog auditLogs = new AuditLog();
            auditLogs.IpAddress = GetIPAddress();
			auditLogs.UserTriggering = userTriggering;
            auditLogs.TableAffected = tableAffected;
            auditLogs.Description = description;

            AuditLogRepository _auditLogsModel = new AuditLogRepository();
            bool result = _auditLogsModel.Add(auditLogs);

            return result;
        }

		public static string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }
    }
}