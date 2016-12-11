using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RouteLister2.Models.ParcelListFromCompanyViewModel
{
    public class ParcelListFromCompanyViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Förnamn")]
        public string FirstName { get; set; }
        [Display(Name = "Efternamn")]
        public string LastName { get; set; }

        [Display(Name = "Stad")]
        public string City { get; set; }

        [Display(Name = "Postnummer")]
        public string PostNr { get; set; }

        [Display(Name = "Adress")]
        public string Adress { get; set; }

        [Display(Name = "Tele 1")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneOne { get; set; }

        [Display(Name = "Tele 2")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneTwo { get; set; }

        [Display(Name = "Artikelnamn")]
        public string Distributor { get; set; }

        [Display(Name = "Leverantör")]
        public string ArticleName { get; set; }

        [Display(Name = "KollieId")]
        public string CollieId { get; set; }

        [Display(Name = "Land")]
        public string Country { get; set; }

        [Display(Name = "Antal")]
        public int ArticleAmount { get; set; }

        [Display(Name = "Leveranstyp")]
        public string DeliveryType { get; set; }

        [Display(Name = "Leveransdatum")]
        public DateTime? DeliveryDate { get; set; }

        [Display(Name = "Bil")]
        public IEnumerable<SelectListItem> RegNrDropDown { get; set; }

        [Display(Name = "RegNr")]
        public string RegistrationNumber { get; set; }


        //public List<ParcelListFromCompanyViewModel> ParcelList = new List<ParcelListFromCompanyViewModel>();

    }

}
