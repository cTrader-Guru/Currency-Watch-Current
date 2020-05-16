﻿/*  CTRADER GURU --> Indicator Template 1.0.6

    Homepage    : https://ctrader.guru/
    Telegram    : https://t.me/ctraderguru
    Twitter     : https://twitter.com/cTraderGURU/
    Facebook    : https://www.facebook.com/ctrader.guru/
    YouTube     : https://www.youtube.com/channel/UCKkgbw09Fifj65W5t5lHeCQ
    GitHub      : https://github.com/ctrader-guru

*/

using System;
using cAlgo.API;
using cAlgo.API.Internals;
using cAlgo.API.Indicators;
using cAlgo.Indicators;

namespace cAlgo
{

    [Indicator(IsOverlay = false, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    [Levels(0)]
    public class INDICATORBASE : Indicator
    {

        #region Enums

        // --> Eventuali enumeratori li mettiamo qui

        #endregion

        #region Identity

        /// <summary>
        /// Nome del prodotto, identificativo, da modificare con il nome della propria creazione
        /// </summary>
        public const string NAME = "Currency Watch Current";

        /// <summary>
        /// La versione del prodotto, progressivo, utilie per controllare gli aggiornamenti se viene reso disponibile sul sito ctrader.guru
        /// </summary>
        public const string VERSION = "1.0.1";

        #endregion

        #region Params

        /// <summary>
        /// Identità del prodotto nel contesto di ctrader.guru
        /// </summary>
        [Parameter(NAME + " " + VERSION, Group = "Identity", DefaultValue = "https://ctrader.guru/product/currency-watch-current/")]
        public string ProductInfo { get; set; }

        [Parameter("End of Day", Group = "Params", DefaultValue = 23)]
        public int EndOfDay { get; set; }

        [Output("First Currency", LineColor = "DodgerBlue")]
        public IndicatorDataSeries First { get; set; }

        [Output("Second Currency", LineColor = "Red")]
        public IndicatorDataSeries Second { get; set; }

        #endregion

        #region Property

        int havecurr = 0;

        double EURUSDopenday = -1;
        double EURGBPopenday = -1;
        double EURJPYopenday = -1;
        double EURAUDopenday = -1;
        double EURCHFopenday = -1;
        double EURCADopenday = -1;
        double EURNZDopenday = -1;

        double GBPUSDopenday = -1;
        double USDJPYopenday = -1;
        double AUDUSDopenday = -1;
        double USDCHFopenday = -1;
        double USDCADopenday = -1;
        double NZDUSDopenday = -1;

        double GBPJPYopenday = -1;
        double GBPAUDopenday = -1;
        double GBPCHFopenday = -1;
        double GBPCADopenday = -1;
        double GBPNZDopenday = -1;

        double AUDJPYopenday = -1;
        double CHFJPYopenday = -1;
        double CADJPYopenday = -1;
        double NZDJPYopenday = -1;

        double CADCHFopenday = -1;
        double AUDCADopenday = -1;
        double NZDCADopenday = -1;

        double AUDCHFopenday = -1;
        double NZDCHFopenday = -1;

        double AUDNZDopenday = -1;

        private readonly string[] EURcross =
        {
            "EURUSD",
            "EURGBP",
            "EURJPY",
            "EURAUD",
            "EURCHF",
            "EURCAD",
            "EURNZD"
        };

        private readonly string[] USDcross =
        {
            "EURUSD",
            "GBPUSD",
            "USDJPY",
            "AUDUSD",
            "USDCHF",
            "USDCAD",
            "NZDUSD"
        };

        private readonly string[] GBPcross =
        {
            "EURGBP",
            "GBPUSD",
            "GBPJPY",
            "GBPAUD",
            "GBPCHF",
            "GBPCAD",
            "GBPNZD"
        };

        private readonly string[] JPYcross =
        {
            "EURJPY",
            "USDJPY",
            "GBPJPY",
            "AUDJPY",
            "CHFJPY",
            "CADJPY",
            "NZDJPY"
        };

        private readonly string[] CADcross =
        {
            "EURCAD",
            "USDCAD",
            "GBPCAD",
            "CADJPY",
            "CADCHF",
            "AUDCAD",
            "NZDCAD"
        };

        private readonly string[] CHFcross =
        {
            "EURCHF",
            "USDCHF",
            "GBPCHF",
            "CHFJPY",
            "CADCHF",
            "AUDCHF",
            "NZDCHF"
        };

        private readonly string[] AUDcross =
        {
            "EURAUD",
            "AUDUSD",
            "GBPAUD",
            "AUDJPY",
            "AUDCHF",
            "AUDCAD",
            "AUDNZD"
        };

        private readonly string[] NZDcross =
        {
            "EURNZD",
            "NZDUSD",
            "GBPNZD",
            "NZDJPY",
            "NZDCAD",
            "NZDCHF",
            "AUDNZD"
        };

        #endregion

        #region Indicator Events

        /// <summary>
        /// Viene generato all'avvio dell'indicatore, si inizializza l'indicatore
        /// </summary>
        protected override void Initialize()
        {

            // --> Stampo nei log la versione corrente
            Print("{0} : {1}", NAME, VERSION);

        }

        /// <summary>
        /// Generato ad ogni tick, vengono effettuati i calcoli dell'indicatore
        /// </summary>
        /// <param name="index">L'indice della candela in elaborazione</param>
        public override void Calculate(int index)
        {

            if (Bars.TimeFrame != TimeFrame.Hour)
            {

                if(_canDraw())Chart.DrawStaticText("MyError", "PLEASE, USE THIS INDICATOR WITH TIMEFRAME 1H",VerticalAlignment.Center, HorizontalAlignment.Center, Color.Red);

                return;

            }
            else if (havecurr == -1)
            {

                if (_canDraw()) Chart.DrawStaticText("MyError", "NOT SUPPORT THIS CROSS, ONLY MAJOR CURRENCY : EUR;USD;GBP;JPY;CAD;AUD;CHF;NZD", VerticalAlignment.Center, HorizontalAlignment.Center, Color.Red);
                
                return;

            }

            havecurr = -1;

            if (Array.IndexOf(EURcross, SymbolName) > -1)
                SetValue((SymbolName.IndexOf("EUR") == 0) ? First : Second, index, "EUR", EURcross);

            if (Array.IndexOf(USDcross, SymbolName) > -1)
                SetValue((SymbolName.IndexOf("USD") == 0) ? First : Second, index, "USD", USDcross);

            if (Array.IndexOf(GBPcross, SymbolName) > -1)
                SetValue((SymbolName.IndexOf("GBP") == 0) ? First : Second, index, "GBP", GBPcross);

            if (Array.IndexOf(JPYcross, SymbolName) > -1)
                SetValue((SymbolName.IndexOf("JPY") == 0) ? First : Second, index, "JPY", JPYcross);

            if (Array.IndexOf(CADcross, SymbolName) > -1)
                SetValue((SymbolName.IndexOf("CAD") == 0) ? First : Second, index, "CAD", CADcross);

            if (Array.IndexOf(CHFcross, SymbolName) > -1)
                SetValue((SymbolName.IndexOf("CHF") == 0) ? First : Second, index, "CHF", CHFcross);

            if (Array.IndexOf(AUDcross, SymbolName) > -1)
                SetValue((SymbolName.IndexOf("AUD") == 0) ? First : Second, index, "AUD", AUDcross);

            if (Array.IndexOf(NZDcross, SymbolName) > -1)
                SetValue((SymbolName.IndexOf("NZD") == 0) ? First : Second, index, "NZD", NZDcross);

        }

        #endregion

        #region Private Methods

        private bool _canDraw() {

            return (RunningMode == RunningMode.RealTime || RunningMode == RunningMode.VisualBacktesting);

        }

        private double GetOpenDayPrice(string onecross)
        {

            if (onecross == "EURUSD")
                return EURUSDopenday;

            if (onecross == "EURGBP")
                return EURGBPopenday;

            if (onecross == "EURJPY")
                return EURJPYopenday;

            if (onecross == "EURAUD")
                return EURAUDopenday;

            if (onecross == "EURCHF")
                return EURCHFopenday;

            if (onecross == "EURCAD")
                return EURCADopenday;

            if (onecross == "EURNZD")
                return EURNZDopenday;

            if (onecross == "GBPUSD")
                return GBPUSDopenday;

            if (onecross == "USDJPY")
                return USDJPYopenday;

            if (onecross == "AUDUSD")
                return AUDUSDopenday;

            if (onecross == "USDCHF")
                return USDCHFopenday;

            if (onecross == "USDCAD")
                return USDCADopenday;

            if (onecross == "NZDUSD")
                return NZDUSDopenday;

            if (onecross == "GBPJPY")
                return GBPJPYopenday;

            if (onecross == "GBPAUD")
                return GBPAUDopenday;

            if (onecross == "GBPCHF")
                return GBPCHFopenday;

            if (onecross == "GBPCAD")
                return GBPCADopenday;

            if (onecross == "GBPNZD")
                return GBPNZDopenday;

            if (onecross == "AUDJPY")
                return AUDJPYopenday;

            if (onecross == "CHFJPY")
                return CHFJPYopenday;

            if (onecross == "CADJPY")
                return CADJPYopenday;

            if (onecross == "NZDJPY")
                return NZDJPYopenday;

            if (onecross == "CADCHF")
                return CADCHFopenday;

            if (onecross == "AUDCAD")
                return AUDCADopenday;

            if (onecross == "NZDCAD")
                return NZDCADopenday;

            if (onecross == "AUDCHF")
                return AUDCHFopenday;

            if (onecross == "NZDCHF")
                return NZDCHFopenday;

            if (onecross == "AUDNZD")
                return AUDNZDopenday;

            return -1;

        }

        private void SetOpenDayPrice(string onecross, double openprice)
        {

            if (onecross == "EURUSD")
            {
                EURUSDopenday = openprice;
                return;
            }


            if (onecross == "EURGBP")
            {
                EURGBPopenday = openprice;
                return;
            }

            if (onecross == "EURJPY")
            {
                EURJPYopenday = openprice;
                return;
            }

            if (onecross == "EURAUD")
            {
                EURAUDopenday = openprice;
                return;
            }

            if (onecross == "EURCHF")
            {
                EURCHFopenday = openprice;
                return;
            }

            if (onecross == "EURCAD")
            {
                EURCADopenday = openprice;
                return;
            }

            if (onecross == "EURNZD")
            {
                EURNZDopenday = openprice;
                return;
            }

            if (onecross == "GBPUSD")
            {
                GBPUSDopenday = openprice;
                return;
            }

            if (onecross == "USDJPY")
            {
                USDJPYopenday = openprice;
                return;
            }

            if (onecross == "AUDUSD")
            {
                AUDUSDopenday = openprice;
                return;
            }

            if (onecross == "USDCHF")
            {
                USDCHFopenday = openprice;
                return;
            }

            if (onecross == "USDCAD")
            {
                USDCADopenday = openprice;
                return;
            }

            if (onecross == "NZDUSD")
            {
                NZDUSDopenday = openprice;
                return;
            }

            if (onecross == "GBPJPY")
            {
                GBPJPYopenday = openprice;
                return;
            }

            if (onecross == "GBPAUD")
            {
                GBPAUDopenday = openprice;
                return;
            }

            if (onecross == "GBPCHF")
            {
                GBPCHFopenday = openprice;
                return;
            }

            if (onecross == "GBPCAD")
            {
                GBPCADopenday = openprice;
                return;
            }

            if (onecross == "GBPNZD")
            {
                GBPNZDopenday = openprice;
                return;
            }

            if (onecross == "AUDJPY")
            {
                AUDJPYopenday = openprice;
                return;
            }

            if (onecross == "CHFJPY")
            {
                CHFJPYopenday = openprice;
                return;
            }

            if (onecross == "CADJPY")
            {
                CADJPYopenday = openprice;
                return;
            }

            if (onecross == "NZDJPY")
            {
                NZDJPYopenday = openprice;
                return;
            }

            if (onecross == "CADCHF")
            {
                CADCHFopenday = openprice;
                return;
            }

            if (onecross == "AUDCAD")
            {
                AUDCADopenday = openprice;
                return;
            }

            if (onecross == "NZDCAD")
            {
                NZDCADopenday = openprice;
                return;
            }

            if (onecross == "AUDCHF")
            {
                AUDCHFopenday = openprice;
                return;
            }

            if (onecross == "NZDCHF")
            {
                NZDCHFopenday = openprice;
                return;
            }

            if (onecross == "AUDNZD")
            {
                AUDNZDopenday = openprice;
                return;
            }


        }

        private void SetValue(IndicatorDataSeries View, int index, string CROSSSymbol, string[] cross)
        {

            double crosspips = 0.0;
            // --> Devo fare un ciclio per valutare i cross

            foreach (string onecross in cross)
            {

                try
                {

                    double opendayprice = GetOpenDayPrice(onecross);

                    Symbol tmpcross = Symbols.GetSymbol(onecross);

                    Bars tmpcrosssr = MarketData.GetBars(TimeFrame, tmpcross.Name);

                    int index2 = GetIndexByDate(tmpcrosssr, Bars.OpenTimes[index]);

                    if (tmpcrosssr.OpenTimes[index2].Hour == EndOfDay && tmpcrosssr.OpenTimes[index2].Minute >= 0 && tmpcrosssr.OpenTimes[index2].Minute <= 1)
                    {
                        SetOpenDayPrice(onecross, tmpcrosssr.OpenPrices[index2]);
                        opendayprice = tmpcrosssr.OpenPrices[index2];
                    }

                    if (opendayprice == -1)
                    {

                        havecurr = 1;
                        return;

                    }

                    double tmpcrosspips = 0.0;

                    if (onecross.IndexOf(CROSSSymbol) == 0)
                    {

                        if (tmpcrosssr.ClosePrices[index2] > opendayprice)
                        {

                            tmpcrosspips = (tmpcrosssr.ClosePrices[index2] - opendayprice) / tmpcross.PipSize;
                            crosspips += tmpcrosspips;

                        }
                        else if (opendayprice > tmpcrosssr.ClosePrices[index2])
                        {

                            tmpcrosspips = (opendayprice - tmpcrosssr.ClosePrices[index2]) / tmpcross.PipSize;
                            crosspips -= tmpcrosspips;

                        }

                    }
                    else if (onecross.IndexOf(CROSSSymbol) > 0)
                    {

                        if (tmpcrosssr.ClosePrices[index2] > opendayprice)
                        {

                            tmpcrosspips = (tmpcrosssr.ClosePrices[index2] - opendayprice) / tmpcross.PipSize;
                            crosspips -= tmpcrosspips;

                        }
                        else if (opendayprice > tmpcrosssr.ClosePrices[index2])
                        {

                            tmpcrosspips = (opendayprice - tmpcrosssr.ClosePrices[index2]) / tmpcross.PipSize;
                            crosspips += tmpcrosspips;

                        }

                    }
                    else
                    {

                        Print(string.Format("Errore : {0} non esiste", CROSSSymbol));

                    }


                }
                catch (Exception)
                {

                    //Print(string.Format("Errore : {0}", exc.Message));

                }

            }

            View[index] = crosspips;

            havecurr = 1;

        }

        private int GetIndexByDate(Bars series, DateTime time)
        {
            for (int i = series.ClosePrices.Count - 1; i >= 0; i--)
                if (time == series.OpenTimes[i])
                    return i;
            return -1;
        }

        #endregion

    }

}
