using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.CodeChallenge
{
    public class CourseAddRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int SeasonTermId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int TeacherId { get; set; }
    }
}
