using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain.Addresses;
using Sabio.Models.Requests.Addresses;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/addresses")]
    [ApiController]
    public class AddressApiController : BaseApiController
    {
        private IAddressService _service = null;
        private IAuthenticationService<int> _authService = null;
        public AddressApiController(IAddressService service, ILogger<PingApiController> logger, IAuthenticationService<int> authService) : base(logger) 
        {
            _service = service;
            _authService = authService;
        }


        //Get api/addresses
        [HttpGet]
        public ActionResult< ItemsResponse<Address> > GetAll()
        {
            int code = 200;
            BaseResponse response = null; // do not declare new instance

            try
            {
                List<Address> listOfAddresses = _service.GetRandomAddresses();


                if (listOfAddresses == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemsResponse<Address>() { Items = listOfAddresses};
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message); 
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }


        // api/addresses/{id:int}
        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Address>> Get(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Address anAddress = _service.Get(id);


                if (anAddress == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application Resource not found.");
                    
                }
                else
                {
                    response = new ItemResponse<Address>() { Item = anAddress }; 
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
                // return base.StatusCode(500, new ErrorResponse( $"Generic Error: {ex.Message }" ));
            }



            return StatusCode(iCode, response);
        }


        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(AddressAddRequest model) 
        {
                        
            int userId = _authService.GetCurrentUserId();
            IUserAuthData user = _authService.GetCurrentUser();
            
            //The default response code in this instance is 201. 
            //BUT we do not need it because of what we do below in the try block
            //int iCode = 201;

            //we need this instead of the BaseResponse
            ObjectResult result = null;

            try
            {
                //if this operation errors, it would generate an exception and jump to the catch
                int id = _service.Add(model, user.Id);  // calls full user (not needed. You just need the ID)
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };

                //This sets the status code for us but also set Url that points back to 
                // the Get By Id endpoint. Setting a Url in the Response (for a 201 Response code) is a common practice
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
        public ActionResult<ItemResponse<int>> Update(AddressUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.Update(model);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
               
            }


            return StatusCode(code, response);
        } 
        
        
        
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id) 
        {
            int code = 200;
            BaseResponse response = null; // do not declare new instance

            try
            {
                _service.Delete(id);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                response = new ErrorResponse(ex.Message);
                
            }

            return StatusCode(code, response);
        }
    }
}
