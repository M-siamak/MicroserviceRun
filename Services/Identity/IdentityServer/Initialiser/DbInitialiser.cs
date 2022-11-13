using IdentityModel;
using IdentityServer.DataBase;
using IdentityServer.Model;
using IdentityServer4;
using Microsoft.AspNetCore.Identity;
using static IdentityServer4.Models.IdentityResources;
using System.Security.Claims;
using System.Text.Json;

namespace IdentityServer.Initialiser
{
    public class DbInitialiser : IDbInitialiser
    {
        private readonly ApplicationDbContext _contex;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DbInitialiser(ApplicationDbContext contex, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _contex = contex;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            if(_roleManager.FindByNameAsync(SD.Admin).Result == null)
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Customer)).GetAwaiter().GetResult();
            }
            else { return; }

            ApplicationUser adminUser = new ApplicationUser()
            {
                UserName = "siamak",
                Email = "siamak@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "09127334489",
                LastName = "anzabi"
            };

            _userManager.CreateAsync(adminUser , "a1").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(adminUser , SD.Admin).GetAwaiter().GetResult();
            var address = new
            {
                street_address = "One Hacker Way",
                locality = "Heidelberg",
                postal_code = 69118,
                country = "Germany"
            };
            //var temp = _userManager.AddClaimAsync(adminUser, new Claim[]
            //{
            //                new Claim(JwtClaimTypes.Name, adminUser.FirstName+" "+adminUser.LastName),
            //                new Claim(JwtClaimTypes.GivenName, adminUser.FirstName),
            //                new Claim(JwtClaimTypes.FamilyName, adminUser.LastName),
            //                new Claim(JwtClaimTypes.Role,SD.Admin)
            //}).Result();
        }
    }
}
