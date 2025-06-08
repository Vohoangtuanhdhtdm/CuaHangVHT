using CuaHangVHT.Data;
using Microsoft.AspNetCore.Mvc;
using CuaHangVHT.Helper;
using CuaHangVHT.ViewModels;
using Microsoft.AspNetCore.Authorization;
using AutoMapper.Internal;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Security.Policy;
using AspNetCoreHero.ToastNotification.Abstractions;
using CuaHangVHT.Services;

namespace CuaHangVHT.Controllers
{
    public class CartController : Controller
    {
        private readonly TuanStoreContext db;
        private readonly INotyfService _notyf;
        private readonly IVnPayService _vnPayService;

        public CartController(TuanStoreContext context, INotyfService notyf, IVnPayService vnPayService)
        {
            db = context;
            _notyf = notyf;
            _vnPayService = vnPayService;

        }

        public List<CartItemVM> Cart => HttpContext.Session.Get<List<CartItemVM>>(MySetting.CART_KEY) ?? new List<CartItemVM>();
        public OrderVnPay VnPayHoaDon => HttpContext.Session.Get<OrderVnPay>(MySetting.VnPayHoaHon_KEY) ?? new OrderVnPay();
        public IActionResult Index()
        {
            return View(Cart);
        }

        #region Giỏ Hàng

        [Authorize]
        public IActionResult AddToCart(int id, int size, int quantity = 1)
        {
           
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHh == id && p.MaSizes == size );
            if (item == null)
            {
                // Chưa có trong giỏ hàng
                var hangHoa = db.Products.SingleOrDefault(p => p.ProductId == id);
                
                if (hangHoa == null)
                {
                    TempData["Message"] = $"Khong tim thay";
                    return Redirect("/404");
                }
                // Lấy thông tin size của sản phẩm từ database
                var sizeDB = db.ProductSizes.SingleOrDefault(p => p.ProductId == id && p.ProductSizeId == size);

                if (sizeDB == null || sizeDB.Quantity < quantity)
                {
                    // Nếu không tìm thấy size hoặc số lượng không đủ, thông báo cho người dùng
                    TempData["Message"] = "Kích thước hoặc số lượng không đủ";
                    return RedirectToAction("Detail", new { id });
                }
                
               
                item = new CartItemVM
                {
                    MaHh = hangHoa.ProductId,
                    TenHH = hangHoa.Name,
                    DonGia = hangHoa.Price,
                    TenSize = sizeDB.SizeName, // Thêm kích thước đã chọn vào giỏ hàng
                    MaSizes = sizeDB.ProductSizeId,
                    Hinh = hangHoa.ImageUrl ?? string.Empty,
                    SoLuong = quantity
                };
                gioHang.Add(item);
            }
            else
            {
                // Đã có trong giỏ hàng
                item.SoLuong += quantity;
                // Lấy thông tin size từ database để kiểm tra lại số lượng
                var sizeDB = db.ProductSizes.SingleOrDefault(p => p.ProductId == id && p.ProductSizeId == size);
                if (sizeDB == null || sizeDB.Quantity < quantity)
                {
                    // Nếu không tìm thấy size hoặc số lượng không đủ, thông báo cho người dùng
                    TempData["Message"] = "Kích thước hoặc số lượng không đủ";
                    return RedirectToAction("Detail", new { id });
                }
                
            }

            HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
         
            return RedirectToAction("index");
        }

