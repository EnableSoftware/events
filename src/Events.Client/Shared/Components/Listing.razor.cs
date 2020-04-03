using Events.Shared;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace Events.Client.Shared.Components
{
    public class ListingBase<T> : ComponentBase
    {
        public ListingBase()
        {
        }

        [Parameter]
        public Action<T> RowClicked { get; set; }

        [Parameter]
        public IEnumerable<T> Items { get; set; }

        protected string RowCssClass { get; set; }

        protected Dictionary<string, ICollection<object>> Columns { get; set; }

        // TODO Proof of concept for generic listing component, very experimental, improve
        protected override void OnInitialized()
        {
            if (Items != null)
            {
                Columns = new Dictionary<string, ICollection<object>>();
                foreach (var property in typeof(T).GetProperties())
                {
                    foreach (ListingColumnAttribute attrib in property.GetCustomAttributes(typeof(ListingColumnAttribute), true))
                    {
                        var values = new List<object>();
                        foreach (var item in Items)
                        {
                            values.Add(property.GetValue(item));
                        }

                        Columns.Add(attrib.DisplayName, values);
                    }
                }
            }

            if (RowClicked != null)
            {
                RowCssClass += "cursor-pointer";
            }

            base.OnInitialized();
        }
    }
}
