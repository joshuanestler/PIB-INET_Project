using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Pokewordle.Shared;

namespace Pokewordle.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class DataController : ControllerBase
    {
        [HttpPost]
        public HttpResponseMessage Post([FromBody] PokeData data)
        {
            // Handle the POST request here
            // ...

            return new ActionResult<PokeData>(new PokeData { Id = 10, Name = "Glurak" });
        }

    }
}
