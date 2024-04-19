using DataAccess;
using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public class Temp_CustomerDocumentCheckListRepository : ITemp_CustomerDocumentCheckListRepository
	{
		private DataAccess.GreatEastForex db;

		public Temp_CustomerDocumentCheckListRepository()
		{
			db = new DataAccess.GreatEastForex();
		}

		public IQueryable<Temp_CustomerDocumentCheckLists> Select()
		{
			var result = from c in db.Temp_CustomerDocumentsCheckList select c;

			return result;
		}

		public Temp_CustomerDocumentCheckLists GetSingle(int id)
		{
			try
			{
				IQueryable<Temp_CustomerDocumentCheckLists> records = Select();

				return records.Where(e => e.ID == id).FirstOrDefault();
			}
			catch
			{
				throw;
			}
		}

		public Temp_CustomerDocumentCheckLists GetDocumentCheckList(int CustomerParticularId)
		{
			try
			{
				IQueryable<Temp_CustomerDocumentCheckLists> records = Select();

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
				Temp_CustomerDocumentCheckLists temp = new Temp_CustomerDocumentCheckLists();
				List<Temp_CustomerDocumentCheckLists> templist = new List<Temp_CustomerDocumentCheckLists>();

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
					temp = new Temp_CustomerDocumentCheckLists();
				}

				db.Temp_CustomerDocumentsCheckList.AddRange(templist);

				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool AddSingle(CustomerDocumentCheckList addData)
		{
			try
			{
				Temp_CustomerDocumentCheckLists temp = new Temp_CustomerDocumentCheckLists();

				temp.Company_SelfiePassporWorkingPass = addData.Company_SelfiePassporWorkingPass;
				temp.Company_SelfiePhotoID = addData.Company_SelfiePhotoID;
				temp.Company_AccountOpeningForm = addData.Company_AccountOpeningForm;
				temp.Company_ICWithAuthorizedTradingPersons = addData.Company_ICWithAuthorizedTradingPersons;
				temp.Company_ICWithDirectors = addData.Company_ICWithDirectors;
				temp.Company_BusinessProfileFromAcra = addData.Company_BusinessProfileFromAcra;
				temp.Natural_ICOfCustomer = addData.Natural_ICOfCustomer;
				temp.Natural_BusinessNameCard = addData.Natural_BusinessNameCard;
				temp.Natural_KYCForm = addData.Natural_KYCForm;
				temp.Natural_SelfiePhotoID = addData.Natural_SelfiePhotoID;
				temp.CustomerParticularId = addData.CustomerParticularId;

				db.Temp_CustomerDocumentsCheckList.Add(temp);

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
				Temp_CustomerDocumentCheckLists data = db.Temp_CustomerDocumentsCheckList.Where(e => e.ID == id).FirstOrDefault();

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
				Temp_CustomerDocumentCheckLists data = db.Temp_CustomerDocumentsCheckList.Where(e => e.CustomerParticularId == id).FirstOrDefault();

				db.Temp_CustomerDocumentsCheckList.Remove(data);

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
				db.Temp_CustomerDocumentsCheckList.Where(e => e.CustomerParticularId == id).ToList().ForEach(tcos => db.Temp_CustomerDocumentsCheckList.Remove(tcos));
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