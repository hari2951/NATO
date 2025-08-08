using AutoMapper;
using TransactionApp.Application.DTOs;
using TransactionApp.Domain.Entities;

namespace TransactionApp.Application.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>();
            CreateMap<Transaction, TransactionDto>()
                .ForMember(dest => dest.FullName,
                    opt => opt.MapFrom(src => src.User != null ? $"{src.User.FirstName} {src.User.LastName}" : string.Empty));
            CreateMap<CreateTransactionDto, Transaction>();
        }
    }
}
