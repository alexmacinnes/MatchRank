using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MatchRank.Test
{
    public class PlayerRanker_Tests
    {
        [Test]
        public void Test1()
        {
            RunSim(0, 1000);
        }


        public void RunSim(int randomSeed, int generationCount)
        {
            var random = new Random(randomSeed);

            var players = new Dictionary<string, double>
            {
                { "Abigail", 1.40 },
                { "Ben", 1.35 },
                { "Carol", 1.30 },
                { "Dennis", 1.25 },

                { "Evelyn", 1.20 },
                { "Freddie", 1.15 },
                { "Gigi", 1.10 },
                { "Harold", 1.05 },

                { "Izzie", 0.95 },
                { "Jon", 0.90 },
                { "Kathy", 0.85 },
                { "Lennie", 0.80 },

                { "Maggie", 0.75 },
                { "Noah", 0.70 },
                { "Olivia", 0.65 },
                { "Peter", 0.60 }
            };

            var matchResults = GenerateRandomMatchResults(random, 5, players);

            var ranker = new PlayerRanker();
            foreach (var x in matchResults)                
                ranker.AddMatchScore(x);

            var result = ranker.CalculatePlayerRankings(generationCount);

            Assert.NotNull(result);
        }

        private IEnumerable<MatchScore> GenerateRandomMatchResults(Random random, int roundCount, Dictionary<string, double> players)
        {
            for (int i=0; i<roundCount; i++)
            {
                var playerNames = players.Select(x => x.Key).ToList();

                // 4 matches in a round
                for (int j=0; j<4; j++)
                {
                    string[] teamAPlayers = PickNPlayersFromList(random, playerNames, 2);
                    string[] teamBPlayers = PickNPlayersFromList(random, playerNames, 2);

                    int[] teamARatings = teamAPlayers.Select(x => (int)(players[x] * 1000)).ToArray();
                    int teamARating = (int) teamARatings.Average();

                    int[] teamBRatings = teamBPlayers.Select(x => (int)(players[x] * 1000)).ToArray();
                    int teamBRating = (int)teamBRatings.Average();

                    CreateRandomMatchScore(random, teamARating, teamBRating, out int teamAScore, out int teamBScore);

                    yield return new MatchScore(teamAPlayers, teamBPlayers, teamAScore, teamBScore);
                }
            }
        }

        private void CreateRandomMatchScore(Random random, int teamARating, int teamBRating, out int teamAScore, out int teamBScore)
        {
            teamAScore = 0;
            teamBScore = 0;

            int totalPoints = 20 + random.Next(11);         // each match is between 20 and 30 points
            int totalRating = teamARating + teamBRating;

            for (int i = 0; i < totalPoints; i++)
            {
                int rnd = random.Next(totalRating);
                if (rnd < teamARating)
                    teamAScore++;
                else
                    teamBScore++;
            }
        }

        private string[] PickNPlayersFromList(Random random, List<string> playerNames, int playerCount)
        {
            string[] result = new string[playerCount];
            for (int i=0; i<playerCount; i++)
            {
                int randomIndex = random.Next(playerNames.Count);
                result[i] = playerNames[randomIndex];
                playerNames.RemoveAt(randomIndex);
            }

            return result;
        }

        [Test]
        public void SampleTournamentTest()
        {
            string scoresText = ResourceHelper.ReadEmbeddedResource("SampleTournament.txt");
            var matchScores = scoresText
                .Split(Environment.NewLine)
                .Select(s => s.Trim())
                .Where(s => s.Length > 0)
                .Select(TestLineToMatchScore)
                .ToArray();

            var ranker = new PlayerRanker();
            foreach (var x in matchScores)
                ranker.AddMatchScore(x);

            var rankings = ranker.CalculatePlayerRankings(100);

            string simpleRanking = RankReport.GenerateSimpleReport(matchScores);

            string detailedRanking = RankReport.GenerateDetailedRankingReport(rankings);

            string generationReport = RankReport.PlayerGenerationReport(rankings);

            Assert.IsNotNull(rankings);
        }

        private MatchScore TestLineToMatchScore(string line)
        {
            string normalised = Regex.Replace(line, @"\s+", " ").Trim();
            string[] tokens = normalised.Split(' ');
            string[] leftSubTokens = tokens[0].Split(',');
            string[] rightSubTokens = tokens[1].Split(',');

            string[] leftPlayers = leftSubTokens.Take(leftSubTokens.Length - 1).ToArray();
            int leftScore = int.Parse(leftSubTokens.Last());

            string[] rightPlayers = rightSubTokens.Take(rightSubTokens.Length - 1).ToArray();
            int rightScore = int.Parse(rightSubTokens.Last());

            var result = new MatchScore(leftPlayers, rightPlayers, leftScore, rightScore);
            return result;
        }
    }
}