using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RouteLister2.Data;
using RouteLister2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RouteLister2.Services
{
    [Authorize]
    public class DriverHub : Hub<IDriverHub>
    {
        private SignalRBusinessLayer _signalRBusinessLayer;

        public DriverHub([FromServices] SignalRBusinessLayer signalRBusinessLayer)
        {
            _signalRBusinessLayer = signalRBusinessLayer;
        }


        public async Task Connect()
        {
            //Client adds its id
            await AddClient();
        }
        public override async Task OnConnected()
        {
            await AddClient();
            await Clients.Caller.EnableEverything();
            await base.OnConnected();
        }
        //Logg when client went offline mebbe? leavin nothing in this cept default sofar
        public override async Task OnDisconnected(bool stopCalled)
        {
            //Client has disconnected manually
            if (stopCalled)
            {
                await RemoveClient();
            }
            //Client has timed out
            else
            {

            }
            await Clients.Caller.DisableEverything();
            await base.OnDisconnected(stopCalled);
        }

        public async override Task OnReconnected()
        {
            await AddClient();
            await Clients.Caller.EnableEverything();
            await base.OnReconnected();
        }
     

        private async Task AddClient()
        {
            //Set client id
            await Clients.Client(Context.ConnectionId).SetConnectionId(Context.ConnectionId);
            await Groups.Add(Context.ConnectionId, Context.User.Identity.Name);
        }
        private async Task RemoveClient()
        {

            await Clients.Client(Context.ConnectionId).SetConnectionId(null);
            await Groups.Remove(Context.ConnectionId, Context.User.Identity.Name);
        }


     

        /// <summary>
        /// Pushes changes to a client, in this case changes status on a row
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public async Task ChangeStatusOnOrderRow(int id, string idRef)
        {
            try
            {
                var result = await _signalRBusinessLayer.ChangeStatusOnOrderRow(id);
                await Clients.Caller.ChangeClientRowStatus(idRef, result);
            }
            catch (Exception e)
            {
                //TODO error handling
            }
            
        }
    }
}
