using System.Collections.Generic;

namespace TestTask.Models
{
    public class CalendarDaysModel
    {
        public List<string> WeekDaysNames { get; set; }
        public List<DayModel> Days { get; set; }

        public CalendarDaysModel(List<string> names, List<DayModel> days)
        {
            WeekDaysNames = names;
            Days = days;
        }
    }
}
