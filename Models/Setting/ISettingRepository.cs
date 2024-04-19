using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public interface ISettingRepository
    {
        IList<Setting> GetAll();

        Setting FindCode(string code);

        string GetCodeValue(string code);

        bool Add(Setting addData);

        bool Update(int id, string updateValue);

        bool Delete(int id);
    }
}