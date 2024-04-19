using DataAccess;
using DataAccess.POCO;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public class BeneficiaryRepository : IBeneficiaryRepository
    {
		private DataAccess.GreatEastForex db;

		public BeneficiaryRepository()
		{
			db = new DataAccess.GreatEastForex();
		}

		public IQueryable<Beneficiaries> Select()
		{
			var result = from u in db.Beneficiaries select u;

			return result/*.Where(e => e.IsDeleted == "N")*/;
		}

		public IQueryable<Beneficiaries> SelectAll()
		{
			var result = from u in db.Beneficiaries select u;

			return result;
		}

		public IList<Beneficiaries> GetAll()
		{
			try
			{
				IQueryable<Beneficiaries> records = Select();

				return records.OrderByDescending(e => e.CreatedOn).ToList();
			}
			catch
			{
				throw;
			}
		}

		public Beneficiaries GetSingle(int id)
		{
			try
			{
				IQueryable<Beneficiaries> records = Select();

				return records.Where(e => e.ID == id).FirstOrDefault();
			}
			catch
			{
				throw;
			}
		}

		//public Beneficiaries FindAgentId(string AgentId)
		//{
		//	try
		//	{
		//		IQueryable<Beneficiaries> records = Select();

		//		return records.Where(e => e.AgentId == AgentId).FirstOrDefault();
		//	}
		//	catch
		//	{
		//		throw;
		//	}
		//}

		//public Beneficiaries FindAgentIdNotOwnSelf(int id, string BeneficiaryID)
		//{
		//	try
		//	{
		//		IQueryable<Beneficiaries> records = Select();

		//		return records.Where(e => e.IsYourAccount == AgentId && e.ID != id).FirstOrDefault();
		//	}
		//	catch
		//	{
		//		throw;
		//	}
		//}

		public IPagedList<Beneficiaries> GetPaged(string keyword, int page, int size)
		{
			try
			{
				IQueryable<Beneficiaries> records = Select();

				if (!string.IsNullOrEmpty(keyword))
				{
					records = records.Where(e => e.BeneficiaryFullName.Contains(keyword) || e.BeneficiaryFriendlyName.Contains(keyword));
				}

				return records.OrderByDescending(e => e.CreatedOn).ToPagedList(page, size);
			}
			catch
			{
				throw;
			}
		}

		public bool Add(Beneficiaries addData)
		{
			try
			{
				addData.CreatedOn = DateTime.Now;
				addData.UpdatedOn = DateTime.Now;
				addData.Status = "N";

				db.Beneficiaries.Add(addData);

				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool Update(int id, Beneficiaries updateData)
		{
			try
			{
                Beneficiaries data = db.Beneficiaries.Find(id);

				data.BankAccountNo = updateData.BankAccountNo;
				data.BankAddress = updateData.BankAddress;
				data.BankCode = updateData.BankCode;
				data.BankCountry = updateData.BankCountry;
				data.BankType = updateData.BankType;
				data.BeneficiaryBusinessCategory = updateData.BeneficiaryBusinessCategory;
				data.BeneficiaryCompanyRegistrationNo = updateData.BeneficiaryCompanyRegistrationNo;
				data.BeneficiaryContactNo = updateData.BeneficiaryContactNo;
				data.BeneficiaryFriendlyName = updateData.BeneficiaryFriendlyName;
				data.BeneficiaryFullName = updateData.BeneficiaryFullName;
				data.BeneficiaryNationality = updateData.BeneficiaryNationality;
				data.IsYourAccount = updateData.IsYourAccount;
				data.PaymentDetails = updateData.PaymentDetails;
				data.PurposeOfPayment = updateData.PurposeOfPayment;
				data.SourceOfPayment = updateData.SourceOfPayment;
				data.Status = updateData.Status;
				data.Type = updateData.Type;
				data.UpdatedOn = DateTime.Now;

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
                Beneficiaries data = db.Beneficiaries.Find(id);

				data.Status = "Y";
				data.UpdatedOn = DateTime.Now;

				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		//This section is for Beneficiaries Controller page used.
		public IPagedList<Beneficiaries> GetPagedBeneficiaries(string keyword, int page, int size)
		{
			try
			{
				IQueryable<Beneficiaries> records = Select();

				if (!string.IsNullOrEmpty(keyword))
				{
					records = records.Where(e => e.BeneficiaryFullName.Contains(keyword) || e.BeneficiaryFriendlyName.Contains(keyword) || e.CustomerParticulars.CustomerCode.Contains(keyword));
				}

				return records.OrderByDescending(e => e.ID).ToPagedList(page, size);
			}
			catch
			{
				throw;
			}
		}

		public bool DeleteBeneficiaries(int id)
		{
			try
			{
				Beneficiaries data = db.Beneficiaries.Find(id);

				db.Beneficiaries.Remove(data);
				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public Beneficiaries GetSingleBeneficiaries(int id)
		{
			try
			{
				IQueryable<Beneficiaries> records = Select();

				return records.Where(e => e.ID == id).FirstOrDefault();
			}
			catch
			{
				throw;
			}
		}
	}
}