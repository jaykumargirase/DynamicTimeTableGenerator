using DynamicTime_TableGenerator.Models;
using Microsoft.AspNetCore.Mvc;

namespace DynamicTime_TableGenerator.Controllers
{
    public class TimeTableController : Controller
    {
        private static TimeTableModel _data;

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(TimeTableModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _data = model;
            return RedirectToAction("AddSubjects");
        }

        public IActionResult AddSubjects()
        {
            ViewBag.TotalHours = _data.TotalHours;
            return View(new List<Subject>(new Subject[_data.TotalSubjects]));
        }

        [HttpPost]
        public IActionResult AddSubjects(List<Subject> subjects)
        {
            // Check if subjects are null or empty
            if (subjects == null || subjects.Count == 0)
            {
                ViewBag.Error = "Please enter at least one subject.";
                ViewBag.TotalHours = _data.TotalHours;
                return View(subjects);
            }

            // Validate individual fields
            for (int i = 0; i < subjects.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(subjects[i].Name))
                {
                    ViewBag.Error = $"Subject {i + 1}: Name is required.";
                    ViewBag.TotalHours = _data.TotalHours;
                    return View(subjects);
                }

                if (subjects[i].Hours < 1 || subjects[i].Hours > 10)
                {
                    ViewBag.Error = $"Subject {i + 1}: Hours must be between 1 and 10.";
                    ViewBag.TotalHours = _data.TotalHours;
                    return View(subjects);
                }
            }

            // Check if total hours match
            int totalEnteredHours = subjects.Sum(s => s.Hours);
            if (totalEnteredHours != _data.TotalHours)
            {
                ViewBag.Error = $"Total subject hours must equal total hours ({_data.TotalHours}). Currently: {totalEnteredHours}";
                ViewBag.TotalHours = _data.TotalHours;
                return View(subjects);
            }

            _data.Subjects = subjects;
            return RedirectToAction("Generate");
        }


        public IActionResult Generate()
        {
            var timetable = new List<List<string>>();
            var subjectPool = new List<string>();

            foreach (var sub in _data.Subjects)
            {
                for (int i = 0; i < sub.Hours; i++)
                    subjectPool.Add(sub.Name);
            }

            var rnd = new Random();
            subjectPool = subjectPool.OrderBy(x => rnd.Next()).ToList();

            int index = 0;
            for (int i = 0; i < _data.SubjectsPerDay; i++)
            {
                var row = new List<string>();
                for (int j = 0; j < _data.WorkingDays; j++)
                {
                    row.Add(subjectPool[index++]);
                }
                timetable.Add(row);
            }

            return View(timetable);
        }
    }
}
