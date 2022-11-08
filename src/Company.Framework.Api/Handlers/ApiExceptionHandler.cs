using System.Diagnostics;
using System.Net;
using Company.Framework.Api.Models.Error.Contract.Builder;
using Company.Framework.Api.Models.Error.Response;
using Company.Framework.Core.Exception;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Company.Framework.Api.Handlers
{
    public class ApiExceptionHandler
    {
        private readonly ILogger _logger;
        private readonly ErrorContractBuilder _errorContractBuilder;
        private static readonly IReadOnlyDictionary<ExceptionState, HttpStatusCode> HttpStatusCodeFromExceptionState = new Dictionary<ExceptionState, HttpStatusCode>
        {
            {ExceptionState.Unknown, HttpStatusCode.InternalServerError},
            {ExceptionState.Invalid, HttpStatusCode.BadRequest},
            {ExceptionState.DoesNotExist, HttpStatusCode.NotFound},
            {ExceptionState.AlreadyExists, HttpStatusCode.Conflict},
            {ExceptionState.AuthorizationFailed, HttpStatusCode.Unauthorized},
            {ExceptionState.PreConditionFailed, HttpStatusCode.PreconditionFailed},
            {ExceptionState.PreConditionRequired, HttpStatusCode.PreconditionRequired},
            {ExceptionState.UnProcessable, HttpStatusCode.UnprocessableEntity},
        };

        public ApiExceptionHandler(ILogger<ApiExceptionHandler> logger, ErrorContractBuilder errorContractBuilder)
        {
            _logger = logger;
            _errorContractBuilder = errorContractBuilder;
        }

        public async Task Handle(HttpContext context)
        {
            var exception = context.Features.Get<IExceptionHandlerPathFeature>()!.Error;
            var errorResponse = new ErrorResponse();
            ExceptionState exceptionState;
            if (exception is StatefulCoreException coreException)
            {
                exceptionState = coreException.State;
                errorResponse.Error = _errorContractBuilder.Build(coreException);
                using (_logger.BeginScope(BuildExceptionScopeState))
                    _logger.LogError(exception.Demystify(),
                        "[CoreException] Code: {errorCode} State: {state} Message: {message}",
                        coreException.ActualError.Code, exceptionState, coreException.Message);
            }
            else
            {
                exceptionState = ExceptionState.Unknown;
                errorResponse.Error = _errorContractBuilder.Build();
            }


            context.Response.StatusCode = (int)HttpStatusCodeFromExceptionState[exceptionState];
            await context.Response.WriteAsJsonAsync(errorResponse);
        }

        private static Dictionary<string, object> BuildExceptionScopeState(StatefulCoreException exception)
        {
            var (code, message, _) = exception.ActualError;

            var parameters = new Dictionary<string, object>
            {
                ["Code"] = $"{code}",
                ["Message"] = $"{message}"
            };

            return new Dictionary<string, object>
            {
                { "ActualError", parameters }
            };
        }
    }
}
