using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicLibMVC.Data;
using MusicLibMVC.Models;

namespace MusicLibMVC.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly MusicLibMVCContext _context;

        public AlbumsController(MusicLibMVCContext context)
        {
            _context = context;
        }

        // GET: Albums
        public async Task<IActionResult> Index()
        {
            return View(await _context.Album.ToListAsync());
        }

        // GET: Albums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Album
                .FirstOrDefaultAsync(m => m.Id == id);
            if (album == null)
            {
                return NotFound();
            }
            DetailsViewModel dViewModel = new DetailsViewModel();
            dViewModel.Album = album;
            dViewModel.Tracks = await _context.Track.AsQueryable().Where(m => m.Album.Id == id).ToListAsync();


            return View(dViewModel);
        }
        

        // GET: Albums/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Albums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,title,Artist")] AlbumModel album)
        {
            HttpClient client = new HttpClient();

            string request = "https://ws.audioscrobbler.com/2.0/?method=album.getinfo&artist=" + album.Artist +
                              "&album=" + album.title + "&format=xml&api_key=39f1da971badc80796f46d47f72ea3e5";

            try
            {
                string xmlResponse = await client.GetStringAsync(request);
                XmlDocument albumInfo = new XmlDocument();

                albumInfo.LoadXml(xmlResponse);
                XmlElement root = albumInfo.DocumentElement;
                XmlNodeList tracks = root.SelectNodes("//album//tracks//track");

                XmlNode imgURlNode = root.SelectSingleNode("//album//image[@size=\"extralarge\"]");
                string imgUrl = imgURlNode.InnerText; 
                album.imageURL = imgUrl;

                string trackTitle;
                string sTrackNo;

                foreach (XmlNode node in tracks)

                {
                    trackTitle = node.ChildNodes[0].InnerText;
                    trackTitle = trackTitle.Replace("'", "''");
                    sTrackNo = ((XmlElement)node).GetAttribute("rank");
                    int nTrackNo = Int32.Parse(sTrackNo);
                    TrackModel trackModel = new TrackModel();
                    trackModel.duration = nTrackNo;
                    trackModel.Album = album;
                    trackModel.Title = trackTitle;

                    _context.Add(trackModel);
                }
            }
            catch (Exception ex) { Console.WriteLine(ex); }

            if (ModelState.IsValid)
            {
                _context.Add(album);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else { Console.WriteLine("Nie wiem co sie dzieje ale wiem skad jest ten komunikat"); }
            return View(album);
        }

        // GET: Albums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Album.FindAsync(id);
            if (album == null)
            {
                return NotFound();
            }
            return View(album);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,title,Artist,imageURL")] AlbumModel album)
        {
            if (id != album.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(album);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumExists(album.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(album);
        }

        // GET: Albums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Album
                .FirstOrDefaultAsync(m => m.Id == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var album = await _context.Album.FindAsync(id);
            if (album != null)
            {
                _context.Album.Remove(album);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlbumExists(int id)
        {
            return _context.Album.Any(e => e.Id == id);
        }
    }
}
