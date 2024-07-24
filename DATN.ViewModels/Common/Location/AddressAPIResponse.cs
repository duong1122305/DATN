namespace DATN.ViewModels.Common.Location
{
    public class AddressAPIResponse<T>
    {

        public int error { get; set; }
        public string error_text { get; set; }
        public string? data_name { get; set; }
        public T data { get; set; }
    }
}
