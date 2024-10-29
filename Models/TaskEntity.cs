using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace TaskMIS.Models
{
    public class TaskEntity
    {

        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        [MaxLength(30)]
        [DisplayName("Task Name")]
        public string Title { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Due Date")]
        public string Date { get; set; }

        public bool Status { get; set; }

        public User User { get; set; }

    }
}

