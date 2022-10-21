using Company.Framework.Core.Error;

namespace Company.Framework.Api.Models.Error.Contract
{
    public class ErrorContract
    {
        private string _code;

        private string _message;

        public string Code
        {
            get => string.IsNullOrWhiteSpace(_code) ? GenericError.Code : _code;
            set => _code = value;
        }

        public string Message
        {
            get => string.IsNullOrWhiteSpace(_message) ? GenericError.Message : _message;
            set => _message= value;
        }
    }
}