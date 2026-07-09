using Microsoft.EntityFrameworkCore;
using Rachmistrz.Web.Data;
using Rachmistrz.Web.Models;

namespace Rachmistrz.Web.Services;

public class SupplierService
{
    private readonly ApplicationDbContext _dbContext;

    public SupplierService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Supplier>> GetSuppliersAsync()
    {
        return await _dbContext.Suppliers
            .OrderBy(supplier => supplier.Name)
            .ToListAsync();
    }
}