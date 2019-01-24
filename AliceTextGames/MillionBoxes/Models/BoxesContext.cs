using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MillionBoxes.Models
{
    public sealed class BoxesContext : DbContext
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

        public Box GetBox(int number)
        {
            var targetBox = Boxes.FirstOrDefault(n => n.Number == number);

            if (targetBox != null)
            {
                return targetBox;
            }

            Boxes.Add(new Box { Number = number });
            SaveChanges();
            return Boxes.FirstOrDefault(n => n.Number == number);
        }

        public void SaveToBox(int number, string message)
        {
            var box = GetBox(number);
            box.Message = message;
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

        public User GetUserById(string userId)
        {
            var targetUser = Users.FirstOrDefault(n => n.UserId == userId);

            if (targetUser != null)
            {
                return targetUser;
            }

            Users.Add(new User { UserId = userId });
            SaveChanges();
            return Users.FirstOrDefault(n => n.UserId == userId);
        }

        public void SetOpenedBoxNumber(string userId, int number)
        {
            var user = GetUserById(userId);
            user.OpenedBox = number;
            SaveChanges();
        }

        public void SetUserSaveMode(string userId, bool isSaving)
        {
            var user = GetUserById(userId);
            user.IsSaving = isSaving;
            SaveChanges();
        }

    }
}
