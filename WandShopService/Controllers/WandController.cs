using Microsoft.AspNetCore.Mvc;
using WandShop.Application.Service;
using WandShop.Domain.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WandShopService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WandController : ControllerBase
    {
        private IWandService _wandService;
        public WandController(IWandService wandService)
        {
            _wandService = wandService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var result = await _wandService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var result = await _wandService.GetAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Wand wand)
        {
            var result = await _wandService.AddAsync(wand);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Wand wand)
        {
            var result = await _wandService.UpdateAsync(wand);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var wand = await _wandService.GetAsync(id);
            wand.Deleted = true;
            var result = await _wandService.UpdateAsync(wand);

            return Ok(result);
        }
    }
}
