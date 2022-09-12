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

        public IReadOnlyList<MatchScore> MatchScores { get; }

        public PlayerMatchPerformance(string playerName, double rating, MatchScore matchScore)
            : this(playerName, rating, new [] { matchScore }) { }

        public PlayerMatchPerformance(string playerName, double rating, IList<MatchScore> matchScores)
        {
            PlayerName = playerName;
            Rating = rating;
            MatchScores = new List<MatchScore>(matchScores);
        }
    }
}
