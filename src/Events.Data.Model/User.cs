﻿using System.Collections.Generic;

namespace Events.Data.Model
{
    public class User
    {
        public User()
        {
            EventSignUps = new HashSet<UserEventSignUp>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public ICollection<UserEventSignUp> EventSignUps { get; set; }
        public ICollection<UserCategoryTickets> CategoryTickets { get; set; }
    }
}
