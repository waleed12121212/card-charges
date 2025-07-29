using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlazingPizza.Shared;
using BlazingPizza.Shared.Interfaces;

namespace BlazingPizza;

public class RefillCardRepository : IRefillCardRepository
{
    private readonly PizzaStoreContext _context;

    public RefillCardRepository(PizzaStoreContext context)
    {
        _context = context;
    }

    public async Task<List<RefillCard>> GetByCarrierId(int carrierId)
    {
        return await _context.RefillCards
            .Where(r => r.CarrierID == carrierId)
            .ToListAsync();
    }

    public async Task<RefillCard?> GetByIdAsync(int id)
    {
        return await _context.RefillCards.FindAsync(id);
    }

    public async Task<List<RefillCard>> GetAllAsync()
    {
        return await _context.RefillCards.ToListAsync();
    }

    public async Task<RefillCard> CreateAsync(RefillCard refillCard)
    {
        // Ensure string properties are not null - set default values for required fields
        refillCard.ProductName = refillCard.ProductName ?? string.Empty;
        refillCard.description = refillCard.description ?? string.Empty;
        refillCard.imageName = refillCard.imageName ?? string.Empty;
        refillCard.ProductIdenfity = refillCard.ProductIdenfity ?? string.Empty;
        refillCard.ProductNameHe = refillCard.ProductNameHe ?? string.Empty;
        refillCard.ProductNameEn = refillCard.ProductNameEn ?? string.Empty;
        refillCard.DescriptionHe = refillCard.DescriptionHe ?? string.Empty;
        refillCard.DescriptionEn = refillCard.DescriptionEn ?? string.Empty;
        refillCard.EmailCode = refillCard.EmailCode ?? string.Empty;
        refillCard.ApiProductIdStr = refillCard.ApiProductIdStr ?? string.Empty;
        refillCard.DetailsUrl = refillCard.DetailsUrl ?? string.Empty;
        refillCard.SendMsgBy = refillCard.SendMsgBy ?? "SMS"; // Default value
        refillCard.AdsCardSms = refillCard.AdsCardSms ?? string.Empty;
        refillCard.AdsCardWhatsapp = refillCard.AdsCardWhatsapp ?? string.Empty;

        refillCard.CreatedOn = DateTime.Now;

        _context.RefillCards.Add(refillCard);
        await _context.SaveChangesAsync();
        return refillCard;
    }

    public async Task<RefillCard?> UpdateAsync(RefillCard refillCard)
    {
        var existingCard = await _context.RefillCards.FindAsync(refillCard.id);
        if (existingCard == null)
            return null;

        // Update properties
        existingCard.ProductName = refillCard.ProductName ?? string.Empty;
        existingCard.CardAmount = refillCard.CardAmount;
        existingCard.CarrierID = refillCard.CarrierID;
        existingCard.apiProductId = refillCard.apiProductId;
        existingCard.description = refillCard.description ?? string.Empty;
        existingCard.isActive = refillCard.isActive;
        existingCard.isTemp = refillCard.isTemp;
        existingCard.imageName = refillCard.imageName ?? string.Empty;
        existingCard.ProductIdenfity = refillCard.ProductIdenfity ?? string.Empty;
        existingCard.price = refillCard.price;
        existingCard.wholeSalePercent = refillCard.wholeSalePercent;
        existingCard.Cost = refillCard.Cost;
        existingCard.ProductNameHe = refillCard.ProductNameHe ?? string.Empty;
        existingCard.ProductNameEn = refillCard.ProductNameEn ?? string.Empty;
        existingCard.DescriptionHe = refillCard.DescriptionHe ?? string.Empty;
        existingCard.DescriptionEn = refillCard.DescriptionEn ?? string.Empty;
        existingCard.SendByEmail = refillCard.SendByEmail;
        existingCard.EmailCode = refillCard.EmailCode ?? string.Empty;
        existingCard.ApiProductId1 = refillCard.ApiProductId1;
        existingCard.ApiProductId2 = refillCard.ApiProductId2;
        existingCard.ApiProductIdStr = refillCard.ApiProductIdStr ?? string.Empty;
        existingCard.SiteId = refillCard.SiteId;
        existingCard.SortOrder = refillCard.SortOrder;
        existingCard.IsFav = refillCard.IsFav;
        existingCard.Group = refillCard.Group;
        existingCard.RemoteProviderId = refillCard.RemoteProviderId;
        existingCard.DetailsUrl = refillCard.DetailsUrl ?? string.Empty;
        existingCard.Lotto = refillCard.Lotto;
        existingCard.PriceExcempt = refillCard.PriceExcempt;
        existingCard.PointsEndUser = refillCard.PointsEndUser;
        existingCard.PointsUser = refillCard.PointsUser;
        existingCard.PointsReseller = refillCard.PointsReseller;
        existingCard.SendMsgBy = refillCard.SendMsgBy ?? string.Empty;
        existingCard.AdsCardSms = refillCard.AdsCardSms ?? string.Empty;
        existingCard.AdsCardWhatsapp = refillCard.AdsCardWhatsapp ?? string.Empty;
        existingCard.UpdatedOn = DateTime.Now;

        _context.RefillCards.Update(existingCard);
        await _context.SaveChangesAsync();
        return existingCard;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var refillCard = await _context.RefillCards.FindAsync(id);
        if (refillCard == null)
            return false;

        _context.RefillCards.Remove(refillCard);
        await _context.SaveChangesAsync();
        return true;
    }
} 