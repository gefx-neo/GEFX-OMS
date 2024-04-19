using DataAccess;
using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public class CustomerOtherRepository : ICustomerOtherRepository
    {
        private DataAccess.GreatEastForex db;

        public CustomerOtherRepository()
        {
            db = new DataAccess.GreatEastForex();
        }

        public IQueryable<CustomerOther> Select()
        {
            var result = from c in db.CustomerOthers select c;

            return result;
        }

        public CustomerOther GetSingle(int id)
        {
            try
            {
                IQueryable<CustomerOther> records = Select();

                return records.Where(e => e.ID == id).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public CustomerOther GetOthers(int CustomerParticularId)
        {
            try
            {
                IQueryable<CustomerOther> records = Select();

                return records.Where(e => e.CustomerParticularId == CustomerParticularId).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public bool Add(CustomerOther addData)
        {
            try
            {
                db.CustomerOthers.Add(addData);

                db.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

		public bool AddReject(Temp_CustomerOthers addData, string UpdateStatus)
		{
			try
			{
				CustomerOther customerOthers = new CustomerOther();

				customerOthers.AcraExpiry = addData.AcraExpiry;
				customerOthers.ApprovalBy = addData.ApprovalBy;
				customerOthers.BankAccountNo = addData.BankAccountNo;
				customerOthers.CustomerParticularId = addData.CustomerParticularId;
				customerOthers.CustomerProfile = addData.CustomerProfile;
				customerOthers.GMApprovalAbove = addData.GMApprovalAbove;
				customerOthers.Grading = addData.Grading;
				customerOthers.NextReviewDate = addData.NextReviewDate;
				customerOthers.SalesRemarks = addData.SalesRemarks;
				customerOthers.ScreeningResults = addData.ScreeningResults;
				customerOthers.ScreeningResultsDocument = addData.ScreeningResultsDocument;
				customerOthers.Status = UpdateStatus;

				db.CustomerOthers.Add(customerOthers);
				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool AddApproveKYC(KYC_CustomerOthers addData)
		{
			try
			{
				CustomerOther customerOthers = new CustomerOther();

				customerOthers.AcraExpiry = addData.AcraExpiry;
				customerOthers.ApprovalBy = addData.ApprovalBy;
				customerOthers.BankAccountNo = addData.BankAccountNo;
				customerOthers.CustomerParticularId = addData.CustomerParticularId;
				customerOthers.CustomerProfile = addData.CustomerProfile;
				customerOthers.GMApprovalAbove = addData.GMApprovalAbove;
				customerOthers.Grading = addData.Grading;
				customerOthers.NextReviewDate = addData.NextReviewDate;
				customerOthers.SalesRemarks = addData.SalesRemarks;
				customerOthers.ScreeningResults = addData.ScreeningResults;
				customerOthers.ScreeningResultsDocument = addData.ScreeningResultsDocument;
				customerOthers.Status = "Rejected";

				db.CustomerOthers.Add(customerOthers);
				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool Update(int id, CustomerOther updateData)
        {
            try
            {
                CustomerOther data = db.CustomerOthers.Find(id);

                data.Status = updateData.Status;
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

		public bool UpdateStatus(int id, string status)
		{
			try
			{
				CustomerOther data = db.CustomerOthers.Find(id);

				data.Status = status;
			
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
				CustomerOther data = db.CustomerOthers.Where(e => e.CustomerParticularId == id).FirstOrDefault();

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

		public bool UpdateReject(int id, Temp_CustomerOthers updateData, string UpdateStatus)
		{
			try
			{
				CustomerOther data = db.CustomerOthers.Where(e => e.CustomerParticularId == id).FirstOrDefault();

				data.Status = UpdateStatus;
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
				data.CustomerParticularId = updateData.CustomerParticularId;

				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool UpdateApproveKYC(int id, KYC_CustomerOthers updateData)
		{
			try
			{
				CustomerOther data = db.CustomerOthers.Where(e => e.CustomerParticularId == id).FirstOrDefault();

				data.Status = "Rejected";
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
				data.CustomerParticularId = updateData.CustomerParticularId;

				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool ApproveCustomer(int id, string status, int approvalID)
        {
			try
			{
				CustomerOther data = db.CustomerOthers.Where(e => e.CustomerParticularId == id).FirstOrDefault();

                data.Status = status;
				data.ApprovalBy = approvalID;

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
                CustomerOther data = db.CustomerOthers.Find(id);

                db.CustomerOthers.Remove(data);

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