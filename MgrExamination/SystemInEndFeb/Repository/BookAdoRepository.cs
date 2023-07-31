using EntityClass;
using IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Repository
{
    public class BookAdoRepository : IBookAdoRepository
    {
        private readonly IConfiguration configuration;

        public BookAdoRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<int> AddAsync(Book book)
        {
            var sqlProc = $"AddBook";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ConnStr")))
            {

                using (SqlCommand cmd = new SqlCommand(sqlProc, connection))
                {
                    connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Title", book.Title);
                    cmd.Parameters.AddWithValue("@Price", book.Price);
                    cmd.Parameters.AddWithValue("@AuthorName", book.AuthorName);
                    int result = await cmd.ExecuteNonQueryAsync();
                    return result;
                }

            }

        }


        public async Task<int> DeleteAsync(int id)
        {
            var sqlProc = $"DeleteById";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ConnStr")))
            {
                using (SqlCommand cmd = new SqlCommand(sqlProc, connection))
                {
                    connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    int result = await cmd.ExecuteNonQueryAsync();
                    return result;
                }

            }

        }

        public async Task<IReadOnlyList<Book>> GetAllAsync()
        {
            List<Book> result = new List<Book>();
            var sqlProc = "GetBookAll";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ConnStr")))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand(sqlProc, connection))
                    {
                        connection.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataReader rdr = await cmd.ExecuteReaderAsync();

                        while (rdr.Read())
                        {
                            Book book = new Book();
                            book.Id = Convert.ToInt32(rdr["Id"]);
                            book.Title = rdr["Title"].ToString();
                            book.Price = Convert.ToDouble(rdr["Price"]);
                            book.AuthorName = rdr["AuthorName"].ToString();
                            result.Add(book);
                        }
                        return result;
                    }
                }
                catch (Exception ex)
                {
                    connection.Dispose();
                    throw new Exception($"can not get book all {ex}");

                }

            }

        }

        public async Task<Book> GetByIdAsync(int id)
        {
            Book book = new Book();
            var sqlProc = $"GetBookById";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ConnStr")))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand(sqlProc, connection))
                    {
                        connection.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", id);
                        SqlDataReader rdr = await cmd.ExecuteReaderAsync();
                        while (rdr.Read())
                        {
                            book.Id = Convert.ToInt32(rdr["Id"]);
                            book.Title = rdr["Title"].ToString();
                            book.Price = Convert.ToDouble(rdr["Price"]);
                            book.AuthorName = rdr["AuthorName"].ToString();
                        }
                        return book;
                    }
                }
                catch (Exception ex)
                {
                    connection.Dispose();
                    throw new Exception($"can not get book {id}, {ex}");

                }

            }
        }

        public async Task<int> UpdateAsync(int id, Book book)
        {
            var sqlProc = $"UpdateBookById";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ConnStr")))
            {

                using (SqlCommand cmd = new SqlCommand(sqlProc, connection))
                {
                    connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Title", book.Title);
                    cmd.Parameters.AddWithValue("@Price", book.Price);
                    cmd.Parameters.AddWithValue("@AuthorName", book.AuthorName);
                    int result = await cmd.ExecuteNonQueryAsync();
                    return result;
                }

            }
        }
    }
}
