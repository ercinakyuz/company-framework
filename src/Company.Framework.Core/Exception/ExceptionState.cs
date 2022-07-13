namespace Company.Framework.Core.Exception
{
    public enum ExceptionState
    {
        Unknown,   //500
        Invalid, // 400
        AuthorizationFailed,  //401
        DoesNotExist, //404
        AlreadyExists,  //409
        PreConditionRequired, //428
        PreConditionFailed,    //412
        UnProcessable,   //422
    }
}