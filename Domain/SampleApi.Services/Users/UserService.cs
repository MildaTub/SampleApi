using SampleApi.Models.Exceptions;
using SampleApi.Models.Users;

namespace SampleApi.Services.Users;
public sealed class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> Create(User user)
    {
        await ValidateUserEmail(user.Email);

        User createdUser = await _userRepository.Create(user);
        return createdUser;
    }

    public async Task<User> Get(Guid userId)
    {
        User? user = await _userRepository.FindUserById(userId);
        if (user == null)
        {
            throw new MissingEntityException(nameof(User));
        }

        return user;
    }

    public async Task<List<User>> GetAll()
    {
        List<User> users = await _userRepository.GetAll();
        return users;
    }

    public async Task<User> Update(User userModel)
    {
        var currentUser = await Get(userModel.Id);
        if (!string.Equals(currentUser.Email, userModel.Email))
        {
            await ValidateUserEmail(userModel.Email);
        }

        currentUser.FirstName = userModel.FirstName;
        currentUser.LastName = userModel.LastName;
        currentUser.Email = userModel.Email;
        currentUser.PhoneNumber = userModel.PhoneNumber;
        currentUser.CreatedAt = userModel.CreatedAt;
        
        User updatedUser = await _userRepository.Update(userModel);
        return updatedUser;
    }

    public async Task Delete(Guid userId)
    {
        await Get(userId);
        await _userRepository.Delete(userId);
    }

    private async Task ValidateUserEmail(string userEmail)
    {
        bool isEmailPresent = await _userRepository.DoesEmailExist(userEmail);
        if (isEmailPresent)
        {
            throw new DomainException($"{userEmail} already exists.");
        }
    }
}