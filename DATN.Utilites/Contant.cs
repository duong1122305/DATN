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
        public const string ImgDefault = "img/productDefault.png";
        public const int MaxIMGSize = 5 * 1024 * 1024;//5mb
                                                      // gắn giá trị combobox đi điểm danh
        public const int AllAttendance = -1;//tất cả
        public const int Present = 1;// có mặt
        public const int Absent = 0;//vắng mặt

    }
}
