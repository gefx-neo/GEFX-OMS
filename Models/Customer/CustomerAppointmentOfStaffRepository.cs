using DataAccess;
using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public class CustomerAppointmentOfStaffRepository : ICustomerAppointmentOfStaffRepository
    {
        private DataAccess.GreatEastForex db;

        public CustomerAppointmentOfStaffRepository()
        {
            db = new DataAccess.GreatEastForex();
        }

        public IQueryable<CustomerAppointmentOfStaff> Select()
        {
            var result = from c in db.CustomerAppointmentOfStaffs select c;

            return result;
        }

        public CustomerAppointmentOfStaff GetSingle(int id)
        {
            try
            {
                IQueryable<CustomerAppointmentOfStaff> records = Select();

                return records.Where(e => e.ID == id).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public IList<CustomerAppointmentOfStaff> GetAppointments(int CustomerParticularId)
        {
            try
            {
                IQueryable<CustomerAppointmentOfStaff> records = Select();

                return records.Where(e => e.CustomerParticularId == CustomerParticularId).ToList();
            }
            catch
            {
                throw;
            }
        }

        public bool Add(CustomerAppointmentOfStaff addData)
        {
            try
            {
                db.CustomerAppointmentOfStaffs.Add(addData);

                db.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

		public bool Update(int id, CustomerAppointmentOfStaff updateData)
		{
			try
			{
				CustomerAppointmentOfStaff data = db.CustomerAppointmentOfStaffs.Find(id);

				data.FullName = updateData.FullName;
				data.JobTitle = updateData.JobTitle;
				data.Nationality = updateData.Nationality;
				data.SpecimenSignature = updateData.SpecimenSignature;
				data.ICPassportNo = updateData.ICPassportNo;

				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool AddReject(IList<Temp_CustomerAppointmentOfStaffs> addData)
		{
			try
			{
				CustomerAppointmentOfStaff appointment = new CustomerAppointmentOfStaff();
				List<CustomerAppointmentOfStaff> appointmentlist = new List<CustomerAppointmentOfStaff>();

				foreach (Temp_CustomerAppointmentOfStaffs temp in addData)
				{
					appointment.CustomerParticularId = temp.CustomerParticularId;
					appointment.FullName = temp.FullName;
					appointment.ICPassportNo = temp.ICPassportNo;
					appointment.JobTitle = temp.JobTitle;
					appointment.Nationality = temp.Nationality;
					appointment.SpecimenSignature = temp.SpecimenSignature;

					appointmentlist.Add(appointment);
					appointment = new CustomerAppointmentOfStaff();
				}

				db.CustomerAppointmentOfStaffs.AddRange(appointmentlist);

				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool Delete(int id)
        {
            try
            {
                CustomerAppointmentOfStaff data = db.CustomerAppointmentOfStaffs.Find(id);

                db.CustomerAppointmentOfStaffs.Remove(data);

                db.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

		public bool DeleteAll(int id)
		{
			try
			{
				db.CustomerAppointmentOfStaffs.Where(e => e.CustomerParticularId == id).ToList().ForEach(cas => db.CustomerAppointmentOfStaffs.Remove(cas));
				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}
	}
}