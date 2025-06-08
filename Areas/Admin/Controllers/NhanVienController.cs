using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CuaHangVHT.Data;
using Microsoft.AspNetCore.Authorization;

namespace CuaHangVHT.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class NhanVienController : Controller
    {
        private readonly TuanStoreContext _context;

        public NhanVienController(TuanStoreContext context)
        {
            _context = context;
        }

        // GET: Admin/NhanVien
        public async Task<IActionResult> Index()
        {
            var tuanStoreContext = _context.Employees.Include(e => e.Position).Include(e => e.User);
            return View(await tuanStoreContext.ToListAsync());
        }

        // GET: Admin/NhanVien/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Position)
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Admin/NhanVien/Create
        public IActionResult Create()
        {
            ViewData["PositionId"] = new SelectList(_context.Positions, "PositionId", "PositionName");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Username");
			
			return View();
        }

        // POST: Admin/NhanVien/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,PhoneNumber,Email,PositionId,UserId")] Employee employee)
        {
            var nhanvien = _context.Users.SingleOrDefault(u => u.UserId == employee.UserId);
            // Kiểm tra UserId đã tồn tại trong bảng Employee
            var existingEmployee = _context.Employees.SingleOrDefault(e => e.UserId == employee.UserId);
            if (ModelState.IsValid)
            {
                employee.Email = nhanvien.Email;
                employee.PhoneNumber = nhanvien.PhoneNumber;
                if (existingEmployee != null)
                {
                    // Nếu tồn tại, cập nhật bản ghi
                    existingEmployee.PositionId = employee.PositionId;
                    existingEmployee.PhoneNumber = employee.PhoneNumber;
                    existingEmployee.Email = employee.Email;
                    _context.Update(existingEmployee);
                }
                else
                {
                    // Nếu chưa tồn tại, thêm mới
                    _context.Add(employee);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PositionId"] = new SelectList(_context.Positions, "PositionId", "PositionName", employee.PositionId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Username", employee.UserId);
            return View(employee);
        }

        // GET: Admin/NhanVien/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["PositionId"] = new SelectList(_context.Positions, "PositionId", "PositionName", employee.PositionId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Username", employee.UserId);
            return View(employee);
        }

        // POST: Admin/NhanVien/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,PhoneNumber,Email,PositionId,UserId")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
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
            ViewData["PositionId"] = new SelectList(_context.Positions, "PositionId", "PositionId", employee.PositionId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", employee.UserId);
            return View(employee);
        }

        // GET: Admin/NhanVien/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Position)
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Admin/NhanVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
