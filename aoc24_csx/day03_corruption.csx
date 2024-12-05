using System.Text.RegularExpressions;

#region Variables
Regex mulFinder = new Regex(@"(?<=mul\()\d+,\d+(?=\))");
string input = File.ReadAllText("inputs/03.txt");
int sumMuls = mulFinder.Matches(input).Select(operation => operation.Value.Split(',').Select(value => Int32.Parse(value))).Select(operands => operands.First() * operands.Last()).Sum();
Regex conFinder = new Regex(@"(?<=mul\()\d+,\d+(?=\))|do(?:n't)?\(\)");
bool enabled = true;
int condSum = conFinder.Matches(input).Select(operation => Handle(operation.Value)).Sum();
#endregion

#region Display
Console.WriteLine("Part One:");
Console.WriteLine(sumMuls);
Console.WriteLine("Part Two:");
Console.WriteLine(condSum);
#endregion

#region Methods
int Handle(string operation)
{
    switch (operation)
    {
        case "do()":
            enabled = true;
            return 0;
        case "don't()":
            enabled = false;
            return 0;
        default:
            if (enabled)
            {
                var operands = operation.Split(',').Select(value => Int32.Parse(value));
                return operands.First() * operands.Last();
            }
            else
                return 0;
    }
}
#endregion