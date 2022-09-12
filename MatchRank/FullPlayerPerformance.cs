using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchRank
{
    [DebuggerDisplay("{Name}: {Rating}   F:{RawPlayerPerformance.PointsFor} A:{RawPlayerPerformance.PointsAgainst} D:{RawPlayerPerformance.PointsDifference}")]
    public class FullPlayerPerformance
    {
        public string Name { get; }

        public RawPlayerPerformance RawPlayerPerformance { get; }

        public IReadOnlyList<MatchScore> MatchScores { get; }

        public double Rating { get; }

        public FullPlayerPerformance(string name, RawPlayerPerformance rawPlayerPerformance, double rating, IList<MatchScore> matchScores)
        {
            Name = name;
            RawPlayerPerformance = rawPlayerPerformance;
            Rating = rating;
            MatchScores = new List<MatchScore>(matchScores);
        }
    }
}
