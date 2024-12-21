namespace PrintEditionsApp.Models
{
    public class Textbook : Book
    {
        public string? Subject { get; set; }
        public int CourseNumber { get; set; }
        public string? Institution { get; set; }
        public string? Language { get; set; }

        public Textbook(string info) : base(info) { }

        public override string GetInfo()
        {
            return base.GetInfo() + $", Subject: {Subject}, CourseNumber: {CourseNumber}, Institution: {Institution}, TextLanguage: {Language}";
        }
    }
}
