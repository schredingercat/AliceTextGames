using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliceApi;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AliceController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public AliceResponse Post([FromBody]AliceRequest aliceRequest)
        {
            var session = new SessionResponse()
            {
                session_id = aliceRequest.session.session_id,
                message_id = aliceRequest.session.message_id,
                user_id = aliceRequest.session.user_id
            };
            var aliceResponse = new AliceResponse { session = session };

            var text = aliceRequest.request.original_utterance.ToLower();

            if (aliceRequest.session.New)
            {
                aliceResponse.response.text = "Привет!";
            }
            else if (text == "справка" || text == "помощь")
            {
                aliceResponse.response.text = "Вот тебе справка";
            }
            else
            {
                aliceResponse.response.text = $"Все говорят {text}, а ты купи слона";
            }
            
            aliceResponse.response.tts = aliceResponse.response.text;
            aliceResponse.response.end_session = false;

            return aliceResponse;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
