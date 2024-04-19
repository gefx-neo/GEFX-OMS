using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public interface IProductInventoryRepository
    {
        ProductInventory GetSingle(int id);

        ProductInventory GetProductInventory(int productid);

        bool Add(ProductInventory addData);

        bool Update(int id, ProductInventory updateData);

		bool Update2(int id, decimal amount, string transactiontype);

		bool Delete(int id);
    }
}