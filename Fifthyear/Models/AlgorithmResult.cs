using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifthyear.Models
{
    public class AlgorithmResult
    {
        public int Id { get; set; }
        public int ReusmeID { get; set; }
        public int VacancyId { get; set; }
        public int? CompanyId { get; set; }
        public int? SeekerId { get; set; }
    }
}
