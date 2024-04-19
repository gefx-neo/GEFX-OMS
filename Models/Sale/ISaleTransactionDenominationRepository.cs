using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public interface ISaleTransactionDenominationRepository
    {
        IList<SaleTransactionDenomination> GetTransactionDenominations(int transactionid);

        SaleTransactionDenomination GetSingle(int id);

        bool Add(SaleTransactionDenomination addData);

        bool Delete(int id);
    }
}