using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace addressBook.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString =
            "{0:dd.MM.yyyy.}",ApplyFormatInEditMode =false)]
        public DateTime BirthDay { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }

        public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; }
    }
}