using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public interface IInventoryRepository
    {
        IList<Inventory> GetAll(int productid);

        bool Add(Inventory addData);

        bool Update(int id, Inventory updateData);

        bool Delete(int id);

        IEnumerable<Inventory> ViewlogPagination(int page, int id);

        int GetTotalCount(int id);
    }
}