        [Authorize]
        public IActionResult RemoveCart(int id)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHh == id);
            if (item != null)
            {
                gioHang.Remove(item);
                HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
            }
            return RedirectToAction("index");

        }

        #endregion

        [Authorize]
        [HttpGet]
        public IActionResult MuaHang()
        {
            if (Cart.Count == 0)
            {
                return Redirect("/");
            }
            return View(Cart);
        }

        [Authorize]
        [HttpPost]
        public IActionResult MuaHang(CheckoutVM model, string payment)
        {
            
            if (ModelState.IsValid)
            {
                var CustomerId = int.Parse(User.GetClaimValue(MySetting.CLAIM_CUSTOMERID));
                // Tính toán TotalPrice
                var totalPrice = Cart?.Sum(item => item.DonGia * item.SoLuong) ?? 0;
                var khachHang = new User();


                if (model.GiongKhachHang)
                {
                    khachHang = db.Users.SingleOrDefault(kh => kh.UserId == CustomerId);
                }
                if (payment == "vnpay")
                {
                    decimal finalTotalPrice = 0; // Tổng tiền sau khi áp dụng khuyến mãi trước
                    decimal discount = 0;
                    foreach (var item in Cart)
                    {
                        // Kiểm tra xem sản phẩm có khuyến mãi hay không
                        var activePromotion = db.Promotions
                            .Where(p => db.OrderDetailPromotions
                                .Any(op => op.ProductId == item.MaHh && p.PromotionId == op.PromotionId) &&
                                p.StartPrDate <= DateTime.Now && p.EndPrDate >= DateTime.Now)
                            .FirstOrDefault();

                        if (activePromotion != null)
                        {
                            // Tính phần trăm giảm giá
                            discount = (item.DonGia * activePromotion.DiscountPercent) / 100;
                        }
                        // Tính giá sau giảm
                        decimal finalPrice = item.DonGia - discount;
                        finalTotalPrice += finalPrice * item.SoLuong;
                    }

                // Tạo Hóa đơn tạm vào session, sau khi thành công thì sẽ tạo hóa đơn 
                    var VnpayHoaDonTemp = new OrderVnPay
                    {
                    UserId = CustomerId,
                    Hoten = model.HoTen ?? khachHang.FullName,
                    DiaChi = model.DiaChi ?? khachHang.Address,
                    DienThoai = model.DienThoai ?? khachHang.PhoneNumber,
                    CreatedAt = DateTime.Now,
                    Status = "Đã TT",
                    CachThanhToan = "VnPay",
                    CachVanChuyen = "J & T",
                    MaTrangThai = "Chưa",
                    TotalPrice = finalTotalPrice,
                    GhiChu = model.GhiChu ?? "Không có ghi chú thêm",
                    EmployeeId = null
                  };
                    
                    HttpContext.Session.Set(MySetting.VnPayHoaHon_KEY, VnpayHoaDonTemp);

                    var vnPayModel = new VnPaymentRequestModel
                    {
                        Amount = Cart.Sum(p => p.ThanhTien),
                        CreatedDate = DateTime.Now,
                        Description = $"{model.HoTen} {model.DienThoai}",
                        FullName = model.HoTen,
                        OrderId = new Random().Next(1000, 100000)
                    };
                    return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, vnPayModel));
               }

                // Hoa Don
                var hoadon = new Order
                {
                    UserId = CustomerId,
                    Hoten = model.HoTen ?? khachHang.FullName,
                    DiaChi = model.DiaChi ?? khachHang.Address,
                    DienThoai = model.DienThoai ?? khachHang.PhoneNumber,
                    CreatedAt = DateTime.Now,
                    Status = "Chưa Thanh Toán",
                    CachThanhToan = "COD",
                    CachVanChuyen = "J & T",
                    MaTrangThai = "Chưa",
                    TotalPrice = totalPrice,
                    GhiChu = model.GhiChu ?? "Không có ghi chú thêm",
                    EmployeeId = null
                };
                db.Database.BeginTransaction();
                try
                {

                    db.Add(hoadon);
                    db.SaveChanges();

                    #region Xử Lý Khuyến Mãi
                    var cthds = new List<OrderDetail>();
                    var promotionDetails = new List<OrderDetailPromotion>();
                    decimal finalTotalPrice = 0; // Tổng tiền sau khi áp dụng khuyến mãi
                    decimal discount = 0;
                    foreach (var item in Cart)
                    {
                        // Kiểm tra xem sản phẩm có khuyến mãi hay không
                        var activePromotion = db.Promotions
                            .Where(p => db.OrderDetailPromotions
                                .Any(op => op.ProductId == item.MaHh && p.PromotionId == op.PromotionId) &&
                                p.StartPrDate <= DateTime.Now && p.EndPrDate >= DateTime.Now)
                            .FirstOrDefault();

                        if (activePromotion != null)
                        {
                            // Tính phần trăm giảm giá
                            discount = (item.DonGia * activePromotion.DiscountPercent) / 100;
                        }
                        // Tính giá sau giảm
                        decimal finalPrice = item.DonGia - discount;
                        finalTotalPrice += finalPrice * item.SoLuong;

                        // Lưu các chi tiết hóa đơn
                        var orderDetail = new OrderDetail
                        {
                            OrderId = hoadon.OrderId,
                            Quantity = item.SoLuong,
                            Price = item.DonGia,
                            ProductId = item.MaHh,
                            Discount = discount.ToString(),
                            ProductSizeId = item.MaSizes,
                        };
                        cthds.Add(orderDetail);

                        
                        #endregion
                        // **Cập nhật số lượng kho**
                        var product = db.Products.SingleOrDefault(p => p.ProductId == item.MaHh);
                        if (product != null)
                        {
                            var sizeDB = db.ProductSizes.SingleOrDefault(p => p.ProductId == item.MaHh && p.ProductSizeId == item.MaSizes);
                            if (sizeDB == null || sizeDB.Quantity < item.SoLuong)
                            {
                                // Không đủ hàng trong kho
                                db.Database.RollbackTransaction();
                                TempData["ErrorMessage"] = $"Sản phẩm '{product.Name}' không đủ hàng trong kho.";
                                return RedirectToAction("Cart"); // Hoặc xử lý lỗi khác
                            }
                            // Cap nhat so luong size của sản phẩm từ database

                            sizeDB.Quantity -= item.SoLuong;
                            product.Stock -= item.SoLuong; // Trừ số lượng sản phẩm đã mua, Tru Them Cai Size nua
                            db.ProductSizes.Update(sizeDB);
                            db.Products.Update(product);
                            db.SaveChanges();
                        }
                    }
                    // Cập nhật tổng tiền sau khi áp dụng khuyến mãi

                    hoadon.TotalPrice = finalTotalPrice;
                   
                    db.AddRange(cthds);
                    db.AddRange(promotionDetails);
                    db.SaveChanges();
                    db.Database.CommitTransaction();
                    _notyf.Success("Đặt Hàng Thành Công!");
                    TempData["SuccessMessage"] = "Cảm ơn bạn đã đặt hàng! Chúng tôi sẽ xử lý đơn hàng của bạn sớm.";
                    return RedirectToAction("Success");
                }
                catch
                {
                    _notyf.Error("Đã xảy ra lỗi trong quá trình đặt hàng.");
                    db.Database.RollbackTransaction();
                }
            }
            
            return View(Cart);
        }

        [Authorize]
        public IActionResult PaymentFail()
        {
            return View();
        }

        [Authorize]
        public IActionResult PaymentCallBack()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            if (response == null || response.VnPayResponseCode != "00")
            {
                TempData["Message"] = $"Lỗi thanh toán VN Pay: {response.VnPayResponseCode}";
                return RedirectToAction("PaymentFail");
            }

            var hoadonThanhCong = HttpContext.Session.Get<OrderVnPay>(MySetting.VnPayHoaHon_KEY);
            var hoadon = new Order
            {
                UserId = hoadonThanhCong.UserId,
                Hoten = hoadonThanhCong.Hoten,
                DiaChi = hoadonThanhCong.DiaChi,
                DienThoai = hoadonThanhCong.DienThoai,
                CreatedAt = DateTime.Now,
                Status = hoadonThanhCong.Status,
                CachThanhToan = hoadonThanhCong.CachThanhToan,
                CachVanChuyen = hoadonThanhCong.CachVanChuyen,
                MaTrangThai = hoadonThanhCong.MaTrangThai,
                TotalPrice = hoadonThanhCong.TotalPrice,
                GhiChu = hoadonThanhCong.GhiChu,
                EmployeeId = null
            };
            db.Database.BeginTransaction();
            //Lưu Chi Tiết Hóa Đơn - Cập nhật số lượng Sản Phẩm
            try
            {

                db.Add(hoadon);
                db.SaveChanges();

                #region Xử Lý Khuyến Mãi
                var cthds = new List<OrderDetail>();
                var promotionDetails = new List<OrderDetailPromotion>();
                decimal finalTotalPrice = 0; // Tổng tiền sau khi áp dụng khuyến mãi
                decimal discount = 0;
                foreach (var item in Cart)
                {
                    // Kiểm tra xem sản phẩm có khuyến mãi hay không
                    var activePromotion = db.Promotions
                        .Where(p => db.OrderDetailPromotions
                            .Any(op => op.ProductId == item.MaHh && p.PromotionId == op.PromotionId) &&
                            p.StartPrDate <= DateTime.Now && p.EndPrDate >= DateTime.Now)
                        .FirstOrDefault();

                    if (activePromotion != null)
                    {
                        // Tính phần trăm giảm giá
                        discount = (item.DonGia * activePromotion.DiscountPercent) / 100;
                    }
                    // Tính giá sau giảm
                    decimal finalPrice = item.DonGia - discount;
                    finalTotalPrice += finalPrice * item.SoLuong;

                    // Lưu các chi tiết hóa đơn
                    var orderDetail = new OrderDetail
                    {
                        OrderId = hoadon.OrderId,
                        Quantity = item.SoLuong,
                        Price = item.DonGia,
                        ProductId = item.MaHh,
                        Discount = discount.ToString(),
                        ProductSizeId = item.MaSizes,
                    };
                    cthds.Add(orderDetail);


                    #endregion
                    // **Cập nhật số lượng kho**
                    var product = db.Products.SingleOrDefault(p => p.ProductId == item.MaHh);
                    if (product != null)
                    {
                        var sizeDB = db.ProductSizes.SingleOrDefault(p => p.ProductId == item.MaHh && p.ProductSizeId == item.MaSizes);
                        if (sizeDB == null || sizeDB.Quantity < item.SoLuong)
                        {
                            // Không đủ hàng trong kho
                            db.Database.RollbackTransaction();
                            TempData["ErrorMessage"] = $"Sản phẩm '{product.Name}' không đủ hàng trong kho.";
                            return RedirectToAction("Cart"); // Hoặc xử lý lỗi khác
                        }
                        // Cap nhat so luong size của sản phẩm từ database

                        sizeDB.Quantity -= item.SoLuong;
                        product.Stock -= item.SoLuong; // Trừ số lượng sản phẩm đã mua, Tru Them Cai Size nua
                        db.ProductSizes.Update(sizeDB);
                        db.Products.Update(product);
                        db.SaveChanges();
                    }
                }
             
                db.AddRange(cthds);
                db.AddRange(promotionDetails);
                db.SaveChanges();
                db.Database.CommitTransaction();
                _notyf.Success("Đặt Hàng Thành Công!");
                TempData["SuccessMessage"] = "Cảm ơn bạn đã đặt hàng! Chúng tôi sẽ xử lý đơn hàng của bạn sớm.";
                return RedirectToAction("Success");
            }
            catch
            {
                _notyf.Error("Đã xảy ra lỗi trong quá trình đặt hàng.");
                db.Database.RollbackTransaction();
            }

            return View();
        }

        [Authorize]
        public IActionResult Success()
        {
            var CustomerId = int.Parse(User.GetClaimValue(MySetting.CLAIM_CUSTOMERID));
            var hoaDon = db.Orders
                           .Where(hd => hd.UserId == CustomerId)
                           .OrderByDescending(hd => hd.CreatedAt) // Lấy hóa đơn mới nhất
                           .FirstOrDefault();

            if (hoaDon == null)
            {
                TempData["Message"] = "Không tìm thấy hóa đơn nào.";
                return RedirectToAction("Index", "Cart");
            }

            var chiTietHoaDon = db.OrderDetails
             .Include(od => od.Product)
             .Include(od => od.ProductSize)
             .Where(ct => ct.OrderId == hoaDon.OrderId)
             .ToList();

            decimal discount = 0;

            foreach(var item in chiTietHoaDon)
            {
                discount += Decimal.Parse(item.Discount);
            }

            //Lap hoa don
            var viewModel = new SuccessVM
            {
                HoaDon = hoaDon,
                ChiTietHoaDon = chiTietHoaDon,
                Dicount = discount
            };

            HttpContext.Session.Set<List<CartItem>>(MySetting.CART_KEY, new List<CartItem>());
            return View(viewModel);
        }
        
        [Authorize]    
        public IActionResult LichSuMuaHang()
        {
            var CustomerId = int.Parse(User.GetClaimValue(MySetting.CLAIM_CUSTOMERID));
            var order = db.Orders.Where(o => o.UserId == CustomerId).Include(o => o.OrderDetails).ThenInclude(od => od.Product)
                .OrderByDescending(od => od.CreatedAt).ToList();

            return View(order);
        }

        [Authorize]
        public IActionResult ChiTietLichSuMuaHang(int orderId)
        {
            var order = db.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product).ThenInclude(p => p.ProductSizes)
				.Include(o => o.User)
                .FirstOrDefault(o => o.OrderId == orderId);
			if (order == null)
			{
				return NotFound(); // Nếu không tìm thấy hóa đơn, trả về 404
			}
            return View(order);
		}
	}
}
