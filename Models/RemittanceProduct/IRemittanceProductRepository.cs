using DataAccess.POCO;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public interface IRemittanceProductRepository
	{
		IList<RemittanceProducts> GetAll();

		RemittanceProducts FindCurrencyCode(string CurrencyCode);

		RemittanceProducts FindCurrencyCodeNotOwnSelf(int id, string CurrencyCode);

		RemittanceProducts GetSingle(int id);

		IPagedList<RemittanceProducts> GetPaged(string keyword, int page, int size);

		bool Add(RemittanceProducts addData);

		bool Update(int id, RemittanceProducts updateData);

		bool Delete(int id);
	}
}