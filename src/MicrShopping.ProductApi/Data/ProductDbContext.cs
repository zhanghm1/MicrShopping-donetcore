using Microsoft.EntityFrameworkCore;
using MicrShopping.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicrShopping.ProductApi.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {

        }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductClass> ProductClass { get; set; }
        public DbSet<ProductClassLink> ProductClassLink { get; set; }
    }
}