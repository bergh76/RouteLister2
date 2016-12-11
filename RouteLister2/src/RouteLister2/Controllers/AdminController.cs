using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RouteLister2.Models;
using Microsoft.AspNetCore.Identity;
using RouteLister2.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using RouteLister2.Services;
using RouteLister2.Models.ParcelListFromCompanyViewModel;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RouteLister2.Controllers
{
    [Authorize(Roles = "Admin")]
    //[Route("Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext _context;
        private IMapper _mapper;

        // GET: /<controller>/
        //[Authorize(Roles ="Admin")]
        public AdminController(
            [FromServices] ApplicationDbContext context,
            [FromServices] IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(
            JsonDataListImports jsonData, 
            ParcelListFromCompanyViewModel parcel)
        {
            if (ModelState.IsValid)
            {
                //Uncommented import since webapi is non-existant.
                //await jsonData.GetParcelData(_context);
                //var result = await _unitOfWork.GenericRepository<ParcelListFromCompanyViewModel>().GetAsyncIncluded();
                var result = _context.OrderRows.ProjectTo<ParcelListFromCompanyViewModel>(_mapper.ConfigurationProvider);
                                 //join order in _context.Orders on c.Id equals order.Id
                             //join phone in _context.PhoneNumbers on c.Id equals phone.ContactId
                             //join par in _context.Parcels on c.Id equals par.Id
                             //select new ParcelListFromCompanyViewModel
                             //{

                             //    FirstName = c.FirstName, //Contact
                             //    LastName = c.LastName, //Contact
                             //    Adress = "", //Address
                             //    City = "", //Address
                             //    PostNr = "", //Address
                             //    Distributor = "", //Address
                             //    CollieId = "", //Parcel
                             //    ArticleName = "", //Parcel
                             //    ArticleAmount = 0, //Parcel
                             //    Country = "", //Parcel
                             //    DeliveryType = "", //Parcel
                             //    DeliveryDate = DateTime.Now, //Parcel
                             //    PhoneOne = _context.PhoneNumbers.Where(x => x.ContactId == c.Id)
                             //                .Select(x => x.Number).FirstOrDefault(),
                             //                 //Phone
                             //    PhoneTwo = "", //Phone

                             //};
                List<ParcelListFromCompanyViewModel> outResult = await result.ToListAsync();
                List<SelectListItem> dropDown = await _context.Users.Select(x => new SelectListItem() { Text=x.RegistrationNumber, Value=x.RegistrationNumber }).ToListAsync();
                foreach (var item in outResult)
                {
                    //Hack to set index for dropdown, haven't figured out how to map a dropdown with automapper yet or if its possible at all
                    dropDown[dropDown.IndexOf(dropDown.Where(x => x.Value == item.RegistrationNumber).SingleOrDefault())].Selected = true;
                    item.RegNrDropDown = dropDown.ToList();
                }
                return View(outResult);
            }
            
            return RedirectToAction("Error");
        }

        public IActionResult ShowCarLists()
        {
            return View();
        }

    

        public IActionResult Message()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }

    }

}
