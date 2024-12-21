namespace PrintEditionsApp.Models
{
    public class Journal : PrintEdition
    {
        public string? Periodicity { get; set; } // "Monthly", "Weekly" тощо
        public string? Topic { get; set; }
        public int IssueNumber { get; set; }
        public string? Language { get; set; }

        public Journal(string info) : base(info) { }

        public override string GetInfo()
        {
            return base.GetInfo() + $", Periodicity: {Periodicity}, Topic: {Topic}, Issue: {IssueNumber}, Language: {Language}";
        }

        public override void ShowInfoMessage()
        {
            // Перевизначимо для демонстрації
            System.Windows.Forms.MessageBox.Show("Journal Info: " + GetInfo(), "Journal Info", System.Windows.Forms.MessageBoxButtons.OK);
        }
    }
}
