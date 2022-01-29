using System;
using System.Reflection;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using Jenshin.Impack.API.Model;
using Jenshin.Impack.API.Output;

using Binus.WS.Pattern.Output;
using Binus.WS.Pattern.Service;
using Jenshin.Impack.API.Model.Request;

namespace Jenshin.Impack.API.Services
{
    [ApiController]
    [Route("topup")]
    public class TopUpService : BaseService
    {
        public TopUpService(ILogger<BaseService> logger) : base(logger)
        {
        }

        /// <summary>
        /// Top Up
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /topup
        ///
        /// </remarks>
        /// <returns>Success</returns>
        /// <response code="200">Returns success if topup is successful</response>
        /// 
        /// SOAL NOMOR 5 - Top-Up
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ItemOutput), StatusCodes.Status200OK)]
        public IActionResult TopUp([FromBody] TopUpRequestDTO data, [FromHeader] string Authorization)
        {
            try
            {
                // -- MODIFY THIS TO YOUR OWN STUDENT ID (NIM) !! -- //
                var NIM = "2440067175";
                if(!Authorization.Equals("Basic " + NIM))
                {
                    throw new Exception("Invalid credentials");
                }

                var objJSON = new TopUpOutput();
                objJSON.Message = Helper.TopUpHelper.TopUp(data);
                return new OkObjectResult(objJSON);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new OutputBase(ex));
            }
        }
    }
}