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
        public static Func<ScoreController, int, NoteData, NoteCutInfo, int> customScoreFunc = (ScoreController sc, int score, NoteData data, NoteCutInfo info) =>
        {
            // Example for accessing ScoreController, although eventually will convert to using HTTPStatus
            int maxCombo = (int) typeof(ScoreController).GetField("_maxCombo").GetValue(sc);
            // Uses Plugin.Misses count for now
            int misses = Plugin.Misses;
            // If you miss even a single note, you no longer get any points for the entire song. Otherwise, you get 1 point per success.
            return misses == 0 ? 1 : 0;
        };
    }
}
