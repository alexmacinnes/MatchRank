using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchRank
{
    public class GenerationRanking
    {
        public int Generation { get; }

        public IReadOnlyDictionary<string, PlayerMatchPerformance> PlayerRatings { get; }

        public GenerationRanking(int generation, IEnumerable<PlayerMatchPerformance> playerRatings)
        {
            Generation = generation;

            //var ratings = new Dictionary<string, double>();
            //foreach (var x in playerRatings)
            //    ratings.Add(x.PlayerName, x.Rating);
            //PlayerRatings = ratings;
            PlayerRatings = playerRatings.ToDictionary(x => x.PlayerName, x => x);
        }
    }
}
