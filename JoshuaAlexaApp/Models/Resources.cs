using System;
using System.Collections.Generic;
using System.Text;

namespace JoshuaAlexaApp.Models
{
    public class Resources
    {
            public Resources(string language)
            {
                this.Language = language;
            }
            public string Language { get; set; }
            public string SkillName { get; set; }
            public string HelpMessage { get; set; }
            public string HelpReprompt { get; set; }
            public string StopMessage { get; set; }

    }
}
