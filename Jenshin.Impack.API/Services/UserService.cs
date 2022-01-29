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
using Binus.WS.Pattern.Entities;
using System.Linq;

namespace Jenshin.Impack.API.Services
{
    [ApiController]
    [Route("user")]
    public class UserService : BaseService
    {
        public UserService(ILogger<BaseService> logger) : base(logger)
        {
        }

        /// <summary>
        /// Get All User
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /user
        ///
        /// </remarks>
        /// <returns>A list of all User</returns>
        /// <response code="200">Returns a list of User</response>
        /// 
        /// SOAL NOMOR 1 - Get All User
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserOutput), StatusCodes.Status200OK)]
        public IActionResult GetAllUser()
        {
            try
            {
                var objJSON = new UserOutput();
                objJSON.Data = Helper.UserHelper.GetAllUser();
                return new OkObjectResult(objJSON);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new OutputBase(ex));
            }
        }

        /// <summary>
        /// Get Specific User
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /user
        ///
        /// </remarks>
        /// <returns>A list of all User</returns>
        /// <response code="200">Returns a list of User</response>
        /// 
        /// SOAL NOMOR 2 - Get Specified User
        [HttpGet]
        [Route("specific")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(SpecificUserOutput), StatusCodes.Status200OK)]
        public IActionResult GetSpecificUser([FromQuery] MsUser data)
        {
            try
            {
                var objJSON = new SpecificUserOutput();
                objJSON.Data = Helper.UserHelper.GetSpecificUser(data.UserEmail, data.UserName);
                return new OkObjectResult(objJSON);
            }
            catch (Exception ex)
            {
                if (!String.IsNullOrEmpty(data.UserEmail) && !String.IsNullOrEmpty(data.UserName))
                {
                    return StatusCode(500, new OutputBase(ex));
                }
                else if (String.IsNullOrEmpty(data.UserEmail) && String.IsNullOrEmpty(data.UserName))
                {
                    return StatusCode(400, new OutputBase(ex)
                    {
                        ResultCode = 400,
                    });
                }
                else if (ex.Message.Contains("Account not found"))
                {
                    return StatusCode(404, new OutputBase(ex)
                    {
                        ResultCode = 404,
                    });
                }
                else
                {
                    return StatusCode(500, new OutputBase(ex));
                }
            }
        }
    }
}