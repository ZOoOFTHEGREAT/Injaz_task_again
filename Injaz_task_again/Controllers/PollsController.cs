using Injaz_task_again.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Injaz_task_again.Controllers
{
    public class PollsController : Controller
    {
            private readonly DBContext _context;

            public PollsController(DBContext context)
            {
                _context = context;
            }

        public async Task<IActionResult> Polls()
        {
            var polls = await _context.Polls.Include(p => p.Questions).ThenInclude(q => q.Answers).ToListAsync();
            return View(polls);
        }

        public async Task<IActionResult> Last()
        {
            var lastPoll = await _context.Polls
                .Include(p => p.Questions)
                .ThenInclude(q => q.Answers)
                .OrderByDescending(p => p.Id)
                .FirstOrDefaultAsync();

            if (lastPoll == null)
                return NotFound();

            return View("LastPoll", lastPoll);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AnswerPoll(int pollId, Dictionary<int, int> selectedAnswers)
        {
            foreach (var entry in selectedAnswers)
            {
                int questionId = entry.Key;
                int answerId = entry.Value;

                var answer = await _context.Answers.FirstOrDefaultAsync(a => a.Id == answerId && a.QuestionId == questionId);
                if (answer != null)
                {
                    answer.SelectionCount += 1;
                }
            }
            await _context.SaveChangesAsync();
               return Redirect("/Home/Index");
        }
        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePollForm([Bind("Id,Title")] Poll poll)
        {
            if (ModelState.IsValid)
            {
                _context.Add(poll);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Polls");
        }
        public async Task<IActionResult> DeletePoll(int? id)
        {
            var Poll = await _context.Polls.FindAsync(id);
            _context.Polls.Remove(Poll);
            await _context.SaveChangesAsync();
            return RedirectToAction("Polls");
        }
        public IActionResult AddQuestion(int? id)
        {
            if (id == null)
                return NotFound();

            ViewBag.PollId = id; 
            return View("AddQuestion"); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddQuestionForm( Question question)
        {
            if (ModelState.IsValid)
            {
                _context.Questions.Add(question);
                if (question.Answers != null && question.Answers.Any())
                {
                    foreach (var answer in question.Answers)
                    { 
                        answer.Id = 0;
                        answer.QuestionId = question.Id;
                        _context.Answers.Add(answer);
                    }
                }
                await _context.SaveChangesAsync();
            }
            ViewBag.PollId = question.PollId;
            return RedirectToAction("Polls");
        }
    }

}