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

            //Taking all days of the month and mapping them to DayModel
            var emptyDaysList = Enumerable.Range(1, DateTime.DaysInMonth(year, month))
               .Select(day =>
               {
                   return new DayModel()
                   {
                       Date = new DateTime(year, month, day),
                       TasksCount = null,
                   };
               }).ToList();

            //Changing empty days from list with days with tasks
            var result = emptyDaysList.Where(v => !daysList.Any(x => x.Date == v.Date))
                .Union(daysList).OrderBy(v => v.Date).ToList();

            return result;
        }
    }
}
