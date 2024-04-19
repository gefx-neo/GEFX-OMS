using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public interface IScanOutgoingRepository
    {
        IList<ScanOutgoing> GetAll();

        IList<ScanOutgoing> GetAllScanBy(int id);

        ScanOutgoing GetSingle(int id);

        ScanOutgoing GetSingleScanBy(int id, int userid);

        ScanOutgoing FindMemoID(string memoID);

        bool Add(ScanOutgoing addData);

        bool ConfirmOutgoing(int id);

        bool Delete(int id);
    }
}