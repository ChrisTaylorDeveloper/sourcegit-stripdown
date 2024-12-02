using Avalonia.Data.Converters;

namespace SourceGit.Converters
{
    public static class DoubleConverters
    {
        public static readonly FuncValueConverter<double, double> Increase =
            new FuncValueConverter<double, double>(v => v + 1.0);

        public static readonly FuncValueConverter<double, double> Decrease =
            new FuncValueConverter<double, double>(v => v - 1.0);

        public static readonly FuncValueConverter<double, string> ToPercentage =
            new FuncValueConverter<double, string>(v => (v * 100).ToString("F3") + "%");

        public static readonly FuncValueConverter<double, string> OneMinusToPercentage =
            new FuncValueConverter<double, string>(v => ((1.0 - v) * 100).ToString("F3") + "%");
    }
}
