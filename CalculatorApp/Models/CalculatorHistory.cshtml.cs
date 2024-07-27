using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CalculatorApp.Data;

namespace CalculatorApp.Models
{
    public class CalculatorHistoryModel : PageModel
    {
        private readonly CalculatorApp.Data.ApplicationDbContext _context;

        public CalculatorHistoryModel(CalculatorApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<CalculatorHistory> CalculatorHistory { get;set; } = default!;

        public async Task OnGetAsync()
        {
            CalculatorHistory = await _context.CalculatorHistory.ToListAsync();
        }
    }
}
