using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public interface ITemp_CustomerAppointmentOfStaffRepository
	{
		Temp_CustomerAppointmentOfStaffs GetSingle(int id);

		IList<Temp_CustomerAppointmentOfStaffs> GetAppointments(int CustomerParticularId);

		bool Add(List<CustomerAppointmentOfStaff> addData);

		bool AddSingle(Temp_CustomerAppointmentOfStaffs addData);

		bool Delete(int id);

		bool DeleteAll(int id);
	}
}