using Domain.Models;
using EKundalik.Infrastructure.Persistence;
using Infrastructure.Persistence;

namespace Imtixon_Javlon
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Caller();
            Console.ReadKey();
        }
        static async void Caller()
        {
            LibraryDBContext.CreateDb();
            //BookDBContext bookDB = new();
            //Book book = await bookDB.GetByIdAsync(2);
            //// Console.WriteLine(book);
            //Book book1 = new Book()
            //{
            //    BookName = "God Father",
            //    CreatedDate = DateTime.Now,
            //    PageCount = 1000,
            //};
            //Book book2 = new Book()
            //{
            //    BookName = "God Mother",
            //    CreatedDate = DateTime.Now,
            //    PageCount = 230,
            //};
            ////await bookDB.UpdateAsync(1,book2);
            ////    await bookDB.AddAsync(book1);
            //await bookDB.DeleteAsync(1);

        }
    }
}