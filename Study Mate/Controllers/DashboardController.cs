using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Study_Mate.Models;

namespace Study_Mate.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ActionResult> Index()
        {
            //Last 7 days
            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Today;

            List<Track> SelectedTracks = await _context.Tracks
                .Include(x => x.Subject)
                .Where(y => y.Date >= StartDate && y.Date <= EndDate)
                .ToListAsync();

            //Total pending work
            int TotalPendingWork = SelectedTracks
                .Where(i => i.Subject.Type == "Goal")
                .Sum(j => j.Amount);
            ViewBag.TotalPendingWork = TotalPendingWork.ToString("");

            //Total completed work
            int TotalCompletedWork = SelectedTracks
                .Where(i => i.Subject.Type == "Studied")
                .Sum(j => j.Amount);
            ViewBag.TotalCompletedWork = TotalCompletedWork.ToString("");

            //Balance
            int Balance = TotalPendingWork - TotalCompletedWork;
            ViewBag.Balance = Balance.ToString("");
            

            //Remainig Subjects
            List<Track> GSubj = await _context.Tracks
                .Include(x => x.Subject)
                .Where(y => y.Subject.Type == "Goal")
                .ToListAsync();
            ViewBag.GSubj = GSubj.ToList();

            //Studied Subjects
            List<Track> SSubj = await _context.Tracks
                .Include(x => x.Subject)
                .Where(y => y.Subject.Type == "Studied")
                .ToListAsync();
            ViewBag.SSubj = SSubj.ToList();

            //Doughnut chart
            ViewBag.DoughnutChartData = SelectedTracks
                .Where(i => i.Subject.Type == "Studied")
                .GroupBy(j => j.Subject.SubjectId)
                .Select(k => new
                {
                    SubjectTitleAndIcon = k.First().Subject.Icon + " " + k.First().Subject.Title,
                    amount = k.Sum(j => j.Amount),
                    formattedSession = k.Sum(j => j.Amount).ToString(""),
                })
                .OrderByDescending(l => l.amount)
                .ToList();

            //Spline chart

            //Income
            List<SplineChartData> GoalSummary = SelectedTracks
                .Where(i => i.Subject.Type == "Goal")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData()
                {
                    day = k.First().Date.ToString("dd-MMM"),
                    goal = k.Sum(l => l.Amount)
                })
                .ToList();

            //Expense
            List<SplineChartData> StuduiedSummary = SelectedTracks
                .Where(i => i.Subject.Type == "Studied")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData()
                {
                    day = k.First().Date.ToString("dd-MMM"),
                    studied = k.Sum(l => l.Amount)
                })
                .ToList();

            //Combine Income & Expense
            string[] Last7Days = Enumerable.Range(0, 7)
                .Select(i => StartDate.AddDays(i).ToString("dd-MMM"))
                .ToArray();

            ViewBag.SplineChartData = from day in Last7Days
                                      join goal in GoalSummary on day equals goal.day into dayGoalJoined
                                      from goal in dayGoalJoined.DefaultIfEmpty()
                                      join studied in StuduiedSummary on day equals studied.day into studiedJoined
                                      from studied in studiedJoined.DefaultIfEmpty()
                                      select new
                                      {
                                          day = day,
                                          goal = goal == null ? 0 : goal.goal,
                                          studied = studied == null ? 0 : studied.studied,
                                      };

            //Recent Sessions
            ViewBag.RecentSessions = await _context.Tracks
                .Include(i => i.Subject)
                .OrderByDescending(j => j.Date)
                .Take(5)
                .ToListAsync();

            return View();
        }
    }
    public class SplineChartData
    {
        public string day;
        public int goal;
        public int studied;

    }
}
