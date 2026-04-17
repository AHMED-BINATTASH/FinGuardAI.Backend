using FinGuardAI.DataAccess.Entities;
using FinGuardAI.DataAccess.Parameters;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

namespace FinGuardAI.DataAccess.Persistence.Repositories
{
    public class FinancialResponseRepository
    {
        private readonly AppDbContext _dbContext;

        public FinancialResponseRepository(AppDbContext dbContext)
        { _dbContext = dbContext; }

        public async Task<bool> Add(FinancialResponse entity)
        {
            if (entity == null)
                return false;

            _dbContext.FinancialResponses.Add(entity);

            return await Save();
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _dbContext.FinancialResponses.FindAsync(id);

            if (entity == null)
                return false;

            _dbContext.FinancialResponses.Remove(entity);

            return await Save();
        }

        public async Task<IEnumerable<FinancialResponse>> GetAllAsync()
        {
            return await _dbContext.FinancialResponses.AsNoTracking().ToListAsync();
        }

        public async Task<FinancialResponse> GetByIdAsync(int id)
        {
            return await _dbContext.FinancialResponses.FindAsync(id);
        }

        public async Task<bool> Update(FinancialResponse entity)
        {
            if (entity == null)
                return false;

            FinancialResponse FinancialResponse = await _dbContext.FinancialResponses.FindAsync(entity.Id);

            if (FinancialResponse == null)
                return false;

            _dbContext.Entry(FinancialResponse).CurrentValues.SetValues(entity);

            return await Save();
        }


        public async Task<bool> Save()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
        public async Task<bool> IsExistByFinancialResponseIDAsync(int FinancialResponseID)
        {
            if (FinancialResponseID <= 0)
                return false;

            return await _dbContext.FinancialResponses
                            .AnyAsync(c => c.Id == FinancialResponseID);
        }

   
        public async Task<IEnumerable<FinancialResponse>> GetFilteredResponsesAsync(ResponseFilterParameters p)
        {
         
            var query = _dbContext.FinancialResponses.AsNoTracking().AsQueryable();

         
            if (!string.IsNullOrEmpty(p.Decision))
                query = query.Where(x => x.Decision == p.Decision);

            if (p.RequestId.HasValue)
                query = query.Where(x => x.RequestId == p.RequestId);

            if (p.CreatedBy.HasValue)
                query = query.Where(x => x.CreatedBy == p.CreatedBy);

            if (p.MinAcceptedAmount.HasValue)
                query = query.Where(x => x.AcceptedAmount >= p.MinAcceptedAmount);

            if (p.MaxAcceptedAmount.HasValue)
                query = query.Where(x => x.AcceptedAmount <= p.MaxAcceptedAmount);

            if (p.StartDate.HasValue)
                query = query.Where(x => x.CreatedAt >= p.StartDate);

            if (p.EndDate.HasValue)
                query = query.Where(x => x.CreatedAt <= p.EndDate);

            
            return await query.ToListAsync();
        }
    }
}
