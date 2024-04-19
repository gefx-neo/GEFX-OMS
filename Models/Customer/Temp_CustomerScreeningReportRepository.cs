using DataAccess;
using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public class Temp_CustomerScreeningReportRepository : ITemp_CustomerScreeningReportRepository
	{
		private DataAccess.GreatEastForex db;

		public Temp_CustomerScreeningReportRepository()
		{
			db = new DataAccess.GreatEastForex();
		}

		public IQueryable<Temp_CustomerScreeningReports> Select()
		{
			var result = from c in db.Temp_CustomerScreeningReports select c;

			return result;
		}

		public IList<Temp_CustomerScreeningReports> GetAll(int customerParticularId)
		{
			try
			{
				var records = Select();

				return records.Where(e => e.CustomerParticularId == customerParticularId).ToList();
			}
			catch
			{
				throw;
			}
		}

		public Temp_CustomerScreeningReports GetSingle(int id)
		{
			try
			{
				var records = Select();

				return records.Where(e => e.ID == id).FirstOrDefault();
			}
			catch
			{
				throw;
			}
		}

		public bool Add(List<CustomerScreeningReport> addData)
		{
			try
			{
				Temp_CustomerScreeningReports temp = new Temp_CustomerScreeningReports();
				List<Temp_CustomerScreeningReports> templist = new List<Temp_CustomerScreeningReports>();

				foreach (CustomerScreeningReport screenreport in addData)
				{
					temp.CustomerParticularId = screenreport.CustomerParticularId;
					temp.Date = screenreport.Date;
					temp.DateOfAcra = screenreport.DateOfAcra;
					temp.ScreenedBy = screenreport.ScreenedBy;
					temp.ScreeningReport_1 = screenreport.ScreeningReport_1;
					temp.ScreeningReport_2 = screenreport.ScreeningReport_2;
					temp.Remarks = screenreport.Remarks;

					templist.Add(temp);
					temp = new Temp_CustomerScreeningReports();
				}

				db.Temp_CustomerScreeningReports.AddRange(templist);

				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool AddSingle (Temp_CustomerScreeningReports addData)
		{
			try
			{
				Temp_CustomerScreeningReports temp = new Temp_CustomerScreeningReports();

				temp.CustomerParticularId = addData.CustomerParticularId;
				temp.Date = addData.Date;
				temp.DateOfAcra = addData.DateOfAcra;
				temp.ScreenedBy = addData.ScreenedBy;
				temp.ScreeningReport_1 = addData.ScreeningReport_1;
				temp.ScreeningReport_2 = addData.ScreeningReport_2;
				temp.Remarks = addData.Remarks;

				db.Temp_CustomerScreeningReports.Add(temp);

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
				Temp_CustomerScreeningReports data = db.Temp_CustomerScreeningReports.Where(e => e.CustomerParticularId == id).FirstOrDefault();

				db.Temp_CustomerScreeningReports.Remove(data);

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
				db.Temp_CustomerScreeningReports.Where(e => e.CustomerParticularId == id).ToList().ForEach(tsr => db.Temp_CustomerScreeningReports.Remove(tsr));
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