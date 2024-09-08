using CodeNet.RabbitMQ.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodeNet_App2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueueController(RabbitMQConsumerService consumerService) : ControllerBase
    {
        [HttpDelete]
        public IActionResult StopListening()
        {
            consumerService.StopListening();
            return Ok();
        }

        [HttpPost]
        public IActionResult StartListening()
        {
            consumerService.StartListening();
            return Ok();
        }
    }
}
