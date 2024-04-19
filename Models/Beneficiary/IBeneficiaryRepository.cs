using DataAccess.POCO;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public interface IBeneficiaryRepository
    {
		IList<Beneficiaries> GetAll();

        Beneficiaries GetSingle(int id);

        //Beneficiaries FindAgentId(string AgentId);

        //Beneficiaries FindAgentIdNotOwnSelf(int id, string AgentId);

		IPagedList<Beneficiaries> GetPaged(string keyword, int page, int size);

		bool Add(Beneficiaries addData);

		bool Update(int id, Beneficiaries updateData);

		bool Delete(int id);


		//This section is for Beneficiaries Controller used
		IPagedList<Beneficiaries> GetPagedBeneficiaries(string keyword, int page, int size);

		bool DeleteBeneficiaries(int id);

		Beneficiaries GetSingleBeneficiaries(int id);
	}
}