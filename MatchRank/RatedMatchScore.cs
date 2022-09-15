using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchRank
{
    public class RatedMatchScore : MatchScore
    {
        public double TeamARating { get; }

        public double TeamBRating { get; }

        public RatedMatchScore(IList<string> teamAPlayers, IList<string> teamBPlayers, int teamAScore, int teamBScore, double teamARating, double teamBRating) 
            : base(teamAPlayers, teamBPlayers, teamAScore, teamBScore)
        {
            TeamARating = teamARating;
            TeamBRating = teamBRating;
        }

        public RatedMatchScore(MatchScore matchScore, double teamARating, double teamBRating)
            : this(matchScore.TeamAPlayers.ToList(), matchScore.TeamBPlayers.ToList(), matchScore.TeamAScore, matchScore.TeamBScore, teamARating, teamBRating)
        {
        }
    }
}
