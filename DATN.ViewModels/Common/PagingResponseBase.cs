using System.ComponentModel.DataAnnotations;

namespace DATN.ViewModels.Common
{
    public class PagingResponseData
    {
        [Range(1, 100)]
        public int PageIndex { get; set; }// page hiện tại
        [Range(1, 100)]
        public int PageSize { get; set; }// kích thước page
        public int TotalCount { get; set; }// Tổng số bản ghi
        public int TotalPage { get; set; }// Tổng số trang
        public bool? HasNext => TotalPage > PageIndex;// có next đc ko
        public bool? HasPrev => PageIndex > 0;// có lùi đc ko
        public PagingResponseData(int pageIndex, int pageSize, int tolalCount)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = tolalCount;
            TotalPage = pageSize > 0 ? (int)Math.Ceiling((decimal)TotalCount / PageSize) : 0;
        }
    }
}
