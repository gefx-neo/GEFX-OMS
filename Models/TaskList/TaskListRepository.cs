using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public class TaskListRepository : ITaskListRepository
    {
        public IList<TaskList> Sorting(IList<TaskList> taskLists)
        {
            return taskLists.OrderByDescending(e => e.Urgent).OrderBy(e => e.CollectionDate).ThenBy(e => e.CollectionTime.FirstOrDefault()).ThenBy(e => e.ReferenceNo).ToList();
        }

        public IList<TaskList> Sorting2(IList<TaskList> taskLists)
        {
            return taskLists.OrderByDescending(e => e.Urgent).OrderByDescending(e => e.ID).ThenBy(e => e.ReferenceNo).ToList();
        }
    }

}