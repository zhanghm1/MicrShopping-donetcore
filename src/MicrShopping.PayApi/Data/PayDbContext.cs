﻿using Microsoft.EntityFrameworkCore;
using MicrShopping.Domain.Entities.Pays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicrShopping.PayApi.Data
{
    public class PayDbContext : DbContext
    {
        public PayDbContext(DbContextOptions<PayDbContext> options): base(options)
        {

        }
        public DbSet<PayRecored> PayRecored { get; set; }

    }
}
