using System;

namespace Processor
{
    public class CaseCalculation
    {
        public double KeyPrice { get; set; } = 2.35;

        #region Constructor
        public CaseCalculation()
        {
        }

        public CaseCalculation(double keyprice) : this()
        {
            this.KeyPrice = keyprice;
        }
        #endregion

        private bool AreAllPropertiesValid()
        {
            return this.KeyPrice != default && this.KeyPrice >= 0d;
        }

        public int CalculateFromAvailableMoney(double casePrice, double availableMoney)
        {
            if (availableMoney < casePrice || casePrice == default || availableMoney == default || !this.AreAllPropertiesValid())
            {
                return default;
            }

            if (casePrice <= 0d || availableMoney <= 0)
            {
                return default;
            }

            double price = Math.Round(casePrice + this.KeyPrice, 2);

            return availableMoney == price ? 1 : (int)Math.Floor(availableMoney / price);
        }

        public double CalculateFromCaseCount(double casePrice, int casecount)
        {
            if (casePrice == default || casecount == default)
            {
                return default;
            }

            if (casePrice <= 0d || casecount <= 0)
            {
                return default;
            }

            return (this.KeyPrice + casePrice) * casecount;
        }
    }
}
