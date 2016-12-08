using RouteLister2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouteLister2.Data
{
    public static class FakeDataSets
    {
        public static readonly int Id = 1;

        public static RouteList DeliveryListFactory()
        {
            RouteList model = new RouteList();

            model.Orders = new List<Order>() { OrderFactory() };
            model.Title = "TestDeliveryList";
            model.ApplicationUser = ApplicationUserFactory();
            model.ApplicationUserId = model.ApplicationUser.Id;
            model.Modified = DateTime.Now;
            model.Created = DateTime.Parse("1999-01-01");
            model.Assigned = DateTime.Now;
            return model;
        }

        public static ApplicationUser ApplicationUserFactory()
        {
            ApplicationUser model = new ApplicationUser()
            {
                Id = Id.ToString(),
                RegistrationNumber = "xxx111",
                UserName = "Åke"
            };
            return model;
        }

        public static Order OrderFactory()
        {
            Order model = new Order();
            model.Id = Id;
            model.Destination = DestinationFactory();
            model.OrderRows = new List<OrderRow>();
            model.OrderRows.Add(OrderRowFactory());
            model.OrderStatus = OrderStatusFactory();
            model.OrderStatusId = model.OrderStatus.Id;
            model.OrderType = OrderTypeFactory();
            model.OrderTypeId = model.OrderType.Id;
            return model;

        }

        public static OrderType OrderTypeFactory()
        {
            OrderType model = new OrderType();
            model.Id = Id;
            model.Description = "TestOrderType";
            model.Name = "Curbside";
            return model;
        }

        public static OrderStatus OrderStatusFactory()
        {
            OrderStatus model = new OrderStatus();
            model.Id = Id;
            model.Name = "Leverad";
            model.Description = "Ordern är levererad";
            model.Priority = 1;
            return model;
        }

        public static Destination DestinationFactory()
        {
            Destination model = new Destination();
            model.Id = Id;
            model.Address = AddressFactory();
            model.AddressId = model.Address.Id;
            model.Contact = ContactFactory();
            model.ContactId = model.Contact.Id;

            return model;
        }

        public static Contact ContactFactory()
        {
            Contact model = new Contact();
            model.Id = Id;
            model.FirstName = "Klas";
            model.LastName = "Knutsson";
            model.PhoneNumbers = PhoneNumberFactory();
            return model;

        }

        public static List<PhoneNumber> PhoneNumberFactory()
        {
            return new List<PhoneNumber>()
            {
                    new PhoneNumber() {  Id = Id,Number = "0499 - 12 34 56" },
                    new PhoneNumber() {Id=Id+1,Number = "0703 - 22 33 44" }
            };
        }

        public static Address AddressFactory()
        {

            Address model = new Address();
            model.Id = Id;
            model.City = "Nyköping";
            model.County = "Stockholms Län";

            model.PostNumber = "392 44";
            model.Street = "Björkkvistsgatan 3";

            return model;

        }

        public static OrderRow OrderRowFactory()
        {
            OrderRow model = new OrderRow();
            model.OrderId = Id;
            model.Id = Id;
            model.OrderRowStatus = OrderRowStatusFactory();
            model.OrderRowStatusId = model.OrderRowStatus.Id;
            model.Parcel = ParcelFactory();
            model.ParcelId = model.Parcel.Id;
            model.Count = 1;
            return model;
        }

        public static Parcel ParcelFactory()
        {
            Parcel model = new Parcel();
            model.Id = Id;
            model.Name = "Cylinda Diskmaskin";
            model.ParcelNumber = "078282988529SE";
            return model;
        }

        public static OrderRowStatus OrderRowStatusFactory()
        {
            OrderRowStatus model = new OrderRowStatus();
            model.Id = Id;
            model.Name = "Plockad";
            return model;
        }
    }
}
