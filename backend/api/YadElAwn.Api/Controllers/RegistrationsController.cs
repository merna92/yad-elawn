using Microsoft.AspNetCore.Mvc;
using YadElAwn.Api.Dtos;
using YadElAwn.Api.Services;

namespace YadElAwn.Api.Controllers;

[ApiController]
[Route("api/registrations")]
public class RegistrationsController : ControllerBase
{
    private readonly IRegistrationService _registrations;

    public RegistrationsController(IRegistrationService registrations)
    {
        _registrations = registrations;
    }

    [HttpPost("donor")]
    public async Task<IActionResult> RegisterDonor(RegisterDonorRequest request)
    {
        try
        {
            var result = await _registrations.RegisterDonorAsync(request);
            return Ok(new { userId = result.UserId, donorId = result.RelatedId });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("charity")]
    public async Task<IActionResult> RegisterCharity(RegisterCharityRequest request)
    {
        try
        {
            var result = await _registrations.RegisterCharityAsync(request);
            return Ok(new { userId = result.UserId, charityId = result.RelatedId });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("beneficiary")]
    public async Task<IActionResult> RegisterBeneficiary(RegisterBeneficiaryRequest request)
    {
        try
        {
            var result = await _registrations.RegisterBeneficiaryAsync(request);
            return Ok(new { userId = result.UserId, beneficiaryId = result.RelatedId });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("admin")]
    public async Task<IActionResult> RegisterAdmin(RegisterAdminRequest request)
    {
        try
        {
            var result = await _registrations.RegisterAdminAsync(request);
            return Ok(new { userId = result.UserId, adminId = result.RelatedId });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
