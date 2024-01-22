using Cs2CaseCalculator.Models;
using neXn.Lib.ConfigurationHandler;
using System.Collections.Generic;

namespace Cs2CaseCalculator.Logic
{
    internal static class RuntimeStorage
    {
        public static List<Case> Cases { get; set; } = [];
        public static ConfigurationHandler<Configuration> Configuration { get; set; }
    }
}
