using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Settings
{
    public class TimeSettings
    {
        public bool? Enabled { get; set; }
        public float? currentTime { get; set; }
        public float? dayLength { get; set; }
        public bool? pause { get; set; }
        public float? dynamicScale { get; set; } //I added this because when a player joins, i noticed that 
                                                 //the 'scale' or size of the gameobject isnt set to the MC's,
                                                 //this scale determines the orbiting arc of the moon.
    }
}
