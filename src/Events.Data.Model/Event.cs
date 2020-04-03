using System;
using System.Collections.Generic;

namespace Events.Data.Model
{
    public class Event
    {
        public Event()
        {
            UserSignUps = new HashSet<UserEventSignUp>();
        }

        public int Id { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public DateTimeOffset Date { get; set; }
        public DateTimeOffset? LockedDate { get; set; }
        public int Capacity { get; set; }
        public string Location { get; set; }
        public ICollection<UserEventSignUp> UserSignUps { get; set; }
    }
}
