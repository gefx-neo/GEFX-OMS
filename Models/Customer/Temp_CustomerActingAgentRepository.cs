using DataAccess;
using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public class Temp_CustomerActingAgentRepository : ITemp_CustomerActingAgentRepository
	{
		private DataAccess.GreatEastForex db;

		public Temp_CustomerActingAgentRepository()
		{
			db = new DataAccess.GreatEastForex();
		}

		public IQueryable<Temp_CustomerActingAgents> Select()
		{
			var result = from c in db.Temp_CustomerActingAgents select c;

			return result;
		}

		public Temp_CustomerActingAgents GetSingle(int id)
		{
			try
			{
				IQueryable<Temp_CustomerActingAgents> records = Select();

				return records.Where(e => e.ID == id).FirstOrDefault();
			}
			catch
			{
				throw;
			}
		}

		public Temp_CustomerActingAgents GetActingAgent(int CustomerParticularId)
		{
			try
			{
				IQueryable<Temp_CustomerActingAgents> records = Select();

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
				Temp_CustomerActingAgents temp = new Temp_CustomerActingAgents();
				List<Temp_CustomerActingAgents> templist = new List<Temp_CustomerActingAgents>();

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
					temp = new Temp_CustomerActingAgents();
				}

				db.Temp_CustomerActingAgents.AddRange(templist);

				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool AddSingle(CustomerActingAgent addData)
		{
			try
			{
				Temp_CustomerActingAgents temp = new Temp_CustomerActingAgents();

				temp.ActingAgent = addData.ActingAgent;
				temp.Company_CustomerType = addData.Company_CustomerType;
				temp.Company_Address = addData.Company_Address;
				temp.Company_PlaceOfRegistration = addData.Company_PlaceOfRegistration;
				temp.Company_RegistrationNo = addData.Company_RegistrationNo;
				temp.Company_DateOfRegistration = addData.Company_DateOfRegistration;
				temp.Natural_Name = addData.Natural_Name;
				temp.Natural_PermanentAddress = addData.Natural_PermanentAddress;
				temp.Natural_Nationality = addData.Natural_Nationality;
				temp.Natural_ICPassportNo = addData.Natural_ICPassportNo;
				temp.Natural_DOB = addData.Natural_DOB;
				temp.Relationship = addData.Relationship;
				temp.BasisOfAuthority = addData.BasisOfAuthority;
				temp.CustomerParticularId = addData.CustomerParticularId;

				db.Temp_CustomerActingAgents.Add(temp);

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
				Temp_CustomerActingAgents temp = db.Temp_CustomerActingAgents.Where(e => e.ID == id).FirstOrDefault();

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
				Temp_CustomerActingAgents data = db.Temp_CustomerActingAgents.Find(id);

				db.Temp_CustomerActingAgents.Remove(data);

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