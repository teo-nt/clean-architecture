using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Linq;
using Profile = Application.Profiles.Profile;

namespace Application.Followers
{
    public class List
    {
        public class Query : IRequest<Result<List<Profile>>>
        {
            public string? Predicate { get; set; }
            public string? Username { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<Profile>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext dataContext, IMapper mapper)
            {
                _context = dataContext;
                _mapper = mapper;
            }

            public async Task<Result<List<Profile>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var profiles = new List<Profile>();

                switch (request.Predicate)
                {
                    case "followers":
                        profiles = await _context.UserFollowings.Where(u => u.Target!.UserName == request.Username)
                            .Select(u => u.Observer)
                            .ProjectTo<Profile>(_mapper.ConfigurationProvider)
                            .ToListAsync();
                        break;
                    case "following":
                        profiles = await _context.UserFollowings.Where(u => u.Observer!.UserName == request.Username)
                           .Select(u => u.Target)
                           .ProjectTo<Profile>(_mapper.ConfigurationProvider)
                           .ToListAsync();
                        break;
                }

                return Result<List<Profile>>.Success(profiles);
            }
        }
    } 
}
