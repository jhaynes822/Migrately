using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain.Users;
using Sabio.Models.Requests.Users;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserApiController : BaseApiController
    {
        private IUserServiceV1 _service = null;
        private IAuthenticationService<int> _authService = null;
        public UserApiController(IUserServiceV1 service, ILogger<PingApiController> logger, IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }


        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<User>> GetById(int id) 
        {
            int iCode = 200; //set default ok
            BaseResponse response = null;   

            try
            {
                User aUser = _service.Get(id);

                if (aUser == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("User not found.");
                }
                else
                {
                    response = new ItemResponse<User>() { Item = aUser };
                }

            }
            catch (SqlException sqlEx)
            {
                iCode = 500;
                response= new ErrorResponse($"sqlException Error: {sqlEx.Message}");
                base.Logger.LogError(sqlEx.ToString());
                
            }
            catch (ArgumentException argEx)
            {
                iCode = 500;
                response = new ErrorResponse($"sqlException Error: {argEx.Message}");
                base.Logger.LogError(argEx.ToString());

            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse($"Exception Error: {ex.Message}");
                base.Logger.LogError(ex.ToString());

            }
            return StatusCode(iCode, response);
        }

        [HttpGet]
        public ActionResult<ItemsResponse<User>> GetAll()
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                List<User> listOfUsers = _service.GetAll();

                if (listOfUsers == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Users not found.");
                }
                else
                {
                    response = new ItemsResponse<User>() { Items = listOfUsers };
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
        public ActionResult<ItemResponse<int>> Create(UserAddRequest model)
        {
            ObjectResult result = null;

            try
            {
                int id = _service.Add(model);
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };

                result = Created201(response);
            }
            catch (Exception ex)
            {

                ErrorResponse response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());

                result = StatusCode(500, response);
            }
            return result;
        }

        [HttpPut("{id:int}")]
        public ActionResult<ItemResponse<int>> Update(UserUpdateRequest model)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                _service.Update(model);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                response = new ErrorResponse(ex.Message);
               
            }
            return StatusCode(iCode, response);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
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
        
    }
}
