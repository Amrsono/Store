using Store.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Store.Web.Controllers
{
    public class CatalogController : Controller
    {
        private readonly StoreDbContext _context;

        public CatalogController(StoreDbContext context)
        {
            _context = context;
        }

        // GET: /Catalog
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .ToListAsync();

            return View(products);
        }
    }
}