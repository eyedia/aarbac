#region Copyright Notice
/* Copyright (c) 2017, Deb'jyoti Das - debjyoti@debjyoti.com
 All rights reserved.

 Redistribution and use in source and binary forms, with or without
 modification, are not permitted.Neither the name of the 
 'Deb'jyoti Das' nor the names of its contributors may be used 
 to endorse or promote products derived from this software without 
 specific prior written permission.

 THIS SOFTWARE IS PROVIDED BY Deb'jyoti Das 'AS IS' AND ANY
 EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 DISCLAIMED. IN NO EVENT SHALL Synechron Holdings Inc BE LIABLE FOR ANY
 DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */
#region Developer Information
/*
Author  - Debjyoti Das (debjyoti@debjyoti.com)
Created - 11/12/2017 3:31:31 PM
Description  - 
Modified By - 
Description  - 
*/
#endregion Developer Information

#endregion Copyright Notice
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

