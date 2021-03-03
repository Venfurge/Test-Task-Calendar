using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using TestTask.Exceptions;
using TestTask.Models;
using TestTask.Services;

namespace TestTask.Controllers
{
    public class HomeController : Controller
    {
        private DayService _dayService;
        private TaskService _taskService;

        public HomeController(DayService dayService, TaskService taskService)
        {
            _dayService = dayService;
            _taskService = taskService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Days(DateTime? date = null)
        {
            try
            {
                if (!date.HasValue)
                    date = DateTime.Now;

                ViewData["CurrentDate"] = date.Value;
                ViewData["PreviousDate"] = date.Value.AddMonths(-1);
                ViewData["NextDate"] = date.Value.AddMonths(1);

                var days = await _dayService.GetDays(date.Value.Month, date.Value.Year);

                //Taking first 7 days names
                var weekDaysNames = days.Take(7).Select(v => v.Date.DayOfWeek.ToString()).ToList();

                //Mapping days and days names to one model
                CalendarDaysModel calendar = new CalendarDaysModel(weekDaysNames, days);

                return View(calendar);
            }
            catch (BadRequestException e)
            {
                return BadRequest(e.ToString());
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Tasks(DateTime date)
        {
            ViewData["Date"] = date.ToString("yyyy-MM-dd");

            return View(await _taskService.GetTasks(date));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> AddTask(DateTime date, string text, DateTime time)
        {
            try
            {
                await _taskService.AddTask(date, text, time);
                return RedirectPermanent($"~/Home/Tasks?date={date.ToString("yyyy-MM-dd")}");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> RemoveTask(DateTime date, int id)
        {
            try
            {
                await _taskService.DeleteTask(id);
                return RedirectPermanent($"~/Home/Tasks?date={date.ToString("yyyy-MM-dd")}");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
