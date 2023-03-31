using Bogus;

namespace Company.Framework.Test.Faker
{
    public abstract class CoreFaker
    {
        protected readonly Bogus.Faker Faker;

        protected CoreFaker()
        {
            Faker = new Bogus.Faker();
        }
    }
}
