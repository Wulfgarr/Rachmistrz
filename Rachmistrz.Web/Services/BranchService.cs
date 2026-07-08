using Microsoft.EntityFrameworkCore;
using Rachmistrz.Web.Data;
using Rachmistrz.Web.Models;

namespace Rachmistrz.Web.Services
{
    public class BranchService
    {
        private readonly ApplicationDbContext _dbContext;

        public BranchService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Branch>> GetBranchesAsync()
        {
            return await _dbContext.Branches
                .OrderBy(branch => branch.Code)
                .ToListAsync();
        }
    }
}
