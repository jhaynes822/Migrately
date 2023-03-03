using Sabio.Models.Domain.Friends;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Friends
{
    public class FriendV3AddRequest
    {
        [Required] 
        [StringLength(100, MinimumLength = 2)]
        public string Title { get; set; }
        [Required] 
        [StringLength(100, MinimumLength = 2)]
        public string Bio { get; set; }
        [Required] 
        [StringLength(100, MinimumLength = 2)]
        public string Summary { get; set; }
        [Required] 
        [StringLength(100, MinimumLength = 2)]
        public string Headline { get; set; }
        [Required] 
        [StringLength(100, MinimumLength = 2)]
        public string Slug { get; set; }
        [Required]
        [Range(1, 10000)]
        public int StatusId { get; set; }
        [Required]
        [Range(1, 10000)]
        public int ImageTypeId { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string ImageUrl { get; set; }


        public List<string> Skills { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
    }
}
