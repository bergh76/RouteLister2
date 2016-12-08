using RouteLister2.Models;
using RouteLister2.Models.RouteListerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouteLister2.Services
{
    public interface IDriverHub
    {
        Task ChangeStatusOnOrderRow(int id, string idRef);
        Task SetConnectionId(string connectionId);
        Task ChangeClientRowStatus(string idRef,bool status);
        Task EnableEverything();
        Task DisableEverything();
    }
}
