using System.ComponentModel.DataAnnotations;

namespace MusicLibMVC.Models
{
    public class DetailsViewModel
    {
        
        public AlbumModel Album { get; set; }
        public IEnumerable <TrackModel> Tracks { get; set; }

    }

}
