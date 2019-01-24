using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MillionBoxes.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Key { get; set; }

        public string UserId { get; set; }
        public int OpenedBox { get; set; }
        public bool IsSaving { get; set; }

        public User()
        {
            UserId = string.Empty;
            IsSaving = false;
        }
    }
}
