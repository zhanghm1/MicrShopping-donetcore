using MicrShopping.Infrastructure.Common.BaseModels;
using MicrShopping.PayApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicrShopping.OrderApi.Data
{
    public class PayDbContextSeed : IDbContextSeed
    {
        private PayDbContext _dbContext;

        public PayDbContextSeed(PayDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Init()
        {
        }
    }
}