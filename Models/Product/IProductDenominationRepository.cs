using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public interface IProductDenominationRepository
    {
        ProductDenomination GetSingle(int id);

        IList<ProductDenomination> GetProductDenomination(int productid);

        bool Add(ProductDenomination addData);

        bool Delete(int id);
    }
}