using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Net.WebRequestMethods;




namespace BlizzardApiWeaponRetriever
{
    public class BlizzardApiService  
    {
        private readonly HttpClient _httpClient;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private string _accessToken;
    
        

        public BlizzardApiService(string clientId, string clientSecret)
        {
            _httpClient = new HttpClient();
            _clientId = Environment.GetEnvironmentVariable("BLIZZARD_CLIENT_ID");
            _clientSecret = Environment.GetEnvironmentVariable("BLIZZARD_CLIENT_SECRET");
           
        }

        private async Task<string> GetAccessTokenAsync()
        {
            var authenticationString = $"{_clientId}:{_clientSecret}";
            var base64String = Convert.ToBase64String(
                System.Text.Encoding.ASCII.GetBytes(authenticationString));

            var request = new HttpRequestMessage(HttpMethod.Post, "https://oauth.battle.net/token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64String);
 
            request.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            });

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var tokenObject = JObject.Parse(content);
            _accessToken = tokenObject["access_token"].ToString();

            return _accessToken;

        }


        public async Task<string> GetItemClassesIndex()
        {
            if (string.IsNullOrEmpty(_accessToken))
            {
                _accessToken = await GetAccessTokenAsync();
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);


            var requestUrl = $"https://eu.api.blizzard.com/data/wow/search/item?locale=en_GB&namespace=static-classic-eu&orderby=level&name.en_US=sword";
            
 
            var response = await _httpClient.GetAsync(requestUrl);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        
        }
    }
}