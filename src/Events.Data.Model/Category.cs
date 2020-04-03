using System.Collections.Generic;

namespace Events.Data.Model
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserCategoryTickets> UserTickets { get; set; }
        public ICollection<Event> Events { get; set; }
    }
}
