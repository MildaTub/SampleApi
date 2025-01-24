using MongoDB.Driver;
using SampleApi.Models.Users;

namespace SampleApi.Mongo.Users
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _collection;
        private readonly TimeProvider _timeProvider;
        public UserRepository(IMongoDatabase mongoDatabase, TimeProvider timeProvider)
        {
            _collection = mongoDatabase.GetCollection<User>("Users");
            _timeProvider = timeProvider;
        }

        public async Task<User> Create(User user)
        {
            user.Id = Guid.NewGuid();
            user.CreatedAt = _timeProvider.GetUtcNow().DateTime;
            await _collection.InsertOneAsync(user);

            return user;
        }
       
        public Task<User> FindUserById(Guid userId)
        {
            FilterDefinition<User> filterDefinition = Builders<User>.Filter.Eq(userFilter => userFilter.Id, userId) &
                                                      Builders<User>.Filter.Eq(userFilter => userFilter.DeletedAt, null);
            return _collection.Find(filterDefinition).FirstOrDefaultAsync();

        }

        public Task<List<User>> GetAll()
        {
            FilterDefinition<User> filterDefinition = Builders<User>.Filter.Eq(userFilter => userFilter.DeletedAt, null);
            return _collection.Find(filterDefinition).ToListAsync();
        }

        public async Task<User> Update(User user)
        {
            user.UpdatedAt = _timeProvider.GetUtcNow().DateTime;
            FilterDefinition<User> filterDefinition = Builders<User>.Filter.Eq(userFilter => userFilter.Id, user.Id);
            await _collection.ReplaceOneAsync(filterDefinition, user);
            
            return user;
        }

        public Task Delete(Guid userId)
        {
            FilterDefinition<User> filterDefinition = Builders<User>.Filter.Eq(userFilter => userFilter.Id, userId);
            UpdateDefinition<User> updateDefinition = Builders<User>.Update.Set(userFilter => userFilter.DeletedAt, _timeProvider.GetUtcNow().DateTime);
            
            return _collection.UpdateOneAsync(filterDefinition, updateDefinition);
        }

        public async Task<bool> DoesEmailExist(string userEmail)
        {
            var filter = Builders<User>.Filter.Eq(user => user.DeletedAt, null) &
                         Builders<User>.Filter.Eq(user => user.Email, userEmail);

            return await _collection.Find(filter).AnyAsync();
        }
    }
}
