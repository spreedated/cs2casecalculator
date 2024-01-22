using CommunityToolkit.Mvvm.ComponentModel;
using Cs2CaseCalculator.Logic;
using Cs2CaseCalculator.Models;
using Cs2CaseCalculator.Views;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;
using Processor;

namespace Cs2CaseCalculator.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        public MainWindow Instance { get; set; }

        #region Bindable Properties
        private ObservableCollection<Case> _Cases;
        public ObservableCollection<Case> Cases
        {
            get
            {
                return this._Cases;
            }
            set
            {
                this._Cases = value;
                base.OnPropertyChanged(nameof(this.Cases));
            }
        }

        private Case _SelectedCase;
        public Case SelectedCase
        {
            get
            {
                return this._SelectedCase;
            }
            set
            {
                this._SelectedCase = value;
                base.OnPropertyChanged(nameof(this.SelectedCase));
                this.TriggerCalculationFromAvailableMoney();
            }
        }

        private int _CountCasesToOpen;
        public int CountCasesToOpen
        {
            get
            {
                return this._CountCasesToOpen;
            }
            set
            {
                this._CountCasesToOpen = value;
                base.OnPropertyChanged(nameof(this.CountCasesToOpen));
            }
        }

        private string _InputMoney;
        public string InputMoney
        {
            get
            {
                return this._InputMoney;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && !Regex.Match(value, "^(?!,$)[\\d,.]+$").Success)
                {
                    return;
                }
                this._InputMoney = value;
                this.InputMoneyValue = string.IsNullOrEmpty(this._InputMoney) ? 0d : double.Parse(this._InputMoney.Replace(",", "."), CultureInfo.InvariantCulture);
                base.OnPropertyChanged(nameof(this.InputMoney));
            }
        }

        private double _InputMoneyValue;
        public double InputMoneyValue
        {
            get
            {
                return this._InputMoneyValue;
            }
            set
            {
                this._InputMoneyValue = value;
                base.OnPropertyChanged(nameof(this.InputMoneyValue));
                this.TriggerCalculationFromAvailableMoney();
            }
        }

        private int _PossibleCasesCalculated;
        public int PossibleCasesCalculated
        {
            get
            {
                return this._PossibleCasesCalculated;
            }
            set
            {
                this._PossibleCasesCalculated = value;
                base.OnPropertyChanged(nameof(this.PossibleCasesCalculated));
            }
        }
        #endregion

        public void TriggerCalculationFromAvailableMoney()
        {
            if ((this.SelectedCase == null || this.SelectedCase.Price == default) || this.InputMoneyValue == default)
            {
                return;
            }
            CaseCalculation c = new(RuntimeStorage.Configuration.RuntimeConfiguration.KeyPrice);
            this.PossibleCasesCalculated = c.CalculateFromAvailableMoney(this.SelectedCase.Price, this.InputMoneyValue);
        }

        public void UpdateCache()
        {
            //TODO: Make GUI animation

            Task.Factory.StartNew(async () =>
            {
                RetrieveCasePrices rcp = new();
                await rcp.Update();
                RuntimeStorage.Configuration.RuntimeConfiguration.CachedCases = rcp.Cases;
                RuntimeStorage.Configuration.RuntimeConfiguration.CacheDateTime = DateTime.Now;
                RuntimeStorage.Configuration.Save();

                Log.Information("Cases updated");

                RuntimeStorage.Cases = rcp.Cases;
                this.Cases = new(rcp.Cases);
            });
        }
    }
}
