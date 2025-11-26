using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebClient.DTOs;
using WebClient.Data;
using WebAPI.Services.Interfaces;

namespace WebClient.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("authToken");
            if (token == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var Books = await _service.GetAllAsync();
            return View(Books);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Book = await _service.GetByIdAsync((Guid)id);
            if (Book == null)
            {
                return NotFound();
            }

            return View(Book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductDTO dto)
        {
            if (ModelState.IsValid)
            {

                await _service.CreateAsync(dto);
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Book = await _service.GetByIdAsync((Guid)id);
            if (Book == null)
            {
                return NotFound();
            }
            return View(Book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UpdateProductDTO dto)
        {
            if (id != id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                await _service.UpdateAsync(id, dto);

                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Book = await _service.GetByIdAsync((Guid)id);
            if (Book == null)
            {
                return NotFound();
            }

            return View(Book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {

            if (id == null)
            {
                return NotFound();
            }

            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
