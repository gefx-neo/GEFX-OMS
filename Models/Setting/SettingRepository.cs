using DataAccess;
using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public class SettingRepository : ISettingRepository
    {
        private DataAccess.GreatEastForex db;

        public SettingRepository()
        {
            db = new DataAccess.GreatEastForex();
        }

        public IQueryable<Setting> Select()
        {
            var result = from s in db.Settings select s;

            return result;
        }

        public IList<Setting> GetAll()
        {
            try
            {
                IQueryable<Setting> records = Select();

                return records.OrderBy(e => e.Code).ToList();
            }
            catch
            {
                throw;
            }
        }

        public Setting FindCode(string code)
        {
            try
            {
                IQueryable<Setting> records = Select();

                if (!string.IsNullOrEmpty(code))
                {
                    records = records.Where(e => e.Code == code);
                }

                return records.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public string GetCodeValue(string code)
        {
            try
            {
                IQueryable<Setting> records = Select();

                if (!string.IsNullOrEmpty(code))
                {
                    records = records.Where(e => e.Code == code);
                }

                return records.FirstOrDefault().Value;
            }
            catch
            {
                throw;
            }
        }

        public bool Add(Setting addData)
        {
            try
            {
                db.Settings.Add(addData);

                db.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool Update(int id, string updateValue)
        {
            try
            {
                Setting data = db.Settings.Find(id);

                data.Value = updateValue;

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
                Setting data = db.Settings.Find(id);

                db.Settings.Remove(data);

                db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}