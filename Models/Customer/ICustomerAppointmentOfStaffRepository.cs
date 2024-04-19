using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public interface ICustomerAppointmentOfStaffRepository
    {
        CustomerAppointmentOfStaff GetSingle(int id);

        IList<CustomerAppointmentOfStaff> GetAppointments(int CustomerParticularId);

        bool Add(CustomerAppointmentOfStaff addData);

		bool Update(int id, CustomerAppointmentOfStaff addData);

		bool AddReject(IList<Temp_CustomerAppointmentOfStaffs> addData);

		bool Delete(int id);

		bool DeleteAll(int id);

	}
}