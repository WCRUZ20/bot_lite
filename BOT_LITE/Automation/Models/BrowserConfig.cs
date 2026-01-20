using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOT_LITE.Automation.Models
{
    public class BrowserConfig
    {
        public string Url { get; set; }
        public bool Headless { get; set; } = false;
        public int DefaultTimeoutMs { get; set; } = 30000;
        public int SlowMoMs { get; set; } = 0;
    }
}
