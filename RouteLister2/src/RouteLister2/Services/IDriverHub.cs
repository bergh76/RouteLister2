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
        Task SetConnectionStatus(bool status);
        Task UpdateClient(RouteListViewModel clientModel);
        Task ChangeRowStatus(int id);
        Task Message(Message message);
        Task SetConnectionId(string connectionId);
    }
}
