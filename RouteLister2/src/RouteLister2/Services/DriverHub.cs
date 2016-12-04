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


        public DriverHub([FromServices] UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
            //_userManager = userManager;
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
                //ApplicationUser user = await _userManager.FindByNameAsync(Context.User.Identity.Name);
                ApplicationUser user = (await _unitOfWork.GenericRepository<ApplicationUser>().GetAsyncIncluded(filter: x => x.UserName == Context.User.Identity.Name)).FirstOrDefault();
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

                    //await _userManager.UpdateAsync(user);
                    await _unitOfWork.GenericRepository<ApplicationUser>().UpdateAsync(user);
                    await Clients.Caller.SetConnectionStatus(status);
                    //await Clients.Client(Context.ConnectionId).SetConnectionStatus(status);
                }
            }
        }

        private async Task<bool> IsUserValid()
        {
            var userName = Context.User.Identity.Name;
            if (userName != null)
            {
                //ApplicationUser user = await _userManager.FindByNameAsync(userName);
                ApplicationUser user = (await _unitOfWork.GenericRepository<ApplicationUser>().GetAsyncIncluded(filter: x => x.UserName == Context.User.Identity.Name)).FirstOrDefault();
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
        public async Task ChangeStatusOnOrderRow(int id, string idRef)
        {
            //Return value
            if (await IsUserValid())
            {
                try
                {
                    //Transaction begins
                    //Gets model
                    var model = (await _unitOfWork.GenericRepository<OrderRow>().GetAsyncIncluded(included:x=>x.OrderRowStatus,filter:x=>x.Id==id)).FirstOrDefault();
                    //Checks if there is a model
                    if (model != null)
                    {
                        bool status = false;
                        //Translates orderstatus name to boolean
                     
                        if(model.OrderRowStatus.Name == UnitOfWork.OrderRowStatusTrue)
                        {
                            model.OrderRowStatusId = _unitOfWork.GenericRepository<OrderRowStatus>().GetIncluded(filter: x => x.Name == UnitOfWork.OrderRowStatusFalse).Select(x => x.Id).FirstOrDefault();
                            await _unitOfWork.GenericRepository<OrderRow>().UpdateAsync(model);
                            status = false;
                        }
                        else if (status = model.OrderRowStatus.Name == UnitOfWork.OrderRowStatusFalse)
                        {
                            model.OrderRowStatusId = _unitOfWork.GenericRepository<OrderRowStatus>().GetIncluded(filter: x => x.Name == UnitOfWork.OrderRowStatusTrue).Select(x => x.Id).FirstOrDefault();
                            await _unitOfWork.GenericRepository<OrderRow>().UpdateAsync(model);
                            status = true;
                        }
                        else
                        {
                            //Notify client something some other status prevents them from changing status
                        }

                        //Push change to client
                        await Clients.Caller.ChangeClientRowStatus(idRef, status);
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
                //ApplicationUser user = await _userManager.FindByNameAsync(userName);
                ApplicationUser user = (await _unitOfWork.GenericRepository<ApplicationUser>().GetAsyncIncluded(filter: x => x.UserName == Context.User.Identity.Name)).FirstOrDefault();

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
