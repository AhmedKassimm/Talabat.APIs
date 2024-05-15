﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var User = new AppUser()
                {
                    DisplayName = "Ahmed Nasser",
                    Email = "ahmed.nasser@gmail.com",
                    UserName = "ahmed.nasser",
                    PhoneNumber = "01271155277"
                };
                await userManager.CreateAsync(User,"P@ssw0rd");
            
            }
        }
    }
}
