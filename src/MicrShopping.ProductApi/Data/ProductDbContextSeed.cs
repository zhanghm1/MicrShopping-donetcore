using MicrShopping.Domain.Entities.Products;
using MicrShopping.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicrShopping.ProductApi.Data
{
    public class ProductDbContextSeed
    {
        private ProductDbContext _dbContext;
        public ProductDbContextSeed(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Init()
        {
            List<ProductClass> productClasses = new List<ProductClass>() {
                new ProductClass(){Code=CodePrefix.ProductClassCodePrefix+"0001",Name="分类1" },
                new ProductClass(){Code=CodePrefix.ProductClassCodePrefix+"0002",Name="分类2" },
                new ProductClass(){Code=CodePrefix.ProductClassCodePrefix+"0003",Name="分类3" },
            };

            List<Product> products = new List<Product>() {
                new Product(){Code=CodePrefix.ProductCodePrefix+"0001",FormerPrice=12,RealPrice=10,NowCount=10,Name="产品1" },
                new Product(){Code=CodePrefix.ProductCodePrefix+"0002",FormerPrice=12,RealPrice=10,NowCount=10,Name="产品2" },
                new Product(){Code=CodePrefix.ProductCodePrefix+"0003",FormerPrice=12,RealPrice=10,NowCount=10,Name="产品3" },
            };

            foreach (var item in products)
            {
                var product = _dbContext.Product.FirstOrDefault(a => a.Code == item.Code);
                if (product==null)
                {
                    _dbContext.Product.Add(item);
                }
                else 
                {
                    item.Id = product.Id;
                }
            }
            foreach (var item in productClasses)
            {
                var productClass = _dbContext.ProductClass.FirstOrDefault(a => a.Code == item.Code);
                if (productClass == null)
                {
                    _dbContext.ProductClass.Add(item);
                }
                else
                {
                    item.Id = productClass.Id;
                }
            }
            _dbContext.SaveChanges();


            List<ProductClassLink> productClassLinks = new List<ProductClassLink>();
            foreach (var itemP in products)
            {
                foreach (var itemPC in productClasses)
                {
                    var productClass = _dbContext.ProductClassLink.FirstOrDefault(a => a.ProductClassId== itemPC.Id && a.ProductId==itemP.Id);
                    if (productClass == null)
                    {
                        ProductClassLink productClassLink = new ProductClassLink()
                        {
                            ProductClassId = itemPC.Id,
                            ProductId = itemP.Id
                        };
                        _dbContext.ProductClassLink.Add(productClassLink);
                    }
                }
            }
            _dbContext.SaveChanges();
        }
    }
}
