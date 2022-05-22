using System;
namespace ServiceBus.Producer.Models
{
    public class CustomerCreated
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class OrderCreated
    {
        public string OrderId { get; set; }
    }
}

