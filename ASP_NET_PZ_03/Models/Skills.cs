using System.Reflection.Emit;

namespace ASP_NET_PZ_03.Models
{
    public class Skills
    {
        public string Name { get; set; }
        public int Level { get; set; }

        public string GetProgressBar()
        {
            const int Bars = 10;
            int CompletedBars = (int)Math.Round(this.Level / 10.0);
            int EmptyBars = Bars - CompletedBars;

            List<char> Progress = new List<char>();
            for (int i = 0; i < CompletedBars; i++)
            {
                Progress.Add('=');
            }
            for (int i = 0; i < EmptyBars; i++)
            {
                Progress.Add('#');
            }
            string Result = $"[{new string(Progress.ToArray())}] {this.Level}%";
            return Result;
        }

    }

}
