using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using TanfeethIntegration.DTOs;
using TanfeethIntegration.Services;

namespace TanfeethIntegration.Controllers
{
    public class LookupController : Controller
    {
        private readonly ILookupService _lookupService;

        public LookupController(ILookupService lookupService)
        {
            _lookupService = lookupService;
        }

        public async Task<IActionResult> GetDefendantTypes()
        
        {
            //var defendantTypes = await _lookupService.GetCitiesAsync(); //GetCitesSelectList();

            //if (Cites.Any())
            //{
            //    // Extract the city names
            //    var cityNames = Cites.Select(city => city.nameAr).ToList();

            //    // Store the city names in ViewData or pass it to the view model, depending on your preference
            //    ViewData["CityId"] = new SelectList(cityNames);
            //}
            //else
            //{
            //    // Handle the error case if needed
            //    Console.WriteLine("API Error: Unable to retrieve city data");
            //    ViewData["CityId"] = new SelectList(new List<string>()); // Empty list or handle the error accordingly
            //}
            // var defendantTypes = await _lookupService.GetDefendantTypesAsync();
            var defendantTypes = new List<DefendantTypeLookupDto>
            {
       new DefendantTypeLookupDto { Id = 0, nameAr = "اختر نوع المنفذ ضده", nameEn = "" ,Disabled = true, Selected = true},

                new DefendantTypeLookupDto { Id = 1, nameAr = "شركة مسجلة في المملكة",nameEn="RegisteredCompanyInfo" },
                new DefendantTypeLookupDto { Id = 2, nameAr = "شركة غير مسجلة في المملكة",nameEn="Individual" },
                new DefendantTypeLookupDto { Id = 3, nameAr = "وقف",nameEn="NameEn" },
                new DefendantTypeLookupDto { Id = 4, nameAr = "مؤسسة أهلية",nameEn="NameEnf" },
              new DefendantTypeLookupDto { Id = 5, nameAr = "جمعية أهلية",nameEn="NameEnff" },
                new DefendantTypeLookupDto { Id = 6, nameAr = "فرد",nameEn="NameEn" },
                   new DefendantTypeLookupDto { Id = 7, nameAr = "جهة إدارية",nameEn="NameEnfff" }




                // Add more static types as needed for your test
            };

            return Json(defendantTypes);



           // var testResponse = "[{\"Id\":\"\",\"nameAr\":\"اختر نوع المدعى عليه\",\"Disabled\":true, \"Selected\":true},{\"Id\":1,\"nameAr\":\"شركة مسجلة في المملكة\"},{\"Id\":2,\"nameAr\":\"شركة غير مسجلة في المملكة\"}]"; return Content(testResponse, "application/json");
        }
    }
}
