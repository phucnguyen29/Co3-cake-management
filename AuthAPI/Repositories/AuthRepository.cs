using Microsoft.EntityFrameworkCore;
using WebAPI.Repositories.Interfaces;
using AuthAPI.Models;
using AuthAPI.Data;

namespace AuthAPI.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AuthAPIContext _context;

        public AuthRepository(AuthAPIContext context)
        {
            _context = context;
        }

        public IQueryable<User> GetAll()
        {
            return _context.User.AsQueryable();
        }



        /*  public async Task<int> GetCountAsync()
          {  return await _context.User.CountAsync(); }*/

        public async Task<User?> GetByIdAsync(Guid id)
            => await _context.User.FindAsync(id);

        public async Task AddAsync(User student)
        {
            await _context.User.AddAsync(student);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User student)
        {
            _context.User.Update(student);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User student)
        {
            _context.User.Remove(student);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByUsernameAsync(string name)
            => await _context.User.FirstOrDefaultAsync(u => u.UserName == name);

    }

}
