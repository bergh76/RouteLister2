using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RouteLister2.Data;
using RouteLister2.Models.RouteListerViewModels;
using RouteLister2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouteLister2.Models
{
    public class SignalRBusinessLayer
    {
        private IDriverHub _hub;
        private RouteListerRepository _repo;
        public static readonly string OrderRowStatusTrue = "Plockad";
        public static readonly string OrderRowStatusFalse = "I Lager";

        public SignalRBusinessLayer([FromServices] RouteListerRepository repo, [FromServices] IDriverHub hub = null)
        {
            _repo = repo;
            _hub = hub;
        }
        public async Task<bool> IsUserValid(string userName)
        {
            if (userName != null)
            {
                //ApplicationUser user = await _userManager.FindByNameAsync(userName);
                ApplicationUser user = (await _repo.GetAsync<ApplicationUser>(filter: x => x.UserName == userName)).FirstOrDefault();
                if (user != null)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task AddUser(ApplicationUser applicationUser)
        {
            await _repo.InsertAsync(applicationUser);
        }

        public async Task UpdateUserStatus(bool status, string userId, string connectionId)
        {
            //Getting user
            if (userId != null)
            {
                ApplicationUser user = await _repo.GetAsync<ApplicationUser>(userId);
                if (user != null)
                {
                    if (status)
                    {
                        user.ConnectionId = connectionId;
                    }
                    else
                    {
                        user.ConnectionId = null;
                    }

                    //await _userManager.UpdateAsync(user);
                    await _repo.UpdateAsync(user);
                }
            }
        }

        public async Task<RouteList> GetRouteList(string id)
        {
            var result = (await _repo.GetAsync<RouteList>(filter: x => x.ApplicationUser.RegistrationNumber == id, included: x => x.ApplicationUser));
            return result;
        }

        public async Task SetUserConnectionId(string connectionId, string name = null, string id = null)
        {
            ApplicationUser user = await GetUser(name, id);

            if (user != null)
            {
                user.ConnectionId = connectionId;
                await _repo.UpdateAsync(user);
            }
        }
        public async Task<ApplicationUser> GetUser(string name = null, string id = null)
        {

            //var test = await _repo.GetAsync<ApplicationUser>(filter:x => x.UserName == (name ?? null) || x.Id== (id ?? null), included:null);
            ApplicationUser user = null;
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(id))
            {
                return user;
            }
            else if (!string.IsNullOrEmpty(name))
            {
                user = await _repo.GetAsync<ApplicationUser>(filter: x => x.UserName == name, included: null);
            }
            else if (!string.IsNullOrEmpty(id))
            {
                user = await _repo.GetAsync<ApplicationUser>(id);
            }
            return user;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idRef"></param>
        /// <returns></returns>
        public async Task<bool> ChangeStatusOnOrderRow(int id)
        {
            var model = await _repo.GetAsync<OrderRow>(included: x => x.OrderRowStatus, filter: x => x.Id == id);
            //Checks if there is a model
            if (model == null)
            {
                return false;
            }

            //Translates orderstatus strign name to boolean and changes it to either or two things
            if (model.OrderRowStatus.Name == OrderRowStatusTrue)
            {
                model.OrderRowStatusId = (await _repo.GetAsync<OrderRowStatus>(filter: x => x.Name == OrderRowStatusFalse)).Select(x => x.Id).FirstOrDefault();
                await _repo.UpdateAsync(model);
                return false;
            }
            else if (model.OrderRowStatus.Name == OrderRowStatusFalse)
            {
                model.OrderRowStatusId = (await _repo.GetAsync<OrderRowStatus>(filter: x => x.Name == OrderRowStatusTrue)).Select(x => x.Id).FirstOrDefault();
                await _repo.UpdateAsync(model);
                return true;
            }
            else
            {
                return false;
            }
        }





    }
}
