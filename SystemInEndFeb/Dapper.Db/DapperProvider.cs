using EntityClass;
using IRepository;

namespace Dapper.Db
{
    public class DapperProvider
    {
        public IBookDapperRepository Books { get; }
        public DapperProvider(IBookDapperRepository iBookDapper)
        {
            Books = iBookDapper;
        }
        
    }
   

}