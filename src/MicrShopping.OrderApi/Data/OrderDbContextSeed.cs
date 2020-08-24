using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicrShopping.Infrastructure.Common.BaseModels;

namespace MicrShopping.OrderApi.Data
{
    public class OrderDbContextSeed : IDbContextSeed
    {
        private OrderDbContext _dbContext;

        public OrderDbContextSeed(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Init()
        {
        }
    }
}