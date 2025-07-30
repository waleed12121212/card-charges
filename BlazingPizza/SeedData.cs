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

        // Add Internet Packages seed data
        // Check for specific packages and add missing ones
        var packagesToAdd = new List<BlazingPizza.Shared.InternetPackage>();

        // أوريدو packages
        if (!db.InternetPackages.Any(p => p.CarrierType == BlazingPizza.Shared.CarrierType.أوريدو && p.PackageType == "أسبوعي"))
        {
            packagesToAdd.Add(new BlazingPizza.Shared.InternetPackage
            {
                Name = "حزمة أوريدو الأسبوعية",
                Description = "حزمة إنترنت أسبوعية من أوريدو",
                DataAmountMB = 3072, // 3 GB
                ValidityDays = 7,
                Price = 30.00m,
                Cost = 27.00m,
                CarrierType = BlazingPizza.Shared.CarrierType.أوريدو,
                PackageType = "أسبوعي",
                IsActive = true,
                SortOrder = 2,
                CreatedOn = DateTime.Now
            });
        }

        if (!db.InternetPackages.Any(p => p.CarrierType == BlazingPizza.Shared.CarrierType.أوريدو && p.PackageType == "شهري"))
        {
            packagesToAdd.Add(new BlazingPizza.Shared.InternetPackage
            {
                Name = "حزمة أوريدو الشهرية",
                Description = "حزمة إنترنت شهرية من أوريدو",
                DataAmountMB = 15360, // 15 GB
                ValidityDays = 30,
                Price = 100.00m,
                Cost = 90.00m,
                CarrierType = BlazingPizza.Shared.CarrierType.أوريدو,
                PackageType = "شهري",
                IsActive = true,
                SortOrder = 3,
                CreatedOn = DateTime.Now
            });
        }

        // جوال packages
        if (!db.InternetPackages.Any(p => p.CarrierType == BlazingPizza.Shared.CarrierType.جوال && p.PackageType == "يومي"))
        {
            packagesToAdd.Add(new BlazingPizza.Shared.InternetPackage
            {
                Name = "حزمة جوال اليومية",
                Description = "حزمة إنترنت يومية من جوال",
                DataAmountMB = 500,
                ValidityDays = 1,
                Price = 5.00m,
                Cost = 4.50m,
                CarrierType = BlazingPizza.Shared.CarrierType.جوال,
                PackageType = "يومي",
                IsActive = true,
                SortOrder = 1,
                CreatedOn = DateTime.Now
            });
        }

        if (!db.InternetPackages.Any(p => p.CarrierType == BlazingPizza.Shared.CarrierType.جوال && p.PackageType == "أسبوعي"))
        {
            packagesToAdd.Add(new BlazingPizza.Shared.InternetPackage
            {
                Name = "حزمة جوال الأسبوعية",
                Description = "حزمة إنترنت أسبوعية من جوال",
                DataAmountMB = 2048, // 2 GB
                ValidityDays = 7,
                Price = 25.00m,
                Cost = 22.50m,
                CarrierType = BlazingPizza.Shared.CarrierType.جوال,
                PackageType = "أسبوعي",
                IsActive = true,
                SortOrder = 2,
                CreatedOn = DateTime.Now
            });
        }

        if (!db.InternetPackages.Any(p => p.CarrierType == BlazingPizza.Shared.CarrierType.جوال && p.PackageType == "شهري"))
        {
            packagesToAdd.Add(new BlazingPizza.Shared.InternetPackage
            {
                Name = "حزمة جوال الشهرية",
                Description = "حزمة إنترنت شهرية من جوال",
                DataAmountMB = 10240, // 10 GB
                ValidityDays = 30,
                Price = 80.00m,
                Cost = 72.00m,
                CarrierType = BlazingPizza.Shared.CarrierType.جوال,
                PackageType = "شهري",
                IsActive = true,
                SortOrder = 3,
                CreatedOn = DateTime.Now
            });
        }

        // سيليكوم packages
        if (!db.InternetPackages.Any(p => p.CarrierType == BlazingPizza.Shared.CarrierType.سيليكوم && p.PackageType == "يومي"))
        {
            packagesToAdd.Add(new BlazingPizza.Shared.InternetPackage
            {
                Name = "حزمة سيليكوم اليومية",
                Description = "حزمة إنترنت يومية من سيليكوم",
                DataAmountMB = 400,
                ValidityDays = 1,
                Price = 4.00m,
                Cost = 3.60m,
                CarrierType = BlazingPizza.Shared.CarrierType.سيليكوم,
                PackageType = "يومي",
                IsActive = true,
                SortOrder = 1,
                CreatedOn = DateTime.Now
            });
        }

        if (!db.InternetPackages.Any(p => p.CarrierType == BlazingPizza.Shared.CarrierType.سيليكوم && p.PackageType == "أسبوعي"))
        {
            packagesToAdd.Add(new BlazingPizza.Shared.InternetPackage
            {
                Name = "حزمة سيليكوم الأسبوعية",
                Description = "حزمة إنترنت أسبوعية من سيليكوم",
                DataAmountMB = 1024, // 1 GB
                ValidityDays = 7,
                Price = 20.00m,
                Cost = 18.00m,
                CarrierType = BlazingPizza.Shared.CarrierType.سيليكوم,
                PackageType = "أسبوعي",
                IsActive = true,
                SortOrder = 2,
                CreatedOn = DateTime.Now
            });
        }

        if (!db.InternetPackages.Any(p => p.CarrierType == BlazingPizza.Shared.CarrierType.سيليكوم && p.PackageType == "شهري"))
        {
            packagesToAdd.Add(new BlazingPizza.Shared.InternetPackage
            {
                Name = "حزمة سيليكوم الشهرية",
                Description = "حزمة إنترنت شهرية من سيليكوم",
                DataAmountMB = 8192, // 8 GB
                ValidityDays = 30,
                Price = 65.00m,
                Cost = 58.50m,
                CarrierType = BlazingPizza.Shared.CarrierType.سيليكوم,
                PackageType = "شهري",
                IsActive = true,
                SortOrder = 3,
                CreatedOn = DateTime.Now
            });
        }

        if (packagesToAdd.Any())
        {
            db.InternetPackages.AddRange(packagesToAdd);
            db.SaveChanges();
        }
    }
}