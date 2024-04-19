using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public interface ICustomerScreeningReportRepository
    {
        IList<CustomerScreeningReport> GetAll(int customerParticularId);

        CustomerScreeningReport GetSingle(int id);

        bool Add(CustomerScreeningReport addData);

		bool AddReject(IList<Temp_CustomerScreeningReports> addData);

		bool Delete(int id);

		bool DeleteAll(int id);
	}
}