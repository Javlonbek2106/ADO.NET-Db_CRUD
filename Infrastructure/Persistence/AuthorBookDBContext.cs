using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using EKundalik.Application.Interfaces;
using EKundalik.Infrastructure.Persistence;
using Npgsql;

namespace Infrastructure.Persistence
{
    public class AuthorBookDBContext : IRepository<BookAuthor>
    {
        private static readonly string ConString = LibraryDBContext.conString;
        public async Task AddAsync(BookAuthor obj)
        {
            using (NpgsqlConnection connection = new(ConString))
            {
                connection.Open();
                NpgsqlCommand command = new("insert into book_author (book_id, author_id) values(@book_id, @author_id)", connection);
                command.Parameters.AddWithValue("@book_id", obj.Book);
                command.Parameters.AddWithValue("@author_id", obj.Author);
                int n = await command.ExecuteNonQueryAsync();
                if (n > 0)
                {
                    Console.WriteLine("Added successfully");
                }
                else
                {
                    Console.WriteLine("failed . ");
                }
            }
        }

        public async Task AddRangeAsync(List<BookAuthor> obj)
        {
            foreach (BookAuthor bookAuthor in obj)
            {
                await AddAsync(bookAuthor);
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (NpgsqlConnection connection = new(ConString))
            {
                connection.Open();
                NpgsqlCommand command = new("delete from book_author where id = @id", connection);
                command.Parameters.AddWithValue("@id", id);
                int n = await command.ExecuteNonQueryAsync();
                if (n > 0)
                {
                    Console.WriteLine("Sucsesfully deleted");
                }
                else
                {
                    Console.WriteLine("failed . ");
                }
            }
        }

        public async Task<IEnumerable<BookAuthor>> GetAllAsync()
        {
            using (NpgsqlConnection connection = new(ConString))
            {
                connection.Open();
                NpgsqlCommand command = new("select * from book_author", connection);
                List<BookAuthor> list = new List<BookAuthor>();
                NpgsqlDataReader read = await command.ExecuteReaderAsync();
                while (read.Read())
                {
                    list.Add(new BookAuthor()
                    {
                        BookAuthorId = (int)read["id"],
                        Book = (Book)read["book_id"],
                        Author = (Author)read["author_id"],
                    });
                }
                return list;
            }
        }

        public async Task<BookAuthor> GetByIdAsync(int id)
        {
            using (NpgsqlConnection connection = new(ConString))
            {
                connection.Open();
                NpgsqlCommand command = new("select * from book_author", connection);
                BookAuthor BookAuthor = new();
                NpgsqlDataReader read = await command.ExecuteReaderAsync();
                BookAuthor.BookAuthorId = (int)read["id"];
                BookAuthor.Book = (Book)read["book_id"];
                BookAuthor.Author = (Author)read["author_id"];
                return BookAuthor;
            }
        }

        public async Task<bool> UpdateAsync(int id, BookAuthor entity)
        {
            using (NpgsqlConnection connection = new(ConString))
            {
                connection.Open();
                NpgsqlCommand command = new("update book_author set book_id = @book_id, author_id = @author_id", connection);
                command.Parameters.AddWithValue("@book_id", entity.Book);
                command.Parameters.AddWithValue("@author_id", entity.Author);
                int n = await command.ExecuteNonQueryAsync();
                if (n > 0)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
