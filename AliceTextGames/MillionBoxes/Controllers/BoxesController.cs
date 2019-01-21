using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            //SaveToBox(new Random().Next(999999), "test");
            dataBase.SaveToBox(25, "Its Work!");
            
            var boxes = new List<string>();
            foreach (var box in dataBase.Boxes)
            {
                boxes.Add($"{box.Number}. {box.Message}");
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
        public void Post([FromBody]string value)
        {
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
