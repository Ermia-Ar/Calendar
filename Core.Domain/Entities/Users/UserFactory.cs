using Core.Domain.Enum;

namespace Core.Domain.Entities.Users;

public static class UserFactory
{
    public static User Create(string userName
        , string email, string password, UserCategory category)
    {
        return new User()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = userName,
            Email = email,
            Category = category,
            CreatedDate = DateTime.Now,
            UpdateDate = DateTime.Now,
            IsEdited = false,
			IsActive = true,
        };
	}
}
