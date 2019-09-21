namespace CurrencyReader.Models
{
    public class OperationResult
    {
        public bool IsSuccessful { get; set; }
        public string ResultMessage { get; set; }
    }

    public class OperationResult<T>
    {
        public bool IsSuccessful { get; set; }
        public T Data { get; set; }
        public string ResultMessage { get; set; }
    }
}