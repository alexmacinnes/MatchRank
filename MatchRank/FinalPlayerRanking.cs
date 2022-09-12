using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchRank
{
    public class FinalPlayerRanking
    {
        public IReadOnlyList<GenerationRanking> Generations { get; }

        public IReadOnlyList<FullPlayerPerformance> FinalPlayerRatings { get; }

        public FinalPlayerRanking(IReadOnlyList<GenerationRanking> generations, IReadOnlyList<FullPlayerPerformance> finalPlayerRatings)
        {
            Generations = generations;
            FinalPlayerRatings = finalPlayerRatings;
        }
    }
}
