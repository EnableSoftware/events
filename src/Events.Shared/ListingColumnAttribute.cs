using System;

namespace Events.Shared
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ListingColumnAttribute : Attribute
    {
        private string _displayName;

        public ListingColumnAttribute()
        {
            _displayName = string.Empty;
        }

        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }
    }
}
