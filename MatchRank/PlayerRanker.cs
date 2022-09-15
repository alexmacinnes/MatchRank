namespace MatchRank
{
    public class PlayerRanker
    {
        private readonly List<MatchScore> MatchScores = new List<MatchScore>();    

        public void AddMatchScore(MatchScore m) => MatchScores.Add(m);

        public FinalPlayerRanking CalculatePlayerRankings(int generationCount)
        {
            var rawPerformance = GenerateRawPerformance();
            var playerNames = rawPerformance.Values.Select(x => x.Name).OrderBy(x => x).ToArray();

            var generations = new List<GenerationRanking>();
            generations.Add(GenerateGen0(playerNames));
            
            for (int i=1; i<= generationCount; i++)
            {
                var nextGen = GenerateNextGen(i, generations.Last());
                generations.Add(nextGen);
            }

            var finalPlayerRatings = CreateFinalRatings(rawPerformance, generations.Last());

            var result = new FinalPlayerRanking(generations, finalPlayerRatings);

            return result;
        }

        private IReadOnlyList<FullPlayerPerformance> CreateFinalRatings(Dictionary<string, RawPlayerPerformance> rawPerformance, GenerationRanking generationRanking)
        {
            var unsorted = from r in rawPerformance
                         let playerName = r.Key
                         let raw = r.Value
                         let performance = generationRanking.PlayerRatings[r.Key]
                         select new FullPlayerPerformance(playerName, raw, performance.Rating, performance.MatchScores.ToList());

            var sorted = unsorted
                .OrderByDescending(x => x.Rating)
                .ThenByDescending(x => x.RawPlayerPerformance.WinRatio)
                .ThenByDescending(x => x.RawPlayerPerformance.PointsDifference)
                .ThenByDescending(x => x.RawPlayerPerformance.MatchesPlayed)
                .ToArray();

            return sorted;
        }

        private GenerationRanking GenerateNextGen(int generationIndex, GenerationRanking previousGenerationRanking)
        {
            var playerPerformances = new List<PlayerMatchPerformance>();

            foreach (var m in MatchScores)
            {
                var matchResults = AnalyseMatch(m, previousGenerationRanking);
                playerPerformances.AddRange(matchResults);
            }

            var averagePerformanceByPlayer = playerPerformances
                .GroupBy(x => x.PlayerName)
                .Select(g => new PlayerMatchPerformance(
                    g.Key, 
                    g.Select(x => x.Rating).Average(),
                    g.SelectMany(x => x.MatchScores).ToArray())
                ).ToArray();

            var normalisedPlayerRatings = NormalisePlayerRatings(averagePerformanceByPlayer);

            var nextGen = new GenerationRanking(generationIndex, normalisedPlayerRatings);

            return nextGen;
        }

        private PlayerMatchPerformance[] NormalisePlayerRatings(PlayerMatchPerformance[] playerRatings)
        {
            double averageRatingOfAllPlayers = playerRatings.Select(x => x.Rating).Average();
            var result = playerRatings.Select(x => new PlayerMatchPerformance(
                x.PlayerName, 
                x.Rating / averageRatingOfAllPlayers,
                x.MatchScores.ToList())
            ).ToArray();
            return result;
        }

        private IEnumerable<PlayerMatchPerformance> AnalyseMatch(MatchScore matchScore, GenerationRanking previousGenerationRanking)
        {
            double totalPoints = matchScore.TeamAScore + matchScore.TeamBScore;

            double teamARating = matchScore.TeamAPlayers.Select(x => previousGenerationRanking.PlayerRatings[x].Rating).Average();
            double teamBRating = matchScore.TeamBPlayers.Select(x => previousGenerationRanking.PlayerRatings[x].Rating).Average();
            double totalRating = teamARating + teamBRating;

            double teamAExpectedPoints = totalPoints * teamARating / totalRating;
            double teamBExpectedPoints = totalPoints * teamBRating / totalRating;

            double teamAPerformance = matchScore.TeamAScore / teamAExpectedPoints;
            double teamBPerformance = matchScore.TeamBScore / teamBExpectedPoints;
            var ratedMatchScore = new RatedMatchScore(matchScore, teamAPerformance, teamBPerformance);

            foreach (var a in matchScore.TeamAPlayers)
                yield return new PlayerMatchPerformance(a, teamAPerformance, ratedMatchScore);

            foreach (var b in matchScore.TeamBPlayers)
                yield return new PlayerMatchPerformance(b, teamBPerformance, ratedMatchScore);
        }

        private GenerationRanking GenerateGen0(string[] playerNames)
        {
            double defaultPlayerRating = 1.0;
            var playerRatings = playerNames.Select(x => new PlayerMatchPerformance(x, defaultPlayerRating, null as RatedMatchScore)).ToArray();
            var generation = new GenerationRanking(0, playerRatings);
            return generation;
        }

        private Dictionary<string, RawPlayerPerformance> GenerateRawPerformance()
        {
            var result = new Dictionary<string, RawPlayerPerformance>();

            foreach (var m in MatchScores)
            {
                foreach (var a in m.TeamAPlayers)
                {
                    var raw = result.CreateOrAdd(a, () => new RawPlayerPerformance(a));
                    raw.MatchesPlayed++;
                    raw.PointsFor += m.TeamAScore;
                    raw.PointsAgainst += m.TeamBScore;
                }

                foreach (var b in m.TeamBPlayers)
                {
                    var raw = result.CreateOrAdd(b, () => new RawPlayerPerformance(b));
                    raw.MatchesPlayed++;
                    raw.PointsFor += m.TeamBScore;
                    raw.PointsAgainst += m.TeamAScore;
                }
            }

            return result;
        }
    }
}