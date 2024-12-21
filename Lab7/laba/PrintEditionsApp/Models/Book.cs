namespace PrintEditionsApp.Models
{
    public class Book : PrintEdition
    {
        public string? Author { get; set; }

        public string? Genre { get; set; }

        public string? ISBN { get; set; }

        public int PrintRun { get; set; } 

        public Book(string info) : base(info) { }

        public override string GetInfo()
        {
            return base.GetInfo() + $", Author: {Author}, Genre: {Genre}, ISBN: {ISBN}, PrintRun: {PrintRun}";
        }
    }
}
