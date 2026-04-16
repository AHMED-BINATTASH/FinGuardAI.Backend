namespace FinGuardAI.DataAccess.DTOs
{
    public partial class FinancialResponseDTO
    {
        // DTO للـ FinancialRequest
        public class FinancialRequestDto
        {
            public int Id { get; set; }
            public string RequestName { get; set; }
            public decimal Amount { get; set; }
         
            public string RequestCategory { get; set; }
            public string Description { get; set; }
            public string Files { get; set; } // يمكن تخزين مسارات الملفات كنص أو JSON
            public DateTime CreatedAt { get; set; }
            public int CreatedBy { get; set; }
            public string State { get; set; }

            public FinancialRequestDto() { }

            //public FinancialRequestDto(int id, string requestName, decimal amount, string category,
            //                           string description, string state, DateTime createdAt, string creatorName)
            //{
            //    Id = id;
            //    RequestName = requestName;
            //    Amount = amount;
            //    Category = category;
            //    Description = description;
            //    State = state;
            //    CreatedAt = createdAt;
            //    CreatorName = creatorName;
            //}
        }
    }
}

