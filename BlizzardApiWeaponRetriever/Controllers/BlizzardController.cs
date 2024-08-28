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
       
        
        private readonly BlizzardApiService _blizzardApiService;


        public BlizzardController()
        {
            _blizzardApiService = new BlizzardApiService("cccf534838d8482588acc8e33df981c6","3pZ4dSCVGhu2dDKfUGKl3vTz148C2a8r");

        }


   

        [HttpGet("data/wow/search/item")]
        public async Task<IActionResult> GetItemClasses()
        {
            var itemClasses = await _blizzardApiService.GetItemClassesIndex();
            return Content(itemClasses, "application/json");
        }



    }
}
