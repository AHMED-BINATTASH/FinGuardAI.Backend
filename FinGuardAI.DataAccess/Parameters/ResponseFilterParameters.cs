using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinGuardAI.DataAccess.Parameters
{
    public class ResponseFilterParameters
    {
        public string? Decision { get; set; }
        public int? RequestId { get; set; }
        public int? CreatedBy { get; set; } // هذا هو الـ UserId الذي اتخذ القرار
        public decimal? MinAcceptedAmount { get; set; }
        public decimal? MaxAcceptedAmount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
