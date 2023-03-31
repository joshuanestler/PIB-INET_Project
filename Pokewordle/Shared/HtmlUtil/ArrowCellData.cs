using Pokewordle.Components.Cells;
using System.Drawing;

namespace Pokewordle.Shared.HtmlUtil
{
    public class ArrowCellData : ICellData
    {
        public ColumnType ColumnType { get; }
        public readonly string DisplayString;
        public readonly int Rotation;
        public readonly bool ShowArrow;
        public readonly string FontColor;
        public readonly string Background;
        public readonly string HtmlClass;
        public readonly string HtmlId;

        public ArrowCellData(ColumnType columnType, float guessValue, float valueToGuess, Color? fontColor = null,
            string htmlClass = "", string htmlId = "")
        {
            this.ColumnType = columnType;


            DisplayString = guessValue.ToString();
            Rotation = (guessValue < valueToGuess) ? 180 : 0;
            ShowArrow = (guessValue != valueToGuess);
            Background = Convert.ColorToHexString(
                ShowArrow ? ColorScheme.COLOR_MISTAKE : ColorScheme.COLOR_CORRECT,
                ColorScheme.COLOR_TYPE_NOT_FOUND);
            //Console.WriteLine($"Arrow Cell guessed: {guessValue}, toGuess: {valueToGuess}, rotation: {Rotation}, ShowArrow {ShowArrow}");
            FontColor = Convert.ColorToHexString(fontColor, ColorScheme.COLOR_TYPE_NOT_FOUND);
            HtmlClass = htmlClass;
            HtmlId = htmlId;
        }

        public string GetColumnWidth()
        {
            return HeaderGenerator.GetColumnWidth(ColumnType).ToString();
        }
    }
}
