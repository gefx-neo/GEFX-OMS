using DataAccess.POCO;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public interface ICustomerParticularRepository
    {
        IList<CustomerParticular> GetAll();

        IList<CustomerParticular> GetAllCustomers(string from, string to);

        IList<CustomerParticular> GetAllByStatus(string status);

		List<CustomerParticular> GetAllMainCustomer();

		int GetAllByStatusCount(string status);

        CustomerParticular GetSingle(int id);

        CustomerParticular GetSingle2(int id);

		CustomerParticular GetSingle3(int id);

		IPagedList<GetAllCustomerActiveList> GetPaged(string keyword, int page, int size, string searchTag);

        IPagedList<CustomerParticular> GetCustomerDataPaged(string from, string to, int page, int size);

        CustomerParticular FindCustomerCode(string code);

        CustomerParticular FindICPassport(string icpassport);

        CustomerParticular FindEmail(string email);

		int FindEmailNotOwn(int id, string email);


		bool Add(CustomerParticular addData);

        bool Update(int id, CustomerParticular updateData);

		bool UpdateApproveKYC(int id);

		bool UpdateEmergency(CustomerParticular updateData);

		bool UpdateReject(int id, Temp_CustomerParticulars updateData);

		bool UpdateApproveKYC(int id, KYC_CustomerParticulars updateData);

		bool UpdateRejectZeroKYC(int id, Temp_CustomerParticulars updateData, string status = "");

		bool Delete(int id);

		bool UpdateStatus(int id, int lastTempPageUpdate = 0);
    }
}