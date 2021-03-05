using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTask.Exceptions;
using TestTask.Mappers;
using TestTask.Models;

namespace TestTask.Services
{
    public class DayService
    {
        ApplicationContext _db;

        public DayService(ApplicationContext db)
        {
            _db = db;
        }

        public async Task<List<DayModel>> GetDays(int month, int year)
        {
            //Check  if date is correct
            if (month < 1 || month > 12 || year < 1 || year > 9999)
                throw new BadRequestException("month or year are uncorrected");

            var list = _db.Days
                .Include(v => v.Tasks)
                .Where(v => v.Date.Month == month && v.Date.Year == year)
                .AsNoTracking();

            var daysList = await list.Select(v => DayMapper.Map(v))
                .ToListAsync();

            var emptyDaysList = Enumerable.Range(1, DateTime.DaysInMonth(year, month))
                .Select(day => new DateTime(year, month, day)).ToList();

            //First day of current month
            var firstDay = emptyDaysList[0];

            //Days of other monthes for full calendar
            var eitherDays = new List<DateTime>();

            //Taking last days of the previous month
            for (int i = 1; i < (int)firstDay.DayOfWeek; i++)
            {
                eitherDays.Add(firstDay.AddDays(-i));
            }

            int previousMonthDays = eitherDays.Count;

            //Taking first days of the next month
            for (int i = 0; i < 42 - emptyDaysList.Count - previousMonthDays; i++)
            {
                eitherDays.Add(firstDay.AddMonths(1).AddDays(i));
            }

            var calendarDays = emptyDaysList.Concat(eitherDays)
                .Select(day =>
                {
                    return new DayModel()
                    {
                        Date = day,
                        TasksCount = null,
                    };
                }).ToList();

            //Changing empty days from list with days with tasks
            var result = calendarDays.Where(v => !daysList.Any(x => x.Date == v.Date))
                .Union(daysList).OrderBy(v => v.Date).ToList();

            return result;
        }
    }
}
