using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using static BlizzardApiWeaponRetriever.IBlizzardApiService;


namespace BlizzardApiWeaponRetriever
{
    public class BlizzardApiService : IBlizzardApiService, IDisposable
    {
        readonly RestClient _client;
        public OAuthClient _authClient;

        //public OAuthClient OAuthClient;
        public string _accessToken;
        public BlizzardApiService(string apiKey, string apiKeySecret, string baseUrl)
        {

            var options = new RestClientOptions("https://eu.api.blizzard.com/");
            _client = new RestClient(options);
        }

        public async Task<ItemClassesIndex> GetItemClasses(string item)
        {

            if (string.IsNullOrEmpty(_accessToken))
            {
                _accessToken = _authClient.GetTokenOut();
            }
            var response = await _client.GetAsync<BlizzardSingleWeapon<ItemClassesIndex>>(
            "weapons/by/name/{sword}",
            new { item }
            );
            return response!.Data;
        }
        record BlizzardSingleWeapon<T>(T Data);


        public void Dispose()
        {
            _client?.Dispose();
            GC.SuppressFinalize(this);
        }



    }
}