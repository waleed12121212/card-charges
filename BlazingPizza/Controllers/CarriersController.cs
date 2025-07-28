using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/carriers")]
[ApiController]
[IgnoreAntiforgeryToken]
public class CarriersController : ControllerBase
{
    private readonly ICarrierRepository _repo;
    public CarriersController(ICarrierRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<List<Carrier>> GetCarriers()
    {
        return await _repo.GetCarriers();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Carrier>> GetCarrier(int id)
    {
        var carrier = await _repo.GetCarrierById(id);
        if (carrier == null)
        {
            return NotFound();
        }
        return carrier;
    }

    [HttpPost]
    public async Task<ActionResult<Carrier>> CreateCarrier(Carrier carrier)
    {
        if (carrier == null)
        {
            return BadRequest("Carrier data is required");
        }

        if (string.IsNullOrWhiteSpace(carrier.carrierName))
        {
            return BadRequest("Carrier name is required");
        }

        // Ensure string properties are not null
        carrier.carrierName = carrier.carrierName ?? string.Empty;
        carrier.internetSubscribEmail = carrier.internetSubscribEmail ?? string.Empty;
        carrier.ImeiUserName = carrier.ImeiUserName ?? string.Empty;
        carrier.ImeiAPIKey = carrier.ImeiAPIKey ?? string.Empty;
        carrier.ImeiAPIURL = carrier.ImeiAPIURL ?? string.Empty;
        carrier.APIPassword = carrier.APIPassword ?? string.Empty;
        carrier.imageName = carrier.imageName ?? string.Empty;

        var createdCarrier = await _repo.CreateCarrier(carrier);
        return CreatedAtAction(nameof(GetCarrier), new { id = createdCarrier.id }, createdCarrier);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCarrier(int id, Carrier carrier)
    {
        if (carrier == null)
        {
            return BadRequest("Carrier data is required");
        }

        if (id != carrier.id)
        {
            return BadRequest();
        }

        if (string.IsNullOrWhiteSpace(carrier.carrierName))
        {
            return BadRequest("Carrier name is required");
        }

        // Ensure string properties are not null
        carrier.carrierName = carrier.carrierName ?? string.Empty;
        carrier.internetSubscribEmail = carrier.internetSubscribEmail ?? string.Empty;
        carrier.ImeiUserName = carrier.ImeiUserName ?? string.Empty;
        carrier.ImeiAPIKey = carrier.ImeiAPIKey ?? string.Empty;
        carrier.ImeiAPIURL = carrier.ImeiAPIURL ?? string.Empty;
        carrier.APIPassword = carrier.APIPassword ?? string.Empty;
        carrier.imageName = carrier.imageName ?? string.Empty;

        var updatedCarrier = await _repo.UpdateCarrier(carrier);
        if (updatedCarrier == null)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCarrier(int id)
    {
        var result = await _repo.DeleteCarrier(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}