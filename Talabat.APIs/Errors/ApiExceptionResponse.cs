namespace Talabat.APIs.Errors
{
    public class ApiExceptionResponse:ApiErrorResponse
    {
        public string? Details { get; set; }
        public ApiExceptionResponse(int StatuesCode,string? Message=null,string? Details=null):base(StatuesCode, Message)
        {
            this.Details = Details;
        }
    }
}
