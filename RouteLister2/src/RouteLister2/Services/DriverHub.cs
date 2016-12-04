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
    public class DriverHub : Hub<IDriverHub>
    {
        private UnitOfWork _unitOfWork;
        private UserManager<ApplicationUser> _userManager;

        public DriverHub([FromServices] UnitOfWork unitOfWork, [FromServices] UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<bool> Connect()
        {
            if (await IsUserValid())
            {
                return true;
            }
            return false;
        }

        public override async Task OnConnected()
        {
            await UpdateUserStatus(true);
            await base.OnConnected();
        }
        //Logg when client went offline mebbe? leavin nothing in this cept default sofar
        public override async Task OnDisconnected(bool stopCalled)
        {
            if (stopCalled)
            {
                await UpdateUserStatus(false);
            }
            else
            {
                await UpdateUserStatus(true);
            }
            await base.OnDisconnected(stopCalled);
        }

        public async override Task OnReconnected()
        {
            await UpdateUserStatus(true);
            await base.OnReconnected();
        }
        private async Task UpdateUserStatus(bool status)
        {
            //Getting user
            var userName = Context.User.Identity.Name;
            if (userName != null)
            {
                ApplicationUser user = await _userManager.FindByNameAsync(Context.User.Identity.Name);
                if (user != null)
                {
                    if (status)
                    {
                        await Clients.Client(Context.ConnectionId).SetConnectionId(Context.ConnectionId);
                        user.ConnectionId = Context.ConnectionId;
                    }
                    else
                    {
                        await Clients.Client(Context.ConnectionId).SetConnectionId(null);
                        user.ConnectionId = null;
                    }
                    await _userManager.UpdateAsync(user);
                    await Clients.Client(Context.ConnectionId).SetConnectionStatus(status);
                }
            }
        }

        private async Task<bool> IsUserValid()
        {
            var userName = Context.User.Identity.Name;
            if (userName != null)
            {
                ApplicationUser user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Pushes changes to a client, in this case changes status on a row
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task ChangeStatusOnOrderRow(int id)
        {
            //Return value
            if (await IsUserValid())
            {
                try
                {
                    //Transaction begins
                    //Gets model
                    var model = _unitOfWork.GenericRepository<OrderRow>().Get(id);
                    //Checks if there is a model
                    if (model != null)
                    {
                        //Translates orderstatus name to boolean
                        string status = UnitOfWork.OrderRowStatusFalse;
                        if (model.OrderRowStatus.Name != UnitOfWork.OrderRowStatusFalse)
                        {
                            status = UnitOfWork.OrderRowStatusTrue;
                            //Changes id to correct one if nothing is out of order(like the status is not either of the alternatives)
                            model.OrderRowStatusId = _unitOfWork.GenericRepository<OrderRowStatus>().GetIncluded(filter: x => x.Name == status).Select(x => x.Id).FirstOrDefault();
                        }
                        else if (model.OrderRowStatus.Name != UnitOfWork.OrderRowStatusTrue)
                        {
                            //Do nothing since string is already set, and bool is already false
                        }
                        else
                        {
                            //Do nothing since default is false;
                        }

                        //Push change to client
                        await Clients.Caller.ChangeStatusOnOrderRow(id);
                    }

                }
                catch (Exception e)
                {
                    //Not sure this is needed since error handling is the controllers responsibility not bizniz layah, hence the throw i think
                    throw e;
                }

            }

        }
        /// <summary>
        /// inserts connection statusupdate for users
        /// </summary>
        /// <returns></returns>
        public async Task UpdateUserStatus(string name, bool isOnline)
        {



            try
            {
                var userName = Context.User.Identity.Name;
                ApplicationUser user = await _userManager.FindByNameAsync(userName);

                if (user == null)
                {

                }
                else
                {
                    _unitOfWork.GenericRepository<UserConnectionStatus>().Insert(new UserConnectionStatus() { Status = isOnline, ApplicationUserId = user.Id });

                }
            }
            catch (Exception e)
            {
                //Not sure this is needed since error handling is the controllers responsibility not bizniz layah, hence the throw i think
                throw e;
            }

        }

    }
}
