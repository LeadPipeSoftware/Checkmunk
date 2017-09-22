using Checkmunk.Domain.Common;
using Checkmunk.Domain.Users.ValueObjects;
using LeadPipe.Net;
using LeadPipe.Net.Extensions;
using System.Net.Mail;

namespace Checkmunk.Domain.Users.AggregateRoots
{
    public class User : IAmPersistable
    {
        private Address billingAddress;
        private string emailAddress;
        private string firstName;
        private bool isEnabled;
        private string lastName;
        private Address mailingAddress;
        private PhoneNumber phoneNumber;

        public User(string emailAddress, string firstName, string lastName)
            : this(emailAddress)
        {
            this.firstName = firstName;
            this.lastName = lastName;
        }

        public User(string emailAddress)
        {
            Guard.Will.ProtectAgainstNullOrEmptyStringArgument(() => emailAddress);

            if (IsEmailAddressValid(emailAddress).IsFalse()) throw new InvalidEmailAddressException(emailAddress);

            this.billingAddress = new Address();
            this.mailingAddress = new Address();

            this.isEnabled = true;
            this.emailAddress = emailAddress;
        }

        protected internal User()
        {
            // For ORM use
        }

        public virtual int PersistenceId { get; set; }

        public virtual Address BillingAddress
        {
            get => billingAddress;
            protected set => billingAddress = value;
        }

        public virtual string EmailAddress
        {
            get => emailAddress;
            protected set => emailAddress = value;
        }

        public virtual string FirstName
        {
            get => firstName;
            protected set => firstName = value;
        }

        public virtual bool IsEnabled
        {
            get => isEnabled;
            protected set => isEnabled = value;
        }

        public virtual string LastName
        {
            get => lastName;
            protected set => lastName = value;
        }

        public virtual Address MailingAddress
        {
            get => mailingAddress;
            protected set => mailingAddress = value;
        }

        public virtual PhoneNumber PhoneNumber
        {
            get => phoneNumber;
            protected set => phoneNumber = value;
        }

        public static bool IsEmailAddressValid(string emailAddress)
        {
            try
            {
                var mailAddress = new MailAddress(emailAddress);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public void ChangeBillingAddress(Address billingAddress)
        {
            Guard.Will.ProtectAgainstNullArgument(() => billingAddress);

            this.billingAddress = billingAddress;
        }

        public void ChangeBillingAddressToMailingAddress()
        {
            this.billingAddress = this.mailingAddress;
        }

        public void ChangeMailingAddress(Address mailingAddress)
        {
            Guard.Will.ProtectAgainstNullArgument(() => mailingAddress);

            this.mailingAddress = mailingAddress;
        }

        public void ChangeName(string firstName, string lastName)
        {
            this.firstName = firstName;
            this.lastName = lastName;
        }

        public void ChangePhoneNumber(PhoneNumber phoneNumber)
        {
            Guard.Will.ProtectAgainstDefaultValueArgument(() => phoneNumber);

            this.phoneNumber = phoneNumber;
        }

        public void DisableUser()
        {
            this.isEnabled = false;
        }

        public void EnableUser()
        {
            this.isEnabled = true;
        }
    }
}