using TestTask.Entities;
using TestTask.Models;

namespace TestTask.Mappers
{
    public static class DayMapper
    {
        public static DayModel Map(DayEntity entity)
        {
            return new DayModel()
            {
                Date = entity.Date,
                TasksCount = entity.Tasks.Count,
            };
        }
    }
}
