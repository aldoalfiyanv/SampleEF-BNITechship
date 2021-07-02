using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SampleEF.Helpers;
using SampleEF.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SampleEF.Data
{
    public class UserData : IUser
    {
        private AppSettings _appSettings;
        private UserManager<IdentityUser> _userManager;
        public UserData(IOptions<AppSettings> appSettings, 
            UserManager<IdentityUser> userManager)
        {
            _appSettings = appSettings.Value;
            _userManager = userManager;
        }

        public async Task<User> Authenticate(string username, string password)
        {
            var userFind = await _userManager.CheckPasswordAsync(await _userManager.FindByNameAsync(username), password);
            if (!userFind)
                return null;

            var user = new User
            {
                Username = username
            };

            //MEMBUAT JWT TOKEN
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.Username));

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1), /*DURASI TOKEN*/
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            user.Password = string.Empty;
            return user;
        }

        public async Task Registration(User user)
        {
            try
            {
                var _user = new IdentityUser { UserName = user.Username, Email = user.Username };
                var result = await _userManager.CreateAsync(_user, user.Password);

                if (!result.Succeeded)
                    throw new Exception("Gagal menambahkan data user");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

       
    }
}
