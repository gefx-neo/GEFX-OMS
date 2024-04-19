using DataAccess.POCO;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public interface IUserRepository
    {
        IList<User> GetAll();

        IList<User> GetAllUsers(string from, string to);

        User GetSingle(int id);

        IPagedList<User> GetPaged(string keyword, int page, int size);

        IPagedList<User> GetUserDataPaged(string from, string to, int page, int size);

        User FindNric(string nric);

        User FindEmail(string email);

        bool Add(User addData);

        bool Update(int id, User updateData);

        bool UpdateProfile(int id, User updateData);

        bool UpdateResetPasswordToken(int id, string token);

        bool Login(int id);

        bool Delete(int id);
    }
}