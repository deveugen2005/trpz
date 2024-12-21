using System;
using System.Windows.Forms;
using System.IO;

namespace PrintEditionsApp.Models
{
    public abstract class PrintEdition : MyObject, IHasInfo, IPrintable, IStorable
    {
        public string? Title { get; set; } = string.Empty;

        public string? Publisher { get; set; }
        public int Year { get; set; }
        public int Pages { get; set; }

        public PrintEdition(string info) : base(info) { }

        public virtual string GetInfo()
        {
            return $"Title: {Title}, Publisher: {Publisher}, Year: {Year}, Pages: {Pages}";
        }

        public virtual void Print()
        {
            Console.WriteLine(GetInfo());
        }

        public virtual void SaveToFile(string filePath)
        {
            File.AppendAllText(filePath, GetInfo() + Environment.NewLine);
        }

        public override void ShowInfoMessage()
        {
            MessageBox.Show(InfoString, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public override void PrintInfoToConsole()
        {
            Console.WriteLine(InfoString);
        }
    }
}
