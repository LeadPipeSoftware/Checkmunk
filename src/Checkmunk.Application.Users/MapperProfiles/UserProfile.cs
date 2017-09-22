using AutoMapper;
using Checkmunk.Contracts.Users.V1.Models;
using Checkmunk.Domain.Users.AggregateRoots;
using Checkmunk.Domain.Users.ValueObjects;

namespace Checkmunk.Application.Users.MapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<PhoneNumber, PhoneNumberModel>();
            CreateMap<Address, AddressModel>();
        }
    }
}
