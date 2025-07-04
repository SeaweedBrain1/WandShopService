﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WandShop.Application.Service;
using WandShop.Domain.Models;
using WandShop.Domain.Models.Dto;

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

        [HttpGet("valid")]
        public async Task<ActionResult> GetValid()
        {
            var result = await _wandService.GetAllValidAsync();
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

        [HttpGet("by")]
        public async Task<ActionResult> GetBy([FromQuery] WandFilterDto filter)
        {
            var result = await _wandService.GetWandsByAsync(filter);

            if (result == null || result.Count == 0)
                return NotFound("No wands found matching the provided filters.");

            return Ok(result);
        }

        [Authorize(Policy = "EmployeeOnly")]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateWandDto createWandDto)
        {
            var result = await _wandService.AddAsync(createWandDto);

            return Ok(result);
        }

        [Authorize(Policy = "EmployeeOnly")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] UpdateWandDto updateWandDto)
        {
            var result = await _wandService.UpdateAsync(id, updateWandDto);

            return Ok(result);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {

            var result = await _wandService.DeleteWand(id);

            return Ok(result);
        }
    }
}