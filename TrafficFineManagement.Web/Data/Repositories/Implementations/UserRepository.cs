using Microsoft.EntityFrameworkCore;
using TrafficFineManagement.Web.Data.Repositories.Interfaces;
using TrafficFineManagement.Web.Models.Entities;
using TrafficFineManagement.Web.Models.Enums;

namespace TrafficFineManagement.Web.Data.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<User?> GetByIdAsync(string id)
        {
            return _context.Users
                .FirstOrDefaultAsync(user => user.Id == id);
        }

        public Task<List<User>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            AccountType? accountType)
        {
            var query = _context.Users.AsNoTracking();

            query = ApplyFilters(query, searchTerm, accountType);

            return query
                .OrderByDescending(user => user.CreatedAt)
                .ThenBy(user => user.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        private IQueryable<User> ApplyFilters(
            IQueryable<User> query,
            string? searchTerm,
            AccountType? accountType)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var search = searchTerm.Trim();

                if (search.All(char.IsDigit))
                {
                    query = query.Where(user =>
                        user.IdentityNumber == search ||
                        user.TaxNumber == search);
                }

                else
                {
                    query = query.Where(user =>
                    user.Name.Contains(search) ||
                    user.Surname.Contains(search) ||
                    (user.UserName != null &&
                     user.UserName.Contains(search)) ||
                    (user.CompanyName != null &&
                     user.CompanyName.Contains(search)));
                }
            }

            if (accountType.HasValue)
            {
                query = query.Where(user =>
                    user.AccountType == accountType.Value);
            }

            return query;
        }
    }
}
