using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Back.DAL;
using Back.Models;
using Microsoft.AspNetCore.Hosting;
using Back.Helpers;

namespace Back.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class TestimonialsController : Controller
    {
        private  AppDbContext _context;
        private IWebHostEnvironment _env;
        public TestimonialsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            return View(_context.Testimonials);
        }
        // GET: AdminPanel/Testimonials/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminPanel/Testimonials/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Testimonial testimonial)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!testimonial.Photo.CheckFileType("image/"))
            {
                return View();
            }
            if (!testimonial.Photo.CheckFileSize(200))
            {
                return View();
            }


            testimonial.Image = await testimonial.Photo.SaveFileAsync(_env.WebRootPath, "images");
            await _context.Testimonials.AddAsync(testimonial);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: AdminPanel/Testimonials/Edit/5
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial == null)
            {
                return NotFound();
            }
            return View(testimonial);
        }

        // POST: AdminPanel/Testimonials/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Testimonial newtestimonial)
        {
            if (id==null)
            {
                return BadRequest();
            }
            var oldtestimonial = _context.Testimonials.Find(id);
            if (oldtestimonial == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!newtestimonial.Photo.CheckFileType("image/"))
            {
                return View();
            }
            if (!newtestimonial.Photo.CheckFileSize(200))
            {
                return View();
            }
            var path = Helper.GetPath(_env.WebRootPath, "images", oldtestimonial.Image);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            newtestimonial.Image = await newtestimonial.Photo.SaveFileAsync(_env.WebRootPath, "images");
            oldtestimonial.Image = newtestimonial.Image;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var testimonial = _context.Testimonials.Find(id);
            if (testimonial == null)
            {
                return NotFound();
            }
            var path = Helper.GetPath(_env.WebRootPath, "images", testimonial.Image);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _context.Testimonials.Remove(testimonial);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
