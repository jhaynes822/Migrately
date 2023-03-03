using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Services.Interfaces;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Models.Domain.Friends;
using Sabio.Models.Requests.Friends;
using Sabio.Models;
using Sabio.Web.Models.Responses;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/v3/friends")]
    [ApiController]
    public class FriendApiControllerV3 : BaseApiController
    {
        private IFriendService _service = null;
        private IAuthenticationService<int> _authService = null;
        public FriendApiControllerV3(IFriendService service, ILogger<PingApiController> logger, IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<FriendV3>> FriendV3Get(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                FriendV3 aFriend = _service.GetV3(id);

                if (aFriend == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Friend Not Found.");
                }
                else
                {
                    response = new ItemResponse<FriendV3>() { Item = aFriend };
                }
            }
            catch (SqlException sqlEx)
            {
                iCode = 500;
                response = new ErrorResponse($"SqlException Error: {sqlEx.Message}");
                base.Logger.LogError(sqlEx.ToString());

            }
            catch (ArgumentException argEx)
            {
                iCode = 500;

                response = new ErrorResponse($"ArgumentException Error: {argEx.Message}");
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
        public ActionResult<ItemsResponse<FriendV3>> FriendV3GetAll()
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                List<FriendV3> listOfFriends = _service.GetAllV3();

                if (listOfFriends == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Friends Not Found.");
                }
                else
                {
                    response = new ItemsResponse<FriendV3>() { Items = listOfFriends };
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

        [HttpPost]
        public ActionResult<ItemResponse<int>> FriendV3Add(FriendV3AddRequest model)
        {
            int userId = _authService.GetCurrentUserId();
            IUserAuthData user = _authService.GetCurrentUser();

            ObjectResult result = null;

            try
            {
                int id = _service.FriendV3Add(model, user.Id);
                ItemResponse<int> response = new ItemResponse<int> { Item = id };

                result = Created201(response);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }
            return result;
        }

        [HttpPut("{id:int}")]
        public ActionResult<ItemResponse<int>> FriendV3Update(FriendV3UpdateRequest model)
        {
            int userId = _authService.GetCurrentUserId();
            IUserAuthData user = _authService.GetCurrentUser();

            int iCode = 200;
            BaseResponse response = null;

            try
            {
                _service.FriendV3Update(model, user.Id);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(iCode, response);
        }

        [HttpDelete("{id:int}")]
        public ActionResult FriendV1Delete(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                _service.Delete(id);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {


                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(iCode, response);
        }

        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<FriendV3>>> PaginationV3(int pageIndex, int pageSize)
        {
            ActionResult result = null;

            try
            {
                Paged<FriendV3> paged = _service.PaginationV3(pageIndex, pageSize);

                if (paged == null)
                {
                    result = NotFound404(new ErrorResponse("Records Not Found"));
                }
                else
                {
                    ItemResponse<Paged<FriendV3>> response = new ItemResponse<Paged<FriendV3>>();
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
        
        [HttpGet("search")]
        public ActionResult<ItemResponse<Paged<FriendV3>>> SearchPaginatedV3(int pageIndex, int pageSize, string query)
        {
            ActionResult result = null;

            try
            {
                Paged<FriendV3> paged = _service.SearchPaginatedV3(pageIndex, pageSize, query);

                if (paged == null)
                {
                    result = NotFound404(new ErrorResponse("Records Not Found"));
                }
                else
                {
                    ItemResponse<Paged<FriendV3>> response = new ItemResponse<Paged<FriendV3>>();
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
