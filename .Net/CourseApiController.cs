using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Services.Interfaces;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Services.CodeChallenge;
using Sabio.Models.Requests.Friends;
using Sabio.Models;
using Sabio.Web.Models.Responses;
using System;
using Sabio.Models.Requests.CodeChallenge;
using Sabio.Models.Domain.Friends;
using System.Data.SqlClient;
using Sabio.Models.Domain.CodeChallenge;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/courses")]
    [ApiController]
    public class CourseApiController : BaseApiController
    {
        private ICourseService _service = null;
        private IAuthenticationService<int> _authService = null;
        public CourseApiController(ICourseService service, ILogger<PingApiController> logger, IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> AddCourse(CourseAddRequest model)
        {

            ObjectResult result = null;

            try
            {
                int id = _service.Create(model);
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

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Course>> GetCourseById(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Course aCourse = _service.GetById(id);

                if (aCourse == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Course Not Found.");
                }
                else
                {
                    response = new ItemResponse<Course>() { Item = aCourse };
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

        [HttpPut("{id:int}")]
        public ActionResult<ItemResponse<int>> UpdateCourse(CourseUpdateRequest model)
        {


            int iCode = 200;
            BaseResponse response = null;

            try
            {
                _service.UpdateCourse(model);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(iCode, response);
        }

        [HttpDelete("students/{id:int}")]
        public ActionResult DeleteStudent(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                _service.DeleteStudent(id);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {


                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(iCode, response);
        }

        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<Course>>> GetByPage(int pageIndex, int pageSize)
        {
            ActionResult result = null;

            try
            {
                Paged<Course> paged = _service.GetCoursesByPage(pageIndex, pageSize);

                if (paged == null)
                {
                    result = NotFound404(new ErrorResponse("Records Not Found"));
                }
                else
                {
                    ItemResponse<Paged<Course>> response = new ItemResponse<Paged<Course>>();
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
