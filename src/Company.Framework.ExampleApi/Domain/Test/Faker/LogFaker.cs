using Company.Framework.Core.Logging;
using Company.Framework.Test.Faker;

namespace Company.Framework.ExampleApi.Domain.Test.Faker;

public class LogFaker : CoreFaker
{
    public Log Log()
    {
        return Core.Logging.Log.Load(By());
    }

    public string By()
    {
        return Faker.Person.FullName;
    }
}