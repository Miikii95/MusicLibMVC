using System.ComponentModel.DataAnnotations;
namespace MusicLibMVC.Models
{
    public class AlbumModel
    {
        public int Id { get; set; }
        public string? title { get; set; }
        public string? Artist { get; set; }
        public string? imageURL {  get; set; }

    }
}
