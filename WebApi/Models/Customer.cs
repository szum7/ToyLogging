namespace WebApi.Models
{
    public class Customer
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? SSN { get; set; }

        public DateTime? DOB { get; set; }

        public string? PAN { get; set; } // primary account number
    }
}