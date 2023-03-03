using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Services.Interfaces;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Models.Domain.Friends;
using Sabio.Web.Models.Responses;
using System.Data.SqlClient;
using System;
using Sabio.Models.Domain.Practice;
using System.Collections.Generic;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/people")]
    [ApiController]
    public class PeopleApiController : BaseApiController
    {
        private IPeopleService _service = null;
        private IAuthenticationService<int> _authService = null;
        public PeopleApiController(IPeopleService service, ILogger<PingApiController> logger, IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }
        [HttpGet("{id:int}")]
            public ActionResult<ItemResponse<PracticePeople>> Get(int id)
            {
                int iCode = 200;
                BaseResponse response = null;

                try
                {
                    PracticePeople aPerson = _service.Get(id);

                    if (aPerson == null)
                    {
                        iCode = 404;
                        response = new ErrorResponse("Person Not Found.");
                    }
                    else
                    {
                        response = new ItemResponse<PracticePeople>() { Item = aPerson };
                    }
                }
                catch (Exception ex)
                {
                    iCode = 500;
                    base.Logger.LogError(ex.ToString());
                    response = new ErrorResponse($"Exception Error: {ex.Message}");

                }
                return StatusCode(iCode, response);

            }



        [HttpGet]
        public ActionResult<ItemsResponse<PracticePeople>> GetAll()
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                List<PracticePeople> listOfPeople = _service.GetAll();

                if (listOfPeople == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Peolpe Not Found.");
                }
                else
                {
                    response = new ItemsResponse<PracticePeople>() { Items = listOfPeople };
                }
            }
            catch (Exception ex)
            {

                iCode = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(iCode, response);
        }
    }
}