using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace Eyedia.Aarbac.Framework
{
    public class ExecutionTime : IDisposable
    {       
        public Dictionary<string, Stopwatch> Items { get; private set; }
        public ExecutionTime()
        {
            Items = new Dictionary<string, Stopwatch>();
        }

        public bool Start(string itemName)
        {
            if (!Items.ContainsKey(itemName))
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                Items.Add(itemName, stopwatch);
                return true;
            }
            else
            {
                Items[itemName].Start();
                return false;
            }

        }

        public void Stop(string itemName)
        {
            if (Items.ContainsKey(itemName))
            {                
                Items[itemName].Stop();                
            }
            else
            {
                throw new KeyNotFoundException("The item '" + itemName + "' was not defined/started! Please Start the item first.");
            }
        }

        const string __breakline = "------------------------------------------------------------";
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();            
            if (Items.Count > 0)
            {
                sb.AppendLine(__breakline);
                var maxKey = Items.Aggregate((l, r) => l.Key.Length > r.Key.Length ? l : r).Key;
                var totalTime = Items.Sum(i => i.Value.ElapsedMilliseconds);

                foreach (KeyValuePair<string, Stopwatch> item in Items)
                {
                    sb.AppendLine(string.Format("{0}:{1}", item.Key.PadRight(maxKey.Length, ' '),
                        FormatMiliseconds(item.Value.ElapsedMilliseconds)));
                }
                sb.AppendLine(__breakline);
                sb.AppendLine(string.Format("{0}:{1}", "Total Time".PadRight(maxKey.Length, ' '),
                        FormatMiliseconds(totalTime)));
                sb.AppendLine(__breakline);
            }
            return sb.ToString();
        }

        string FormatMiliseconds(long elapsedMilliseconds)
        {
            return FormatMiliseconds(TimeSpan.FromMilliseconds(elapsedMilliseconds));
        }

            string FormatMiliseconds(TimeSpan t)
        {
            return string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                        t.Hours,
                        t.Minutes,
                        t.Seconds,
                        t.Milliseconds);
        }
        public void Dispose()
        {
            Items.Clear();
            Items = null;
            GC.SuppressFinalize(this);
        }
    }
}
