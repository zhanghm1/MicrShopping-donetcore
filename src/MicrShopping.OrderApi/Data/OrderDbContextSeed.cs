using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicrShopping.OrderApi.Data
{
    public class OrderDbContextSeed
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
