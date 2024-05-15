namespace Talabat.APIs.Errors
{
    public class ApiErrorResponse
    {
        public int StatuesCode { get; set; }
        public string? Message { get; set; }
        public ApiErrorResponse(int StatuesCode,string? message = null) 
        {
            this.StatuesCode = StatuesCode;
            Message = message ?? GetDefaultMessageForStatusCode(StatuesCode);
        }
        private string? GetDefaultMessageForStatusCode(int statuesCode)
        {
            return statuesCode switch
            {
                400 => "A bad Request, You Have Made",
                401 => "Authorized, You Are Not",
                404 => "Resourses Not Found",
                500 => "There Is Server Error",
                _ => null
            };
        }
    }
}
