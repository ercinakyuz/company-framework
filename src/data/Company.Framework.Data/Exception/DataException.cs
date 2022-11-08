using Company.Framework.Core.Error;
using Company.Framework.Core.Exception;

namespace Company.Framework.Data.Exception
{
    public class DataException : StatelessCoreException
    {
        public DataException(CoreError error) : base(error)
        {
        }
    }
}
