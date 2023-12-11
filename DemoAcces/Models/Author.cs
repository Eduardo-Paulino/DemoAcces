namespace DemoAcces.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Author() { }

        public Author(string firstName, string lastName) 
        {
            Id = 0;
            FirstName = firstName;
            LastName = lastName;
        }

        public Author(int id, string firstName, string lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

        internal void Add(Author author)
        {
            throw new NotImplementedException();
        }
    }
}
