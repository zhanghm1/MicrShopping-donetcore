// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using MicrShopping.Domain;

namespace MicrShopping.Infrastructure.EFCore
{
    public class ApplicationDbSeedData
    {
        public static void EnsureSeedData(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                List<AddUser> users = new List<AddUser>()
                {
                    new AddUser(){UserName="admin",Password="Admin123456!",NickName="哈哈哈",Claims=new List<Claim>{
                        new Claim(JwtClaimTypes.Name, "admin"),
                        new Claim(JwtClaimTypes.Role, "admin"),
                        }
                    },
                    new AddUser(){UserName="admin1",Password="Admin123456!",NickName="嘻嘻嘻",Claims=new List<Claim>{
                        new Claim(JwtClaimTypes.Name, "admin1"),
                        new Claim(JwtClaimTypes.Role, "admin1"),
                        }
                    }
                };
                foreach (var item in users)
                {
                    var alice = userMgr.FindByNameAsync(item.UserName).Result;
                    if (alice == null)
                    {
                        alice = new ApplicationUser
                        {
                            UserName = item.UserName
                        };
                        var result = userMgr.CreateAsync(alice, item.Password).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(alice, item.Claims).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        //Log.Debug("alice created");
                    }
                    else
                    {
                        //Log.Debug("alice already exists");
                    }
                }
            }
        }
    }

    public class AddUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string NickName { get; set; }

        public List<Claim> Claims { get; set; }
    }
}