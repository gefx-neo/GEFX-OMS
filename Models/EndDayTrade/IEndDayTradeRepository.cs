using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public interface IEndDayTradeRepository
    {
        IList<EndDayTrade> GetAll(string activationTime);

        List<EndDayTrade> GetAll(DateTime lastApproval);

        IList<EndDayTrade> GetAllDAT(string fromDate, string toDate);

        EndDayTrade GetProductTrade(int productId, string activationTime = null);

        EndDayTrade GetLastRecord();

        EndDayTrade GetProductPreviousTrade(int productId, DateTime lastActivationTime);

        EndDayTrade GetProductLastTrade(int productId, string activationDate = null);

        EndDayTrade GetProductNextTrade(int productId, DateTime currentActivation);

        EndDayTrade GetProductFutureTrade(int productId, DateTime currentActivation);

        EndDayTrade GetProductCurrentTrade(int productId, DateTime lastApproval);

        bool Add(EndDayTrade addData);

        bool Update(int id, EndDayTrade updateData);

        bool Delete(int id);
    }
}