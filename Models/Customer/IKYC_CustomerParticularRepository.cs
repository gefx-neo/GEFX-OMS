using DataAccess.POCO;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public interface IKYC_CustomerParticularRepository
	{
		IList<KYC_CustomerParticulars> GetAll();

		IList<KYC_CustomerParticulars> GetAllCustomers(string from, string to);

		IList<KYC_CustomerParticulars> GetAllByStatus(string status);

		int GetAllByStatusCount(string status);

		KYC_CustomerParticulars GetSingle(int id);

		KYC_CustomerParticulars GetSingle2(int id);

		IPagedList<GetAllCustomerActiveList> GetPaged(string keyword, int page, int size, string searchTag);

		IPagedList<KYC_CustomerParticulars> GetCustomerDataPaged(string from, string to, int page, int size);

		KYC_CustomerParticulars FindCustomerCode(string code);

		KYC_CustomerParticulars FindICPassport(string icpassport);

		KYC_CustomerParticulars FindEmail(string email);

		bool Add(CustomerParticular addData);

		bool Update(int id, CustomerParticular updateData);

		bool Delete(int id);
	}
}