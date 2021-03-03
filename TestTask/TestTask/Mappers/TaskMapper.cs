using TestTask.Entities;
using TestTask.Models;

namespace TestTask.Mappers
{
    public static class TaskMapper
    {
        public static TaskModel Map(TaskEntity entity)
        {
            return new TaskModel()
            {
                Id = entity.Id,
                Text = entity.Text,
                Time = entity.Time,
            };
        }
    }
}
