using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public interface IScanIncomingRepository
    {
        IList<ScanIncoming> GetAll();

        IList<ScanIncoming> GetAllScanBy(int id);

        ScanIncoming GetSingle(int id);

        ScanIncoming GetSingleScanBy(int id, int userid);

        ScanIncoming FindMemoID(string memoID);

        bool Add(ScanIncoming addData);

        bool ConfirmIncoming(int id);

        bool Delete(int id);
    }
}