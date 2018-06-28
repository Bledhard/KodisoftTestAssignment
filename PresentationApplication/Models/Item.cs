using System;

namespace PresentationApplication.Models
{
    public class Item
    {
        public int ID { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }

        public Item()
        {
            Link = "";
            Title = "";
            Content = "";
            PublishDate = DateTime.Today;
        }
    }
}
