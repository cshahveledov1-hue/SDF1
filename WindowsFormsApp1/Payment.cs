namespace WindowsFormsApp1
{
    public class Payment
    {
        public string Title { get; set; }
        public double Amount { get; set; }

        public Payment(string title, double amount)
        {
            Title = title;
            Amount = amount;
        }

        public override string ToString()
        {
            return $"{Title} - {Amount:C2}";
        }
    }
}