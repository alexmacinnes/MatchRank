using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchRank
{
    public class RawPlayerPerformance
    {
        public string Name { get; }

        public int MatchesPlayed { get; set; } = 0;

        public int PointsFor { get; set; } = 0;

        public int PointsAgainst { get; set; } = 0;

        public double WinRatio => (PointsFor, PointsAgainst) switch
        {
            //TODO - switch needed? - only final case?
            (0, _) => 0.0,
            (_, 0) => 1.0,
            (int f, int a) => ((double)f) / ((double)(a + f))
        };

        public int PointsDifference => PointsFor - PointsAgainst;

        public RawPlayerPerformance(string name) => Name = name;
    }
}
