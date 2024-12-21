using System;

namespace PrintEditionsApp.Models
{
    public abstract class MyObject
    {
        protected string InfoString;

        public MyObject(string info)
        {
            InfoString = info;
        }

        public abstract void ShowInfoMessage();
        public abstract void PrintInfoToConsole();
    }
}
