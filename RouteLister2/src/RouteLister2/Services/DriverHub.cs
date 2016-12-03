using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RouteLister2.Data;
using RouteLister2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouteLister2.Services
{
    public class DriverHub : Hub<IDriverHub>
    {
        private UnitOfWork _unitOfWork;

        public DriverHub([FromServices] UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public override async Task OnConnected()
        {
            //Set connectionID for connected clients only
            await Clients.Client(Context.ConnectionId).SetConnectionId(Context.ConnectionId);
            //Update the clients status as connect in the database
            if(_unitOfWork.UpdateUserStatus(Context.User.Identity.Name, true))
            {
                await Clients.Client(Context.ConnectionId).SetConnectionStatus(true);
                await base.OnConnected();
            }
            

        }
        //Logg when client went offline mebbe? leavin nothing in this cept default sofar
        public override Task OnDisconnected(bool stopCalled)
        {
            //Update status
            _unitOfWork.UpdateUserStatus(Context.User.Identity.Name, true);
            return base.OnDisconnected(stopCalled);
        }

        public async override Task OnReconnected()
        {
            if (_unitOfWork.UpdateUserStatus(Context.User.Identity.Name, true))
            {
                await Clients.Client(Context.ConnectionId).SetConnectionId(Context.ConnectionId);
                await Clients.Client(Context.ConnectionId).SetConnectionStatus(true);
                await base.OnReconnected();
            }
        }
    }
}
