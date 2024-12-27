using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AppUser : IdentityUser
    {
        public string? DisplayName { get; set; }
        public string? Bio { get; set; }

        public ICollection<ActivityAttendee> Activities { get; set; } = new HashSet<ActivityAttendee>();
        public ICollection<Photo> Photos { get; set; } = new HashSet<Photo>();
        public ICollection<UserFollowing> Followings { get; set; } = new HashSet<UserFollowing>();
        public ICollection<UserFollowing> Followers { get; set; } = new HashSet<UserFollowing>();
    }
}
