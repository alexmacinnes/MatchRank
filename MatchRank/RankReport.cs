using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchRank
{
    public class RankReport
    {
        private class SimplePerformance
        {
            public string Name { get; set; }
            public int Matches { get; set; }
            public int PointsWon { get; set; }
            public int PointsLost { get; set; }
            public int PointsPlayed => PointsWon + PointsLost;
            public int PointsDiff => PointsWon - PointsLost;
            public double WinPercentage => ((double)PointsWon) / PointsPlayed;

        }
        public static string GenerateSimpleReport(IList<MatchScore> scores)
        {
            var allPlayers = scores
                .SelectMany(x => x.TeamAPlayers.Union(x.TeamBPlayers))
                .Distinct()
                .ToDictionary(x => x, x => new SimplePerformance() { Name = x });

            foreach (var s in scores)
            {
                var teamAPerformances = s.TeamAPlayers.Select(x => allPlayers[x]);
                var teamBPerformances = s.TeamBPlayers.Select(x => allPlayers[x]);

                foreach (var x in teamAPerformances)
                {
                    x.Matches++;
                    x.PointsWon += s.TeamAScore;
                    x.PointsLost += s.TeamBScore;
                }

                foreach (var x in teamBPerformances)
                {
                    x.Matches++;
                    x.PointsWon += s.TeamBScore;
                    x.PointsLost += s.TeamAScore;
                }
            }

            var allPerformances = allPlayers.Values.ToArray();
            var ranked = allPerformances
                .OrderByDescending(x => x.PointsWon)
                .ThenByDescending(x => x.WinPercentage)
                .ToArray();

            var sb = new StringBuilder();
            sb.AppendLine("Name            Match  Total    Won   Lost   Diff    Win%");
            sb.AppendLine("--------------  -----  -----  -----  -----  -----  ------");
            foreach (var x in ranked)
            {
                string name = x.Name.PadRight(14);
                string matches = x.Matches.ToString().PadLeft(5);
                string total = x.PointsPlayed.ToString().PadLeft(5);
                string won = x.PointsWon.ToString().PadLeft(5);
                string lost = x.PointsLost.ToString().PadLeft(5);
                string diff = x.PointsDiff.ToString().PadLeft(5);
                string percent = x.WinPercentage.ToString("P2").PadLeft(6);

                string line = $"{name}  {matches}  {total}  {won}  {lost}  {diff}  {percent}";
                sb.AppendLine(line);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GenerateDetailedRankingReport(FinalPlayerRanking rankings)
        {
            var ratingsByPlayer = rankings.FinalPlayerRatings.ToDictionary(
                x => x.Name,
                x => x.Rating);

            var sb = new StringBuilder();
            sb.AppendLine("Name             Rating  Match  Total    Won   Lost   Diff    Win%");
            sb.AppendLine("--------------  -------  -----  -----  -----  -----  -----  ------");
            foreach (var r in rankings.FinalPlayerRatings)
            {
                string name = r.Name.PadRight(14);
                string rating = (r.Rating * 1000).ToString("F2").PadLeft(7);
                string matches = r.RawPlayerPerformance.MatchesPlayed.ToString().PadLeft(5);
                string total = (r.RawPlayerPerformance.PointsFor + r.RawPlayerPerformance.PointsAgainst).ToString().PadLeft(5);
                string won = r.RawPlayerPerformance.PointsFor.ToString().PadLeft(5);
                string lost = r.RawPlayerPerformance.PointsAgainst.ToString().PadLeft(5);
                string diff = r.RawPlayerPerformance.PointsDifference.ToString().PadLeft(5);
                string percent = r.RawPlayerPerformance.WinRatio.ToString("P2").PadLeft(6);

                string line = $"{name}  {rating}  {matches}  {total}  {won}  {lost}  {diff}  {percent}";
                sb.AppendLine(line);

                foreach (var m in r.MatchScores)
                {
                    AppendMatchDetails(m, r.Name, ratingsByPlayer, sb);
                }
                sb.AppendLine();
            }

            return sb.ToString().Trim();
        }

        private static void AppendMatchDetails(MatchScore m, string playerName, Dictionary<string, double> ratingsByPlayer, StringBuilder sb)
        {
            List<string> leftPlayers;
            double leftScore;
            List<string> rightPlayers;
            double rightScore;
            double matchRating = 0;

            if (m.TeamAPlayers.Contains(playerName))
            {
                leftPlayers = m.TeamAPlayers.ToList();
                leftScore = m.TeamAScore;
                rightPlayers = m.TeamBPlayers.ToList();
                rightScore = m.TeamBScore;
            }
            else
            {
                leftPlayers = m.TeamBPlayers.ToList();
                leftScore = m.TeamBScore;
                rightPlayers = m.TeamAPlayers.ToList();
                rightScore = m.TeamAScore;
            }

            leftPlayers.Remove(playerName);
            leftPlayers.Insert(0, playerName);

            string leftText = string.Join(", ", leftPlayers.Select(x => $"{x}({1000.0 * ratingsByPlayer[x]:F0})")).PadLeft(35);
            string rightText = string.Join(", ", rightPlayers.Select(x => $"{x}({1000.0 * ratingsByPlayer[x]:F0})")).PadRight(35);

            string leftScoreText = leftScore.ToString().PadLeft(3);
            string rightScoreText = rightScore.ToString().PadRight(3);

            string ratingText = (1000.0 * matchRating).ToString("F2");

            string line = $"{leftText} {leftScoreText} V {rightScoreText} {rightText}  Match Rating: {ratingText}";
            sb.AppendLine(line);


        }
    }
}
