
using EntityClass;

namespace IRepository
{
    public interface IBookDapperRepository : IGenericRepository<Book>
    {
        Task<bool> BulkDelete(string ids);
    }
}
