using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.InsuranceQuotes
{
    public class InsuranceQuoteAddRequest
    {

        [Required]
        [Range(1, int.MaxValue)]
        public int InsuranceId { get; set; }
        [DataType(DataType.Date)]
        public DateTime CoverageStartDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime CoverageEndDate { get; set; }
        [Range(1, int.MaxValue)]
        public int Citizenship { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Age { get; set; }
        [Range(1, int.MaxValue)]
        public int MailingAddress { get; set; }
        [Range(1, int.MaxValue)]
        public int TravelDestination { get; set; }
        [Range(1, int.MaxValue)]
        public int PolicyRangeId { get; set; }
        [Range(typeof(bool), "false", "true")]
        public bool IsArrivedInUSA { get; set; }
        [Range(1, int.MaxValue)]
        public int VisaTypeId { get; set; }

    }
}
