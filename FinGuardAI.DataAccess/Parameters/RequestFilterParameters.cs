using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinGuardAI.DataAccess.Parameters
{
    public class RequestFilterParameters
    {

        public string? Status { get; set; }
        public string? RequestCategory { get; set; }
        public int? CreatedBy { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}
