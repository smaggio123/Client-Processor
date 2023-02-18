namespace ClientApp.Models
{
    public class Customer
    {
        public int Client_ID { get; } = -1;
        public string FirstName { get; } = string.Empty;
        public string LastName { get; } = string.Empty;
        public string PhoneNumber { get; } = string.Empty;
        public string Email { get; } = string.Empty;


        public Customer()
        {
            
        }

        public Customer(int client_ID, string firstName, string lastName, string phonenumber, string email)
        {
            Client_ID = client_ID;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phonenumber;
            Email = email;
        }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
    }
}
