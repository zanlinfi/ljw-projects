
using IRepository;

namespace ADOHelper.Db
{
    public class AdoProvider
    {
        public IBookAdoRepository Books { get; }
        public AdoProvider(IBookAdoRepository ibookAdo)
        {
            Books = ibookAdo;
        }
    }
}