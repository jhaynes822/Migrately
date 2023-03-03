using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain.Practice;
using Sabio.Services.Interfaces;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System.Collections.Generic;
using System;
using Sabio.Models.Domain.Friends;
using Sabio.Models;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/v3/people")]
    [ApiController]
    public class PeopleApiControllerV3 : BaseApiController
    {
        private IPeopleService _service = null;
        private IAuthenticationService<int> _authService = null;
        public PeopleApiControllerV3(IPeopleService service, ILogger<PingApiController> logger, IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }
        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<PracticePeopleV3>> GetV3(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                PracticePeopleV3 aPerson = _service.GetV3(id);

                if (aPerson == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Person Not Found.");
                }
                else
                {
                    response = new ItemResponse<PracticePeopleV3>() { Item = aPerson };
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
        public ActionResult<ItemsResponse<PracticePeopleV3>> GetAllV3()
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                List<PracticePeopleV3> listOfPeople = _service.GetAllV3();

                if (listOfPeople == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Peolpe Not Found.");
                }
                else
                {
                    response = new ItemsResponse<PracticePeopleV3>() { Items = listOfPeople };
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

        [HttpGet("search")]
        public ActionResult<ItemResponse<Paged<PracticePeopleV3>>> SearchPaginatedV3(int pageIndex, int pageSize, string query)
        {
            ActionResult result = null;

            try
            {
                Paged<PracticePeopleV3> paged = _service.SearchPaginatedV3(pageIndex, pageSize, query);

                if (paged == null)
                {
                    result = NotFound404(new ErrorResponse("Records Not Found"));
                }
                else
                {
                    ItemResponse<Paged<PracticePeopleV3>> response = new ItemResponse<Paged<PracticePeopleV3>>();
                    response.Item = paged;
                    result = Ok200(response);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                result = StatusCode(500, new ErrorResponse(ex.Message.ToString()));
            }
            return result;
        }
    }
}
