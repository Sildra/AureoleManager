using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;

#region COM Interop

using XlAxisType = Microsoft.Office.Interop.Excel.XlAxisType;
using XlCategoryType = Microsoft.Office.Interop.Excel.XlCategoryType;
using XlChartType = Microsoft.Office.Interop.Excel.XlChartType;
using XlLegendPosition = Microsoft.Office.Interop.Excel.XlLegendPosition;
using XlTimeUnit = Microsoft.Office.Interop.Excel.XlTimeUnit;
#endregion


namespace AureoleManager.SkillManager {
    class SkillCalculator {
        #region CONSTS
        private const string Filename = @"C:\Users\Sildra\Desktop\Dev\Book1.xlsx";
        private const string Title = "Title";
        private const string Param1 = "Probability";
        private const string Param2 = @"Cumul.probability (‰)";
        private const string Param3 = "Mean Damage";
        private const string Param4 = "Damage";
        #endregion

        readonly string _name;
        float _meanValue;
        readonly List<ProbabilityDamage> _skills = new List<ProbabilityDamage>();

        public SkillCalculator(string name, uint minAccuracy, int baseDamage, float majorBonus, float minorBonus) {
            _name = name;
            _meanValue = 0;

            var pc = new ProbabilityCalculator(minAccuracy, majorBonus, minorBonus);
            foreach (var item in pc.Probabilities) {
                _skills.Add(new ProbabilityDamage(item.Proba, item.CumulProba, 
                    baseDamage, item.BonusDamage));
            }
            CalculateMeanValue();
        }

        public SkillCalculator(string name, SkillCalculator skill1, SkillCalculator skill2) {
            float cumulProba = 0;

            if (name != null)
                _name = name;
            else
                _name = skill1._name + " + " + skill2._name;

            foreach (var item1 in skill1._skills) {
                foreach (var item2 in skill2._skills) {
                    Merge((item1.GetProbability() * item2.GetProbability()) / 100,
                        item1.GetDamage() + item2.GetDamage());
                }
            }
            _skills.Sort();
            foreach (var item in _skills) {
                cumulProba += item.GetProbability();
                item.SetCumulProba(cumulProba);
            }
            CalculateMeanValue();
        }

        public SkillCalculator(Skill skill) {
            throw new NotImplementedException();
        }

        void Merge(float proba, int damage) {
            if (_skills.Any(item => item.Merge(proba, damage))) {
                return;
            }
            _skills.Add(new ProbabilityDamage(proba, damage));
        }

        public void Print() {
            Console.WriteLine("{0}, Mean damage : {1}", _name, _meanValue);
            Probability.PrintHeader();
            foreach (var item in _skills) {
                item.Print();
            }
            Console.WriteLine();
        }

        void BuildChart(_Worksheet worksheet, uint size) {
            var xlCharts = (ChartObjects)worksheet.ChartObjects(Type.Missing);
            var myChart = xlCharts.Add(10, 80, 400, 200);
            var chartPage = myChart.Chart;

            var chartRange = worksheet.Range["A1", "C" + size];
            chartPage.SetSourceData(chartRange);
            chartPage.SetElement(MsoChartElementType.msoElementPrimaryCategoryAxisTitleAdjacentToAxis);
            chartPage.SetElement(MsoChartElementType.msoElementPrimaryValueAxisTitleAdjacentToAxis);
            chartPage.SetElement(MsoChartElementType.msoElementChartTitleAboveChart);
            chartPage.ChartTitle.Text = _name;
            chartPage.Axes(XlAxisType.xlCategory).AxisTitle.Text = Param2;
            chartPage.Axes(XlAxisType.xlValue).AxisTitle.Text = Param4;
            chartPage.ChartType = XlChartType.xlLine;
            chartPage.Legend.Position = XlLegendPosition.xlLegendPositionBottom;
            chartPage.Axes(XlAxisType.xlCategory).CategoryType = XlCategoryType.xlTimeScale;
            chartPage.Axes(XlAxisType.xlCategory).MajorUnit = 100;
            chartPage.Axes(XlAxisType.xlCategory).MajorUnitScale = XlTimeUnit.xlDays;

            //export chart as picture file
            chartPage.Export(@"C:\Users\Sildra\Desktop\Dev\excel_chart_export.png");
        }

        [SuppressMessage("ReSharper", "UseIndexedProperty")]
        public void GenerateGraph() {
            uint i = 0;
            var application = new Application();
            var workbook = application.Workbooks.Add();
            var worksheet = workbook.Worksheets[1] as Worksheet;

            application.DisplayAlerts = false;
            if (worksheet == null)
                return;
            worksheet.Cells[1, 2] = Param3;
            worksheet.Cells[1, 3] = Param4;
            {
                var floats = new float[_skills.Count * 2, 2];
                var ints = new int[_skills.Count * 2, 1];
                float previousProba = 0;

                foreach (var item in _skills) {
                    floats[i, 0] = previousProba;
                    floats[i, 1] = _meanValue;
                    ints[i, 0] = item.GetDamage();
                    ++i;
                    previousProba = item.GetCumulProba() * 10;
                    floats[i, 0] = previousProba;
                    floats[i, 1] = _meanValue;
                    ints[i, 0] = item.GetDamage();
                    ++i;
                }
                worksheet.Range["A2", "B" + i + 1].set_Value(null, floats);
                worksheet.Range["C2", "C" + i + 1].set_Value(null, ints);
            }
            BuildChart(worksheet, i + 1);

            workbook.SaveAs(Filename, XlFileFormat.xlWorkbookDefault, null, null, null, null, XlSaveAsAccessMode.xlExclusive);
            workbook.Close(true);
            application.Quit();
        }

        void CalculateMeanValue() {
            foreach (var item in _skills) {
                _meanValue += (item.GetProbability() * item.GetDamage());
            }
            _meanValue /= 100;
        }
    }
}
