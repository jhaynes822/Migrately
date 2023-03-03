using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.CodeChallenge
{
    public class CourseUpdateRequest : CourseAddRequest
    {
        public int Id { get; set; }
    }
}
