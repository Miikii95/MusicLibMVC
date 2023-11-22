using System.ComponentModel.DataAnnotations;

namespace MusicLibMVC.Models
{
    public class TrackModel
    {
        [Key]
       public int Id { get; set; }
        public string? Title { get; set; }
         public AlbumModel Album { get; set; }
        public int duration { get; set; }


    }
}
