namespace Checkmunk.Contracts.Users.V1.Models
{
    public class UserModel
    {
        public AddressModel BillingAddress { get; set; }

        public string EmailAddress { get; set; }

        public string FirstName { get; set; }

        public bool IsEnabled { get; set; }

        public string LastName { get; set; }

        public AddressModel MailingAddress { get; set; }

        public PhoneNumberModel PhoneNumber { get; set; }
    }
}
