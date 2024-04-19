using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAccess
{
	public class SaleTransactionsModel
	{
		public int SaleTransaction_ID { get; set; }

		public int Sale_ID { get; set; }

		public string SaleTransaction_TransactionType { get; set; }

		public int SaleTransaction_CurrencyID { get; set; }

	}
}