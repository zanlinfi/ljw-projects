using EFCore.Db;
using EntityClass;
using IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

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

        public async Task<Result> GetBooksAsync(Page req)
        {
            Result res = new Result();
            Page page = new Page();
            int total = 0;
            if (string.IsNullOrEmpty(req.KeyWord))
            {
                res.Res = await ctx.Books.AsNoTracking().Skip((req.PageIndex - 1) * req.PageSize).Take(req.PageSize).ToListAsync();
                //Skip((result.page - 1) * result.pageSize).Take(result.pageSize).ToList();
                res.Total = await ctx.Books.AsNoTracking().CountAsync();

            }
            res.Res = await ctx.Books.AsNoTracking().Where(s => s.Id.ToString().Contains(req.KeyWord) || s.Title.Contains(req.KeyWord) || s.AuthorName.Contains(req.KeyWord))
                    .OrderBy(s => s.Id)
                    .Skip((req.PageIndex - 1) * req.PageSize).Take(req.PageSize)
                    .ToListAsync();
            res.Total = await ctx.Books.AsNoTracking().Where(s => s.Id.ToString().Contains(req.KeyWord) || s.Title.Contains(req.KeyWord) || s.AuthorName.Contains(req.KeyWord))
                .CountAsync();

            return res;
        }

        public async Task<int> AddAsync(Book book)
        {
            var bExis = GetByIdAsync(book.Id);
            if (bExis != null)
            {
                return 0;
            }
            var success = await ctx.Books.AddAsync(book);
            await ctx.SaveChangesAsync();
            if (success.State != EntityState.Added)
            {
                logger.LogInformation("add failure");
                return 0;
            }
  
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

        public async Task<bool> AddBookAsync(BookRequest req)
        {
            //var bTitle = await ctx.Books.FirstOrDefaultAsync(b => b.Title == req.Title);
            //if (bTitle.Title != null)
            //{
            //    return false;
            //}

            Book book = new Book() { Title = req.Title, Price = req.Price, AuthorName = req.AuthorName };

            var success = await ctx.Books.AddAsync(book);

            if (success.State != EntityState.Added)
            {
                return false;
            }
            await ctx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditBookAsync(Book req)
        {
            var b = await ctx.Books.FirstOrDefaultAsync(b => b.Id == req.Id);
            if (b == null)
            {
                logger.LogInformation("get a book is empty");
                return false;
            }
            b.Title = req.Title;
            b.AuthorName = req.AuthorName;
            b.Price = req.Price;
            var success = ctx.Update(b);
            if (success.State != EntityState.Modified)
            {
                return false;
            }
            await ctx.SaveChangesAsync();
            return true;
        }
    }
}