using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;


namespace BlizzardApiWeaponRetriever.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class BlizzardController : Controller
    {

        public string nameOfSearch;
        private readonly BlizzardApiService _blizzardApiService;


        public BlizzardController()
        {
            _blizzardApiService = new BlizzardApiService(Environment.GetEnvironmentVariable("BLIZZARD_CLIENT_ID"), Environment.GetEnvironmentVariable("BLIZZARD_CLIENT_SECRET"));

        }


   

        [HttpGet("data/wow/search/item/{nameOfSearch}")]
        public async Task<IActionResult> GetItems(string nameOfSearch)
        {
          
            var itemClasses = await _blizzardApiService.RequestItems(nameOfSearch);
            return Content(itemClasses, "application/json");
        }



    }
}
