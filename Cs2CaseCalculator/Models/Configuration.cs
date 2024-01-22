using System;
using System.Collections.Generic;
using System.Drawing;

namespace Cs2CaseCalculator.Models
{
    internal class Configuration
    {
        public double KeyPrice { get; set; } = 2.35d;
        public Point WindowLocation { get; set; }
        public int LastSelectedCase { get; set; }
        public List<Case> CachedCases { get; set; }
        public DateTime CacheDateTime { get; set; }
    }
}
