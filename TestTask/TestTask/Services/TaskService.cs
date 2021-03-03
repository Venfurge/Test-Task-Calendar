using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTask.Entities;
using TestTask.Mappers;
using TestTask.Models;

namespace TestTask.Services
{
    public class TaskService
    {
        ApplicationContext _db;

        public TaskService(ApplicationContext db)
        {
            _db = db;
        }

        public async Task<List<TaskModel>> GetTasks(DateTime date)
        {
            var tasks = _db.Tasks.Where(v => v.Day.Date == date);

            var result = await tasks.Select(v => TaskMapper.Map(v)).ToListAsync();

            return result;
        }

        public async Task AddTask(DateTime date, string text, DateTime time)
        {
            var day = await _db.Days
                .FirstOrDefaultAsync(v => v.Date == date);

            //If there is no day for the task, creating it
            if (day == null)
            {
                day = new DayEntity() { Date = date };
                await _db.Days.AddAsync(day);
            }

            //Adding task for the day
            await _db.Tasks.AddAsync(new TaskEntity()
            {
                Text = text,
                Time = time.ToString("HH:mm"),
                Day = day
            });

            await _db.SaveChangesAsync();
        }

        public async Task DeleteTask(int id)
        {
            //Find task by id
            var task = await _db.Tasks
                .FindAsync(id);

            //Remove it
            _db.Tasks.Remove(task);
            await _db.SaveChangesAsync();

            //Taking tasks count of the day where we delete task
            var tasksCount = await _db.Tasks
                .CountAsync(v => v.DayId == task.DayId);

            //If there is no tasks, delete it
            if (tasksCount == 0)
            {
                var day = await _db.Days
                    .FindAsync(task.DayId);

                _db.Days.Remove(day);
            }

            await _db.SaveChangesAsync();
        }
    }
}
