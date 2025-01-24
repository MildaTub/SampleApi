using SampleApi.Contracts.Users;

namespace SampleApi.Mappers;

public static class UserMapper
{
    public static Models.Users.User ToDomain(this CreateUserRequest userRequest)
    {
        Models.Users.User userModel = new()
        {
            FirstName = userRequest.FirstName,
            LastName = userRequest.LastName,
            Email = userRequest.Email,
            PhoneNumber = userRequest.PhoneNumber,
        };

        return userModel;
    }

    public static Models.Users.User ToDomain(this UpdateUserRequest userRequest, Guid userId)
    {
        Models.Users.User userModel = new()
        {
            Id = userId,
            FirstName = userRequest.FirstName,
            LastName = userRequest.LastName,
            Email = userRequest.Email,
            PhoneNumber = userRequest.PhoneNumber,
        };

        return userModel;
    }

    public static User ToApi(this Models.Users.User userModel)
    {
        User userApi = new()
        {
            UserId = userModel.Id,
            FirstName = userModel.FirstName,
            LastName = userModel.LastName,
            Email = userModel.Email,
            PhoneNumber = userModel.PhoneNumber,
            CreatedAt = userModel.CreatedAt,
            UpdatedAt = userModel.UpdatedAt
        };

        return userApi;
    }

    public static List<User> ToApi(this List<Models.Users.User> userModels)
    {
        List<User> users = userModels.Select(user => user.ToApi()).ToList();
        return users;
    }
}