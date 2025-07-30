using BlazingPizza.Shared;
using BlazingPizza.Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazingPizza.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InternetPackageController : ControllerBase
{
    private readonly IInternetPackageRepository _repository;

    public InternetPackageController(IInternetPackageRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<InternetPackage>>> GetAll()
    {
        var packages = await _repository.GetAllAsync();
        return Ok(packages);
    }

    [HttpGet("active")]
    public async Task<ActionResult<List<InternetPackage>>> GetActivePackages()
    {
        var packages = await _repository.GetActivePackagesAsync();
        return Ok(packages);
    }

    [HttpGet("carrier/{carrierTypeId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<InternetPackage>>> GetByCarrierType(int carrierTypeId)
    {
        if (!Enum.IsDefined(typeof(CarrierType), carrierTypeId))
        {
            return BadRequest("Invalid carrier type");
        }
        
        var carrierType = (CarrierType)carrierTypeId;
        var packages = await _repository.GetByCarrierTypeAsync(carrierType);
        return Ok(packages);
    }

    [HttpGet("active/carrier/{carrierTypeId}")]
    public async Task<ActionResult<List<InternetPackage>>> GetActiveByCarrierType(int carrierTypeId)
    {
        if (!Enum.IsDefined(typeof(CarrierType), carrierTypeId))
        {
            return BadRequest("Invalid carrier type");
        }
        
        var carrierType = (CarrierType)carrierTypeId;
        var packages = await _repository.GetActivePackagesByCarrierAsync(carrierType);
        return Ok(packages);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<InternetPackage>> GetById(int id)
    {
        var package = await _repository.GetByIdAsync(id);
        if (package == null)
        {
            return NotFound();
        }
        return Ok(package);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<InternetPackage>> Create([FromBody] InternetPackage package)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdPackage = await _repository.CreateAsync(package);
        return CreatedAtAction(nameof(GetById), new { id = createdPackage.Id }, createdPackage);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<InternetPackage>> Update(int id, [FromBody] InternetPackage package)
    {
        if (id != package.Id)
        {
            return BadRequest("ID mismatch");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updatedPackage = await _repository.UpdateAsync(package);
        if (updatedPackage == null)
        {
            return NotFound();
        }

        return Ok(updatedPackage);
    }

    [HttpPost("seed-missing")]
    public async Task<ActionResult> SeedMissingPackages()
    {
        var packagesToAdd = new List<InternetPackage>();

        // جوال packages
        if (!await PackageExists(CarrierType.جوال, "يومي"))
        {
            packagesToAdd.Add(new InternetPackage
            {
                Name = "حزمة جوال اليومية",
                Description = "حزمة إنترنت يومية من جوال",
                DataAmountMB = 500,
                ValidityDays = 1,
                Price = 5.00m,
                Cost = 4.50m,
                CarrierType = CarrierType.جوال,
                PackageType = "يومي",
                IsActive = true,
                SortOrder = 1,
                CreatedOn = DateTime.Now
            });
        }

        if (!await PackageExists(CarrierType.جوال, "أسبوعي"))
        {
            packagesToAdd.Add(new InternetPackage
            {
                Name = "حزمة جوال الأسبوعية",
                Description = "حزمة إنترنت أسبوعية من جوال",
                DataAmountMB = 2048,
                ValidityDays = 7,
                Price = 25.00m,
                Cost = 22.50m,
                CarrierType = CarrierType.جوال,
                PackageType = "أسبوعي",
                IsActive = true,
                SortOrder = 2,
                CreatedOn = DateTime.Now
            });
        }

        if (!await PackageExists(CarrierType.جوال, "شهري"))
        {
            packagesToAdd.Add(new InternetPackage
            {
                Name = "حزمة جوال الشهرية",
                Description = "حزمة إنترنت شهرية من جوال",
                DataAmountMB = 10240,
                ValidityDays = 30,
                Price = 80.00m,
                Cost = 72.00m,
                CarrierType = CarrierType.جوال,
                PackageType = "شهري",
                IsActive = true,
                SortOrder = 3,
                CreatedOn = DateTime.Now
            });
        }

        // سيليكوم packages
        if (!await PackageExists(CarrierType.سيليكوم, "يومي"))
        {
            packagesToAdd.Add(new InternetPackage
            {
                Name = "حزمة سيليكوم اليومية",
                Description = "حزمة إنترنت يومية من سيليكوم",
                DataAmountMB = 400,
                ValidityDays = 1,
                Price = 4.00m,
                Cost = 3.60m,
                CarrierType = CarrierType.سيليكوم,
                PackageType = "يومي",
                IsActive = true,
                SortOrder = 1,
                CreatedOn = DateTime.Now
            });
        }

        if (!await PackageExists(CarrierType.سيليكوم, "أسبوعي"))
        {
            packagesToAdd.Add(new InternetPackage
            {
                Name = "حزمة سيليكوم الأسبوعية",
                Description = "حزمة إنترنت أسبوعية من سيليكوم",
                DataAmountMB = 1024,
                ValidityDays = 7,
                Price = 20.00m,
                Cost = 18.00m,
                CarrierType = CarrierType.سيليكوم,
                PackageType = "أسبوعي",
                IsActive = true,
                SortOrder = 2,
                CreatedOn = DateTime.Now
            });
        }

        if (!await PackageExists(CarrierType.سيليكوم, "شهري"))
        {
            packagesToAdd.Add(new InternetPackage
            {
                Name = "حزمة سيليكوم الشهرية",
                Description = "حزمة إنترنت شهرية من سيليكوم",
                DataAmountMB = 8192,
                ValidityDays = 30,
                Price = 65.00m,
                Cost = 58.50m,
                CarrierType = CarrierType.سيليكوم,
                PackageType = "شهري",
                IsActive = true,
                SortOrder = 3,
                CreatedOn = DateTime.Now
            });
        }

        // أوريدو packages
        if (!await PackageExists(CarrierType.أوريدو, "أسبوعي"))
        {
            packagesToAdd.Add(new InternetPackage
            {
                Name = "حزمة أوريدو الأسبوعية",
                Description = "حزمة إنترنت أسبوعية من أوريدو",
                DataAmountMB = 3072,
                ValidityDays = 7,
                Price = 30.00m,
                Cost = 27.00m,
                CarrierType = CarrierType.أوريدو,
                PackageType = "أسبوعي",
                IsActive = true,
                SortOrder = 2,
                CreatedOn = DateTime.Now
            });
        }

        if (!await PackageExists(CarrierType.أوريدو, "شهري"))
        {
            packagesToAdd.Add(new InternetPackage
            {
                Name = "حزمة أوريدو الشهرية",
                Description = "حزمة إنترنت شهرية من أوريدو",
                DataAmountMB = 15360,
                ValidityDays = 30,
                Price = 100.00m,
                Cost = 90.00m,
                CarrierType = CarrierType.أوريدو,
                PackageType = "شهري",
                IsActive = true,
                SortOrder = 3,
                CreatedOn = DateTime.Now
            });
        }

        foreach (var package in packagesToAdd)
        {
            await _repository.CreateAsync(package);
        }

        return Ok(new { message = $"Added {packagesToAdd.Count} packages", packages = packagesToAdd.Select(p => p.Name) });
    }

    private async Task<bool> PackageExists(CarrierType carrierType, string packageType)
    {
        var packages = await _repository.GetByCarrierTypeAsync(carrierType);
        return packages.Any(p => p.PackageType == packageType);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _repository.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
} 