using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public interface ITemp_CustomerScreeningReportRepository
	{
		IList<Temp_CustomerScreeningReports> GetAll(int customerParticularId);

		Temp_CustomerScreeningReports GetSingle(int id);

		bool Add(List<CustomerScreeningReport> addData);

		bool AddSingle(Temp_CustomerScreeningReports addData);

		bool Delete(int id);

		bool DeleteAll(int id);
	}
}