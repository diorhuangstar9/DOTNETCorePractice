using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Producer.Services;
using ServiceBus.Producer.Models;

namespace Service.Producer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagingController : ControllerBase
    {
        private readonly IMessagePublisher _messagePublisher;

        public MessagingController(IMessagePublisher messagePublisher)
        {
            _messagePublisher = messagePublisher;
        }

        [HttpPost("publish/customer")]
        public async Task<IActionResult> PublishCustomerAsync(CustomerCreated customer)
        {
            await _messagePublisher.Publish(customer);
            return Ok();
        }

        [HttpPost("publish/order")]
        public async Task<IActionResult> PublishOrderAsync(OrderCreated order)
        {
            await _messagePublisher.Publish(order);
            return Ok();
        }
    }

}

