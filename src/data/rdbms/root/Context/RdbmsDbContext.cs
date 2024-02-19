using NHibernate;

namespace Company.Framework.Data.Rdbms.Context
{
    public class RdbmsDbContext(ISessionFactory sessionFactory) : IRdbmsDbContext
    {
        public ISession Session => sessionFactory.OpenSession();
    }

}
