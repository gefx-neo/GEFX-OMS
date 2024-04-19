using DataAccess;
using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public class CustomerScreeningReportRepository : ICustomerScreeningReportRepository
    {
        private DataAccess.GreatEastForex db;

        public CustomerScreeningReportRepository()
        {
            db = new DataAccess.GreatEastForex();
        }

        public IQueryable<CustomerScreeningReport> Select()
        {
            var result = from c in db.CustomerScreeningReports select c;

            return result;
        }

        public IList<CustomerScreeningReport> GetAll(int customerParticularId)
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

        public CustomerScreeningReport GetSingle(int id)
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

        public bool Add(CustomerScreeningReport addData)
        {
            try
            {
                db.CustomerScreeningReports.Add(addData);

                db.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

		public bool AddReject(IList<Temp_CustomerScreeningReports> addData)
		{
			try
			{
				CustomerScreeningReport screeningReport = new CustomerScreeningReport();
				List<CustomerScreeningReport> screenList = new List<CustomerScreeningReport>();

				foreach (Temp_CustomerScreeningReports temp in addData)
				{
					screeningReport.CustomerParticularId = temp.CustomerParticularId;
					screeningReport.Date = temp.Date;
					screeningReport.DateOfAcra = temp.DateOfAcra;
					screeningReport.Remarks = temp.Remarks;
					screeningReport.ScreenedBy = temp.ScreenedBy;
					screeningReport.ScreeningReport_1 = temp.ScreeningReport_1;
					screeningReport.ScreeningReport_2 = temp.ScreeningReport_2;

					screenList.Add(screeningReport);
					screeningReport = new CustomerScreeningReport();
				}

				db.CustomerScreeningReports.AddRange(screenList);

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
                CustomerScreeningReport data = db.CustomerScreeningReports.Find(id);

                db.CustomerScreeningReports.Remove(data);

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
				db.CustomerScreeningReports.Where(e => e.CustomerParticularId == id).ToList().ForEach(csr => db.CustomerScreeningReports.Remove(csr));
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