using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CuaHangVHT.Data;
using CuaHangVHT.Areas.Admin.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace CuaHangVHT.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Manager, Staff")]
    public class HoaDonController : Controller
    {
        private readonly TuanStoreContext _context;

        public HoaDonController(TuanStoreContext context)
        {
            _context = context;
        }

        // GET: Admin/HoaDon
        public async Task<IActionResult> Index()
        {
            var tuanStoreContext = _context.Orders.Where(em => em.EmployeeId == null).Include(o => o.Employee).Include(o => o.User).OrderByDescending(O => O.CreatedAt);
            return View(await tuanStoreContext.ToListAsync());
        }

        // Đơn Hàng Đã Được Duyệt
        public async Task<IActionResult> HoaDonDaDuyet()
        {
            var tuanStoreContext = _context.Orders.Where(em => em.EmployeeId != null && !em.MaTrangThai.Equals("Xong")).Include(o => o.Employee).Include(o => o.User).OrderByDescending(O => O.CreatedAt);
            return View(await tuanStoreContext.ToListAsync());
        }

        // Đơn Hàng Hoàn Tất
        public async Task<IActionResult> HoaDonHoanThanh()
        {
            var tuanStoreContext = _context.Orders.Where(em => em.EmployeeId != null && em.MaTrangThai.Equals("Xong")).Include(o => o.Employee).Include(o => o.User).OrderByDescending(O => O.CreatedAt);
            return View(await tuanStoreContext.ToListAsync());
        }


        // GET: Admin/HoaDon/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.OrderDetails).ThenInclude(o => o.Product).ThenInclude(pr => pr.ProductSizes)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            var orderModel = new ChiTietDonHangVM
            {
                HoaDon = order,
                CTHD = (List<OrderDetail>)order.OrderDetails.ToList(),
                KhachHang = order.User
            };



            return View(orderModel);
        }

        // GET: Admin/HoaDon/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Admin/HoaDon/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,UserId,TotalPrice,Status,CreatedAt,UpdatedAt,Hoten,DiaChi,CachThanhToan,CachVanChuyen,DienThoai,MaTrangThai,GhiChu,EmployeeId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", order.EmployeeId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", order.UserId);
            return View(order);
        }

        // GET: Admin/HoaDon/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", order.EmployeeId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", order.UserId);
           
            return View(order);
        }

        // POST: Admin/HoaDon/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,UserId,TotalPrice,Status,CreatedAt,UpdatedAt,Hoten,DiaChi,CachThanhToan,CachVanChuyen,DienThoai,MaTrangThai,GhiChu,EmployeeId")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", order.EmployeeId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", order.UserId);
            return View(order);
        }

        // GET: Admin/HoaDon/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Employee)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Admin/HoaDon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
