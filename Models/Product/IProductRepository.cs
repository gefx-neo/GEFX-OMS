using DataAccess.POCO;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public interface IProductRepository
    {
        IList<Product> GetAll();

        IList<Product> GetAll(string transactionType);

        IList<Product> GetAll2(string transactionType);

        IList<Product> GetAll(List<string> transactionType);

        IList<Product> GetAllProducts(string from, string to);

        Product GetSingle(int id);

        IPagedList<Product> GetPaged(string keyword, int page, int size);

        IPagedList<Product> GetProductDataPaged(string from, string to, int page, int size);

        Product FindCurrencyCode(string code);

        bool Add(Product addData);

        bool Update(int id, Product updateData);

        bool Delete(int id);

    }
}