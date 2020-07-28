﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Couponel.Business.Identities.Offerors.Models;
using Couponel.Business.Identities.Offerors.Services.Interfaces;
using Couponel.Business.Identities.Offerors.Models;
using Couponel.Business.Identities.Offerors.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Couponel.API.Controllers
{
    [Route("offerer")]
    [ApiController]
    public class OffererController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOffererService _offererService;

        public OffererController(ILogger<HomeController> logger, IOffererService offererService)
        {
            _logger = logger;
            _offererService = offererService;
        }

        [HttpGet("{offererId}")]
        public async Task<IActionResult> GetById([FromRoute] Guid offererId)
        {
            var result = await _offererService.GetById(offererId);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateOffererModel model)
        {
            var result = await _offererService.Add(model);

            return Created(result.Id.ToString(), null);
        }

        [HttpDelete("{offererId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid offererId)
        {
            await _offererService.Delete(offererId);

            return NoContent();
        }
    }
}