using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Serilog.Context;
using System.Text;
using System.Text.RegularExpressions;
using TanfeethIntegration.Data;
using TanfeethIntegration.DTOs;
using TanfeethIntegration.Models;
using TanfeethIntegration.Services;

namespace TanfeethIntegration.Controllers
{
    [UserExistsAuthorize]
    public class RequestsController : Controller
    {
        private readonly LogDbContext _dbContext;
        private readonly IGovAgencyRequestService _govAgencyRequestService;
        private readonly ILookupService _lookupService;
        public RequestsController(LogDbContext dbContext, IGovAgencyRequestService govAgencyRequestService, ILookupService lookupService )
        {

            _dbContext = dbContext;
            _govAgencyRequestService = govAgencyRequestService;
            _lookupService = lookupService;
            _lookupService= lookupService;
        }
        // YourController.cs
        // YourController.cs
        private int? ExtractRequestNumberFromResponse(string response)
        {
            if (response != null)
            {
                var match = Regex.Match(response, "\"requestNumber\":(\\d+)");
                return match.Success ? int.Parse(match.Groups[1].Value) : (int?)null;
            }

            return null;
        }
        public async Task<IActionResult> GetStatus(int requestNumber)
        {
            try
            {
                // Retrieve ExecutionRequestStatus from the lookup service
                var executionRequestStatusList = await _lookupService.GetExecutionRequestStatusAsync();

                if (executionRequestStatusList.Any())
                {
                    // Create a SelectListItem list for the ExecutionRequestStatus
                    var executionRequestStatusOptions = executionRequestStatusList
                        .Select(status => new SelectListItem
                        {
                            Value = status.Id.ToString(),
                            Text = status.nameAr
                        })
                        .ToList();

                    // Get the request status from the gov agency service
                    var response = await _govAgencyRequestService.GetRequestStatusAsync(requestNumber);

                    if (response.isSuccess)
                    {
                        // The request was successful, and you can access the data
                        var requestData = response.data;

                        // Access the StatusId property
                        int statusId = requestData.Data.statusId;
                        if (requestData.Data.ValidationResults != null)
                        {
                            StringBuilder statusDetails = new StringBuilder();

                            foreach (var validationResult in requestData.Data.ValidationResults)
                            {
                                // Assuming you want to concatenate all details in a single string
                                statusDetails.AppendLine($"{validationResult.code}: {validationResult.details}");
                            }

                            ViewData["ValidationResults"] = statusDetails.ToString();
                        }

                        if (statusId != 0)
                        {
                            // Find the corresponding nameAr for the given StatusId
                            var statusNameAr = executionRequestStatusList
                                .Where(status => status.Id == statusId)
                                .Select(status => status.nameAr)
                                .FirstOrDefault();

                            // Now you have the nameAr for the given StatusId
                            if (!string.IsNullOrEmpty(statusNameAr))
                            {
                                // Store the statusNameAr in ViewData for use in the view
                                ViewData["StatusNameAr"] = statusNameAr;
                            }
                        }

                        // Handle the data as needed
                        // ...

                        // Return a partial view with the status data
                        return PartialView("_StatusPartialView");
                    }
                    else
                    {
                        // The request failed, and you can access the error message
                        var errorMessage = response.error;

                        // Handle the error as needed
                        // ...

                        return View("Error", errorMessage);
                    }
                }
                else
                {
                    // Handle the case when ExecutionRequestStatus is empty
                    return View("Error", "No execution request statuses available.");
                }
            }
            catch (Exception ex)
            {
                // An exception occurred during the request
                // Log the exception or handle it as needed
                // ...

                return View("YourErrorView", "An unexpected error occurred.");
            }
        }




        public IActionResult Index()
        {
            var xx = _dbContext.RequestResponseLogs.Select(r=>r.Response);
            var requestNumbers = _dbContext.RequestResponseLogs
    .Where(r => r.Response != null && r.Response.Contains("\"requestNumber\":"))
    .AsEnumerable()
    .Select(r =>
    {
        // Deserialize the JSON string
        var responseObject = JsonConvert.DeserializeObject<dynamic>(r.Response);

        // Access the requestNumber property
        var requestNumber = responseObject?.data?.requestNumber?.ToString();

        // Convert to int if needed
        int.TryParse(requestNumber, out int result);

        return result;
    })
    .ToList();




            return View(requestNumbers);
        }


        public IActionResult GetRequestDetails(int requestNumber)
        {
            var request = _dbContext.RequestResponseLogs
                   .FirstOrDefault(r => r.Response.Contains($"\"requestNumber\":\"{requestNumber}\""));


            if (request == null)
            {
                return NotFound();
            }

            var jsonData = request.Request;
            var details = JsonConvert.DeserializeObject<RequestModel>(jsonData);

            return PartialView("_RequestDetailsPartial", details);
        }
    }
}
