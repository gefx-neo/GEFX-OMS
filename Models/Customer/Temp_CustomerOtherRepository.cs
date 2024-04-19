using DataAccess;
using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public class Temp_CustomerOtherRepository : ITemp_CustomerOtherRepository
	{
		private DataAccess.GreatEastForex db;

		public Temp_CustomerOtherRepository()
		{
			db = new DataAccess.GreatEastForex();
		}

		public IQueryable<Temp_CustomerOthers> Select()
		{
			var result = from c in db.Temp_CustomerOthers select c;

			return result;
		}

		public Temp_CustomerOthers GetSingle(int id)
		{
			try
			{
				IQueryable<Temp_CustomerOthers> records = Select();

				return records.Where(e => e.ID == id).FirstOrDefault();
			}
			catch
			{
				throw;
			}
		}

		public Temp_CustomerOthers GetOthers(int CustomerParticularId)
		{
			try
			{
				IQueryable<Temp_CustomerOthers> records = Select();

				return records.Where(e => e.CustomerParticularId == CustomerParticularId).FirstOrDefault();
			}
			catch
			{
				throw;
			}
		}

		public bool Add(CustomerOther addData, string NewStatus = "")
		{
			try
			{
				Temp_CustomerOthers temp = new Temp_CustomerOthers();

				temp.Status = addData.Status;
				temp.NewStatus = NewStatus;
				temp.ApprovalBy = addData.ApprovalBy;
				temp.ScreeningResults = addData.ScreeningResults;
				temp.ScreeningResultsDocument = addData.ScreeningResultsDocument;
				temp.Grading = addData.Grading;
				temp.NextReviewDate = addData.NextReviewDate;
				temp.AcraExpiry = addData.AcraExpiry;
				temp.BankAccountNo = addData.BankAccountNo;
				temp.GMApprovalAbove = addData.GMApprovalAbove;
				temp.CustomerProfile = addData.CustomerProfile;
				temp.SalesRemarks = addData.SalesRemarks;
				temp.CustomerParticularId = addData.CustomerParticularId;

				db.Temp_CustomerOthers.Add(temp);

				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool Update(int id, CustomerOther updateData, string NewStatus = "")
		{
			try
			{
				Temp_CustomerOthers data = db.Temp_CustomerOthers.Where(e => e.ID == id).FirstOrDefault();

				data.Status = updateData.Status;
				data.NewStatus = NewStatus;
				data.ApprovalBy = updateData.ApprovalBy;
				data.ScreeningResults = updateData.ScreeningResults;
				data.ScreeningResultsDocument = updateData.ScreeningResultsDocument;
				data.Grading = updateData.Grading;
				data.NextReviewDate = updateData.NextReviewDate;
				data.AcraExpiry = updateData.AcraExpiry;
				data.BankAccountNo = updateData.BankAccountNo;
				data.GMApprovalAbove = updateData.GMApprovalAbove;
				data.CustomerProfile = updateData.CustomerProfile;
				data.SalesRemarks = updateData.SalesRemarks;

				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool ApproveCustomer(int id, string status)
		{
			try
			{
				Temp_CustomerOthers data = db.Temp_CustomerOthers.Where(e => e.CustomerParticularId == id).FirstOrDefault();

				data.Status = status;

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
				Temp_CustomerOthers data = db.Temp_CustomerOthers.Where(e => e.CustomerParticularId == id).FirstOrDefault();

				db.Temp_CustomerOthers.Remove(data);

				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool UpdateEmergency(int id, CustomerOther updateData)
		{
			try
			{
				Temp_CustomerOthers data = db.Temp_CustomerOthers.Where(e => e.CustomerParticularId == id).FirstOrDefault();

				if (data != null)
				{
					data.Status = updateData.Status;
					db.Configuration.ValidateOnSaveEnabled = false;
					db.SaveChanges();
					db.Configuration.ValidateOnSaveEnabled = true;
					return true;
				}
				else
				{
					return false;
				}
			}
			catch
			{
				throw;
			}
		}
	}
}