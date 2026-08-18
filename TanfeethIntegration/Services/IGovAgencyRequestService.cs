using TanfeethIntegration.DTOs;
using TanfeethIntegration.Models;

namespace TanfeethIntegration.Services
{
    public interface IGovAgencyRequestService
    {
        Task<ApiResponse<CreateGovAgencyResponseDto>> CreateGovAgencyRequestAsync(
     RequestModel requestModel);
        Task<ApiResponse<GetRequestStatusResponseDto>> GetRequestStatusAsync(int requestNumber);

    }
}
