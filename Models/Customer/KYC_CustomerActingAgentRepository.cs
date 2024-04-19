using DataAccess;
using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public class KYC_CustomerActingAgentRepository : IKYC_CustomerActingAgentRepository
	{
		private DataAccess.GreatEastForex db;

		public KYC_CustomerActingAgentRepository()
		{
			db = new DataAccess.GreatEastForex();
		}

		public IQueryable<KYC_CustomerActingAgents> Select()
		{
			var result = from c in db.KYC_CustomerActingAgents select c;

			return result;
		}

		public KYC_CustomerActingAgents GetSingle(int id)
		{
			try
			{
				IQueryable<KYC_CustomerActingAgents> records = Select();

				return records.Where(e => e.ID == id).FirstOrDefault();
			}
			catch
			{
				throw;
			}
		}

		public KYC_CustomerActingAgents GetActingAgent(int CustomerParticularId)
		{
			try
			{
				IQueryable<KYC_CustomerActingAgents> records = Select();

				return records.Where(e => e.CustomerParticularId == CustomerParticularId).FirstOrDefault();
			}
			catch
			{
				throw;
			}
		}

		public bool Add(List<CustomerActingAgent> addData)
		{
			try
			{
				KYC_CustomerActingAgents temp = new KYC_CustomerActingAgents();
				List<KYC_CustomerActingAgents> templist = new List<KYC_CustomerActingAgents>();

				foreach (CustomerActingAgent agent in addData)
				{
					temp.ActingAgent = agent.ActingAgent;
					temp.Company_CustomerType = agent.Company_CustomerType;
					temp.Company_Address = agent.Company_Address;
					temp.Company_PlaceOfRegistration = agent.Company_PlaceOfRegistration;
					temp.Company_RegistrationNo = agent.Company_RegistrationNo;
					temp.Company_DateOfRegistration = agent.Company_DateOfRegistration;
					temp.Natural_Name = agent.Natural_Name;
					temp.Natural_PermanentAddress = agent.Natural_PermanentAddress;
					temp.Natural_Nationality = agent.Natural_Nationality;
					temp.Natural_ICPassportNo = agent.Natural_ICPassportNo;
					temp.Natural_DOB = agent.Natural_DOB;
					temp.Relationship = agent.Relationship;
					temp.BasisOfAuthority = agent.BasisOfAuthority;
					temp.CustomerParticularId = agent.CustomerParticularId;

					templist.Add(temp);
					temp = new KYC_CustomerActingAgents();
				}

				db.KYC_CustomerActingAgents.AddRange(templist);

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
				KYC_CustomerActingAgents temp = db.KYC_CustomerActingAgents.Where(e => e.ID == id).FirstOrDefault();

				temp.ActingAgent = updateData.ActingAgent;
				temp.Company_CustomerType = updateData.Company_CustomerType;
				temp.Company_Address = updateData.Company_Address;
				temp.Company_PlaceOfRegistration = updateData.Company_PlaceOfRegistration;
				temp.Company_RegistrationNo = updateData.Company_RegistrationNo;
				temp.Company_DateOfRegistration = updateData.Company_DateOfRegistration;
				temp.Natural_Name = updateData.Natural_Name;
				temp.Natural_PermanentAddress = updateData.Natural_PermanentAddress;
				temp.Natural_Nationality = updateData.Natural_Nationality;
				temp.Natural_ICPassportNo = updateData.Natural_ICPassportNo;
				temp.Natural_DOB = updateData.Natural_DOB;
				temp.Relationship = updateData.Relationship;
				temp.BasisOfAuthority = updateData.BasisOfAuthority;

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
				KYC_CustomerActingAgents data = db.KYC_CustomerActingAgents.Find(id);

				db.KYC_CustomerActingAgents.Remove(data);

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