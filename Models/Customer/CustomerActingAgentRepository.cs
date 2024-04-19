using DataAccess;
using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public class CustomerActingAgentRepository : ICustomerActingAgentRepository
    {
        private DataAccess.GreatEastForex db;

        public CustomerActingAgentRepository()
        {
            db = new DataAccess.GreatEastForex();
        }

        public IQueryable<CustomerActingAgent> Select()
        {
            var result = from c in db.CustomerActingAgents select c;

            return result;
        }

        public CustomerActingAgent GetSingle(int id)
        {
            try
            {
                IQueryable<CustomerActingAgent> records = Select();

                return records.Where(e => e.ID == id).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public CustomerActingAgent GetActingAgent(int CustomerParticularId)
        {
            try
            {
                IQueryable<CustomerActingAgent> records = Select();

                return records.Where(e => e.CustomerParticularId == CustomerParticularId).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public bool Add(CustomerActingAgent addData)
        {
            try
            {
                db.CustomerActingAgents.Add(addData);

                db.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool Update(int id, CustomerActingAgent updateData)
        {
            try
            {
                CustomerActingAgent data = db.CustomerActingAgents.Find(id);

                data.ActingAgent = updateData.ActingAgent;
                data.Company_CustomerType = updateData.Company_CustomerType;
                data.Company_Address = updateData.Company_Address;
                data.Company_PlaceOfRegistration = updateData.Company_PlaceOfRegistration;
                data.Company_RegistrationNo = updateData.Company_RegistrationNo;
                data.Company_DateOfRegistration = updateData.Company_DateOfRegistration;
                data.Natural_Name = updateData.Natural_Name;
                data.Natural_PermanentAddress = updateData.Natural_PermanentAddress;
                data.Natural_Nationality = updateData.Natural_Nationality;
                data.Natural_ICPassportNo = updateData.Natural_ICPassportNo;
                data.Natural_DOB = updateData.Natural_DOB;
                data.Relationship = updateData.Relationship;
                data.BasisOfAuthority = updateData.BasisOfAuthority;

                db.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

		public bool UpdateReject(int id, Temp_CustomerActingAgents updateData)
		{
			try
			{
				CustomerActingAgent data = db.CustomerActingAgents.Where(e => e.CustomerParticularId == id).FirstOrDefault();

				data.ActingAgent = updateData.ActingAgent;
				data.Company_CustomerType = updateData.Company_CustomerType;
				data.Company_Address = updateData.Company_Address;
				data.Company_PlaceOfRegistration = updateData.Company_PlaceOfRegistration;
				data.Company_RegistrationNo = updateData.Company_RegistrationNo;
				data.Company_DateOfRegistration = updateData.Company_DateOfRegistration;
				data.Natural_Name = updateData.Natural_Name;
				data.Natural_PermanentAddress = updateData.Natural_PermanentAddress;
				data.Natural_Nationality = updateData.Natural_Nationality;
				data.Natural_ICPassportNo = updateData.Natural_ICPassportNo;
				data.Natural_DOB = updateData.Natural_DOB;
				data.Relationship = updateData.Relationship;
				data.BasisOfAuthority = updateData.BasisOfAuthority;

				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool UpdateApproveKYC(int id, KYC_CustomerActingAgents updateData)
		{
			try
			{
				CustomerActingAgent data = db.CustomerActingAgents.Where(e => e.CustomerParticularId == id).FirstOrDefault();

				data.ActingAgent = updateData.ActingAgent;
				data.Company_CustomerType = updateData.Company_CustomerType;
				data.Company_Address = updateData.Company_Address;
				data.Company_PlaceOfRegistration = updateData.Company_PlaceOfRegistration;
				data.Company_RegistrationNo = updateData.Company_RegistrationNo;
				data.Company_DateOfRegistration = updateData.Company_DateOfRegistration;
				data.Natural_Name = updateData.Natural_Name;
				data.Natural_PermanentAddress = updateData.Natural_PermanentAddress;
				data.Natural_Nationality = updateData.Natural_Nationality;
				data.Natural_ICPassportNo = updateData.Natural_ICPassportNo;
				data.Natural_DOB = updateData.Natural_DOB;
				data.Relationship = updateData.Relationship;
				data.BasisOfAuthority = updateData.BasisOfAuthority;

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
                CustomerActingAgent data = db.CustomerActingAgents.Find(id);

                db.CustomerActingAgents.Remove(data);

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
				db.CustomerActingAgents.Where(e => e.CustomerParticularId == id).ToList().ForEach(caa => db.CustomerActingAgents.Remove(caa));
				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool AddSingle(int id, Temp_CustomerActingAgents updateData)
		{
			try
			{
				CustomerActingAgent data = new CustomerActingAgent();

				data.CustomerParticularId = id;
				data.ActingAgent = updateData.ActingAgent;
				data.Company_CustomerType = updateData.Company_CustomerType;
				data.Company_Address = updateData.Company_Address;
				data.Company_PlaceOfRegistration = updateData.Company_PlaceOfRegistration;
				data.Company_RegistrationNo = updateData.Company_RegistrationNo;
				data.Company_DateOfRegistration = updateData.Company_DateOfRegistration;
				data.Natural_Name = updateData.Natural_Name;
				data.Natural_PermanentAddress = updateData.Natural_PermanentAddress;
				data.Natural_Nationality = updateData.Natural_Nationality;
				data.Natural_ICPassportNo = updateData.Natural_ICPassportNo;
				data.Natural_DOB = updateData.Natural_DOB;
				data.Relationship = updateData.Relationship;
				data.BasisOfAuthority = updateData.BasisOfAuthority;

				db.CustomerActingAgents.Add(data);
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