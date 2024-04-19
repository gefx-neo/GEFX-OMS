using DataAccess;
using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public class KYC_CustomerOtherRepository : IKYC_CustomerOtherRepository
	{
		private DataAccess.GreatEastForex db;

		public KYC_CustomerOtherRepository()
		{
			db = new DataAccess.GreatEastForex();
		}

		public IQueryable<KYC_CustomerOthers> Select()
		{
			var result = from c in db.KYC_CustomerOthers select c;

			return result;
		}

		public KYC_CustomerOthers GetSingle(int id)
		{
			try
			{
				IQueryable<KYC_CustomerOthers> records = Select();

				return records.Where(e => e.ID == id).FirstOrDefault();
			}
			catch
			{
				throw;
			}
		}

		public KYC_CustomerOthers GetOthers(int CustomerParticularId)
		{
			try
			{
				IQueryable<KYC_CustomerOthers> records = Select();

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
				KYC_CustomerOthers temp = new KYC_CustomerOthers();

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

				db.KYC_CustomerOthers.Add(temp);

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
				KYC_CustomerOthers data = db.KYC_CustomerOthers.Where(e => e.ID == id).FirstOrDefault();

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
				KYC_CustomerOthers data = db.KYC_CustomerOthers.Where(e => e.CustomerParticularId == id).FirstOrDefault();

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
				KYC_CustomerOthers data = db.KYC_CustomerOthers.Where(e => e.CustomerParticularId == id).FirstOrDefault();

				db.KYC_CustomerOthers.Remove(data);

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