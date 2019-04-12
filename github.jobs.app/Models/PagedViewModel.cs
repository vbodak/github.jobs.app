using System.Collections.Generic;

namespace github.jobs.app.Models
{
    public class PagedViewModel<T>
    {
        public List<T> Items { get; set; }
        public int Total { get; set; }
        public int Page { get; set; }
    }
}