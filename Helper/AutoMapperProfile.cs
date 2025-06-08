using AutoMapper;
using CuaHangVHT.Data;
using CuaHangVHT.ViewModels;

namespace CuaHangVHT.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            // Cùng tên thì nó sẽ map - map 1 chiều từ DangKyViewModel qua -> User
            CreateMap<DangKyViewModel, User>();
            CreateMap<BlogVM, BlogPost>();
            // Chỉ rõ nếu khác tên
            //.ForMember(kh => kh.HoTen, option => option.MapFrom(RegisterVM => RegisterVM.HoTen))
            //.ReverseMap(); // map 2 chiều
        }
    }
}
