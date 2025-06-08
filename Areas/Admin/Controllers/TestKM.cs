using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CuaHangVHT.Data;

namespace CuaHangVHT.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TestKM : Controller
    {
        private readonly TuanStoreContext _context;

        public TestKM(TuanStoreContext context)
        {
            _context = context;
        }

        // GET: Admin/TestKM
        public async Task<IActionResult> Index()
        {
            var tuanStoreContext = _context.OrderDetailPromotions.Include(o => o.Product).Include(o => o.Promotion);
            return View(await tuanStoreContext.ToListAsync());
        }

        // GET: Admin/TestKM/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetailPromotion = await _context.OrderDetailPromotions
                .Include(o => o.Product)
                .Include(o => o.Promotion)
                .FirstOrDefaultAsync(m => m.OrderDetailPromotionId == id);
            if (orderDetailPromotion == null)
            {
                return NotFound();
            }

            return View(orderDetailPromotion);
        }

        // GET: Admin/TestKM/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId");
            ViewData["PromotionId"] = new SelectList(_context.Promotions, "PromotionId", "PromotionId");
            return View();
        }

        // POST: Admin/TestKM/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderDetailPromotionId,PromotionId,ProductId")] OrderDetailPromotion orderDetailPromotion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderDetailPromotion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", orderDetailPromotion.ProductId);
            ViewData["PromotionId"] = new SelectList(_context.Promotions, "PromotionId", "PromotionId", orderDetailPromotion.PromotionId);
            return View(orderDetailPromotion);
        }

        // GET: Admin/TestKM/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetailPromotion = await _context.OrderDetailPromotions.FindAsync(id);
            if (orderDetailPromotion == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", orderDetailPromotion.ProductId);
            ViewData["PromotionId"] = new SelectList(_context.Promotions, "PromotionId", "PromotionId", orderDetailPromotion.PromotionId);
            return View(orderDetailPromotion);
        }

        // POST: Admin/TestKM/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderDetailPromotionId,PromotionId,ProductId")] OrderDetailPromotion orderDetailPromotion)
        {
            if (id != orderDetailPromotion.OrderDetailPromotionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderDetailPromotion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderDetailPromotionExists(orderDetailPromotion.OrderDetailPromotionId))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", orderDetailPromotion.ProductId);
            ViewData["PromotionId"] = new SelectList(_context.Promotions, "PromotionId", "PromotionId", orderDetailPromotion.PromotionId);
            return View(orderDetailPromotion);
        }

        // GET: Admin/TestKM/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetailPromotion = await _context.OrderDetailPromotions
                .Include(o => o.Product)
                .Include(o => o.Promotion)
                .FirstOrDefaultAsync(m => m.OrderDetailPromotionId == id);
            if (orderDetailPromotion == null)
            {
                return NotFound();
            }

            return View(orderDetailPromotion);
        }

        // POST: Admin/TestKM/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderDetailPromotion = await _context.OrderDetailPromotions.FindAsync(id);
            if (orderDetailPromotion != null)
            {
                _context.OrderDetailPromotions.Remove(orderDetailPromotion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderDetailPromotionExists(int id)
        {
            return _context.OrderDetailPromotions.Any(e => e.OrderDetailPromotionId == id);
        }
    }
}
