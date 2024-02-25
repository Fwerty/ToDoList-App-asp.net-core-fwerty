using Microsoft.AspNetCore.Http;
namespace myAppToDoList.Areas.Identity.Data
{
    public class UrunEkle
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? url { get; set; }

    }
}
