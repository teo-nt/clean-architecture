using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ActivityAttendee
    {
        public string AppUserId { get; set; } = string.Empty;
        public AppUser AppUser { get; set; } = null!;
        public Guid ActivityId { get; set; }
        public Activity Activity { get; set; } = null!;

        public bool IsHost { get; set; }
    }
}
