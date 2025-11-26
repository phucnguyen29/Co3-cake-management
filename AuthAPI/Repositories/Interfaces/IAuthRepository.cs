using AuthAPI.Models;
namespace WebAPI.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        IQueryable<User> GetAll();
        //Task<int> GetCountAsync();  //Khi nào có Odata thì sử dụng
        Task<User?> GetByIdAsync(Guid id);
        Task AddAsync(User item);
        Task UpdateAsync(User item);
        Task DeleteAsync(User item);
        Task<User?> GetByUsernameAsync(string name); //Đặc trưng


    }
}
