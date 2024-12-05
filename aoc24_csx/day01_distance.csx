#region Variables
var Lists = OrderAndTranspose("inputs/01.txt");
var Counts = Lists.Last().GroupBy(i => i).ToDictionary(g => g.Key, g => g.Count());
#endregion

#region Display
Console.WriteLine("Part One:");
Console.WriteLine(SumDistances());
Console.WriteLine("Part Two:");
Console.WriteLine(SumSimilarities());
#endregion

#region Methods
public IEnumerable<IEnumerable<int>> OrderAndTranspose(string InputFile)
{
    return File.ReadLines(InputFile) //gets file line by line
            .Select(line => line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries) //splits each line into 2 values.
                                    .Select(id => Int32.Parse(id))) //converts values to integers
            .SelectMany(row => row.Select((item, index) => new { item, index })) //converts list of lists into list of tuples
            .GroupBy(i => i.index, i => i.item) // converts list of tuples into list of 2 groups, each containing elements of a tuple(the 2 columns)
                                                // the result of the SelectMany and GroupBy is that the list is now transposed.
            .Select(group => group.Order()); //converts groups into ordered lists of the numbers inside of them
}
public int SumDistances()
{
    return Lists.First().Zip(Lists.Last(),
            (first, second) => Math.Abs(first - second)).Sum();
}
public int SumSimilarities()
{
    return Lists.First().Select(i => i * (Counts.ContainsKey(i) ? Counts[i] : 0)).Sum();
}
#endregion