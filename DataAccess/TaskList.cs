using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAccess
{
    public class TaskList
    {
        public int ID { get; set; }

        public string Task { get; set; }

        public string ReferenceNo { get; set; }

        public string Name { get; set; }

        public DateTime? CollectionDate { get; set; }

        public string CollectionTime { get; set; }

        public string TransactionType { get; set; }

        public string Type { get; set; }

        public string Vessel { get; set; }

        public string Amount { get; set; }

        public string Status { get; set; }

        public string Urgent { get; set; }

        public string UrgentClass { get; set; }

        public List<SaleTransaction> SaleTransactions { get; set; }
        public List<RemittanceOrders> RemittanceOrders { get; set; }
    }
}