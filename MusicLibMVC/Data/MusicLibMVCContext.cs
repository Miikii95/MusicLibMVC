using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicLibMVC.Models;

namespace MusicLibMVC.Data
{
    public class MusicLibMVCContext : DbContext
    {
        public MusicLibMVCContext (DbContextOptions<MusicLibMVCContext> options)
            : base(options)
        {
        }

        public DbSet<MusicLibMVC.Models.AlbumModel> Album { get; set; } = default!;
        public DbSet<MusicLibMVC.Models.TrackModel> Track { get; set; } 
    }
}
