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
    public class DriverHub : Hub
    {

        private SignalRBusinessLayer _signalRBusinessLayer;
        private ConnectionMapping<string> _mappings;

        public DriverHub([FromServices] SignalRBusinessLayer signalRBusinessLayer,
            [FromServices] ConnectionMapping<string> mappings)
        {
            _signalRBusinessLayer = signalRBusinessLayer;
            _mappings = mappings;
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
            var name = Context.User.Identity.Name;
            //Map client to its group(use group as name)
            _mappings.Add(name, Context.ConnectionId);
        }
        private async Task RemoveClient()
        {

            await Clients.Client(Context.ConnectionId).SetConnectionId(null);
            var name = Context.User.Identity.Name;
            _mappings.Remove(name, Context.ConnectionId);
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

        public async Task MessageSomeone(string name, string message)
        {
           
        }
    }
}
