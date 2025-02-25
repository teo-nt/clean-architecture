﻿using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Followers
{
    public class FollowToggle
    {
        public class Command : IRequest<Result<Unit>?>
        {
            public string TargetUsername { get; set; } = string.Empty;
        }

        public class Handler : IRequestHandler<Command, Result<Unit>?>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
            }

            public async Task<Result<Unit>?> Handle(Command request, CancellationToken cancellationToken)
            {
                var observer = await _context.Users.FirstOrDefaultAsync(u => u.UserName == _userAccessor.GetUsername());

                var target = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.TargetUsername);
                if (target is null) return null;

                var following = await _context.UserFollowings.FindAsync(observer!.Id, target.Id);

                if (following is null)
                {
                    following = new UserFollowing
                    {
                        Observer = observer,
                        Target = target
                    };
                    await _context.UserFollowings.AddAsync(following);
                }
                else
                {
                    _context.UserFollowings.Remove(following);
                }
                var success = await _context.SaveChangesAsync() > 0;
                if (success) return Result<Unit>.Success(Unit.Value);
                return Result<Unit>.Failure("Failed to update following");
            }
        }
    }
}
