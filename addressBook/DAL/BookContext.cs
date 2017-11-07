using addressBook.Models;
using System.Data.Entity;

namespace addressBook.DAL
{
    public class BookContext:DbContext
    {
        public BookContext():base("BookContext"){ }

        public DbSet<User> Users { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
    }
}