using System;
using System.ComponentModel.DataAnnotations;

namespace TestTask.Entities
{
    public class TaskEntity
    {
        public int Id { get; set; }
        public string Text { get; set; }

        [Required]
        public string Time { get; set; }

        public int DayId { get; set; }
        public DayEntity Day { get; set; }
    }
}
