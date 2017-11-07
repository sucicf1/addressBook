namespace addressBook.Models
{
    public class PhoneNumber
    {
        public int PhoneNumberID { get; set; }
        public int Number { get; set; }
        public string Type { get; set; }
        public bool IsDefault { get; set; }
        public int UserID { get; set; }

        public virtual User User { get; set; }
    }
}