using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Appointments;
using Sabio.Models.Domain.Countries;
using Sabio.Models.Domain.Insurances;
using Sabio.Models.Requests.Appointments;
using Sabio.Models.Requests.InsuranceQuotes;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/insurances/quotes")]
    [ApiController]
    public class InsuranceQuoteApiController : BaseApiController
    {
        private IInsuranceQuoteService _service = null;
        private IAuthenticationService<int> _authService = null;
        public InsuranceQuoteApiController(IInsuranceQuoteService service, ILogger<InsuranceQuoteApiController> logger, IAuthenticationService<int> authService) : base(logger) 
        {
            _service = service;
            _authService = authService;
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<InsuranceQuote>> GetById(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                InsuranceQuote insuranceQuote = _service.GetById(id);
                if (insuranceQuote == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application Resource not found.");
                }
                else
                {
                    response = new ItemResponse<InsuranceQuote> { Item = insuranceQuote };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse($"Generic Exception: {ex.Message}");
            }
            return StatusCode(iCode, response);

        }


        [HttpGet("createdby/paginate")]
        public ActionResult<ItemResponse<Paged<InsuranceQuote>>> GetByCreatedBy(string user, int pageIndex, int pageSize)
        {
            ActionResult result = null;
            try
            {
                
                Paged<InsuranceQuote> paged = _service.GetByCreatedByPaginated(user, pageIndex, pageSize);
                if (paged == null)
                {
                    result = NotFound404(new ErrorResponse("Records Not Found"));
                }
                else
                {
                    ItemResponse<Paged<InsuranceQuote>> response = new ItemResponse<Paged<InsuranceQuote>>();
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

        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<InsuranceQuote>>> GetAllPaginated(int pageIndex, int pageSize)
        {
            ActionResult result = null;

            try
            {
                Paged<InsuranceQuote> paged = _service.GetAllPaginated(pageIndex, pageSize);

                if (paged == null)
                {
                    result = NotFound404(new ErrorResponse("Records Not Found"));
                }
                else
                {
                    ItemResponse<Paged<InsuranceQuote>> response = new ItemResponse<Paged<InsuranceQuote>>();
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

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(InsuranceQuoteAddRequest model)
        {
            int userId = _authService.GetCurrentUserId();

            ObjectResult result = null;

            try
            {
                int id = _service.Add(model, userId);
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
        public ActionResult<ItemResponse<int>> Update(InsuranceQuoteUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
                _service.Update(model, userId);
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
            BaseResponse response = null;

            try
            {
                _service.Delete(id);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [HttpGet("insurance")]
        public ActionResult<ItemResponse<List<Insurance>>> GetAllInsurance()
        {
            ActionResult result = null;

            try
            {
                List<Insurance> listOfInsurances = _service.GetAllInsurance();

                if (listOfInsurances == null)
                {
                    result = NotFound404(new ErrorResponse("Records Not Found"));
                }
                else
                {
                    ItemResponse<List<Insurance>> response = new ItemResponse<List<Insurance>>();
                    response.Item = listOfInsurances;
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

        [HttpGet("country")]
        public ActionResult<ItemResponse<List<Country>>> GetAllCountries()
        {
            ActionResult result = null;

            try
            {
                List<Country> listOfCountries = _service.GetAllCountries();

                if (listOfCountries == null)
                {
                    result = NotFound404(new ErrorResponse("Records Not Found"));
                }
                else
                {
                    ItemResponse<List<Country>> response = new ItemResponse<List<Country>>();
                    response.Item = listOfCountries;
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


        [HttpGet("useremail/paginate")]
        public ActionResult<ItemResponse<Paged<InsuranceQuote>>> GetByEmailPaginated(string userEmail, int pageIndex, int pageSize)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                
                Paged<InsuranceQuote> paged = _service.GetByEmailPaginated(userEmail, pageIndex, pageSize);
                if (paged == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<InsuranceQuote>> { Item = paged };
                   
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }

            return StatusCode(iCode, response);
        }
        
        [HttpGet("daterange/paginate")]
        public ActionResult<ItemResponse<Paged<InsuranceQuote>>> GetByDateRangePaginated(DateTime dateRangeStart, DateTime dateRangeEnd, int pageIndex, int pageSize)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {

                Paged<InsuranceQuote> paged = _service.GetByDateRangePaginated(dateRangeStart, dateRangeEnd, pageIndex, pageSize);
                if (paged == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<InsuranceQuote>> { Item = paged };

                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }

            return StatusCode(iCode, response);
        }

        [HttpGet("createdby/daterange/paginate")]
        public ActionResult<ItemResponse<Paged<InsuranceQuote>>> GetByUserDateRangePaginated(string user, DateTime dateRangeStart, DateTime dateRangeEnd, int pageIndex, int pageSize)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {

                Paged<InsuranceQuote> paged = _service.GetByUserDateRangePaginated(user, dateRangeStart, dateRangeEnd, pageIndex, pageSize);
                if (paged == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<InsuranceQuote>> { Item = paged };

                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }

            return StatusCode(iCode, response);
        }
    }
}
