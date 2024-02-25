using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace myAppToDoList.Areas.Identity.Data
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? Color { get; set; }
        public string? Color2 { get; set; }
        public bool isPublish { get; set; }
        public string? Expire { get; set; }
        public int Expire2 { get; set; }
        public string? Description { get; set; }
        public DateTime? PublishDate { get; set; }
        public string? url { get; set; }

    }
}
