using addressBook.Models;
using System;
using System.Collections.Generic;

namespace addressBook.DAL
{
    public class BookInitializer:System.Data.Entity.DropCreateDatabaseAlways<BookContext>
    {
        protected override void Seed(BookContext context)
        {
            var users = new List<User>()
            {
                new User{Name="Georg",Surname="Alonso", BirthDay=DateTime.Parse("1990-01-01")},
                new User{Name="Jack",Surname="Sainz", BirthDay=DateTime.Parse("1950-05-05")},
                new User{Name="Denise",Surname="Brock", BirthDay=DateTime.Parse("1953-09-05")}
            };
            users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();

            var numbers = new List<PhoneNumber>
            {
                new PhoneNumber{Number=142546, Type="home", IsDefault=true, UserID=users.Find(u=>u.Surname=="Alonso").UserID},
                new PhoneNumber{Number=747575, Type="mobile", IsDefault=false, UserID=users.Find(u=>u.Surname=="Alonso").UserID},
                new PhoneNumber{Number=536465, Type="home", IsDefault=false, UserID=users.Find(u=>u.Surname=="Alonso").UserID},

                new PhoneNumber{Number=243535, Type="home", IsDefault=false, UserID=users.Find(u=>u.Surname=="Sainz").UserID},
                new PhoneNumber{Number=253353, Type="mobile", IsDefault=true, UserID=users.Find(u=>u.Surname=="Sainz").UserID},
                new PhoneNumber{Number=364754, Type="mobile", IsDefault=false, UserID=users.Find(u=>u.Surname=="Sainz").UserID},

                new PhoneNumber{Number=583736, Type="mobile", IsDefault=false, UserID=users.Find(u=>u.Surname=="Brock").UserID},
                new PhoneNumber{Number=254746, Type="home", IsDefault=false, UserID=users.Find(u=>u.Surname=="Brock").UserID},
                new PhoneNumber{Number=293847, Type="home", IsDefault=true, UserID=users.Find(u=>u.Surname=="Brock").UserID}
            };
            numbers.ForEach(p => context.PhoneNumbers.Add(p));
            context.SaveChanges();
        }
    }
}