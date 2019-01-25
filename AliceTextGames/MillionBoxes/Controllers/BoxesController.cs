using System.Collections.Generic;
using System.Linq;
using AliceApi;
using Microsoft.AspNetCore.Mvc;
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
            var boxes = new List<string> { $"Количество записей - {dataBase.Boxes.Count()}" };
            boxes.AddRange(dataBase.Boxes.OrderBy(n => n.Number).Select(n => $"{n.Number} - {n.Message}"));

            return boxes;
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return $"ID: \"{id}\" - MESSAGE: \"{dataBase.ReadFromBox(id)}\"";
        }

        // POST api/<controller>
        [HttpPost]
        public AliceResponse Post([FromBody]AliceRequest aliceRequest)
        {
            return RequestToResponse.MakeResponse(aliceRequest, dataBase);
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
