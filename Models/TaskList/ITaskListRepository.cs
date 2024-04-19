using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public interface ITaskListRepository
    {
        IList<TaskList> Sorting(IList<TaskList> taskLists);
        IList<TaskList> Sorting2(IList<TaskList> taskLists);
    }
}