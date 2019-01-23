﻿using System;
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
