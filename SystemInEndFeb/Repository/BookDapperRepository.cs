using Dapper;
using EntityClass;
using IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Repository
{
    public class BookDapperRepository : IBookDapperRepository
    {
        private readonly IConfiguration configuration;

        public BookDapperRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<int> AddAsync(Book book)
        {
            book.Id = book.Id;
            var sql = "Insert into TBook (Title,Price,AuthorName) VALUES (@Title,@Price,@AuthorName)";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ConnStr")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, book);
                return result;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var sql = "DELETE FROM TBook WHERE Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ConnStr")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, new { Id = id });
                return result;
            }
        }

        public async Task<IReadOnlyList<Book>> GetAllAsync()
        {
            var sql = "select * from TBook";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ConnStr")))
            {
                connection.Open();
                var result = await connection.QueryAsync<Book>(sql);
                return result.ToList();
            }
        }



        public async Task<Book> GetByIdAsync(int id)
        {
            var sql = "select * from TBook where Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ConnStr")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<Book>(sql, new { Id = id });
                return result;
            }
        }

        public async Task<int> UpdateAsync(int id,Book book)
        {
            book.Id = id;
            var sql = "UPDATE TBook SET Title = @Title, Price = @Price, AuthorName = @AuthorName WHERE Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ConnStr")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, book);
                return result;
            }
        }



        
    }
}
