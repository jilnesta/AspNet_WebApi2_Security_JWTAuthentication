﻿using Microsoft.AspNet.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApi2_Owin_OAuthAccessTokensAndClaims.Models.DomainEntities.Identity;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Claims
{
    /// <summary>
    /// Responsible for generating claims for app users
    /// </summary>
    public class ApplicationClaimsFactory : ClaimsIdentityFactory<ApplicationUser>
    {
        public ApplicationClaimsFactory()
        {
            // Customize the type names of claims (just to simplify them)
            base.UserNameClaimType = "username";
            base.RoleClaimType = "role";
        }

        public override async Task<ClaimsIdentity> CreateAsync(UserManager<ApplicationUser, string> manager, ApplicationUser user, string authenticationType)
        {
            var ci = await base.CreateAsync(manager, user, authenticationType);

            // Authorization level claim
            ci.AddClaim(CreateClaim("level", user.Person.Level.ToString()));

            // Full Time Employee Claim
            var daysInWork = (DateTime.Now.Date - user.Person.JoinDate).TotalDays;

            if (daysInWork > 90)
            {
                ci.AddClaim(CreateClaim("FTE", "1"));

            }
            else
            {
                ci.AddClaim(CreateClaim("FTE", "0"));
            }

            return ci;
        }

        private Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value, ClaimValueTypes.String);
        }
    }
}