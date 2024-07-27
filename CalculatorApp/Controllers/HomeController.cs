using CalculatorApp.Data;
using CalculatorApp.Models;
using CalculatorApp.Logic;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;


[assembly: InternalsVisibleTo("CalculatorTests")]

namespace CalculatorController
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Calculator(CalculatorViewModel model)
        {
            var userId = User.Identity?.Name;
            if (!string.IsNullOrEmpty(userId)) 
                model.CalculatorHistories = _context.CalculatorHistory
                    .Where(e => e.UserId == userId)
                    .OrderByDescending(e => e.Id)
                    .ToList();
            
            return View(model);
        }

        [HttpPost]
        public IActionResult Calculate(CalculatorViewModel model)
        {
            var input = model.Input;
            var userId = User.Identity?.Name;
            if (model.Input != null)
            {
                try
                {
                    model.Input = CalculatorLogic.EvaluateExpression(model.Input).ToString();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Input", ex.Message);
                    if(!string.IsNullOrEmpty(userId))
                        model.CalculatorHistories = GetCalculatorHistories(userId);
                    return View("Calculator", model);
                }
            }
            ViewData["model"] = model;
            if (!string.IsNullOrEmpty(userId))
            {
                CalculatorHistory calculatorHistory = new CalculatorHistory
                {
                    UserId = userId,
                    Input = input,
                    Output = model.Input
                };
                _context.CalculatorHistory.Add(calculatorHistory);
                _context.SaveChanges();
                model.CalculatorHistories = GetCalculatorHistories(userId);
            }
            return View("Calculator", model);

        }

        private List<CalculatorHistory> GetCalculatorHistories(string userId)
        {
            return _context.CalculatorHistory
                    .Where(e => e.UserId == userId)
                    .OrderByDescending(e => e.Id)
                    .ToList();
        }
    }
}
