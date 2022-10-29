using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ArcadeBot.Infrastructure.Entities
{
    public class ServerConfigOverride
    {
        [Key]
        public string Key { get; set; }
        public virtual Server Server { get; set; }
        public string ConfigKey { get; set; }
        public string OverrideValue { get; set; }
        public bool Active { get; set; }
    }
}
