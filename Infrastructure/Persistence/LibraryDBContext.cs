using Npgsql;
namespace EKundalik.Infrastructure.Persistence
{
    public class LibraryDBContext
    {
        public static string conString = File.ReadAllText(@"C:..\\..\\..\\..\\Infrastructure\\Appconfig.txt");
        public static void InitializeTables()
        {
            try
            {
                using NpgsqlConnection connection = new NpgsqlConnection(conString);
                connection.Open();
                string query = @"CREATE TABLE book
                (
                      book_id serial NOT NULL,
                      book_name character varying NOT NULL,
                      created_date date NOT NULL,
                      page_count integer,
                      PRIMARY KEY (book_id)
                 );
                    CREATE TABLE author
                    (
                    author_id serial NOT NULL,
                    author_name character varying NOT NULL,
                    birth_date date,
                    address varchar,
                    PRIMARY KEY (author_id)
                );
                    create table book_author
                 (
                    id serial not null,
                    book_id integer not null references book(book_id),
                    author_id integer not null references author(author_id)
                );";

                NpgsqlCommand command = new(query, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static async void CreateDb()
        {
            try
            {
                using NpgsqlConnection connection = new NpgsqlConnection(conString);
                connection.Open();
                connection.Close();
            }
            catch (NpgsqlException e)
            {

                if (e.Message.Contains("3D000", StringComparison.OrdinalIgnoreCase))
                {
                    string newconString = conString;
                    newconString = newconString.Replace("library", "postgres");
                    using NpgsqlConnection connection = new(newconString);
                    connection.Open();
                    string query = "create database library";
                    NpgsqlCommand command = new(query, connection);
                    command.ExecuteNonQuery();
                    InitializeTables();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

    }
}
