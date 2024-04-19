using DataAccess;
using DataAccess.POCO;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public class AgentRepository : IAgentRepository
	{
		private DataAccess.GreatEastForex db;

		public AgentRepository()
		{
			db = new DataAccess.GreatEastForex();
		}

		public IQueryable<Agents> Select()
		{
			var result = from u in db.Agents select u;

			return result.Where(e => e.IsDeleted == "N");
		}

		public IQueryable<Agents> SelectAll()
		{
			var result = from u in db.Agents select u;

			return result;
		}

		public IList<Agents> GetAll()
		{
			try
			{
				IQueryable<Agents> records = Select();

				return records.OrderByDescending(e => e.CreatedOn).ToList();
			}
			catch
			{
				throw;
			}
		}

		public Agents GetSingle(int id)
		{
			try
			{
				IQueryable<Agents> records = Select();

				return records.Where(e => e.ID == id).FirstOrDefault();
			}
			catch
			{
				throw;
			}
		}

		public Agents FindAgentId(string AgentId)
		{
			try
			{
				IQueryable<Agents> records = Select();

				return records.Where(e => e.AgentId == AgentId).FirstOrDefault();
			}
			catch
			{
				throw;
			}
		}

		public Agents FindAgentIdNotOwnSelf(int id, string AgentId)
		{
			try
			{
				IQueryable<Agents> records = Select();

				return records.Where(e => e.AgentId == AgentId && e.ID != id).FirstOrDefault();
			}
			catch
			{
				throw;
			}
		}

		public IPagedList<Agents> GetPaged(string keyword, int page, int size)
		{
			try
			{
				IQueryable<Agents> records = Select();

				if (!string.IsNullOrEmpty(keyword))
				{
					records = records.Where(e => e.AgentId.Contains(keyword) || e.CompanyName.Contains(keyword));
				}

				return records.OrderByDescending(e => e.CreatedOn).ToPagedList(page, size);
			}
			catch
			{
				throw;
			}
		}

		public bool Add(Agents addData)
		{
			try
			{
				addData.CreatedOn = DateTime.Now;
				addData.UpdatedOn = DateTime.Now;
				addData.IsDeleted = "N";

				db.Agents.Add(addData);

				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool Update(int id, Agents updateData)
		{
			try
			{
				Agents data = db.Agents.Find(id);

				data.AgentId = updateData.AgentId;
				data.CompanyName = updateData.CompanyName;
				data.ContactNumber = updateData.ContactNumber;
				data.ContactPerson = updateData.ContactPerson;
				data.Status = updateData.Status;
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
				Agents data = db.Agents.Find(id);

				data.IsDeleted = "Y";
				data.UpdatedOn = DateTime.Now;

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