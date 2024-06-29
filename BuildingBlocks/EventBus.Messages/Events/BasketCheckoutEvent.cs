using System.ComponentModel.DataAnnotations;

namespace EventBus.Messages.Events
{
    public class BasketCheckoutEvent : IntegrationBaseEvent
    {
        public string UserName { get; set; }

        public decimal TotalPrice { get; set; }

        //address

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [EmailAddress]
        public string EmailAddress { get; set; }

        public string Country { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }

        //payment

        public string BankName { get; set; }
        public string RefCode { get; set; }

        public byte PaymentMethod { get; set; }
    }
}
