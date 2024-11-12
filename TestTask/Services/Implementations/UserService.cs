using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Enums;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUser()
        {
            var result = await _context.Users
                .Include(u => u.Orders)
                .Select(u => new { User = u, TotalAmount = u.Orders
                .Where(o => o.CreatedAt.Year == 2003 && o.Status == OrderStatus.Delivered).Sum(o => o.Price * o.Quantity) })
                .OrderByDescending(x => x.TotalAmount)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return result.User;
        }

        public async Task<List<User>> GetUsers()
        {
            return await _context.Users
                .Include(u => u.Orders)
                .Where(u => u.Orders.Any(o => o.CreatedAt.Year == 2010 && o.Status == OrderStatus.Paid))
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
