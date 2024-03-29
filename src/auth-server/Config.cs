﻿using Duende.IdentityServer.Models;
using IdentityModel;

namespace IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource()
            {
                Name = "verification",
                UserClaims = new List<string> 
                { 
                    JwtClaimTypes.Email,
                    JwtClaimTypes.EmailVerified
                }
            },
            new IdentityResources.Email(),
            new IdentityResource {
                Name = "roles",
                UserClaims = new List<string> {"role"}
            },
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("catalog.api"),
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            // interactive client using code flow + pkce
            new Client
            {
                ClientId = "interactive",
                ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },
                    
                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "http://localhost:5002/signin-oidc" },
                FrontChannelLogoutUri = "http://localhost:5002/signout-oidc",
                PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                AllowOfflineAccess = true,
                AllowedScopes = {
                    "openid",
                    "profile",
                    "catalog.api",
                    "verification" },
                
                //https://stackoverflow.com/questions/48207770/identityserver4-custom-authenticationhandler-cant-find-all-claims-for-a-user?answertab=active#tab-top
                // Allowed for User.IsInRole("[role]")
                AlwaysSendClientClaims = true,
                AlwaysIncludeUserClaimsInIdToken = true,
            },
        };
}
