﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RouteLister2.Data;
using RouteLister2.Models.RouteListerViewModels;
using RouteLister2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RouteLister2.Models
{
    public class SignalRBusinessLayer
    {
        //private IDriverHub _hub;
        private RouteListerRepository _repo;
        public static readonly string OrderRowStatusTrue = "Plockad";

        public async Task<T> Get<T>(int id) where T : class
        {
            return await _repo.GetAsync<T>(id);
        }

        public static readonly string OrderRowStatusFalse = "I Lager";
        private IMapper _mapper;

        public async Task<List<SelectListItem>> GetParcelDropDown(int? Id)
        {
            return await _repo.Get<Parcel>().Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString(), Selected = x.Id == Id }).ToListAsync();
        }

        public SignalRBusinessLayer([FromServices] RouteListerRepository repo, [FromServices] IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
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

        public async Task<IEnumerable<SelectListItem>> GetAddressDropDown(int? id)
        {
            return await _repo.Get<Address>().Select(x => new SelectListItem() { Text = x.PostNumber + " " + x.Street, Value = x.Id.ToString(), Selected = x.Id == id }).ToListAsync();
        }

        internal Task UpdateRegNrOnOrderRow<T>(object newEntry)
        {
            throw new NotImplementedException();
        }

        public async Task<RouteList> GetRouteListFromOrderRowId(int id, string RegistrationNumber)
        {
            
            var result = from or in _repo.Get<OrderRow>(x => x.Id == id, null, null)
                         where or.Order.RouteList.ApplicationUser.RegistrationNumber == RegistrationNumber
                         select or.Order.RouteList;
            return await result.FirstOrDefaultAsync();
 
            
            
        }

        public async Task MoveOrderRowToOtherDriver(int id, string registrationNumber)
        {
            //OrderRow to move to a routelist that is on another vehicle/user
            OrderRow orderRowToMove = await _repo.GetAsync<OrderRow>(id);
            //See if there's any routeList already on that driver(today only).(need to make a function purely for this in businesslayer
            RouteList routeListToMoveTo = await _repo.Get<RouteList>(x => x.ApplicationUser.RegistrationNumber==registrationNumber && x.Created.Date==DateTime.Today,x=>x.OrderByDescending(y=>y.Created), x => x.ApplicationUser).FirstOrDefaultAsync();
            //Is there one already?
            if (routeListToMoveTo == null)
            {
                //If not, create one and assign to the driver
                routeListToMoveTo = new RouteList();
                //Get driver
                routeListToMoveTo.ApplicationUserId = await _repo.Get<ApplicationUser>(x => x.RegistrationNumber == registrationNumber,null,null).Select(x=>x.Id).FirstOrDefaultAsync();
                //Set other things  routelist needs to have
                routeListToMoveTo.Title = registrationNumber + " utkörningslista";
                routeListToMoveTo.Assigned = DateTime.Now;
                routeListToMoveTo.Orders = new List<Order>();
                //Create a brand new shiny order to place this orderrow in
                Order order = new Order();
                //..
            }
        }
        public async Task MoveOrderRowToOrder(int orderRowId, int targetOrderId)
        {
            OrderRow orderRow = await _repo.GetAsync<OrderRow>(orderRowId);
            orderRow.OrderId = targetOrderId;
            await _repo.UpdateAsync(orderRow);
        }
        public async Task CreateOrderForNewOrderRow(int Id  ,string FirstName ,string LastName ,string City, 
            string PostNr , string Adress , string PhoneOne , string PhoneTwo , string Distributor , string ArticleName , 
            string CollieId , string Country , int ArticleAmount, string DeliveryType, DateTime? DeliveryDate, 
            IEnumerable<SelectListItem> RegNrDropDown , string RegistrationNumber)
        {

        }

        public async Task<RouteList> CreateNewRouteList(string RegistrationNumber, string CollieId, int RowNumberId)
        {
            OrderRow orderRow = (await _repo.GetAsync<OrderRow>(x=>x.Id==RowNumberId, null, x=>x.Order)).FirstOrDefault();
            //Need to create order first
            if (orderRow.Order == null)
            {
                Order order = new Order();
            }
            return null;
        }

        public async Task<IEnumerable<SelectListItem>> GetContactsDropDown(int? id)
        {
            return await _repo.Get<Contact>().Select(x => new SelectListItem() { Text = x.FirstName + " " + x.LastName, Value = x.Id.ToString(), Selected = x.Id == id }).ToListAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetOrderTypDropDown(int? id)
        {
            return await _repo.Get<OrderType>().Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString(), Selected = x.Id == id }).ToListAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetRouteListDropDown(int? id)
        {
           return await _repo.Get<RouteList>().Select(x => new SelectListItem() { Text = x.Title, Value = x.Id.ToString(), Selected = x.Id == id }).ToListAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetOrderStatusDropDown(int? id)
        {
           return await _repo.Get<OrderStatus>().Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString(), Selected = x.Id == id }).ToListAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetDestinationDropDown(int? id)
        {
            return await _repo.Get<Destination>(null, null, x => x.Address, x => x.Contact).Select(x => new SelectListItem() { Text = x.Contact.FirstName + " " + x.Contact.LastName + " " + x.Address.City + " " + x.Address.Street, Value = x.Id.ToString(), Selected = x.Id == id }).ToListAsync();
        }

        public async Task AddUser(ApplicationUser applicationUser)
        {
            await _repo.InsertAsync(applicationUser);
        }

        public async Task<List<SelectListItem>> GetOrdersDropDown(int? id)
        {
            return await _repo.Get<Order>().Select(x => new SelectListItem() { Text = x.DestinationId.ToString() + x.OrderTypeId.ToString() + x.OrderStatusId.ToString(), Value = x.Id.ToString(), Selected = x.Id == id }).ToListAsync();
        }

        public async Task<List<SelectListItem>> GetOrderRowStatusDropDown(int? id)
        {
            if (id.HasValue) { 
                return await _repo.Get<OrderRowStatus>().Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString(), Selected = x.Id == id }).ToListAsync();
            }
            else
            {
                return await _repo.Get<OrderRowStatus>().Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToListAsync();
            }
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

        public async Task Insert<T>(T t) where T : class
        {
            await _repo.InsertAsync<T>(t);
        }

        public async Task<RouteList> GetRouteList(int? id =null ,string ApplicationUserId = null, string Title = null, DateTime? Created = null, DateTime? Modified =null, DateTime? Assigned = null,string RegistrationNumber = null)
        {
            return (await GetAllRouteList(id,ApplicationUserId,Title,Created,Modified,Assigned,RegistrationNumber)).FirstOrDefault();
        }

        public async Task<IEnumerable<RouteList>> GetAllRouteList(int? id = null, string ApplicationUserId = null, string Title = null, DateTime? Created = null, DateTime? Modified = null, DateTime? Assigned = null, string RegistrationNumber = null)
        {
            //Default ordering = no order(see null)
            //Default filtering = no filtering, see first null
            IQueryable<RouteList> query = _repo.Get<RouteList>(null,
                    null, x => x.ApplicationUser, x => x.Orders);
            if (RegistrationNumber != null)
            {
                query = query.Where(x => x.ApplicationUser.RegistrationNumber == RegistrationNumber);
            }
            if (id.HasValue)
            {
                query = query.Where(x => x.Id == id);
            }
            if (!string.IsNullOrEmpty(ApplicationUserId))
            {
                query = query.Where(x => x.ApplicationUserId == ApplicationUserId);
            }
            if (!string.IsNullOrEmpty(Title))
            {
                query = query.Where(x => x.Title == Title);
            }
            if (Created.HasValue)
            {
                query = query.Where(x => x.Created == Created);
            }
            if (Modified.HasValue)
            {
                query = query.Where(x => x.Modified == Modified);
            }
            if (Assigned.HasValue)
            {
                query = query.Where(x => x.Assigned == Assigned);
            }

            return await query.ToListAsync();
        }
        public async Task<RouteListViewModel> GetRouteListViewModelByRegistrationNumber(string RegistrationNumber)
        {
            var result = _repo.Get<RouteList>(x => x.ApplicationUser.RegistrationNumber == RegistrationNumber, null, x => x.ApplicationUser, x => x.Orders).ProjectTo<RouteListViewModel>(_mapper.ConfigurationProvider);
            return await result.FirstOrDefaultAsync();
        }


        
        public async Task<IEnumerable<RouteListViewModel>> GetAllRouteLists()
        {
             var result = await _repo.Get<RouteList>(null,null,x=>x.ApplicationUser).ProjectTo<RouteListViewModel>(_mapper.ConfigurationProvider).ToListAsync(); 
            return result;
        }

        public async Task<IEnumerable<SelectListItem>> GetRegistrationNumberDropDown(string ApplicationUserId) 
        {
            var result = _repo.Get<ApplicationUser>(null,null,null);
            if (string.IsNullOrEmpty(ApplicationUserId))
            {
                return await result.Select(x => new SelectListItem() { Text = x.RegistrationNumber, Value = x.Id.ToString(), Selected = x.Id == ApplicationUserId }).ToListAsync();
            }
            else
            {
                return await result.Select(x => new SelectListItem() { Text = x.RegistrationNumber, Value = x.Id.ToString() }).ToListAsync();
            }
            
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
