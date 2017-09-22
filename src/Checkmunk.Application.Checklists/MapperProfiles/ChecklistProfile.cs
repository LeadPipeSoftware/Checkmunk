using AutoMapper;
using Checkmunk.Contracts.Checklists.V1.Models;
using Checkmunk.Domain.Checklists.AggregateRoots;
using Checkmunk.Domain.Checklists.Entities;
using Checkmunk.Domain.Checklists.ValueObjects;

namespace Checkmunk.Application.Checklists.MapperProfiles
{
    public class ChecklistProfile : Profile
    {
        public ChecklistProfile()
        {
            CreateMap<Checklist, ChecklistModel>();
            CreateMap<ChecklistItem, ChecklistItemModel>();
            CreateMap<User, UserModel>();
        }
    }
}
