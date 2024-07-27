namespace CalculatorApp.Models
{
    public class CalculatorViewModel
    {
        public string Input { get; set; } = "";
        public List<CalculatorHistory> CalculatorHistories { get; set; } = new List<CalculatorHistory>();
    }
}
