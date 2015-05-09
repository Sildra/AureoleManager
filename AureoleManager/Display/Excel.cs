using System;
using System.Diagnostics.CodeAnalysis;
using AureoleManager.SkillManager;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using XlAxisType = Microsoft.Office.Interop.Excel.XlAxisType;
using XlCategoryType = Microsoft.Office.Interop.Excel.XlCategoryType;
using XlChartType = Microsoft.Office.Interop.Excel.XlChartType;
using XlLegendPosition = Microsoft.Office.Interop.Excel.XlLegendPosition;
using XlTimeUnit = Microsoft.Office.Interop.Excel.XlTimeUnit;

namespace AureoleManager.Display {
    public static class Excel {
        #region Public members

        /// <summary>
        ///     Builds the current data as an Excel graph in an image
        /// </summary>
        /// <param name="data">Data to be displayed</param>
        /// <param name="pngFile">Path to the PNG file</param>
        /// <param name="excelFile">Path to the Excel workbook</param>
        public static void ToGraph(SkillDamage data, string pngFile, string excelFile = null) {
            var application = new Application();
            var workbook = application.Workbooks.Add();
            var worksheet = workbook.Worksheets[1] as Worksheet;

            application.DisplayAlerts = false;
            if (worksheet == null)
                return;
            worksheet.Cells[1, 2] = Data2;
            worksheet.Cells[1, 3] = ValueTitle;
            CreateData(worksheet, data);
            BuildChart(worksheet, data.Name, (uint)data.Hits.Count * 2 + 1, pngFile);

            if (excelFile != null)
                workbook.SaveAs(excelFile, XlFileFormat.xlWorkbookDefault, null, null, null, null,
                    XlSaveAsAccessMode.xlExclusive);
            workbook.Close(true);
            application.Quit();
        }

        #endregion

        #region Private members

        [SuppressMessage("ReSharper", "UseIndexedProperty")]
        private static void CreateData(_Worksheet worksheet, SkillDamage data) {
            var floats = new float[data.Hits.Count * 2, 2];
            var ints = new int[data.Hits.Count * 2, 1];
            var meanValue = data.MeanValue;
            float previousProba = 0;
            uint i = 0;

            foreach (var item in data.Hits) {
                floats[i, 0] = previousProba;
                floats[i, 1] = meanValue;
                ints[i, 0] = item.Damage;
                ++i;
                previousProba = item.CumulProba * 10;
                floats[i, 0] = previousProba;
                floats[i, 1] = meanValue;
                ints[i, 0] = item.Damage;
                ++i;
            }
            worksheet.Range["A2", "B" + i + 1].set_Value(null, floats);
            worksheet.Range["C2", "C" + i + 1].set_Value(null, ints);
        }

        private static void SetExcelChart(_Chart chartPage, string title) {
            chartPage.SetElement(MsoChartElementType.msoElementPrimaryCategoryAxisTitleAdjacentToAxis);
            chartPage.SetElement(MsoChartElementType.msoElementPrimaryValueAxisTitleAdjacentToAxis);
            chartPage.SetElement(MsoChartElementType.msoElementChartTitleAboveChart);
            chartPage.ChartTitle.Text = title;
            chartPage.Axes(XlAxisType.xlCategory).AxisTitle.Text = CategoryTitle;
            // chartPage.Axes(XlAxisType.xlValue).AxisTitle.Text = ValueTitle;
            chartPage.ChartType = XlChartType.xlLine;
            chartPage.Legend.Position = XlLegendPosition.xlLegendPositionBottom;
            chartPage.Axes(XlAxisType.xlCategory).CategoryType = XlCategoryType.xlTimeScale;
            chartPage.Axes(XlAxisType.xlCategory).MajorUnit = 100;
            chartPage.Axes(XlAxisType.xlCategory).MajorUnitScale = XlTimeUnit.xlDays;
        }

        private static void BuildChart(_Worksheet worksheet, string title, uint size, string pngFile) {
            var xlCharts = (ChartObjects)worksheet.ChartObjects(Type.Missing);
            var myChart = xlCharts.Add(10, 80, 400, 200);
            var chartPage = myChart.Chart;

            var chartRange = worksheet.Range["A1", "C" + size];
            chartPage.SetSourceData(chartRange);
            SetExcelChart(chartPage, title);
            //export chart as picture file
            chartPage.Export(pngFile);
        }

        #endregion

        #region CONSTS

        private const string Filename = @"C:\Users\Sildra\Desktop\Dev\Book1.xlsx";
        private const string Data1 = "Probability";
        private const string Data2 = "Mean Damage";
        private const string CategoryTitle = @"Cumul.probability (‰)";
        private const string ValueTitle = "Damage";

        #endregion
    }
}