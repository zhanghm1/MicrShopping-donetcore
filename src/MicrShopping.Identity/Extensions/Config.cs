// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace MicrShopping.Identity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };


        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>
            {
                new ApiResource("orderapi", "order",new List<string>{JwtClaimTypes.Role }),
                new ApiResource("payapi", "pay",new List<string>{JwtClaimTypes.Role }),
                new ApiResource("productapi", "product",new List<string>{JwtClaimTypes.Role }),
                new ApiResource("usermanageapi", "usermanage api ",new List<string>{JwtClaimTypes.Role }),
                new ApiResource("testapi", "test api ",new List<string>{JwtClaimTypes.Role }),
                
            };

        public static IEnumerable<Client> Clients(IConfiguration Configuration) 
        {
            System.Console.WriteLine("Clients__MVCUrl :"+ Configuration["Clients:MVCUrl"]);
            System.Console.WriteLine("Clients__JsVueUrl :" + Configuration["Clients:JsVueUrl"]);

            string Clients_MVCUrl = Configuration["Clients:MVCUrl"];
            string Clients_JsVueUrl = Configuration["Clients:JsVueUrl"];

            return new List<Client>
            {
                // interactive ASP.NET Core MVC client
                new Client
                {
                    ClientId = "mvc",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireConsent = true,
                    RequirePkce = true,
                
                    // where to redirect to after login
                    RedirectUris = { Clients_MVCUrl + "/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { Clients_MVCUrl + "/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "orderapi","usermanageapi","productapi","testapi","payapi"
                    },

                    AllowOfflineAccess = true
                },
                // JavaScript Client
                new Client
                {
                    ClientId = "js-vue",
                    ClientName = "JavaScript Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris =           { Clients_JsVueUrl + "/#/callback" },
                    PostLogoutRedirectUris = { Clients_JsVueUrl },
                    AllowedCorsOrigins =     { Clients_JsVueUrl },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "orderapi","usermanageapi","productapi","testapi","payapi"
                    }
                },
                // app Client
                new Client
                {
                    ClientId = "app",
                    ClientName = "app",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "orderapi","usermanageapi","productapi","testapi","payapi"
                    }
                }
            };
        }
            
    }
}