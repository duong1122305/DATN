namespace DATN.ViewModels.Common
{
    public class ResponseData<T>
    {
        public ResponseData()
        {

        }
        public ResponseData(T data)
        {
            IsSuccess = true;
            Data = data;
        }
        public ResponseData(bool isSucsses, string error)
        {
            IsSuccess = isSucsses;
            Error = error;
        }
        public bool IsSuccess { get; set; }
        public string? Error { get; set; }
        public T? Data { get; set; }
    }

}
