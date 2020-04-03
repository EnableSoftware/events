namespace Events.Data.Model
{
    public class UserEventSignUp
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }

        public int Priority { get; set; }
    }
}
