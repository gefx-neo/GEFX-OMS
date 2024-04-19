using DataAccess.POCO;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public interface ITemp_CustomerParticularRepository
	{
		IList<Temp_CustomerParticulars> GetAll();

		IList<Temp_CustomerParticulars> GetAllCustomers(string from, string to);

		IList<Temp_CustomerParticulars> GetAllByStatus(string status);

		int GetAllByStatusCount(string status);

		Temp_CustomerParticulars GetSingle(int id);

		Temp_CustomerParticulars GetSingle2(int id);

		IPagedList<GetAllCustomerActiveList> GetPaged(string keyword, int page, int size, string searchTag);

		IPagedList<Temp_CustomerParticulars> GetCustomerDataPaged(string from, string to, int page, int size);

		Temp_CustomerParticulars FindCustomerCode(string code);

		Temp_CustomerParticulars FindICPassport(string icpassport);

		Temp_CustomerParticulars FindEmail(string email);

		bool Add(CustomerParticular addData);

		bool Update(int id, CustomerParticular updateData);

		bool Delete(int id);

		bool UpdateEmergency(CustomerParticular updateData);
	}
}