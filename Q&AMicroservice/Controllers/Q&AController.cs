using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Q_AMicroservice.Entities;
using RabbitMQProcessor;

namespace Q_AMicroservice.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Q_AController : ControllerBase
    {
        private readonly IGenericMessageProducer _rabbitMQProducer;
        public Q_AController(IGenericMessageProducer rabbitMQProducer)
        {
            _rabbitMQProducer = rabbitMQProducer;
        }
        [HttpPost("AskAQuestion")]
        public async Task<ActionResult> AskQuestion(Question question)
        {
            _rabbitMQProducer.SendingMessage(question, "questionQueue");
            return Ok();
        }
    }
}
