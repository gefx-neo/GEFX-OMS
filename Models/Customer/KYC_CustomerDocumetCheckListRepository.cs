using DataAccess;
using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public class KYC_CustomerDocumentCheckListRepository : IKYC_CustomerDocumentCheckListRepository
	{
		private DataAccess.GreatEastForex db;

		public KYC_CustomerDocumentCheckListRepository()
		{
			db = new DataAccess.GreatEastForex();
		}

		public IQueryable<KYC_CustomerDocumentCheckLists> Select()
		{
			var result = from c in db.KYC_CustomerDocumentCheckLists select c;

			return result;
		}

		public KYC_CustomerDocumentCheckLists GetSingle(int id)
		{
			try
			{
				IQueryable<KYC_CustomerDocumentCheckLists> records = Select();

				return records.Where(e => e.ID == id).FirstOrDefault();
			}
			catch
			{
				throw;
			}
		}

		public KYC_CustomerDocumentCheckLists GetDocumentCheckList(int CustomerParticularId)
		{
			try
			{
				IQueryable<KYC_CustomerDocumentCheckLists> records = Select();

				return records.Where(e => e.CustomerParticularId == CustomerParticularId).FirstOrDefault();
			}
			catch
			{
				throw;
			}
		}

		public bool Add(List<CustomerDocumentCheckList> addData)
		{
			try
			{
				KYC_CustomerDocumentCheckLists temp = new KYC_CustomerDocumentCheckLists();
				List<KYC_CustomerDocumentCheckLists> templist = new List<KYC_CustomerDocumentCheckLists>();

				foreach (CustomerDocumentCheckList checklist in addData)
				{
					temp.Company_SelfiePassporWorkingPass = checklist.Company_SelfiePassporWorkingPass;
					temp.Company_SelfiePhotoID = checklist.Company_SelfiePhotoID;
					temp.Company_AccountOpeningForm = checklist.Company_AccountOpeningForm;
					temp.Company_ICWithAuthorizedTradingPersons = checklist.Company_ICWithAuthorizedTradingPersons;
					temp.Company_ICWithDirectors = checklist.Company_ICWithDirectors;
					temp.Company_BusinessProfileFromAcra = checklist.Company_BusinessProfileFromAcra;
					temp.Natural_ICOfCustomer = checklist.Natural_ICOfCustomer;
					temp.Natural_BusinessNameCard = checklist.Natural_BusinessNameCard;
					temp.Natural_KYCForm = checklist.Natural_KYCForm;
					temp.Natural_SelfiePhotoID = checklist.Natural_SelfiePhotoID;
					temp.CustomerParticularId = checklist.CustomerParticularId;

					templist.Add(temp);
					temp = new KYC_CustomerDocumentCheckLists();
				}

				db.KYC_CustomerDocumentCheckLists.AddRange(templist);

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
				KYC_CustomerDocumentCheckLists data = db.KYC_CustomerDocumentCheckLists.Where(e => e.ID == id).FirstOrDefault();

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
				KYC_CustomerDocumentCheckLists data = db.KYC_CustomerDocumentCheckLists.Where(e => e.CustomerParticularId == id).FirstOrDefault();

				db.KYC_CustomerDocumentCheckLists.Remove(data);

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