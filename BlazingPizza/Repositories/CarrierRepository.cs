using BlazingPizza;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CarrierRepository : ICarrierRepository
{
    private readonly PizzaStoreContext _context;
    public CarrierRepository(PizzaStoreContext context)
    {
        _context = context;
    }
    
    public async Task<List<Carrier>> GetCarriers()
    {
        return await _context.Carriers.ToListAsync();
    }

    public async Task<Carrier?> GetCarrierById(int id)
    {
        return await _context.Carriers.FindAsync(id);
    }

    public async Task<Carrier> CreateCarrier(Carrier carrier)
    {
        // Ensure string properties are not null
        carrier.carrierName = carrier.carrierName ?? string.Empty;
        carrier.internetSubscribEmail = carrier.internetSubscribEmail ?? string.Empty;
        carrier.ImeiUserName = carrier.ImeiUserName ?? string.Empty;
        carrier.ImeiAPIKey = carrier.ImeiAPIKey ?? string.Empty;
        carrier.ImeiAPIURL = carrier.ImeiAPIURL ?? string.Empty;
        carrier.APIPassword = carrier.APIPassword ?? string.Empty;
        carrier.imageName = carrier.imageName ?? string.Empty;

        _context.Carriers.Add(carrier);
        await _context.SaveChangesAsync();
        return carrier;
    }

    public async Task<Carrier?> UpdateCarrier(Carrier carrier)
    {
        var existingCarrier = await _context.Carriers.FindAsync(carrier.id);
        if (existingCarrier == null)
        {
            return null;
        }

        // Ensure string properties are not null
        carrier.carrierName = carrier.carrierName ?? string.Empty;
        carrier.internetSubscribEmail = carrier.internetSubscribEmail ?? string.Empty;
        carrier.ImeiUserName = carrier.ImeiUserName ?? string.Empty;
        carrier.ImeiAPIKey = carrier.ImeiAPIKey ?? string.Empty;
        carrier.ImeiAPIURL = carrier.ImeiAPIURL ?? string.Empty;
        carrier.APIPassword = carrier.APIPassword ?? string.Empty;
        carrier.imageName = carrier.imageName ?? string.Empty;

        // Update properties
        existingCarrier.carrierName = carrier.carrierName;
        existingCarrier.isActive = carrier.isActive;
        existingCarrier.UnderMaintenance = carrier.UnderMaintenance;
        existingCarrier.wholeSalePercent = carrier.wholeSalePercent;
        existingCarrier.internetSubscribEmail = carrier.internetSubscribEmail;
        existingCarrier.ImeiUserName = carrier.ImeiUserName;
        existingCarrier.ImeiAPIKey = carrier.ImeiAPIKey;
        existingCarrier.ImeiAPIURL = carrier.ImeiAPIURL;
        existingCarrier.APIPassword = carrier.APIPassword;
        existingCarrier.AdsSendSMS = carrier.AdsSendSMS;
        existingCarrier.AdsSendWP = carrier.AdsSendWP;
        existingCarrier.InvoiceCategory = carrier.InvoiceCategory;
        existingCarrier.imageName = carrier.imageName;

        await _context.SaveChangesAsync();
        return existingCarrier;
    }

    public async Task<bool> DeleteCarrier(int id)
    {
        var carrier = await _context.Carriers.FindAsync(id);
        if (carrier == null)
        {
            return false;
        }

        _context.Carriers.Remove(carrier);
        await _context.SaveChangesAsync();
        return true;
    }
}