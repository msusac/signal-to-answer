using SignalToAnswer.Dtos;
using SignalToAnswer.Entities;

namespace SignalToAnswer.Mappers.Dtos
{
    public class UserDtoMapper
    {
        public UserDto Map(User user, string token)
        {
            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.UserName,
                Role = user.UserRole.Role.Name
            };

            if (!string.IsNullOrEmpty(token))
            {
                userDto.Token = token;
            }

            return userDto;
        }
    }
}
