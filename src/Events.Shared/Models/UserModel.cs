namespace Events.Shared.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [ListingColumn(DisplayName = "Name")]
        public string Name { get; set; }

        [ListingColumn(DisplayName = "Email")]
        public string Email { get; set; }

        public string Role { get; set; }

        [ListingColumn(DisplayName = "Is Admin")]
        public bool IsAdmin { get; set; }
    }
}
