namespace MatchRank
{
    public class MatchScore
    {
        public IReadOnlyList<string> TeamAPlayers { get; }

        public IReadOnlyList<string> TeamBPlayers { get; }

        public int TeamAScore { get; }

        public int TeamBScore { get; }

        public MatchScore(IList<string> teamAPlayers, IList<string> teamBPlayers, int teamAScore, int teamBScore)
        {
            TeamAPlayers = new List<string>(teamAPlayers);
            TeamBPlayers = new List<string>(teamBPlayers);
            TeamAScore = teamAScore;
            TeamBScore = teamBScore;
        }
    }
}