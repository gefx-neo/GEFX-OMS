using DataAccess;
using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public class CustomerDocumentCheckListRepository : ICustomerDocumentCheckListRepository
    {
        private DataAccess.GreatEastForex db;

        public CustomerDocumentCheckListRepository()
        {
            db = new DataAccess.GreatEastForex();
        }

        public IQueryable<CustomerDocumentCheckList> Select()
        {
            var result = from c in db.CustomerDocumentCheckLists select c;

            return result;
        }

        public CustomerDocumentCheckList GetSingle(int id)
        {
            try
            {
                IQueryable<CustomerDocumentCheckList> records = Select();

                return records.Where(e => e.ID == id).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public CustomerDocumentCheckList GetDocumentCheckList(int CustomerParticularId)
        {
            try
            {
                IQueryable<CustomerDocumentCheckList> records = Select();

                return records.Where(e => e.CustomerParticularId == CustomerParticularId).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public bool Add(CustomerDocumentCheckList addData)
        {
            try
            {
                db.CustomerDocumentCheckLists.Add(addData);

                db.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

		public bool AddReject(Temp_CustomerDocumentCheckLists addData)
		{
			try
			{
				CustomerDocumentCheckList checkList = new CustomerDocumentCheckList();
				checkList.CustomerParticularId = addData.CustomerParticularId;
				checkList.Company_SelfiePassporWorkingPass = addData.Company_SelfiePassporWorkingPass;
				checkList.Company_SelfiePhotoID = addData.Company_SelfiePhotoID;
				checkList.Company_AccountOpeningForm = addData.Company_AccountOpeningForm;
				checkList.Company_BusinessProfileFromAcra = addData.Company_BusinessProfileFromAcra;
				checkList.Company_ICWithAuthorizedTradingPersons = addData.Company_ICWithAuthorizedTradingPersons;
				checkList.Company_ICWithDirectors = addData.Company_ICWithDirectors;
				checkList.Natural_BusinessNameCard = addData.Natural_BusinessNameCard;
				checkList.Natural_ICOfCustomer = addData.Natural_ICOfCustomer;
				checkList.Natural_KYCForm = addData.Natural_KYCForm;
				checkList.Natural_SelfiePhotoID = addData.Natural_SelfiePhotoID;

				db.CustomerDocumentCheckLists.Add(checkList);
				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool AddApproveKYC(KYC_CustomerDocumentCheckLists addData)
		{
			try
			{
				CustomerDocumentCheckList checkList = new CustomerDocumentCheckList();
				checkList.CustomerParticularId = addData.CustomerParticularId;
				checkList.Company_SelfiePassporWorkingPass = addData.Company_SelfiePassporWorkingPass;
				checkList.Company_SelfiePhotoID = addData.Company_SelfiePhotoID;
				checkList.Company_AccountOpeningForm = addData.Company_AccountOpeningForm;
				checkList.Company_BusinessProfileFromAcra = addData.Company_BusinessProfileFromAcra;
				checkList.Company_ICWithAuthorizedTradingPersons = addData.Company_ICWithAuthorizedTradingPersons;
				checkList.Company_ICWithDirectors = addData.Company_ICWithDirectors;
				checkList.Natural_BusinessNameCard = addData.Natural_BusinessNameCard;
				checkList.Natural_ICOfCustomer = addData.Natural_ICOfCustomer;
				checkList.Natural_KYCForm = addData.Natural_KYCForm;
				checkList.Natural_SelfiePhotoID = addData.Natural_SelfiePhotoID;

				db.CustomerDocumentCheckLists.Add(checkList);
				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool Update(int id, CustomerDocumentCheckList updateData)
        {
            try
            {
                CustomerDocumentCheckList data = db.CustomerDocumentCheckLists.Find(id);

				data.Company_SelfiePassporWorkingPass = updateData.Company_SelfiePassporWorkingPass;
				data.Company_SelfiePhotoID = updateData.Company_SelfiePhotoID;
                data.Company_AccountOpeningForm = updateData.Company_AccountOpeningForm;
                data.Company_ICWithAuthorizedTradingPersons = updateData.Company_ICWithAuthorizedTradingPersons;
                data.Company_ICWithDirectors = updateData.Company_ICWithDirectors;
                data.Company_BusinessProfileFromAcra = updateData.Company_BusinessProfileFromAcra;
                data.Natural_ICOfCustomer = updateData.Natural_ICOfCustomer;
                data.Natural_BusinessNameCard = updateData.Natural_BusinessNameCard;
                data.Natural_KYCForm = updateData.Natural_KYCForm;
				data.Natural_SelfiePhotoID = updateData.Natural_SelfiePhotoID;

                db.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

		public bool UpdateReject(int id, Temp_CustomerDocumentCheckLists updateData)
		{
			try
			{
				CustomerDocumentCheckList data = db.CustomerDocumentCheckLists.Where(e => e.CustomerParticularId == id).FirstOrDefault();

				data.CustomerParticularId = updateData.CustomerParticularId;
				data.Company_SelfiePassporWorkingPass = updateData.Company_SelfiePassporWorkingPass;
				data.Company_SelfiePhotoID = updateData.Company_SelfiePhotoID;
				data.Company_AccountOpeningForm = updateData.Company_AccountOpeningForm;
				data.Company_ICWithAuthorizedTradingPersons = updateData.Company_ICWithAuthorizedTradingPersons;
				data.Company_ICWithDirectors = updateData.Company_ICWithDirectors;
				data.Company_BusinessProfileFromAcra = updateData.Company_BusinessProfileFromAcra;
				data.Natural_ICOfCustomer = updateData.Natural_ICOfCustomer;
				data.Natural_BusinessNameCard = updateData.Natural_BusinessNameCard;
				data.Natural_KYCForm = updateData.Natural_KYCForm;
				data.Natural_SelfiePhotoID = updateData.Natural_SelfiePhotoID;

				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool UpdateApproveKYC(int id, KYC_CustomerDocumentCheckLists updateData)
		{
			try
			{
				CustomerDocumentCheckList data = db.CustomerDocumentCheckLists.Where(e => e.CustomerParticularId == id).FirstOrDefault();

				data.CustomerParticularId = updateData.CustomerParticularId;
				data.Company_SelfiePassporWorkingPass = updateData.Company_SelfiePassporWorkingPass;
				data.Company_SelfiePhotoID = updateData.Company_SelfiePhotoID;
				data.Company_AccountOpeningForm = updateData.Company_AccountOpeningForm;
				data.Company_ICWithAuthorizedTradingPersons = updateData.Company_ICWithAuthorizedTradingPersons;
				data.Company_ICWithDirectors = updateData.Company_ICWithDirectors;
				data.Company_BusinessProfileFromAcra = updateData.Company_BusinessProfileFromAcra;
				data.Natural_ICOfCustomer = updateData.Natural_ICOfCustomer;
				data.Natural_BusinessNameCard = updateData.Natural_BusinessNameCard;
				data.Natural_KYCForm = updateData.Natural_KYCForm;
				data.Natural_SelfiePhotoID = updateData.Natural_SelfiePhotoID;

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
                CustomerDocumentCheckList data = db.CustomerDocumentCheckLists.Find(id);

                db.CustomerDocumentCheckLists.Remove(data);

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
				db.CustomerDocumentCheckLists.Where(e => e.CustomerParticularId == id).ToList().ForEach(dcl => db.CustomerDocumentCheckLists.Remove(dcl));
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