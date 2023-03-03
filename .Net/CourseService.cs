using Sabio.Data.Providers;
using Sabio.Models.Requests.Friends;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Models.Requests.CodeChallenge;
using Sabio.Models.Domain.Friends;
using Sabio.Models.Domain.CodeChallenge;
using Sabio.Data;
using Sabio.Models;

namespace Sabio.Services.CodeChallenge
{
    public class CourseService : ICourseService
    {
        IDataProvider _data = null;

        public CourseService(IDataProvider data) { _data = data; }

        public int Create(CourseAddRequest model)
        {
            int id = 0;

            string procName = "dbo.Courses_Insert";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                col.Add(idOut);
            },
            returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object oId = returnCollection["@Id"].Value;

                int.TryParse(oId.ToString(), out id);
            });
            return id;
        }

        public Course GetById(int id)
        {
            string procName = "dbo.Courses_SelectById";

            Course aCourse = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);

            }, delegate (IDataReader reader, short set)
            {
                aCourse = MapSingleCourse(reader);
            }
            );
            return aCourse;

        }

        public void UpdateCourse(CourseUpdateRequest model)
        {
            string procName = "dbo.Courses_Update";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);
                col.AddWithValue("@Id", model.Id);
            },
            returnParameters: null);
        }

        public void DeleteStudent(int id)
        {
            string procName = "[dbo].[Students_Delete]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            }, returnParameters: null);
        }

        public Paged<Course> GetCoursesByPage(int pageIndex, int pageSize)
        {
            string procName = "dbo.Courses_Pagination";
            Paged<Course> pagedList = null;
            List<Course> listOfCourses = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName, (param) =>
            {
                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
            }, (reader, recordSetIndex) =>
            {
                Course aCourse = MapSingleCourse(reader);
                totalCount = reader.GetSafeInt32(6);

                if (listOfCourses == null)
                {
                    listOfCourses = new List<Course>();
                }
                listOfCourses.Add(aCourse);
            });
            if (listOfCourses != null)
            {
                pagedList = new Paged<Course>(listOfCourses, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        private static Course MapSingleCourse(IDataReader reader)
        {
            Course aCourse = new Course();

            //aCourse.SeasonTermId = new SeasonTerm();

            //aCourse.TeacherId = new Teacher();

            int startingIndex = 0;

            aCourse.Id = reader.GetSafeInt32(startingIndex++);
            aCourse.Name = reader.GetSafeString(startingIndex++);
            aCourse.Description = reader.GetSafeString(startingIndex++);
            aCourse.SeasonTerm = reader.GetSafeString(startingIndex++);
            aCourse.Teacher = reader.GetSafeString(startingIndex++);
            aCourse.Students = reader.DeserializeObject<List<Student>>(startingIndex++);

            return aCourse;
        }

        private static void AddCommonParams(CourseAddRequest model, SqlParameterCollection col)
        {

            col.AddWithValue("@Name", model.Name);
            col.AddWithValue("@Description", model.Description);
            col.AddWithValue("@SeasonTermId", model.SeasonTermId);
            col.AddWithValue("@TeacherId", model.TeacherId);

        }
    }
}
