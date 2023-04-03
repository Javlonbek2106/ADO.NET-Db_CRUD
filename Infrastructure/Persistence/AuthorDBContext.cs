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
    public class AuthorDBContext : IRepository<Author>
    {
        private static readonly string ConString = LibraryDBContext.conString;

        public async Task AddAsync(Author obj)
        {
            using (NpgsqlConnection connection = new(ConString))
            {
                connection.Open();
                NpgsqlCommand command = new("insert into author (author_name, birth_date, address) values(@author_name, @birth_date, @address)", connection);
                command.Parameters.AddWithValue("@author_name", obj.AuthorName);
                command.Parameters.AddWithValue("@birth_date", obj.BirthDate);
                command.Parameters.AddWithValue("@address", obj.Address);
                int n = await command.ExecuteNonQueryAsync();
                if(n > 0)
                {
                    Console.WriteLine("Added successfully");
                }
                else
                {
                    Console.WriteLine("failed . ");
                }
            }
        }

        public async Task AddRangeAsync(List<Author> obj)
        {
            foreach (Author author in obj)
            {
                await AddAsync(author);
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (NpgsqlConnection connection = new(ConString))
            {
                connection.Open();
                NpgsqlCommand command = new("delete from author where author_id = @id", connection);
                command.Parameters.AddWithValue("@id", id);
                int n = await command.ExecuteNonQueryAsync();
                if(n > 0)
                {
                    Console.WriteLine("Sucsesfully deleted");
                }
                else
                {
                    Console.WriteLine("failed . ");
                }
            }
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            using (NpgsqlConnection connection = new(ConString))
            {
                connection.Open();
                NpgsqlCommand command = new("select * from author", connection);
                List<Author> list = new List<Author>();
                NpgsqlDataReader read = await command.ExecuteReaderAsync();
                while (read.Read())
                {
                    list.Add( new Author()
                    {
                        AuthorId = (int)read["author_id"],
                        AuthorName = read["author_name"].ToString(),
                        Address = read["address"].ToString(),
                        BirthDate = (DateTime)read["birth_date"]
                    });
                }
                return list;
            }
        }

        public async Task<Author> GetByIdAsync(int id)
        {
            using (NpgsqlConnection connection = new(ConString))
            {
                connection.Open();
                NpgsqlCommand command = new("select * from author where author_id = @id", connection);
                command.Parameters.AddWithValue("@id", id);
                Author author = new();
                NpgsqlDataReader read = await command.ExecuteReaderAsync();
                author.AuthorId = (int)read["author_id"];
                author.AuthorName = read["author_name"].ToString();
                author.BirthDate = (DateTime)read["birth_date"];
                author.Address = read["address"].ToString();
                return author;
            }
        }

        public async Task<bool> UpdateAsync(int id, Author entity)
        {
            using (NpgsqlConnection connection = new(ConString))
            {
                connection.Open();
                NpgsqlCommand command = new("update author set author_name = @author_name, birth_date = @birth_date, address = @address", connection);
                command.Parameters.AddWithValue("@author_name", entity.AuthorName);
                command.Parameters.AddWithValue("@birth_date", entity.BirthDate);
                command.Parameters.AddWithValue("@address", entity.Address);
                int n = await command.ExecuteNonQueryAsync();
                if(n > 0)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
