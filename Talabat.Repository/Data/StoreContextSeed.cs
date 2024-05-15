using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregation;

namespace Talabat.Repository.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext dbContext)
        {
            if (!dbContext.ProductBrands.Any())//one element inside collection
            {
                var brandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                if (brands?.Count > 0)
                {
                    foreach (var brand in brands)
                        await dbContext.ProductBrands.AddAsync(brand);

                    await dbContext.SaveChangesAsync();
                }
            }

            if (!dbContext.ProductTypes.Any())//one element inside collection
            {
                var TypesData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);

                if (types?.Count > 0)
                {
                    foreach (var type in types)
                        await dbContext.ProductTypes.AddAsync(type);

                    await dbContext.SaveChangesAsync();
                }
            }

            if (!dbContext.Products.Any())//one element inside collection
            {
                var productData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productData);

                if (products?.Count > 0)
                {
                    foreach (var product in products)
                        await dbContext.Products.AddAsync(product);

                    await dbContext.SaveChangesAsync();
                }
            }
            if (!dbContext.DeliveryMethods.Any())//one element inside collection
            {
                var MethodsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(MethodsData);

                if (deliveryMethods?.Count > 0)
                {
                    foreach (var methods in deliveryMethods)
                        await dbContext.DeliveryMethods.AddAsync(methods);

                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
