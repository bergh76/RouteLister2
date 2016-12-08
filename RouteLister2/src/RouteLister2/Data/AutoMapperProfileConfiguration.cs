﻿using AutoMapper;
using RouteLister2.Models;
using RouteLister2.Models.ParcelListFromCompanyViewModel;
using RouteLister2.Models.RouteListerViewModels;
using System.Linq;

namespace RouteLister2.Data
{
    public class AutoMapperProfileConfiguration : Profile
    {

        public AutoMapperProfileConfiguration() : base()
        {
           
            CreateMap<RouteList, RouteListViewModel>()
                .ForMember(x => x.DeliveryListId, opt => opt.MapFrom(t => t.Id))
                .ForMember(x => x.Title, opt => opt.MapFrom(t => t.Title))
                .ForMember(x => x.RegNr, opt => opt.MapFrom(t => t.ApplicationUser.RegistrationNumber))
                .ForMember(x => x.Assigned, opt => opt.MapFrom(t => t.Assigned))
                .ForMember(x => x.Orders, opt => opt.MapFrom(t => t.Orders)
                );

            //Real test right here
            CreateMap<Order, OrderDetailViewModel>()

                .ForMember(x => x.TotalCount, opt => opt.MapFrom(t => t.OrderRows.Sum(z => z.Count)))
                .ForMember(x => x.OrderId, opt => opt.MapFrom(t => t.Id))
                .ForMember(x => x.Address, opt => opt.MapFrom(t => t.Destination.Address.Street))
                .ForMember(x => x.City, opt => opt.MapFrom(t => t.Destination.Address.City))
                .ForMember(x => x.FirstName, opt => opt.MapFrom(t => t.Destination.Contact.FirstName))
                .ForMember(x => x.LastName, opt => opt.MapFrom(t => t.Destination.Contact.LastName))
                .ForMember(x => x.PhoneNumbers, opt => opt.MapFrom(t => t.Destination.Contact.PhoneNumbers.Select(y => y.Number)))
                .ForMember(x => x.PostNumber, opt => opt.MapFrom(t => t.Destination.Address.PostNumber))
                .ForMember(x => x.DeliveryTypeName, opt => opt.MapFrom(t => t.OrderType.Name))
                .ForMember(x => x.RouteListId, opt => opt.MapFrom(t => t.RouteListId))
                ;


            CreateMap<Order, OrderRowViewModel>()
                             .ForMember(x => x.OrderId, opt => opt.MapFrom(t => t.Id))
                             .ForAllOtherMembers(x => x.Ignore());
            CreateMap<OrderRow, OrderRowViewModel>()
                .ForMember(x => x.OrderRowId, opt => opt.MapFrom(t => t.Id))
                .ForMember(x => x.Count, opt => opt.MapFrom(t => t.Count))
                .ForMember(x => x.OrderRowStatus, opt => opt.MapFrom(t => t.OrderRowStatus))
                .ForMember(x => x.ParcelName, opt => opt.MapFrom(t => t.Parcel.Name))
                .ForMember(x => x.ParcelNumber, opt => opt.MapFrom(t => t.Parcel.ParcelNumber))
                .ForMember(x => x.OrderRowStatus, opt => opt.MapFrom(t => t.OrderRowStatus.Name == SignalRBusinessLayer.OrderRowStatusTrue))
                .ForMember(x => x.OrderId, opt => opt.MapFrom(t => t.OrderId))
                ;


            CreateMap<ApplicationUser, RouteListViewModel>()
                .ForMember(x => x.RegNr, opt => opt.MapFrom(t => t.RegistrationNumber))
                .ForAllOtherMembers(x => x.Ignore())
               .ForAllOtherMembers(x => x.Ignore())
                ;

            CreateMap<ParcelListFromCompanyViewModel, Parcel>()
               .ForMember(x => x.ParcelNumber, opt => opt.MapFrom(t => t.CollieId))
               .ForMember(x => x.Name, opt => opt.MapFrom(t => t.ArticleName))
               .ForAllOtherMembers(x => x.Ignore())
               ;
            CreateMap<ParcelListFromCompanyViewModel, Contact>()
               .ForMember(x => x.FirstName, opt => opt.MapFrom(t => t.FirstName))
               .ForMember(x => x.LastName, opt => opt.MapFrom(t => t.LastName))
               .ForAllOtherMembers(x => x.Ignore())
               ;
            CreateMap<ParcelListFromCompanyViewModel, Address>()
              .ForMember(x => x.Street, opt => opt.MapFrom(t => t.Adress))
              .ForMember(x => x.City, opt => opt.MapFrom(t => t.City))
              .ForMember(x => x.PostNumber, opt => opt.MapFrom(t => t.PostNr))
              .ForMember(x => x.County, opt => opt.MapFrom(t => t.Country))
              .ForAllOtherMembers(x => x.Ignore())
              ;
            CreateMap<ParcelListFromCompanyViewModel, PhoneNumber>()
             .ForMember(x => x.Number, opt => opt.MapFrom(t => t.PhoneTwo))
             .ForAllOtherMembers(x => x.Ignore())
             ;

        }
    }
}