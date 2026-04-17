using FinGuardAI.DataAccess.Entities;
using FinGuardAI.DataAccess.Parameters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FinGuardAI.DataAccess.Entities.FinancialRequest;

namespace FinGuardAI.DataAccess.Persistence.Repositories
{
    public class FinancialRequestRepository
    {
        private readonly AppDbContext _dbContext;

        public FinancialRequestRepository(AppDbContext dbContext)
        { _dbContext = dbContext; }

        public async Task<bool> Add(FinancialRequest entity)
        {
            if (entity == null)
                return false;

            _dbContext.FinancialRequests.Add(entity);

            return await Save();
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _dbContext.FinancialRequests.FindAsync(id);

            if (entity == null)
                return false;

            _dbContext.FinancialRequests.Remove(entity);

            return await Save();
        }

        public async Task<IEnumerable<FinancialRequest>> GetAllAsync()
        {
            return await _dbContext.FinancialRequests.AsNoTracking().ToListAsync();
        }

        public async Task<FinancialRequest> GetByIdAsync(int id)
        {
            return await _dbContext.FinancialRequests.FindAsync(id);
        }

        public async Task<bool> Update(FinancialRequest entity)
        {
            if (entity == null)
                return false;

            FinancialRequest FinancialRequest = await _dbContext.FinancialRequests.FindAsync(entity.Id);

            if (FinancialRequest == null)
                return false;

            _dbContext.Entry(FinancialRequest).CurrentValues.SetValues(entity);

            return await Save();
        }
        public async Task<bool> Save()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
        public async Task<bool> IsExistByFinancialRequestIDAsync(int FinancialRequestID)
        {
            if (FinancialRequestID <= 0)
                return false;

            return await _dbContext.FinancialRequests
                            .AnyAsync(c => c.Id == FinancialRequestID);
        }


        public async Task<IEnumerable<FinancialRequest>> GetFilteredRequestsAsync(RequestFilterParameters p)
        {
          
            var query = _dbContext.FinancialRequests.AsNoTracking().AsQueryable();
         
            if (!string.IsNullOrEmpty(p.Status))
                query = query.Where(x => x.State == p.Status);

            if (!string.IsNullOrEmpty(p.RequestCategory))
            {
                if (Enum.TryParse<Categories>(p.RequestCategory, true, out var categoryEnum))
                {
                    query = query.Where(x => x.RequestCategory == categoryEnum);
                }
            }
            

            if (p.CreatedBy.HasValue)
                query = query.Where(x => x.CreatedBy == p.CreatedBy);

            if (p.MinAmount.HasValue)
                query = query.Where(x => x.Amount >= p.MinAmount);

            if (p.MaxAmount.HasValue)
                query = query.Where(x => x.Amount <= p.MaxAmount);

            if (p.StartDate.HasValue)
                query = query.Where(x => x.CreatedAt >= p.StartDate);

            if (p.EndDate.HasValue)
                query = query.Where(x => x.CreatedAt <= p.EndDate);

          
            return await query.ToListAsync();
        }

    }
}