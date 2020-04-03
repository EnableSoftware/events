namespace Events.Data.Model
{
    public class UserCategoryTickets
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int Penalty { get; set; }
    }
}
