using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MillionBoxes.Models
{
    public class BoxesContext : DbContext
    {
        public DbSet<Box> Boxes { get; set; }
        public DbSet<User> Users { get; set; }

        public BoxesContext(DbContextOptions<BoxesContext> options) : base(options)
        {
            Database.EnsureCreated();
            //if (Database.EnsureCreated())
            //{
            //    Database.Migrate();
            //}
        }

        public void SaveToBox(int number, string message)
        {
            var box = new Box { Number = number, Message = message };
            var targetBox = Boxes.FirstOrDefault(n => n.Number == number);

            if (targetBox != null)
            {
                targetBox.Message = message;
            }
            else
            {
                Boxes.Add(box);
            }

            SaveChanges();
        }

        public string ReadFromBox(int number)
        {
            var box = Boxes.FirstOrDefault(n => n.Number == number);

            if (box == null)
            {
                return string.Empty;
            }

            return box.Message;
        }

    }
}
