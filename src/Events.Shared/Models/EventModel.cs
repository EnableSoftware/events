using System;
using System.Collections.Generic;

namespace Events.Shared.Models
{
    public class EventModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }

        [ListingColumn(DisplayName = "Date")]
        public DateTimeOffset Date { get; set; }

        [ListingColumn(DisplayName = "Capacity")]
        public int Capacity { get; set; }

        public IEnumerable<EventAttendeeModel> Attendees { get; set; }

        [ListingColumn(DisplayName = "Location")]
        public string Location { get; set; }

        public bool SignedUp { get; set; }
        public DateTimeOffset? LockedDate { get; set; }

        [ListingColumn(DisplayName = "# of sign ups")]
        public int SignedUpCount { get; set; }
    }
}
