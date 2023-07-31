using EntityClass;

namespace IRepository
{
    public interface IBookEFRepository : IGenericRepository<Book>
    {
        Task<Result> GetBooksAsync(Page req);
        Task<bool> AddBookAsync(BookRequest req);

        Task<bool> EditBookAsync(Book req);

    }
}