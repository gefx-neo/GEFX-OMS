using DataAccess;
using DataAccess.POCO;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public class UserRepository : IUserRepository
    {
        private DataAccess.GreatEastForex db;

        public UserRepository()
        {
            db = new DataAccess.GreatEastForex();
        }

        public IQueryable<User> Select()
        {
            var result = from u in db.Users select u;

            return result.Where(e => e.IsDeleted == "N");
        }

        public IQueryable<User> SelectAll()
        {
            var result = from u in db.Users select u;

            return result;
        }

        public IList<User> GetAll()
        {
            try
            {
                IQueryable<User> records = Select();

                return records.OrderByDescending(e => e.CreatedOn).ToList();
            }
            catch
            {
                throw;
            }
        }

        public IList<User> GetAllUsers(string from, string to)
        {
            try
            {
                IQueryable<User> records = SelectAll();

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

                return records.OrderByDescending(e => e.CreatedOn).ToList();
            }
            catch
            {
                throw;
            }
        }

        public User GetSingle(int id)
        {
            try
            {
                IQueryable<User> records = Select();

                return records.Where(e => e.ID == id).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public IPagedList<User> GetPaged(string keyword, int page, int size)
        {
            try
            {
                IQueryable<User> records = Select();

                if (!string.IsNullOrEmpty(keyword))
                {
                    records = records.Where(e => e.NRIC.Contains(keyword) || e.Name.Contains(keyword));
                }

                return records.OrderByDescending(e => e.CreatedOn).ToPagedList(page, size);
            }
            catch
            {
                throw;
            }
        }

        public IPagedList<User> GetUserDataPaged(string from, string to, int page, int size)
        {
            try
            {
                IQueryable<User> records = Select();

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

                return records.OrderByDescending(e => e.CreatedOn).ToPagedList(page, size);
            }
            catch
            {
                throw;
            }
        }

        public User FindNric(string nric)
        {
            try
            {
                IQueryable<User> records = Select();

                return records.Where(e => e.NRIC == nric).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public User FindEmail(string email)
        {
            try
            {
                IQueryable<User> records = Select();

                return records.Where(e => e.Email == email).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public bool Add(User addData)
        {
            try
            {
                addData.CreatedOn = DateTime.Now;
                addData.UpdatedOn = DateTime.Now;
                addData.IsDeleted = "N";
                addData.LastLogin = DateTime.Now;

                db.Users.Add(addData);

                db.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool Update(int id, User updateData)
        {
            try
            {
                User data = db.Users.Find(id);

                data.NRIC = updateData.NRIC;
                data.Name = updateData.Name;
                data.Email = updateData.Email;
                data.Role = updateData.Role;
                data.Status = updateData.Status;
                data.Password = updateData.Password;
                data.UpdatedOn = DateTime.Now;

                db.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool UpdateProfile(int id, User updateData)
        {
            try
            {
                User data = db.Users.Find(id);

                data.Password = updateData.Password;
                data.UpdatedOn = DateTime.Now;

                db.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool UpdateResetPasswordToken(int id, string token)
        {
            try
            {
                User data = db.Users.Find(id);

                data.ResetPasswordToken = token;
                data.UpdatedOn = DateTime.Now;

                db.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool Login(int id)
        {
            try
            {
                User data = db.Users.Find(id);

                data.LastLogin = DateTime.Now;

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
                User data = db.Users.Find(id);

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