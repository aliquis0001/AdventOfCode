using System.Threading;
#region Variables
char[][] game = File.ReadLines("input").Select(x => x.ToCharArray()).ToArray();
char[] searchString = "XMAS".ToCharArray();
[Flags]
enum Direction
{
    North = 1,
    South = 2,
    East = 4,
    West = 8
}
int foundStrings = 0;
List<Thread> searchThreads = new();
#endregion

#region Part One
Console.WriteLine("Part One:");
for (int i = 0; i < game.Length; i++)
{
    for (int j = 0; j < game[i].Length; j++)
    {
        if (game[i][j].Equals(searchString[0]))
        {
            SearchLaunch(i, j);
        }
    }
}
foreach (var thread in searchThreads)
{
    thread.Join();
}
Console.WriteLine(foundStrings);
#endregion

#region Part Two
int crosses;
for (int i = 1; i < game.Length - 1; i++)
{
    for (int j = 1; j < game[i].Length - 1; j++)
    {
        if (game[i][j].Equals('A'))
        {
            if (CrossCheck(i, j))
                crosses++;
        }
    }
}
Console.WriteLine("Part Two:");
Console.WriteLine(crosses);
#endregion

#region Methods
void checkValue(int target, int row, int col, Direction d)
{
    if (target == searchString.Length)
    {
        int matchCount = Interlocked.Increment(ref foundStrings);
        return;
    }
    if (row >= 0 && col >= 0 && row < game.Length && col < game[row].Length)
    {
        if (game[row][col] == searchString[target])
        {
            target++;
            (row, col) = Adjust(row, col, d);

            checkValue(target, row, col, d);
            return;
        }
    }
}
void SearchLaunch(int i, int j)
{
    for (int k = 1; k <= 10; k++)
    {
        if (!((k & 3) == 3))
        {
            Direction bearing = (Direction)k;
            var endpoint = Adjust(i, j, bearing, searchString.Length - 1);
            if (endpoint.Item1 >= 0 && endpoint.Item2 >= 0 && endpoint.Item1 < game.Length && endpoint.Item2 < game[endpoint.Item1].Length)
            {
                Thread launch = new Thread(() => checkValue(0, i, j, bearing));
                searchThreads.Add(launch);
                launch.Start();
            }
        }
    }
}
bool CrossCheck(int row, int col)
{
    if (row == 0 || col == 0 || row + 1 == game.Length || col + 1 == game[row].Length)
        return false;

    char nw = game[row - 1][col - 1];
    if ((nw != 'M' && nw != 'S'))
        return false;

    char ne = game[row - 1][col + 1];
    if ((ne != 'M' && ne != 'S'))
        return false;

    char sw = game[row + 1][col - 1];
    if ((sw != 'M' && sw != 'S'))
        return false;

    char se = game[row + 1][col + 1];
    if ((se != 'M' && se != 'S'))
        return false;

    if (nw == sw)
        return ((ne != nw) && (ne == se));
    else if (nw == ne)
        return (sw == se);
    else
        return false;
}
#endregion
(int, int) Adjust(int row, int col, Direction d, int amount = 1)
{
    if ((d & Direction.North) != 0)
    {
        row -= amount;
    }
    else if ((d & Direction.South) != 0)
    {
        row += amount;
    }

    if ((d & Direction.East) != 0)
    {
        col += amount;
    }
    else if ((d & Direction.West) != 0)
    {
        col -= amount;
    }
    return ((row, col));
}