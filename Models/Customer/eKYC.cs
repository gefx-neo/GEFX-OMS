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
    public class eKYCRepository
    {
        private DataAccess.GreatEastForex db;

        public eKYCRepository()
        {
            db = new DataAccess.GreatEastForex();
        }

        public IQueryable<KYC> Select()
        {
            var result = from c in db.eKYC select c;

            return result;
        }

        public int FindArtemisID(int id)
        {
            try
            {
                IQueryable<KYC> records = Select();
                var rec = records.Where(e => e.cust_id == id).FirstOrDefault();
                if(rec != null)
                {
                    return rec.Artemis_custId;
                }
                return 0;
            }
            catch
            {
                throw;
            }
        }

    }
}