namespace BlazingPizza;

public static class SeedData
{
    public static void Initialize(PizzaStoreContext db)
    {
        if (!db.Carriers.Any())
        {
            db.Carriers.AddRange(new Carrier[]
            {
                new Carrier { 
                    carrierName = "Carrier 1", 
                    isActive = true, 
                    wholeSalePercent = 5, 
                    internetSubscribEmail = "carrier1@email.com",
                    ImeiUserName = "user1",
                    ImeiAPIKey = "key1",
                    ImeiAPIURL = "https://api1.example.com",
                    APIPassword = "password1",
                    AdsSendSMS = false,
                    AdsSendWP = false
                },
                new Carrier { 
                    carrierName = "Carrier 2", 
                    isActive = true, 
                    wholeSalePercent = 7, 
                    internetSubscribEmail = "carrier2@email.com",
                    ImeiUserName = "user2",
                    ImeiAPIKey = "key2",
                    ImeiAPIURL = "https://api2.example.com",
                    APIPassword = "password2",
                    AdsSendSMS = false,
                    AdsSendWP = false
                }
            });
            db.SaveChanges();
        }

        if (!db.RefillCards.Any())
        {
            db.RefillCards.AddRange(new RefillCard[]
            {
                new RefillCard { 
                    ProductName = "بطاقة 10$", 
                    CardAmount = 10, 
                    CarrierID = db.Carriers.First().id, 
                    isActive = true, 
                    price = 10, 
                    wholeSalePercent = 5, 
                    CreatedOn = DateTime.Now,
                    description = "بطاقة شحن بقيمة 10 دولار",
                    imageName = "card10.jpg",
                    ProductIdenfity = "CARD10",
                    ProductNameHe = "بطاقة 10$",
                    ProductNameEn = "10$ Card",
                    DescriptionHe = "بطاقة شحن بقيمة 10 دولار",
                    DescriptionEn = "10$ recharge card",
                    SendByEmail = false,
                    EmailCode = "",
                    ApiProductIdStr = "",
                    SortOrder = 1,
                    IsFav = false,
                    SendMsgBy = "SMS",
                    AdsCardSms = "",
                    AdsCardWhatsapp = ""
                },
                new RefillCard { 
                    ProductName = "بطاقة 20$", 
                    CardAmount = 20, 
                    CarrierID = db.Carriers.First().id, 
                    isActive = true, 
                    price = 20, 
                    wholeSalePercent = 5, 
                    CreatedOn = DateTime.Now,
                    description = "بطاقة شحن بقيمة 20 دولار",
                    imageName = "card20.jpg",
                    ProductIdenfity = "CARD20",
                    ProductNameHe = "بطاقة 20$",
                    ProductNameEn = "20$ Card",
                    DescriptionHe = "بطاقة شحن بقيمة 20 دولار",
                    DescriptionEn = "20$ recharge card",
                    SendByEmail = false,
                    EmailCode = "",
                    ApiProductIdStr = "",
                    SortOrder = 2,
                    IsFav = false,
                    SendMsgBy = "SMS",
                    AdsCardSms = "",
                    AdsCardWhatsapp = ""
                }
            });
            db.SaveChanges();
        }
    }
}