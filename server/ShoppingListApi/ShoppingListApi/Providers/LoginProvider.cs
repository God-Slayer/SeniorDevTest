using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ShoppingListApi.Constants;
using ShoppingListApi.Interfaces;
using ShoppingListApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShoppingListApi.Providers
{
    public class LoginProvider : ILoginProvider
    {
        private readonly FacebookCredentials _facebookCredentials;
        private readonly Jwt _jwt;
        private readonly HttpClient _httpClient;

        public LoginProvider(
            IOptions<FacebookCredentials> facebookCredentials,
            IOptions<Jwt> jwt,
            HttpClient httpClient)
        {
            _facebookCredentials = facebookCredentials.Value;
            _httpClient = httpClient;
            _jwt = jwt.Value;
        }

        public async Task<ApiResponse> LoginWithFacebook(string credentials)
        {
            FBUser fbToken = await ValidateFacebookToken(credentials);

            if (fbToken == null || !fbToken.Data.IsValid)
            {
                return new ApiResponse()
                {
                    Message = "Token is not valid",
                    Success = false,
                };
            }

            FBUserInfo userInfo = await GetFacebookUserInfo(credentials);

            if (userInfo == null)
            {
                return new ApiResponse()
                {
                    Message = "No user information available.",
                    Success = false
                };
            }

            string jwtToken = await GenerateJwtToken(userInfo.Id);

            return new ApiResponse()
            {
                Data = userInfo,
                Message = jwtToken,
                Success = true,
            };
        }

        private async Task<FBUser> ValidateFacebookToken(string credentials)
        {
            HttpResponseMessage debugTokenResponse = await _httpClient.GetAsync($"{FBConstants.ValidateEndpoint}{credentials}&access_token={_facebookCredentials.AppId}|{_facebookCredentials.AppSecret}");
            string response = await debugTokenResponse.Content.ReadAsStringAsync();
            FBUser userObj = JsonConvert.DeserializeObject<FBUser>(response);

            return userObj ?? null;
        }

        private async Task<FBUserInfo> GetFacebookUserInfo(string credentials)
        {
            HttpResponseMessage meResponse = await _httpClient.GetAsync($"{FBConstants.UserEndpoint}{credentials}");
            var userContent = await meResponse.Content.ReadAsStringAsync();
            var userContentObj = JsonConvert.DeserializeObject<FBUserInfo>(userContent);

            return userContentObj ?? null;
        }

        private async Task<string> GenerateJwtToken(string userId)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(_jwt.Secret);
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", userId) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string encrypterToken = tokenHandler.WriteToken(token);

            return encrypterToken;
        }
    }
}