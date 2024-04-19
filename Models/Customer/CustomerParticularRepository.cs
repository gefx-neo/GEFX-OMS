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
using System.Web.Helpers;

namespace GreatEastForex.Models
{
    public class CustomerParticularRepository : ICustomerParticularRepository
    {
        private DataAccess.GreatEastForex db;

        public CustomerParticularRepository()
        {
            db = new DataAccess.GreatEastForex();
        }

        public IQueryable<CustomerParticular> Select()
        {
            var result = from c in db.CustomerParticulars select c;

            return result.Where(e => e.IsDeleted == "N");
        }

        

        public IQueryable<CustomerParticular> SelectAll()
        {
            var result = from c in db.CustomerParticulars select c;

            return result;
        }

        public IList<CustomerParticular> GetAll()
        {
            try
            {
                IQueryable<CustomerParticular> records = Select();

                return records.OrderBy(e => !string.IsNullOrEmpty(e.Company_RegisteredName) ? e.Company_RegisteredName : !string.IsNullOrEmpty(e.Natural_EmployedEmployerName) ? e.Natural_EmployedEmployerName : e.Natural_SelfEmployedBusinessName).ToList();
            }
            catch
            {
                throw;
            }
        }

        public IList<CustomerParticular> GetAllCustomers(string from, string to)
        {
            try
            {
                IQueryable<CustomerParticular> records = Select();

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

        public IList<CustomerParticular> GetAllByStatus(string status)
        {
            try
            {
                IQueryable<CustomerParticular> records = Select();

                return records.Where(e => e.Others.FirstOrDefault().Status == status).OrderBy(e => !string.IsNullOrEmpty(e.Company_RegisteredName) ? e.Company_RegisteredName : !string.IsNullOrEmpty(e.Natural_EmployedEmployerName) ? e.Natural_EmployedEmployerName : e.Natural_SelfEmployedBusinessName).ToList();
            }
            catch
            {
                throw;
            }
        }

		public List<CustomerParticular> GetAllMainCustomer()
		{
			try
			{
				IQueryable<CustomerParticular> records = Select();
				return records.Where(e => e.IsSubAccount == 0 && e.CustomerType == "Corporate & Trading Company" && e.Others.FirstOrDefault().Status == "Active").ToList();
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
                IQueryable<CustomerParticular> records = Select();

                return records.Where(e => e.Others.FirstOrDefault().Status == status).OrderBy(e => !string.IsNullOrEmpty(e.Company_RegisteredName) ? e.Company_RegisteredName : !string.IsNullOrEmpty(e.Natural_EmployedEmployerName) ? e.Natural_EmployedEmployerName : e.Natural_SelfEmployedBusinessName).Count();
            }
            catch
            {
                throw;
            }
        }

        public CustomerParticular GetSingle(int id)
        {
            try
            {
                IQueryable<CustomerParticular> records = Select();

                return records.Where(e => e.ID == id).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public CustomerParticular GetSingle2(int id)
        {
            try
            {
                var records = db.CustomerParticulars.Where(e => e.IsDeleted == "N").ToList();

                return records.Where(e => e.ID == id).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

		public CustomerParticular GetSingle3(int id)
		{
			try
			{
				var records = db.CustomerParticulars.Where(e => e.ID == id).FirstOrDefault();

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

        public IPagedList<CustomerParticular> GetCustomerDataPaged(string from, string to, int page, int size)
        {
            try
            {
                IQueryable<CustomerParticular> records = Select();

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

        public CustomerParticular FindCustomerCode(string code)
        {
            try
            {
                IQueryable<CustomerParticular> records = Select();

                return records.Where(e => e.CustomerCode == code).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public CustomerParticular FindICPassport(string icpassport)
        {
            try
            {
                IQueryable<CustomerParticular> records = Select();

                return records.Where(e => e.Natural_ICPassportNo == icpassport).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public CustomerParticular FindEmail(string email)
        {
            try
            {
                IQueryable<CustomerParticular> records = Select();

                return records.Where(e => e.Company_Email == email || e.Natural_Email == email).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

		public int FindEmailNotOwn(int id, string email)
		{
			try
			{
				IQueryable<CustomerParticular> records = Select();

				return records.Where(e => (e.Company_Email == email || e.Natural_Email == email) && e.ID != id).Count();
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
                addData.CreatedOn = DateTime.Now;
                addData.UpdatedOn = DateTime.Now;
                addData.IsDeleted = "N";
				addData.isKYCVerify = 0;

                db.CustomerParticulars.Add(addData);

                db.SaveChanges();

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
                CustomerParticular data = db.CustomerParticulars.Find(id);

                data.CustomerCode = updateData.CustomerCode;
                data.CustomerType = updateData.CustomerType;
                data.Company_RegisteredName = updateData.Company_RegisteredName;
                //data.Company_RegisteredAddress = updateData.Company_RegisteredAddress;
                data.Company_BusinessAddress1 = updateData.Company_BusinessAddress1;
                data.Company_BusinessAddress2 = updateData.Company_BusinessAddress2;
				data.Company_BusinessAddress3 = updateData.Company_BusinessAddress3;
				data.Company_PostalCode = updateData.Company_PostalCode;
                data.Company_ContactName = updateData.Company_ContactName;
                data.Company_TelNo = updateData.Company_TelNo;
				data.Company_ContactNoH = updateData.Company_ContactNoH;
				data.Company_ContactNoO = updateData.Company_ContactNoO;
				data.Company_ContactNoM = updateData.Company_ContactNoM;
				data.Company_ICPassport = updateData.Company_ICPassport;
				data.Company_JobTitle = updateData.Company_JobTitle;
				data.Company_Nationality = updateData.Company_Nationality;
				data.Company_Country = updateData.Company_Country;
				data.Company_CountryCode = updateData.Company_CountryCode;
				data.Company_FaxNo = updateData.Company_FaxNo;
                data.Company_Email = updateData.Company_Email;
                data.Company_PlaceOfRegistration = updateData.Company_PlaceOfRegistration;
                data.Company_DateOfRegistration = updateData.Company_DateOfRegistration;
                data.Company_RegistrationNo = updateData.Company_RegistrationNo;
                data.Company_TypeOfEntity = updateData.Company_TypeOfEntity;
                data.Company_TypeOfEntityIfOthers = updateData.Company_TypeOfEntityIfOthers;
                data.Company_PurposeAndIntended = updateData.Company_PurposeAndIntended;
				data.DOB = updateData.DOB;
				data.Natural_Name = updateData.Natural_Name;
                data.Natural_PermanentAddress = updateData.Natural_PermanentAddress;
				data.Natural_PermanentAddress2 = updateData.Natural_PermanentAddress2;
				data.Natural_PermanentAddress3 = updateData.Natural_PermanentAddress3;
				data.Natural_PermanentPostalCode = updateData.Natural_PermanentPostalCode;
				data.Natural_MailingAddress = updateData.Natural_MailingAddress;
				data.Natural_MailingAddress2 = updateData.Natural_MailingAddress2;
				data.Natural_MailingAddress3 = updateData.Natural_MailingAddress3;
				data.Mailing_PostalCode = updateData.Mailing_PostalCode;
				data.Natural_Nationality = updateData.Natural_Nationality;
                data.Natural_ICPassportNo = updateData.Natural_ICPassportNo;
                data.Natural_DOB = updateData.Natural_DOB;
                data.Natural_ContactNoH = updateData.Natural_ContactNoH;
                data.Natural_ContactNoO = updateData.Natural_ContactNoO;
                data.Natural_ContactNoM = updateData.Natural_ContactNoM;
                data.Natural_Email = updateData.Natural_Email;
                data.Natural_EmploymentType = updateData.Natural_EmploymentType;
                data.Natural_EmployedEmployerName = updateData.Natural_EmployedEmployerName;
                data.Natural_EmployedJobTitle = updateData.Natural_EmployedJobTitle;
                data.Natural_EmployedRegisteredAddress = updateData.Natural_EmployedRegisteredAddress;
				data.Natural_EmployedRegisteredAddress2 = updateData.Natural_EmployedRegisteredAddress2;
				data.Natural_EmployedRegisteredAddress3 = updateData.Natural_EmployedRegisteredAddress3;
				data.Natural_SelfEmployedBusinessName = updateData.Natural_SelfEmployedBusinessName;
                data.Natural_SelfEmployedRegistrationNo = updateData.Natural_SelfEmployedRegistrationNo;
                data.Natural_SelfEmployedBusinessAddress = updateData.Natural_SelfEmployedBusinessAddress;
                data.Natural_SelfEmployedBusinessPrincipalPlace = updateData.Natural_SelfEmployedBusinessPrincipalPlace;
				data.Shipping_Address1 = updateData.Shipping_Address1;
				data.Shipping_Address2 = updateData.Shipping_Address2;
				data.Shipping_Address3 = updateData.Shipping_Address3;
				data.Shipping_PostalCode = updateData.Shipping_PostalCode;
				data.Customer_Title = updateData.Customer_Title;
				data.Password = updateData.Password;
				data.ResetPasswordToken = updateData.ResetPasswordToken;
				data.LastPasswordUpdated = updateData.LastPasswordUpdated;
				data.Surname = updateData.Surname;
				data.GivenName = updateData.GivenName;
				data.SearchTags = updateData.SearchTags;
				data.UpdatedOn = DateTime.Now;
				data.hasCustomerAccount = updateData.hasCustomerAccount;
				data.isVerify = updateData.isVerify;
				data.VerifyAccountToken = updateData.VerifyAccountToken;
				data.lastTempPageUpdate = updateData.lastTempPageUpdate;
				data.EnableTransactionType = updateData.EnableTransactionType;
				data.IsSubAccount = updateData.IsSubAccount;

                db.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

		public bool UpdateEmergency(CustomerParticular updateData)
		{
			try
			{
				CustomerParticular data = db.CustomerParticulars.Find(updateData.ID);

				data.CustomerType = updateData.CustomerType;

				if (updateData.CustomerType == "Corporate & Trading Company")
				{
					data.Company_Email = updateData.Company_Email;
					data.Natural_Email = null;
				}
				else
				{
					data.Company_Email = null;
					data.Natural_Email = updateData.Natural_Email;
				}

				if (!string.IsNullOrEmpty(updateData.Password))
				{
					data.Password = updateData.Password;
				}

				data.Customer_Title = updateData.Customer_Title;
				data.Surname = updateData.Surname;
				data.GivenName = updateData.GivenName;

				data.isVerify = updateData.isVerify;

				if (updateData.isVerify == 1)
				{
					data.VerifyAccountToken = null;
				}
				else
				{
					data.VerifyAccountToken = updateData.VerifyAccountToken;
				}

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

		public bool UpdateApproveKYC(int id)
		{
			try
			{
				CustomerParticular data = db.CustomerParticulars.Where(e => e.ID == id).FirstOrDefault();

				if (data != null)
				{
					data.isKYCVerify = 1;
					data.UpdatedOn = DateTime.Now;
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

		public bool UpdateReject(int id, Temp_CustomerParticulars updateData)
		{
			try
			{
				CustomerParticular data = db.CustomerParticulars.Where(e => e.ID == id).FirstOrDefault();

				data.CustomerCode = updateData.CustomerCode;
				data.CustomerType = updateData.CustomerType;
				data.Company_RegisteredName = updateData.Company_RegisteredName;
				data.Company_RegisteredAddress = updateData.Company_RegisteredAddress;
				data.Company_BusinessAddress1 = updateData.Company_BusinessAddress1;
				data.Company_BusinessAddress2 = updateData.Company_BusinessAddress2;
				data.Company_BusinessAddress3 = updateData.Company_BusinessAddress3;
				data.Company_PostalCode = updateData.Company_PostalCode;
				data.Company_ContactName = updateData.Company_ContactName;
				data.Company_TelNo = updateData.Company_TelNo;
				data.Company_ContactNoH = updateData.Company_ContactNoH;
				data.Company_ContactNoO = updateData.Company_ContactNoO;
				data.Company_ContactNoM = updateData.Company_ContactNoM;
				data.Company_ICPassport = updateData.Company_ICPassport;
				data.Company_JobTitle = updateData.Company_JobTitle;
				data.Company_Nationality = updateData.Company_Nationality;
				data.Company_Country = updateData.Company_Country;
				data.Company_CountryCode = updateData.Company_CountryCode;
				data.Company_FaxNo = updateData.Company_FaxNo;
				data.Company_Email = updateData.Company_Email;
				data.Company_PlaceOfRegistration = updateData.Company_PlaceOfRegistration;
				data.Company_DateOfRegistration = updateData.Company_DateOfRegistration;
				data.Company_RegistrationNo = updateData.Company_RegistrationNo;
				data.Company_TypeOfEntity = updateData.Company_TypeOfEntity;
				data.Company_TypeOfEntityIfOthers = updateData.Company_TypeOfEntityIfOthers;
				data.Company_PurposeAndIntended = updateData.Company_PurposeAndIntended;
				data.DOB = updateData.DOB;
				data.Natural_Name = updateData.Natural_Name;
				data.Natural_PermanentAddress = updateData.Natural_PermanentAddress;
				data.Natural_PermanentAddress2 = updateData.Natural_PermanentAddress2;
				data.Natural_PermanentAddress3 = updateData.Natural_PermanentAddress3;
				data.Natural_PermanentPostalCode = updateData.Natural_PermanentPostalCode;
				data.Natural_MailingAddress = updateData.Natural_MailingAddress;
				data.Natural_MailingAddress2 = updateData.Natural_MailingAddress2;
				data.Natural_MailingAddress3 = updateData.Natural_MailingAddress3;
				data.Mailing_PostalCode = updateData.Mailing_PostalCode;
				data.Natural_Nationality = updateData.Natural_Nationality;
				data.Natural_ICPassportNo = updateData.Natural_ICPassportNo;
				data.Natural_DOB = updateData.Natural_DOB;
				data.Natural_ContactNoH = updateData.Natural_ContactNoH;
				data.Natural_ContactNoO = updateData.Natural_ContactNoO;
				data.Natural_ContactNoM = updateData.Natural_ContactNoM;
				data.Natural_Email = updateData.Natural_Email;
				data.Natural_EmploymentType = updateData.Natural_EmploymentType;
				data.Natural_EmployedEmployerName = updateData.Natural_EmployedEmployerName;
				data.Natural_EmployedJobTitle = updateData.Natural_EmployedJobTitle;
				data.Natural_EmployedRegisteredAddress = updateData.Natural_EmployedRegisteredAddress;
				data.Natural_EmployedRegisteredAddress2 = updateData.Natural_EmployedRegisteredAddress2;
				data.Natural_EmployedRegisteredAddress3 = updateData.Natural_EmployedRegisteredAddress3;
				data.Natural_SelfEmployedBusinessName = updateData.Natural_SelfEmployedBusinessName;
				data.Natural_SelfEmployedRegistrationNo = updateData.Natural_SelfEmployedRegistrationNo;
				data.Natural_SelfEmployedBusinessAddress = updateData.Natural_SelfEmployedBusinessAddress;
				data.Natural_SelfEmployedBusinessPrincipalPlace = updateData.Natural_SelfEmployedBusinessPrincipalPlace;
				data.Shipping_Address1 = updateData.Shipping_Address1;
				data.Shipping_Address2 = updateData.Shipping_Address2;
				data.Shipping_Address3 = updateData.Shipping_Address3;
				data.Shipping_PostalCode = updateData.Shipping_PostalCode;
				data.SearchTags = updateData.SearchTags;
				data.Password = updateData.Password;
				data.ResetPasswordToken = updateData.ResetPasswordToken;
				data.LastPasswordUpdated = updateData.LastPasswordUpdated;
				data.IsSubAccount = updateData.IsSubAccount;
				data.Customer_Title = updateData.Customer_Title;
				data.Surname = updateData.Surname;
				data.GivenName = updateData.GivenName;
				data.hasCustomerAccount = updateData.hasCustomerAccount;
				data.isVerify = updateData.isVerify;
				data.VerifyAccountToken = updateData.VerifyAccountToken;
				data.UpdatedOn = DateTime.Now;
				data.IsDeleted = updateData.IsDeleted;
				data.EnableTransactionType = updateData.EnableTransactionType;
				data.lastTempPageUpdate = 0;
				data.isKYCVerify = 1;
				data.lastKYCPageUpdate = 0;

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

		public bool UpdateRejectZeroKYC(int id, Temp_CustomerParticulars updateData, string status = "")
		{
			try
			{
				CustomerParticular data = db.CustomerParticulars.Where(e => e.ID == id).FirstOrDefault();

				data.CustomerCode = updateData.CustomerCode;
				data.CustomerType = updateData.CustomerType;
				data.Company_RegisteredName = updateData.Company_RegisteredName;
				data.Company_RegisteredAddress = updateData.Company_RegisteredAddress;
				data.Company_BusinessAddress1 = updateData.Company_BusinessAddress1;
				data.Company_BusinessAddress2 = updateData.Company_BusinessAddress2;
				data.Company_BusinessAddress3 = updateData.Company_BusinessAddress3;
				data.Company_PostalCode = updateData.Company_PostalCode;
				data.Company_ContactName = updateData.Company_ContactName;
				data.Company_TelNo = updateData.Company_TelNo;
				data.Company_ContactNoH = updateData.Company_ContactNoH;
				data.Company_ContactNoO = updateData.Company_ContactNoO;
				data.Company_ContactNoM = updateData.Company_ContactNoM;
				data.Company_ICPassport = updateData.Company_ICPassport;
				data.Company_JobTitle = updateData.Company_JobTitle;
				data.Company_Nationality = updateData.Company_Nationality;
				data.Company_Country = updateData.Company_Country;
				data.Company_CountryCode = updateData.Company_CountryCode;
				data.Company_FaxNo = updateData.Company_FaxNo;
				data.Company_Email = updateData.Company_Email;
				data.Company_PlaceOfRegistration = updateData.Company_PlaceOfRegistration;
				data.Company_DateOfRegistration = updateData.Company_DateOfRegistration;
				data.Company_RegistrationNo = updateData.Company_RegistrationNo;
				data.Company_TypeOfEntity = updateData.Company_TypeOfEntity;
				data.Company_TypeOfEntityIfOthers = updateData.Company_TypeOfEntityIfOthers;
				data.Company_PurposeAndIntended = updateData.Company_PurposeAndIntended;
				data.DOB = updateData.DOB;
				data.Natural_Name = updateData.Natural_Name;
				data.Natural_PermanentAddress = updateData.Natural_PermanentAddress;
				data.Natural_PermanentAddress2 = updateData.Natural_PermanentAddress2;
				data.Natural_PermanentAddress3 = updateData.Natural_PermanentAddress3;
				data.Natural_PermanentPostalCode = updateData.Natural_PermanentPostalCode;
				data.Natural_MailingAddress = updateData.Natural_MailingAddress;
				data.Natural_MailingAddress2 = updateData.Natural_MailingAddress2;
				data.Natural_MailingAddress3 = updateData.Natural_MailingAddress3;
				data.Mailing_PostalCode = updateData.Mailing_PostalCode;
				data.Natural_Nationality = updateData.Natural_Nationality;
				data.Natural_ICPassportNo = updateData.Natural_ICPassportNo;
				data.Natural_DOB = updateData.Natural_DOB;
				data.Natural_ContactNoH = updateData.Natural_ContactNoH;
				data.Natural_ContactNoO = updateData.Natural_ContactNoO;
				data.Natural_ContactNoM = updateData.Natural_ContactNoM;
				data.Natural_Email = updateData.Natural_Email;
				data.Natural_EmploymentType = updateData.Natural_EmploymentType;
				data.Natural_EmployedEmployerName = updateData.Natural_EmployedEmployerName;
				data.Natural_EmployedJobTitle = updateData.Natural_EmployedJobTitle;
				data.Natural_EmployedRegisteredAddress = updateData.Natural_EmployedRegisteredAddress;
				data.Natural_EmployedRegisteredAddress2 = updateData.Natural_EmployedRegisteredAddress2;
				data.Natural_EmployedRegisteredAddress3 = updateData.Natural_EmployedRegisteredAddress3;
				data.Natural_SelfEmployedBusinessName = updateData.Natural_SelfEmployedBusinessName;
				data.Natural_SelfEmployedRegistrationNo = updateData.Natural_SelfEmployedRegistrationNo;
				data.Natural_SelfEmployedBusinessAddress = updateData.Natural_SelfEmployedBusinessAddress;
				data.Natural_SelfEmployedBusinessPrincipalPlace = updateData.Natural_SelfEmployedBusinessPrincipalPlace;
				data.Shipping_Address1 = updateData.Shipping_Address1;
				data.Shipping_Address2 = updateData.Shipping_Address2;
				data.Shipping_Address3 = updateData.Shipping_Address3;
				data.Shipping_PostalCode = updateData.Shipping_PostalCode;
				data.SearchTags = updateData.SearchTags;
				data.Password = updateData.Password;
				data.ResetPasswordToken = updateData.ResetPasswordToken;
				data.LastPasswordUpdated = updateData.LastPasswordUpdated;
				data.IsSubAccount = updateData.IsSubAccount;
				data.Customer_Title = updateData.Customer_Title;
				data.Surname = updateData.Surname;
				data.GivenName = updateData.GivenName;
				data.hasCustomerAccount = updateData.hasCustomerAccount;
				data.isVerify = updateData.isVerify;
				data.VerifyAccountToken = updateData.VerifyAccountToken;
				data.UpdatedOn = DateTime.Now;
				data.IsDeleted = updateData.IsDeleted;
				data.EnableTransactionType = updateData.EnableTransactionType;

				if (status == "Rejected")
				{
					data.lastKYCPageUpdate = 1;
				}

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

		public bool UpdateApproveKYC(int id, KYC_CustomerParticulars updateData)
		{
			try
			{
				CustomerParticular data = db.CustomerParticulars.Where(e => e.ID == id).FirstOrDefault();

				data.CustomerCode = updateData.CustomerCode;
				data.CustomerType = updateData.CustomerType;
				data.Company_RegisteredName = updateData.Company_RegisteredName;
				data.Company_RegisteredAddress = updateData.Company_RegisteredAddress;
				data.Company_BusinessAddress1 = updateData.Company_BusinessAddress1;
				data.Company_BusinessAddress2 = updateData.Company_BusinessAddress2;
				data.Company_BusinessAddress3 = updateData.Company_BusinessAddress3;
				data.Company_PostalCode = updateData.Company_PostalCode;
				data.Company_ContactName = updateData.Company_ContactName;
				data.Company_TelNo = updateData.Company_TelNo;
				data.Company_ContactNoH = updateData.Company_ContactNoH;
				data.Company_ContactNoO = updateData.Company_ContactNoO;
				data.Company_ContactNoM = updateData.Company_ContactNoM;
				data.Company_ICPassport = updateData.Company_ICPassport;
				data.Company_JobTitle = updateData.Company_JobTitle;
				data.Company_Nationality = updateData.Company_Nationality;
				data.Company_Country = updateData.Company_Country;
				data.Company_CountryCode = updateData.Company_CountryCode;
				data.Company_FaxNo = updateData.Company_FaxNo;
				data.Company_Email = updateData.Company_Email;
				data.Company_PlaceOfRegistration = updateData.Company_PlaceOfRegistration;
				data.Company_DateOfRegistration = updateData.Company_DateOfRegistration;
				data.Company_RegistrationNo = updateData.Company_RegistrationNo;
				data.Company_TypeOfEntity = updateData.Company_TypeOfEntity;
				data.Company_TypeOfEntityIfOthers = updateData.Company_TypeOfEntityIfOthers;
				data.Company_PurposeAndIntended = updateData.Company_PurposeAndIntended;
				data.DOB = updateData.DOB;
				data.Natural_Name = updateData.Natural_Name;
				data.Natural_PermanentAddress = updateData.Natural_PermanentAddress;
				data.Natural_PermanentAddress2 = updateData.Natural_PermanentAddress2;
				data.Natural_PermanentAddress3 = updateData.Natural_PermanentAddress3;
				data.Natural_PermanentPostalCode = updateData.Natural_PermanentPostalCode;
				data.Natural_MailingAddress = updateData.Natural_MailingAddress;
				data.Natural_MailingAddress2 = updateData.Natural_MailingAddress2;
				data.Natural_MailingAddress3 = updateData.Natural_MailingAddress3;
				data.Mailing_PostalCode = updateData.Mailing_PostalCode;
				data.Natural_Nationality = updateData.Natural_Nationality;
				data.Natural_ICPassportNo = updateData.Natural_ICPassportNo;
				data.Natural_DOB = updateData.Natural_DOB;
				data.Natural_ContactNoH = updateData.Natural_ContactNoH;
				data.Natural_ContactNoO = updateData.Natural_ContactNoO;
				data.Natural_ContactNoM = updateData.Natural_ContactNoM;
				data.Natural_Email = updateData.Natural_Email;
				data.Natural_EmploymentType = updateData.Natural_EmploymentType;
				data.Natural_EmployedEmployerName = updateData.Natural_EmployedEmployerName;
				data.Natural_EmployedJobTitle = updateData.Natural_EmployedJobTitle;
				data.Natural_EmployedRegisteredAddress = updateData.Natural_EmployedRegisteredAddress;
				data.Natural_EmployedRegisteredAddress2 = updateData.Natural_EmployedRegisteredAddress2;
				data.Natural_EmployedRegisteredAddress3 = updateData.Natural_EmployedRegisteredAddress3;
				data.Natural_SelfEmployedBusinessName = updateData.Natural_SelfEmployedBusinessName;
				data.Natural_SelfEmployedRegistrationNo = updateData.Natural_SelfEmployedRegistrationNo;
				data.Natural_SelfEmployedBusinessAddress = updateData.Natural_SelfEmployedBusinessAddress;
				data.Natural_SelfEmployedBusinessPrincipalPlace = updateData.Natural_SelfEmployedBusinessPrincipalPlace;
				data.Shipping_Address1 = updateData.Shipping_Address1;
				data.Shipping_Address2 = updateData.Shipping_Address2;
				data.Shipping_Address3 = updateData.Shipping_Address3;
				data.Shipping_PostalCode = updateData.Shipping_PostalCode;
				data.SearchTags = updateData.SearchTags;
				data.Password = updateData.Password;
				data.ResetPasswordToken = updateData.ResetPasswordToken;
				data.LastPasswordUpdated = updateData.LastPasswordUpdated;
				data.IsSubAccount = updateData.IsSubAccount;
				data.Customer_Title = updateData.Customer_Title;
				data.Surname = updateData.Surname;
				data.GivenName = updateData.GivenName;
				data.hasCustomerAccount = updateData.hasCustomerAccount;
				data.isVerify = updateData.isVerify;
				data.VerifyAccountToken = updateData.VerifyAccountToken;
				data.UpdatedOn = DateTime.Now;
				data.IsDeleted = updateData.IsDeleted;
				data.EnableTransactionType = updateData.EnableTransactionType;

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
                CustomerParticular data = db.CustomerParticulars.Find(id);

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

		public bool UpdateStatus(int id, int lastTempPageUpdate = 0)
		{
			try
			{
				CustomerParticular data = db.CustomerParticulars.Where(e => e.ID == id).FirstOrDefault();

				data.lastTempPageUpdate = lastTempPageUpdate;
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