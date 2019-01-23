using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AliceApi;
using AliceApi.Modules;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MillionBoxes.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MillionBoxes.Controllers
{
    [Route("api/[controller]")]
    public class BoxesController : Controller
    {
        public BoxesContext dataBase;

        public BoxesController(BoxesContext context)
        {
            dataBase = context;
        }
        
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            //dataBase.SaveToBox(25, "It Works!");
            dataBase.SaveToBox(new Random().Next(99), "It Works!");

            var boxes = new List<string>();
            foreach (var box in dataBase.Boxes)
            {
                boxes.Add($"{box.Number}. {box.Message} - {DateTime.Now}");
            }

            return boxes;
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return $"id: {id} {dataBase.ReadFromBox(id)}" ;
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
            else if (EntitiesConvert.TryParseInt(aliceRequest.request.nlu.entities, out int value) && text.Contains("открой"))
            {
                aliceResponse.response.text = $"В коробке номер {value} лежит сообщение: {dataBase.ReadFromBox(value)}";
                dataBase.SaveOpenedBoxNumber(aliceRequest.session.user_id, value);
            }
            else if (text.Contains("сохрани") && dataBase.Users.Any(n => n.UserId == aliceRequest.session.user_id) && dataBase.Users.FirstOrDefault(n=> n.UserId == aliceRequest.session.user_id)?.OpenedBox !=0)
            {
                aliceResponse.response.text = $"Сохраняю сообщение в коробку номер { dataBase.Users.FirstOrDefault(n => n.UserId == aliceRequest.session.user_id)?.OpenedBox}. Что написать?";
                dataBase.Users.FirstOrDefault(n => n.UserId == aliceRequest.session.user_id).IsSaving = true;

            }
            else if (dataBase.Users.FirstOrDefault(n => n.UserId == aliceRequest.session.user_id).IsSaving)
            {

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
