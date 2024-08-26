using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static BlizzardApiWeaponRetriever.IBlizzardApiService;

namespace BlizzardApiWeaponRetriever.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class BlizzardController : ControllerBase
    {

        private readonly BlizzardApiService _blizzardApiService;
        public OAuthClient _authClient;
        public string _accessToken;


        public BlizzardController()
        {
            _blizzardApiService = new BlizzardApiService(Environment.GetEnvironmentVariable("CLIENTID"), Environment.GetEnvironmentVariable("CLIENTSECRET"), "https://eu.api.blizzard.com/");

        }


        [HttpGet("character/{region}/{realmSlug}/{characterName}")]
        public async Task<IActionResult> GetCharacter(string region, string realmSlug, string characterName)
        {
            var characterProfile = await _blizzardApiService.GetCharacterProfileAsync(region, realmSlug, characterName);
            return Content(characterProfile, "application/json");
        }


        [HttpGet("/data/wow/item-class/index")]
        public async Task<OneHandedSword> GetSword(string sword)
        {

            if (string.IsNullOrEmpty(_accessToken))
            {
                _accessToken = _blizzard();
            }
            var response = await _blizzardApiService.GetAsync<BlizzardSingleWeapon<OneHandedSword>>(
            "weapons/by/name/{sword}",
            new { sword }
            );
            return response!.Data;
        }
        record BlizzardSingleWeapon<T>(T Data);

    }
}
