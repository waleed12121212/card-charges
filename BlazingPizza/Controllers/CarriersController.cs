using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/carriers")]
[ApiController]
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
}