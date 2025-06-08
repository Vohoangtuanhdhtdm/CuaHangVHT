using CuaHangVHT.Data;

namespace CuaHangVHT.Areas.Admin.ViewModel
{
    public class OrderIndexViewModel
    {
       
            public IEnumerable<Order> FilteredOrders { get; set; }
            public string SelectedFilter { get; set; }
       

    }

}
