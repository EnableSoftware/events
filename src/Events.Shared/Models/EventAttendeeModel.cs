namespace Events.Shared.Models
{
    public class EventAttendeeModel
    {
        public int Id { get; set; }

        [ListingColumn(DisplayName = "Name")]
        public string Name { get; set; }
    }
}
