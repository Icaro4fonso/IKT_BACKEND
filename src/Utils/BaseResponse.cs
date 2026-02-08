using System.Net;

namespace IKT_BACKEND.Utils
{
    public abstract class BaseResponse<T>
    {
        public bool Success { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        protected BaseResponse(bool success, HttpStatusCode statusCode)
        {
            Success = success;
            StatusCode = statusCode;
        }

        public abstract T GetValue();
        public abstract string GetErrorMessage();
    }

    public class OkResponse<T> : BaseResponse<T>
    {
        public T Data { get; set; }
        public OkResponse(T data) : base(true, HttpStatusCode.OK)
        {
            Data = data;
        }

        public override T GetValue()
        {
            return Data;
        }

        public override string GetErrorMessage()
        {
            throw new NotImplementedException();
        }
    }

    public class ErrorResponse<T> : BaseResponse<T>
    {
        public string ErrorMessage { get; set; }
        public ErrorResponse(string errorMessage) : base(false, HttpStatusCode.BadRequest)
        {
            ErrorMessage = errorMessage;
        }

        public override string GetErrorMessage()
        {
            return ErrorMessage;
        }

        public override T GetValue()
        {
            throw new NotImplementedException();
        }
    }

}
