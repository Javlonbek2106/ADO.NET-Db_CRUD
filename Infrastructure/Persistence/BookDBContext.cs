using Domain.Models;
using EKundalik.Application.Interfaces;
using EKundalik.Infrastructure.Persistence;
using Npgsql;

namespace Infrastructure.Persistence
{
    public class BookDBContext : IRepository<Book>
    {
        private static readonly string ConString = LibraryDBContext.conString;
        public async Task AddAsync(Book book)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(ConString))
            {
                connection.Open();
                NpgsqlCommand InsertCommand = new NpgsqlCommand(@"insert into book (book_name, created_date, page_count)
                                                                values(@book_name, @created_date, @page_count)", connection);
                InsertCommand.Parameters.AddWithValue("@book_name", book.BookName);
                InsertCommand.Parameters.AddWithValue("@created_date", book.CreatedDate);
                InsertCommand.Parameters.AddWithValue("@page_count", book.PageCount);
                int result = await InsertCommand.ExecuteNonQueryAsync();
                if (result > 0)
                {
                    Console.WriteLine("Added successfully ");
                }
                else
                {
                    Console.WriteLine("failed. ");
                }

            }


        }

        public async Task AddRangeAsync(List<Book> obj)
        {
            foreach (Book book in obj)
            {
                await AddAsync(book);
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(ConString))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand("delete from book where book_id=@id", connection);
                command.Parameters.AddWithValue("@id", id);
                int result = await command.ExecuteNonQueryAsync();
                if (result > 0)
                {
                    Console.WriteLine("Successfully deleted. ");
                }
                else Console.WriteLine("failed. ");



            }
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(ConString))
            {
                connection.Open();
                NpgsqlCommand SelectCommand = new NpgsqlCommand("select * from book", connection);
                List<Book> list = new List<Book>();
                NpgsqlDataReader read = await SelectCommand.ExecuteReaderAsync();
                while (read.Read())
                {
                    list.Add(new Book()
                    {
                        BookId = (int)read["book_id"],
                        BookName = read["book_name"].ToString(),
                        CreatedDate = (DateTime)read["created_date"],
                        PageCount = (int)read["page_count"]

                    });
                }
                return list;

            }
        }

        public async Task<Book> GetByIdAsync(int book_id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(ConString))
            {
                connection.Open();
                NpgsqlCommand SelectCommand = new NpgsqlCommand($"select * from book where book_id=@book_id ", connection);
                SelectCommand.Parameters.AddWithValue("@book_id", book_id);
                NpgsqlDataReader reader = await SelectCommand.ExecuteReaderAsync();
                Book book = new Book();
                while (reader.Read())
                {
                    book = new Book()
                    {
                        BookId = (int)reader["book_id"],
                        BookName = reader["book_name"]?.ToString() ?? "",
                        CreatedDate = (DateTime)reader["created_date"],
                        PageCount = (int)reader["page_count"]
                    };

                }
                return book;
            }
        }

        public async Task<bool> UpdateAsync(int BookId, Book entity)
        {
            using (NpgsqlConnection connection = new(ConString))
            {
                connection.Open();
                NpgsqlCommand UpdateCommand = new("update book set book_name = @book_name, created_date = @created_date, page_count= @page_count where book_id = @book_id", connection);
                UpdateCommand.Parameters.AddWithValue("@book_name", entity.BookName);
                UpdateCommand.Parameters.AddWithValue("@created_date", entity.CreatedDate);
                UpdateCommand.Parameters.AddWithValue("@page_count", entity.PageCount);
                UpdateCommand.Parameters.AddWithValue("@book_id", BookId);
                int result = await UpdateCommand.ExecuteNonQueryAsync();
                if (result > 0)
                {
                    return true;

                }
                return false;
            }

        }
    }
}