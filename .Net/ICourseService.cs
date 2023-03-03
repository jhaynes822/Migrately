using Sabio.Models;
using Sabio.Models.Domain.CodeChallenge;
using Sabio.Models.Requests.CodeChallenge;

namespace Sabio.Services.CodeChallenge
{
    public interface ICourseService
    {
        int Create(CourseAddRequest model);
        Course GetById(int id);
        void UpdateCourse(CourseUpdateRequest model);
        void DeleteStudent(int id);
        Paged<Course> GetCoursesByPage(int pageIndex, int pageSize);
    }
}