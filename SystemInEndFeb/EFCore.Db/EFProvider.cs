using IRepository;

namespace EFCore.Db
{
    public class EFProvider
    {
        public IBookEFRepository Books { get; }
        public ILoginEFRepository Ids { get; }
        public EFProvider(IBookEFRepository books, ILoginEFRepository ids)
        {
            Books = books;
            Ids = ids;  
        }
    }
}
