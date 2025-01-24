namespace SampleApi.Models.Users;

public interface IUserRepository
{
    public Task<User> Create(User user);
    public Task<User?> FindUserById(Guid userId);
    public Task<List<User>> GetAll();
    public Task<User> Update(User user);
    public Task Delete(Guid userId);
    public Task<bool> DoesEmailExist(string userEmail);
}