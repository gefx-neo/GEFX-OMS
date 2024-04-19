using DataAccess;
using DataAccess.POCO;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public class KYC_CustomerParticularRepository : IKYC_CustomerParticularRepository
	{
		private DataAccess.GreatEastForex db;

		public KYC_CustomerParticularRepository()
		{
			db = new DataAccess.GreatEastForex();
		}

		public IQueryable<KYC_CustomerParticulars> Select()
		{
			var result = from c in db.KYC_CustomerParticulars select c;

			return result.Where(e => e.IsDeleted == "N");
		}

		public IQueryable<KYC_CustomerParticulars> SelectAll()
		{
			var result = from c in db.KYC_CustomerParticulars select c;

			return result;
		}

		public IList<KYC_CustomerParticulars> GetAll()
		{
			try
			{
				IQueryable<KYC_CustomerParticulars> records = Select();

				return records.OrderBy(e => !string.IsNullOrEmpty(e.Company_RegisteredName) ? e.Company_RegisteredName : !string.IsNullOrEmpty(e.Natural_EmployedEmployerName) ? e.Natural_EmployedEmployerName : e.Natural_SelfEmployedBusinessName).ToList();
			}
			catch
			{
				throw;
			}
		}

		public IList<KYC_CustomerParticulars> GetAllCustomers(string from, string to)
		{
			try
			{
				IQueryable<KYC_CustomerParticulars> records = Select();

				if (!string.IsNullOrEmpty(from))
				{
					DateTime fromDate = Convert.ToDateTime(from + " 00:00:00");
					records = records.Where(e => e.CreatedOn >= fromDate);
				}

				if (!string.IsNullOrEmpty(to))
				{
					DateTime toDate = Convert.ToDateTime(to + " 23:59:59");
					records = records.Where(e => e.CreatedOn <= toDate);
				}

				return records.OrderBy(e => !string.IsNullOrEmpty(e.Company_RegisteredName) ? e.Company_RegisteredName : !string.IsNullOrEmpty(e.Natural_EmployedEmployerName) ? e.Natural_EmployedEmployerName : e.Natural_SelfEmployedBusinessName).ToList();
			}
			catch
			{
				throw;
			}
		}

		public IList<KYC_CustomerParticulars> GetAllByStatus(string status)
		{
			try
			{
				IQueryable<KYC_CustomerParticulars> records = Select();

				return records.Where(e => e.Others.FirstOrDefault().Status == status).OrderBy(e => !string.IsNullOrEmpty(e.Company_RegisteredName) ? e.Company_RegisteredName : !string.IsNullOrEmpty(e.Natural_EmployedEmployerName) ? e.Natural_EmployedEmployerName : e.Natural_SelfEmployedBusinessName).ToList();
			}
			catch
			{
				throw;
			}
		}

		public int GetAllByStatusCount(string status)
		{
			try
			{
				IQueryable<KYC_CustomerParticulars> records = Select();

				return records.Where(e => e.Others.FirstOrDefault().Status == status).OrderBy(e => !string.IsNullOrEmpty(e.Company_RegisteredName) ? e.Company_RegisteredName : !string.IsNullOrEmpty(e.Natural_EmployedEmployerName) ? e.Natural_EmployedEmployerName : e.Natural_SelfEmployedBusinessName).Count();
			}
			catch
			{
				throw;
			}
		}

		public KYC_CustomerParticulars GetSingle(int id)
		{
			try
			{
				IQueryable<KYC_CustomerParticulars> records = Select();

				return records.Where(e => e.ID == id).FirstOrDefault();
			}
			catch
			{
				throw;
			}
		}

		public KYC_CustomerParticulars GetSingle2(int id)
		{
			try
			{
				var records = db.KYC_CustomerParticulars.Where(e => e.Customer_MainID == id).FirstOrDefault();

				return records;
			}
			catch
			{
				throw;
			}
		}

		public IPagedList<GetAllCustomerActiveList> GetPaged(string keyword, int page, int size, string searchTag = null)
		{
			try
			{
				//Start
				using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GreatEastForex"].ConnectionString))
				{
					conn.Open();

					List<GetAllCustomerActiveList> customerList = new List<GetAllCustomerActiveList>();

					SqlCommand cmd = new SqlCommand("GetAllCustomerActiveList", conn);

					cmd.ExecuteNonQuery();
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.CommandTimeout = 5000;

					// execute the command
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						customerList = reader.Cast<IDataRecord>()
						.Select(x => new GetAllCustomerActiveList
						{
							ID = (int)x["ID"],
							IsSubAccount = (long)x["IsSubAccount"],
							IsDeleted = (string)x["IsDeleted"],
							CustomerType = (string)x["CustomerType"],
							Company_RegisteredName = (x["Company_RegisteredName"] == DBNull.Value) ? "" : (string)x["Company_RegisteredName"],
							Natural_Name = (x["Natural_Name"] == DBNull.Value) ? "" : (string)x["Natural_Name"],
							Customer_Profile = (string)x["CustomerProfile"],
							CustomerCode = (x["CustomerCode"] == DBNull.Value) ? "" : (string)x["CustomerCode"],
							Natural_EmployedEmployerName = (x["Natural_EmployedEmployerName"] == DBNull.Value) ? "" : (string)x["Natural_EmployedEmployerName"],
							Natural_SelfEmployedBusinessName = (x["Natural_SelfEmployedBusinessName"] == DBNull.Value) ? "" : (string)x["Natural_SelfEmployedBusinessName"],
							SearchTags = (x["SearchTags"] == DBNull.Value) ? "" : (string)x["SearchTags"],
							Company_ContactName = (x["Company_ContactName"] == DBNull.Value) ? "" : (string)x["Company_ContactName"],
							Company_TelNo = (x["Company_TelNo"] == DBNull.Value) ? "" : (string)x["Company_TelNo"],
							Natural_ContactNoH = (x["Natural_ContactNoH"] == DBNull.Value) ? "" : (string)x["Natural_ContactNoH"],
							Natural_EmploymentType = (x["Natural_EmploymentType"] == DBNull.Value) ? "" : (string)x["Natural_EmploymentType"],
							Natural_Email = (x["Natural_Email"] == DBNull.Value) ? "" : (string)x["Natural_Email"],
							Company_Email = (x["Company_Email"] == DBNull.Value) ? "" : (string)x["Company_Email"],
							Status = (string)x["Status"]
						}).ToList();

						if (!string.IsNullOrEmpty(keyword))
						{
							customerList = customerList.Where(e => CultureInfo.CurrentCulture.CompareInfo.IndexOf(e.CustomerCode, keyword, CompareOptions.IgnoreCase) >= 0 || CultureInfo.CurrentCulture.CompareInfo.IndexOf(e.Company_ContactName, keyword, CompareOptions.IgnoreCase) >= 0 || CultureInfo.CurrentCulture.CompareInfo.IndexOf(e.Natural_Name, keyword, CompareOptions.IgnoreCase) >= 0 || CultureInfo.CurrentCulture.CompareInfo.IndexOf(e.Company_RegisteredName, keyword, CompareOptions.IgnoreCase) >= 0 || CultureInfo.CurrentCulture.CompareInfo.IndexOf(e.Company_Email, keyword, CompareOptions.IgnoreCase) >= 0 || CultureInfo.CurrentCulture.CompareInfo.IndexOf(e.Natural_Email, keyword, CompareOptions.IgnoreCase) >= 0).ToList();
						}

						if (!string.IsNullOrEmpty(searchTag))
						{
							if (searchTag != "0")
							{
								customerList = customerList.Where(e => e.SearchTags.Contains("-" + searchTag + "-")).ToList();
							}
						}

						conn.Close();
						IPagedList<GetAllCustomerActiveList> ipagedListConversion = customerList.OrderBy(e => !string.IsNullOrEmpty(e.Company_RegisteredName) ? e.Company_RegisteredName : !string.IsNullOrEmpty(e.Natural_EmployedEmployerName) ? e.Natural_EmployedEmployerName : e.Natural_SelfEmployedBusinessName).ToPagedList(page, size);
						return ipagedListConversion;
					}
				}
				//End


				//IQueryable<CustomerParticular> records = Select();

				//if (!string.IsNullOrEmpty(keyword))
				//            {
				//	records = records.Where(e => e.CustomerCode.Contains(keyword) || e.Company_RegisteredName.Contains(keyword) || e.Natural_Name.Contains(keyword));
				//            }

				//if (!string.IsNullOrEmpty(searchTag))
				//{
				//	if (searchTag != "0")
				//	{
				//		records = records.Where(e => e.SearchTags.Contains("-" + searchTag + "-"));
				//	}
				//}

				//return records.OrderBy(e => !string.IsNullOrEmpty(e.Company_RegisteredName) ? e.Company_RegisteredName : !string.IsNullOrEmpty(e.Natural_EmployedEmployerName) ? e.Natural_EmployedEmployerName : e.Natural_SelfEmployedBusinessName).ToPagedList(page, size);
			}
			catch
			{
				throw;
			}
		}

		public IPagedList<KYC_CustomerParticulars> GetCustomerDataPaged(string from, string to, int page, int size)
		{
			try
			{
				IQueryable<KYC_CustomerParticulars> records = Select();

				if (!string.IsNullOrEmpty(from))
				{
					DateTime fromDate = Convert.ToDateTime(from + " 00:00:00");
					records = records.Where(e => e.CreatedOn >= fromDate);
				}

				if (!string.IsNullOrEmpty(to))
				{
					DateTime toDate = Convert.ToDateTime(to + " 23:59:59");
					records = records.Where(e => e.CreatedOn <= toDate);
				}

				return records.OrderBy(e => !string.IsNullOrEmpty(e.Company_RegisteredName) ? e.Company_RegisteredName : !string.IsNullOrEmpty(e.Natural_EmployedEmployerName) ? e.Natural_EmployedEmployerName : e.Natural_SelfEmployedBusinessName).ToPagedList(page, size);
			}
			catch
			{
				throw;
			}
		}

		public KYC_CustomerParticulars FindCustomerCode(string code)
		{
			try
			{
				IQueryable<KYC_CustomerParticulars> records = Select();

				return records.Where(e => e.CustomerCode == code).FirstOrDefault();
			}
			catch
			{
				throw;
			}
		}

		public KYC_CustomerParticulars FindICPassport(string icpassport)
		{
			try
			{
				IQueryable<KYC_CustomerParticulars> records = Select();

				return records.Where(e => e.Natural_ICPassportNo == icpassport).FirstOrDefault();
			}
			catch
			{
				throw;
			}
		}

		public KYC_CustomerParticulars FindEmail(string email)
		{
			try
			{
				IQueryable<KYC_CustomerParticulars> records = Select();

				return records.Where(e => e.Company_Email == email || e.Natural_Email == email).FirstOrDefault();
			}
			catch
			{
				throw;
			}
		}

		public bool Add(CustomerParticular addData)
		{
			try
			{
				KYC_CustomerParticulars temp = new KYC_CustomerParticulars();
				addData.CreatedOn = DateTime.Now;
				addData.UpdatedOn = DateTime.Now;
				addData.IsDeleted = "N";

				temp.CustomerCode = addData.CustomerCode;
				temp.CustomerType = addData.CustomerType;
				temp.Company_RegisteredName = addData.Company_RegisteredName;
				temp.Company_RegisteredAddress = addData.Company_RegisteredAddress;
				temp.Company_BusinessAddress1 = addData.Company_BusinessAddress1;
				temp.Company_BusinessAddress2 = addData.Company_BusinessAddress2;
				temp.Company_BusinessAddress3 = addData.Company_BusinessAddress3;
				temp.Company_PostalCode = addData.Company_PostalCode;
				temp.Company_ContactName = addData.Company_ContactName;
				temp.Company_TelNo = addData.Company_TelNo;
				temp.Company_ContactNoH = addData.Company_ContactNoH;
				temp.Company_ContactNoO = addData.Company_ContactNoO;
				temp.Company_ContactNoM = addData.Company_ContactNoM;
				temp.Company_ICPassport = addData.Company_ICPassport;
				temp.Company_JobTitle = addData.Company_JobTitle;
				temp.Company_Nationality = addData.Company_Nationality;
				temp.Company_Country = addData.Company_Country;
				temp.Company_CountryCode = addData.Company_CountryCode;
				temp.Company_FaxNo = addData.Company_FaxNo;
				temp.Company_Email = addData.Company_Email;
				temp.Company_PlaceOfRegistration = addData.Company_PlaceOfRegistration;
				temp.Company_DateOfRegistration = addData.Company_DateOfRegistration;
				temp.Company_RegistrationNo = addData.Company_RegistrationNo;
				temp.Company_TypeOfEntity = addData.Company_TypeOfEntity;
				temp.Company_TypeOfEntityIfOthers = addData.Company_TypeOfEntityIfOthers;
				temp.Company_PurposeAndIntended = addData.Company_PurposeAndIntended;
				temp.DOB = addData.DOB;
				temp.Natural_Name = addData.Natural_Name;
				temp.Natural_PermanentAddress = addData.Natural_PermanentAddress;
				temp.Natural_PermanentAddress2 = addData.Natural_PermanentAddress2;
				temp.Natural_PermanentAddress3 = addData.Natural_PermanentAddress3;
				temp.Natural_PermanentPostalCode = addData.Natural_PermanentPostalCode;
				temp.Natural_MailingAddress = addData.Natural_MailingAddress;
				temp.Natural_MailingAddress2 = addData.Natural_MailingAddress2;
				temp.Natural_MailingAddress3 = addData.Natural_MailingAddress3;
				temp.Mailing_PostalCode = addData.Mailing_PostalCode;
				temp.Natural_Nationality = addData.Natural_Nationality;
				temp.Natural_ICPassportNo = addData.Natural_ICPassportNo;
				temp.Natural_DOB = addData.Natural_DOB;
				temp.Natural_ContactNoH = addData.Natural_ContactNoH;
				temp.Natural_ContactNoO = addData.Natural_ContactNoO;
				temp.Natural_ContactNoM = addData.Natural_ContactNoM;
				temp.Natural_Email = addData.Natural_Email;
				temp.Natural_EmploymentType = addData.Natural_EmploymentType;
				temp.Natural_EmployedEmployerName = addData.Natural_EmployedEmployerName;
				temp.Natural_EmployedJobTitle = addData.Natural_EmployedJobTitle;
				temp.Natural_EmployedRegisteredAddress = addData.Natural_EmployedRegisteredAddress;
				temp.Natural_EmployedRegisteredAddress2 = addData.Natural_EmployedRegisteredAddress2;
				temp.Natural_EmployedRegisteredAddress3 = addData.Natural_EmployedRegisteredAddress3;
				temp.Natural_SelfEmployedBusinessName = addData.Natural_SelfEmployedBusinessName;
				temp.Natural_SelfEmployedRegistrationNo = addData.Natural_SelfEmployedRegistrationNo;
				temp.Natural_SelfEmployedBusinessAddress = addData.Natural_SelfEmployedBusinessAddress;
				temp.Natural_SelfEmployedBusinessPrincipalPlace = addData.Natural_SelfEmployedBusinessPrincipalPlace;
				temp.Shipping_Address1 = addData.Shipping_Address1;
				temp.Shipping_Address2 = addData.Shipping_Address2;
				temp.Shipping_Address3 = addData.Shipping_Address3;
				temp.Shipping_PostalCode = addData.Shipping_PostalCode;
				temp.SearchTags = addData.SearchTags;
				temp.Password = addData.Password;
				temp.ResetPasswordToken = addData.ResetPasswordToken;
				temp.LastPasswordUpdated = addData.LastPasswordUpdated;
				temp.IsSubAccount = addData.IsSubAccount;
				temp.Customer_Title = addData.Customer_Title;
				temp.Surname = addData.Surname;
				temp.GivenName = addData.GivenName;
				temp.hasCustomerAccount = addData.hasCustomerAccount;
				temp.isVerify = addData.isVerify;
				temp.VerifyAccountToken = addData.VerifyAccountToken;
				temp.CreatedOn = addData.CreatedOn;
				temp.UpdatedOn = addData.UpdatedOn;
				temp.IsDeleted = addData.IsDeleted;
				temp.Customer_MainID = addData.ID;
				temp.EnableTransactionType = addData.EnableTransactionType;

				db.Configuration.ValidateOnSaveEnabled = false;
				db.KYC_CustomerParticulars.Add(temp);

				db.SaveChanges();
				db.Configuration.ValidateOnSaveEnabled = true;

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool Update(int id, CustomerParticular updateData)
		{
			try
			{
				KYC_CustomerParticulars temp = db.KYC_CustomerParticulars.Where(e => e.ID == id).FirstOrDefault();

				temp.CustomerCode = updateData.CustomerCode;
				temp.CustomerType = updateData.CustomerType;
				temp.Company_RegisteredName = updateData.Company_RegisteredName;
				temp.Company_RegisteredAddress = updateData.Company_RegisteredAddress;
				temp.Company_BusinessAddress1 = updateData.Company_BusinessAddress1;
				temp.Company_BusinessAddress2 = updateData.Company_BusinessAddress2;
				temp.Company_BusinessAddress3 = updateData.Company_BusinessAddress3;
				temp.Company_PostalCode = updateData.Company_PostalCode;
				temp.Company_ContactName = updateData.Company_ContactName;
				temp.Company_TelNo = updateData.Company_TelNo;
				temp.Company_ContactNoH = updateData.Company_ContactNoH;
				temp.Company_ContactNoO = updateData.Company_ContactNoO;
				temp.Company_ContactNoM = updateData.Company_ContactNoM;
				temp.Company_ICPassport = updateData.Company_ICPassport;
				temp.Company_JobTitle = updateData.Company_JobTitle;
				temp.Company_Nationality = updateData.Company_Nationality;
				temp.Company_Country = updateData.Company_Country;
				temp.Company_CountryCode = updateData.Company_CountryCode;
				temp.Company_FaxNo = updateData.Company_FaxNo;
				temp.Company_Email = updateData.Company_Email;
				temp.Company_PlaceOfRegistration = updateData.Company_PlaceOfRegistration;
				temp.Company_DateOfRegistration = updateData.Company_DateOfRegistration;
				temp.Company_RegistrationNo = updateData.Company_RegistrationNo;
				temp.Company_TypeOfEntity = updateData.Company_TypeOfEntity;
				temp.Company_TypeOfEntityIfOthers = updateData.Company_TypeOfEntityIfOthers;
				temp.Company_PurposeAndIntended = updateData.Company_PurposeAndIntended;
				temp.DOB = updateData.DOB;
				temp.Natural_Name = updateData.Natural_Name;
				temp.Natural_PermanentAddress = updateData.Natural_PermanentAddress;
				temp.Natural_PermanentAddress2 = updateData.Natural_PermanentAddress2;
				temp.Natural_PermanentAddress3 = updateData.Natural_PermanentAddress3;
				temp.Natural_PermanentPostalCode = updateData.Natural_PermanentPostalCode;
				temp.Natural_MailingAddress = updateData.Natural_MailingAddress;
				temp.Natural_MailingAddress2 = updateData.Natural_MailingAddress2;
				temp.Natural_MailingAddress3 = updateData.Natural_MailingAddress3;
				temp.Mailing_PostalCode = updateData.Mailing_PostalCode;
				temp.Natural_Nationality = updateData.Natural_Nationality;
				temp.Natural_ICPassportNo = updateData.Natural_ICPassportNo;
				temp.Natural_DOB = updateData.Natural_DOB;
				temp.Natural_ContactNoH = updateData.Natural_ContactNoH;
				temp.Natural_ContactNoO = updateData.Natural_ContactNoO;
				temp.Natural_ContactNoM = updateData.Natural_ContactNoM;
				temp.Natural_Email = updateData.Natural_Email;
				temp.Natural_EmploymentType = updateData.Natural_EmploymentType;
				temp.Natural_EmployedEmployerName = updateData.Natural_EmployedEmployerName;
				temp.Natural_EmployedJobTitle = updateData.Natural_EmployedJobTitle;
				temp.Natural_EmployedRegisteredAddress = updateData.Natural_EmployedRegisteredAddress;
				temp.Natural_EmployedRegisteredAddress2 = updateData.Natural_EmployedRegisteredAddress2;
				temp.Natural_EmployedRegisteredAddress3 = updateData.Natural_EmployedRegisteredAddress3;
				temp.Natural_SelfEmployedBusinessName = updateData.Natural_SelfEmployedBusinessName;
				temp.Natural_SelfEmployedRegistrationNo = updateData.Natural_SelfEmployedRegistrationNo;
				temp.Natural_SelfEmployedBusinessAddress = updateData.Natural_SelfEmployedBusinessAddress;
				temp.Natural_SelfEmployedBusinessPrincipalPlace = updateData.Natural_SelfEmployedBusinessPrincipalPlace;
				temp.Shipping_Address1 = updateData.Shipping_Address1;
				temp.Shipping_Address2 = updateData.Shipping_Address2;
				temp.Shipping_Address3 = updateData.Shipping_Address3;
				temp.Shipping_PostalCode = updateData.Shipping_PostalCode;
				temp.SearchTags = updateData.SearchTags;
				temp.Password = updateData.Password;
				temp.ResetPasswordToken = updateData.ResetPasswordToken;
				temp.LastPasswordUpdated = updateData.LastPasswordUpdated;
				temp.IsSubAccount = updateData.IsSubAccount;
				temp.Customer_Title = updateData.Customer_Title;
				temp.Surname = updateData.Surname;
				temp.GivenName = updateData.GivenName;
				temp.hasCustomerAccount = updateData.hasCustomerAccount;
				temp.isVerify = updateData.isVerify;
				temp.VerifyAccountToken = updateData.VerifyAccountToken;
				temp.CreatedOn = updateData.CreatedOn;
				temp.UpdatedOn = updateData.UpdatedOn;
				temp.IsDeleted = updateData.IsDeleted;
				temp.Customer_MainID = updateData.ID;
				temp.EnableTransactionType = updateData.EnableTransactionType;

				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();
				db.Configuration.ValidateOnSaveEnabled = true;

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
				KYC_CustomerParticulars data = db.KYC_CustomerParticulars.Find(id);

				data.IsDeleted = "Y";
				data.UpdatedOn = DateTime.Now;

				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();
				db.Configuration.ValidateOnSaveEnabled = true;

				return true;
			}
			catch
			{
				throw;
			}
		}
	}
}