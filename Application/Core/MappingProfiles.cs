﻿using Application.Activities;
using Application.Comments;
using AutoMapper;
using Domain;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            string? currentUsername = null;
            CreateMap<Activity, Activity>();
            CreateMap<Activity, ActivityDto>()
                .ForMember(d => d.HostUsername, opt => opt.MapFrom(a => a.Attendees
                    .FirstOrDefault(aa => aa.IsHost)!.AppUser.UserName));
            CreateMap<ActivityAttendee, AttendeeDto>()
                .ForMember(p => p.DisplayName, opt => opt.MapFrom(a => a.AppUser.DisplayName))
                .ForMember(p => p.Username, opt => opt.MapFrom(a => a.AppUser.UserName))
                .ForMember(p => p.Bio, opt => opt.MapFrom(a => a.AppUser.Bio))
                .ForMember(p => p.Image, opt => opt.MapFrom(a => a.AppUser.Photos.FirstOrDefault(x => x.IsMain)!.Url))
                .ForMember(d => d.FollowersCount, opt => opt.MapFrom(s => s.AppUser.Followers.Count))
                .ForMember(d => d.FollowingCount, opt => opt.MapFrom(s => s.AppUser.Followings.Count))
                .ForMember(d => d.Following, opt => opt.MapFrom(s => s.AppUser.Followers.Any(u => u.Observer!.UserName == currentUsername)));
            CreateMap<AppUser, Profiles.Profile>()
                .ForMember(d => d.Image, opt => opt.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain)!.Url))
                .ForMember(d => d.FollowersCount, opt => opt.MapFrom(s => s.Followers.Count))
                .ForMember(d => d.FollowingCount, opt => opt.MapFrom(s => s.Followings.Count))
                .ForMember(d => d.Following, opt => opt.MapFrom(s => s.Followers.Any(u => u.Observer!.UserName == currentUsername)));
            CreateMap<Comment, CommentDto>()
                .ForMember(p => p.DisplayName, opt => opt.MapFrom(a => a.Author!.DisplayName))
                .ForMember(p => p.Username, opt => opt.MapFrom(a => a.Author!.UserName))
                .ForMember(p => p.Image, opt => opt.MapFrom(a => a.Author!.Photos.FirstOrDefault(x => x.IsMain)!.Url));
        }
    }
}
