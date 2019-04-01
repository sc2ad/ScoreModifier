using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreModifier
{
    class Config
    {
        public static bool enabled { get; set; } = true;
        public static bool overwriteOriginalScore { get; set; } = false;
        public static ScoreType scoreType {get; set;} = ScoreType.Osuv1;
    }
}
