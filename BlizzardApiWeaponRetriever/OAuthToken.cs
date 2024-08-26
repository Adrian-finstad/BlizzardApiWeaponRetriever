using Microsoft.AspNetCore.Authentication;
using RestSharp;
using RestSharp.Authenticators;
using System.Text.Json.Serialization;

namespace BlizzardApiWeaponRetriever
{
    public class OAuthClient : AuthenticatorBase
    {
        record TokenResponse
        {
            [JsonPropertyName("token_type")]
            public string TokenType { get; init; }
            [JsonPropertyName("access_token")]
            public string AccessToken { get; init; }
        }

        readonly string _baseUrl = "https://eu.api.blizzard.com/";
        readonly string _clientId = Environment.GetEnvironmentVariable("CLIENTID");
        readonly string _clientSecret = Environment.GetEnvironmentVariable("CLIENTSECRET");


        public OAuthClient(string baseUrl, string clientId, string clientSecret) : base("")
        {
            _baseUrl = baseUrl;
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        protected override async ValueTask<Parameter> GetAuthenticationParameter(string accessToken)
        {
            Token = string.IsNullOrEmpty(Token) ? await GetToken() : Token;
            return new HeaderParameter(KnownHeaders.Authorization, Token);
        }



        async Task<string> GetToken()
        {
            var options = new RestClientOptions(_baseUrl)
            {
                Authenticator = new HttpBasicAuthenticator(_clientId, _clientSecret),
            };
            using var client = new RestClient(options);

            var request = new RestRequest("https://oauth.battle.net/token")
                .AddParameter("application/x-www-form-urlencoded", $"grant_type=client_credentials&client_id={_clientId}&client_secret={_clientSecret}", ParameterType.RequestBody);
            var response = await client.PostAsync<TokenResponse>(request);
            return $"{response!.TokenType} {response!.AccessToken}";
        }

        public string GetTokenOut()
        {
            if (string.IsNullOrEmpty(Token))
            {
                GetToken();
                return Token;
                
            }
            return Token;
        }

    }
}

