using Microsoft.EntityFrameworkCore;
using Rachmistrz.Web.Data;
using Rachmistrz.Web.Models;

namespace Rachmistrz.Web.Services
{
    public class CostCategoryService
    {
        private readonly ApplicationDbContext _dbContext;

        public CostCategoryService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<CostCategory>> GetCostCategoriesAsync()
        {
            return await _dbContext.CostCategories
                .OrderBy(category => category.Name)
                .ToListAsync();
        }
    }
}
