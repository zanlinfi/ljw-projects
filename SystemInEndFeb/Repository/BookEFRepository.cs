using EFCore.Db;
using EntityClass;
using IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Repository
{
    public class BookEFRepository : IBookEFRepository
    {
        private readonly BookEFDbContext ctx;
        private readonly ILogger<BookEFRepository> logger;

        public BookEFRepository(BookEFDbContext ctx, ILogger<BookEFRepository> logger)
        {
            this.ctx = ctx;
            this.logger = logger;
        }

        public async Task<int> AddAsync(Book book)
        {
            var success = await ctx.Books.AddAsync(book);
            if (success.State != EntityState.Added)
            {
                logger.LogInformation("add failure");
                return 0;
            }
            await ctx.SaveChangesAsync();
            return book.Id;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var book = await ctx.Books.FindAsync(id);
            if (book == null)
            {
                logger.LogInformation("delete failure");
                return 0;
            }
            ctx.Books.Remove(book);
            await ctx.SaveChangesAsync();
            return book.Id;

        }

        public async Task<IReadOnlyList<Book>> GetAllAsync()
        {
            var books = await ctx.Books.AsNoTracking().ToArrayAsync();
            if (books == null)
            {
                logger.LogInformation("get all book is empty");
                return Array.Empty<Book>();
            }
            return books;
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            var book = await ctx.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
            {
                logger.LogInformation("get a book is empty");
                return new Book { };
            }
            return book;
        }

        public async Task<int> UpdateAsync(int id, Book book)
        {
            var b = await ctx.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (b == null)
            {
                logger.LogInformation("get a book is empty");
                return 0;
            }
            b.Title = book.Title;
            b.AuthorName = book.AuthorName;
            b.Price = book.Price;
            ctx.Update(b);
            await ctx.SaveChangesAsync();
            return b.Id;
        }
    }
}