using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DATN.Utilites
{
    public static class Contant
    {
        // Giá trị của hành động
        public const int Arrived = 11;
        public const int Confirm = 12;
        public const int Update = 13;
        public const int Cancel = 14;
        public const int Complete = 15;
        public const int Payment = 16;
        //
        //gán ID của khách lẻ
        public static readonly Guid GuestsID = Guid.Parse("CF9FA787-B64C-462A-A3BA-08DC8D178FC0");
        // Thêm ảnh mặc định
        public const string ImgUser = "img/images.jpg";
		public const int MaxIMGSize = 5*1024*1024;//5mb

	}
}
