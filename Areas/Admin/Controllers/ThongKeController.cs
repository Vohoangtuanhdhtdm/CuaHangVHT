using CuaHangVHT.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization; // Import để sử dụng Calendar

namespace CuaHangVHT.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, Manager, Staff")]
    [Area("Admin")]
	public class ThongKeController : Controller
    {
        private readonly TuanStoreContext db;

        public ThongKeController(TuanStoreContext context) { db = context; }

        #region Thống Kê Sản Phẩm
        public IActionResult SanPhamTopSelling()
        {
                var chartData = db.OrderDetails
               .GroupBy(od => od.Product.Name)
               .Select(g => new
               {
                   ProductName = g.Key,
                   TotalSold = g.Sum(od => od.Quantity)
               })
               .OrderByDescending(g => g.TotalSold)
               .Take(10) // Lấy 10 sản phẩm bán chạy nhất
               .ToList();

                    // Cấu trúc dữ liệu cho ApexCharts
                    var chartDataFormatted = new
                    {
                        series = new[]
                        {
                    new
                    {
                        name = "Sản phẩm bán chạy",
                        data = chartData.Select(x => x.TotalSold).ToArray()
                    }
                },
                        chart = new
                        {
                            type = "bar"
                        },
                        xaxis = new
                        {
                            categories = chartData.Select(x => x.ProductName).ToArray()
                        }
                    };

            ViewBag.ChartData = Newtonsoft.Json.JsonConvert.SerializeObject(chartDataFormatted);

            return View();
        }
        public IActionResult SanPhamDowSelling()
        {
            var chartData = db.OrderDetails
           .GroupBy(od => od.Product.Name)
           .Select(g => new
           {
               ProductName = g.Key,
               TotalSold = g.Sum(od => od.Quantity)
           })
           .OrderBy(g => g.TotalSold)
           .Take(10) // Lấy 10 sản phẩm bán chạy nhất
           .ToList();

            // Cấu trúc dữ liệu cho ApexCharts
            var chartDataFormatted = new
            {
                series = new[]
                {
                    new
                    {
                        name = "Sản phẩm bán chạy",
                        data = chartData.Select(x => x.TotalSold).ToArray()
                    }
                },
                chart = new
                {
                    type = "bar"
                },
                xaxis = new
                {
                    categories = chartData.Select(x => x.ProductName).ToArray()
                }
            };

            ViewBag.ChartData = Newtonsoft.Json.JsonConvert.SerializeObject(chartDataFormatted);

            return View();
        }
        #endregion

        #region Thống Kê Doanh Thu
        public IActionResult KhoHang()
        {

            return View();
        }
      
        public IActionResult DoanhThuThang()
        {

            // Truy vấn dữ liệu cơ bản
            var orders = db.Orders
                .Where(o => o.MaTrangThai != null && o.MaTrangThai.Equals("Xong") && o.CreatedAt.HasValue)
                .Select(o => new
                {
                    o.CreatedAt,
                    TotalRevenuePerOrder = o.OrderDetails.Sum(od => od.Quantity * od.Price) // Tính doanh thu từng đơn hàng
                })
                .ToList(); // Thực hiện truy vấn phía SQL và đưa dữ liệu về bộ nhớ

            // Nhóm dữ liệu và tính toán tổng doanh thu
            var chartData = orders
                .GroupBy(o => new
                {
                    Year = o.CreatedAt.Value.Year,
                    Month = o.CreatedAt.Value.Month
                })
                .Select(g => new
                {
                    Month = g.Key.Month,
                    Year = g.Key.Year,
                    TotalRevenue = g.Sum(o => o.TotalRevenuePerOrder) // Tổng doanh thu của nhóm
                })
                .OrderBy(g => g.Year)
                .ThenBy(g => g.Month)
                .ToList();

            // Cấu trúc dữ liệu cho ApexCharts
            var chartDataFormatted = new
            {
                series = new[]
                {
            new
            {
                name = "Doanh thu theo tháng",
                data = chartData.Select(x => x.TotalRevenue).ToArray()
            }
        },
                chart = new
                {
                    type = "bar"
                },
                xaxis = new
                {
                    categories = chartData.Select(x => $"{x.Month}/{x.Year}").ToArray()
                }
            };

            ViewBag.ChartData = Newtonsoft.Json.JsonConvert.SerializeObject(chartDataFormatted);

            return View();
        }
        public IActionResult DoanhThuNam()
        {

            // Truy vấn dữ liệu cơ bản
            var orders = db.Orders
                .Where(o => o.MaTrangThai != null && o.MaTrangThai.Equals("Xong") && o.CreatedAt.HasValue)
                .Select(o => new
                {
                    o.CreatedAt,
                    TotalRevenuePerOrder = o.OrderDetails.Sum(od => od.Quantity * od.Price) // Tính doanh thu từng đơn hàng
                })
                .ToList(); // Thực hiện truy vấn phía SQL và đưa dữ liệu về bộ nhớ

            // Nhóm dữ liệu và tính toán tổng doanh thu
            var chartData = orders
                .GroupBy(o => o.CreatedAt.Value.Year)
                .Select(g => new
                {
                  
                    Year = g.Key,
                    TotalRevenue = g.Sum(o => o.TotalRevenuePerOrder) // Tổng doanh thu của nhóm
                })
                .OrderBy(g => g.Year)
                .ToList();

            // Cấu trúc dữ liệu cho ApexCharts
            var chartDataFormatted = new
            {
                series = new[]
                {
            new
            {
                name = "Doanh thu theo năm",
                data = chartData.Select(x => x.TotalRevenue).ToArray()
            }
        },
                chart = new
                {
                    type = "bar"
                },
                xaxis = new
                {
                    categories = chartData.Select(x => x.Year.ToString()).ToArray()
                }
            };

            ViewBag.ChartData = Newtonsoft.Json.JsonConvert.SerializeObject(chartDataFormatted);

            return View();
        }
        #endregion
        public IActionResult Test()
        {
            return View();
        }
    }
}
