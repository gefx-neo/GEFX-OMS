using DataAccess;
using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public class Temp_CustomerAppointmentOfStaffRepository : ITemp_CustomerAppointmentOfStaffRepository
	{
		private DataAccess.GreatEastForex db;

		public Temp_CustomerAppointmentOfStaffRepository()
		{
			db = new DataAccess.GreatEastForex();
		}

		public IQueryable<Temp_CustomerAppointmentOfStaffs> Select()
		{
			var result = from c in db.Temp_CustomerAppointmentOfStaffs select c;

			return result;
		}

		public Temp_CustomerAppointmentOfStaffs GetSingle(int id)
		{
			try
			{
				IQueryable<Temp_CustomerAppointmentOfStaffs> records = Select();

				return records.Where(e => e.ID == id).FirstOrDefault();
			}
			catch
			{
				throw;
			}
		}

		public IList<Temp_CustomerAppointmentOfStaffs> GetAppointments(int CustomerParticularId)
		{
			try
			{
				IQueryable<Temp_CustomerAppointmentOfStaffs> records = Select();

				return records.Where(e => e.CustomerParticularId == CustomerParticularId).ToList();
			}
			catch
			{
				throw;
			}
		}

		public bool Add(List<CustomerAppointmentOfStaff> addData)
		{
			try
			{
				Temp_CustomerAppointmentOfStaffs temp = new Temp_CustomerAppointmentOfStaffs();
				List<Temp_CustomerAppointmentOfStaffs> tempList = new List<Temp_CustomerAppointmentOfStaffs>();

				foreach (CustomerAppointmentOfStaff app in addData)
				{
					temp.CustomerParticularId = app.CustomerParticularId;
					temp.FullName = app.FullName;
					temp.ICPassportNo = app.ICPassportNo;
					temp.Nationality = app.Nationality;
					temp.JobTitle = app.JobTitle;

					tempList.Add(temp);
					temp = new Temp_CustomerAppointmentOfStaffs();
				}

				db.Temp_CustomerAppointmentOfStaffs.AddRange(tempList);

				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool AddSingle(Temp_CustomerAppointmentOfStaffs addData)
		{
			try
			{
				Temp_CustomerAppointmentOfStaffs temp = new Temp_CustomerAppointmentOfStaffs();

				temp.CustomerParticularId = addData.CustomerParticularId;
				temp.FullName = addData.FullName;
				temp.ICPassportNo = addData.ICPassportNo;
				temp.Nationality = addData.Nationality;
				temp.JobTitle = addData.JobTitle;

				db.Temp_CustomerAppointmentOfStaffs.Add(temp);

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
				Temp_CustomerAppointmentOfStaffs data = db.Temp_CustomerAppointmentOfStaffs.Find(id);

				db.Temp_CustomerAppointmentOfStaffs.Remove(data);

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
				db.Temp_CustomerAppointmentOfStaffs.Where(e => e.CustomerParticularId == id).ToList().ForEach(tcos => db.Temp_CustomerAppointmentOfStaffs.Remove(tcos));
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