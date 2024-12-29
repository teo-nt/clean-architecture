using Application.Activities;
using Application.Comments;
using AutoMapper;
using Domain;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Activity, Activity>();
            CreateMap<Activity, ActivityDto>()
                .ForMember(d => d.HostUsername, opt => opt.MapFrom(a => a.Attendees
                    .FirstOrDefault(aa => aa.IsHost)!.AppUser.UserName));
            CreateMap<ActivityAttendee, AttendeeDto>()
                .ForMember(p => p.DisplayName, opt => opt.MapFrom(a => a.AppUser.DisplayName))
                .ForMember(p => p.Username, opt => opt.MapFrom(a => a.AppUser.UserName))
                .ForMember(p => p.Bio, opt => opt.MapFrom(a => a.AppUser.Bio))
                .ForMember(p => p.Image, opt => opt.MapFrom(a => a.AppUser.Photos.FirstOrDefault(x => x.IsMain)!.Url));
            CreateMap<AppUser, Profiles.Profile>()
                .ForMember(d => d.Image, opt => opt.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain)!.Url))
                .ForMember(d => d.FollowersCount, opt => opt.MapFrom(s => s.Followers.Count))
                .ForMember(d => d.FollowingCount, opt => opt.MapFrom(s => s.Followings.Count));
            CreateMap<Comment, CommentDto>()
                .ForMember(p => p.DisplayName, opt => opt.MapFrom(a => a.Author!.DisplayName))
                .ForMember(p => p.Username, opt => opt.MapFrom(a => a.Author!.UserName))
                .ForMember(p => p.Image, opt => opt.MapFrom(a => a.Author!.Photos.FirstOrDefault(x => x.IsMain)!.Url));
        }
    }
}
