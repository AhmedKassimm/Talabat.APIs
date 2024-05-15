namespace Talabat.APIs.Errors
{
    public class ApiValidationErrorResponse: ApiErrorResponse//status code => Message
    {
        public IEnumerable<string >Errors { get; set; }
        public ApiValidationErrorResponse() : base(400)
        {
            Errors = new List<string>();
        }
    }
}
