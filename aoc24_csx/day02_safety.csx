#region Variables
var Reports = File.ReadLines("inputs/02.txt");
int safeRuns = Reports.Select(report => Analyze(ParseReport(report))).Where(result => result).Count();
int dampRuns = Reports.Select(report => Dampen(ParseReport(report))).Where(result => result).Count();
#endregion

#region Display
Console.WriteLine("Part One:");
Console.WriteLine(safeRuns);
Console.WriteLine("Part Two:");
Console.WriteLine(dampRuns);
#endregion

#region Methods
public IEnumerable<int> ParseReport(string Report)
{
    return Report.Split(" ").Select(value => Int32.Parse(value));
}
public bool Analyze(IEnumerable<int> values)
{
    var differences = values.Select((value, index) => (index > 0 ? value - values.ElementAt(index - 1) : 0))
                                .TakeLast(values.Count() - 1);
    return (differences.All(x => x < 0) || differences.All(x => x > 0)) && (differences.Select(x => Math.Abs(x)).Max() <= 3);
}
public bool Dampen(IEnumerable<int> values)
{
    bool safe = Analyze(values);
    int i = 0;
    while (!safe && i < values.Count())
    {
        safe = Analyze(values.Where((value, index) => index != i));
        i++;
    }
    return safe;
}
#endregion