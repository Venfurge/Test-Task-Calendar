using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestTask.Entities
{
    public class DayEntity
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public List<TaskEntity> Tasks { get; set; }
    }
}
