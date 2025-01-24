using SampleApi.Models.Users;

namespace SampleApi.Services.Users;

public interface IUserService
{
    public Task<User> Create(User user);
    public Task<User> Get(Guid userId);
    public Task<List<User>> GetAll();
    public Task<User> Update(User user);
    public Task Delete(Guid userId);
}