using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchRank
{
    public class PlayerMatchPerformance
    {
        public string PlayerName { get; }

        public double Rating { get; }

        public IReadOnlyList<RatedMatchScore> MatchScores { get; }

        public PlayerMatchPerformance(string playerName, double rating, RatedMatchScore matchScore)
            : this(playerName, rating, new [] { matchScore }) { }

        public PlayerMatchPerformance(string playerName, double rating, IList<RatedMatchScore> matchScores)
        {
            PlayerName = playerName;
            Rating = rating;
            MatchScores = new List<RatedMatchScore>(matchScores);
        }
    }
}
