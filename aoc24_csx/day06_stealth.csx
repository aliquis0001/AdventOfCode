var baseMap = File.ReadLines("inputs/06.txt").Select(line => line.ToCharArray()).ToList();
var testMap = baseMap.ConvertAll(x => (char[]) x.Clone());
(int Row, int Col) guard;
(int Row, int Col) guardStart;
char[] guardDirections = new char[4] { '^', '>', 'v', '<' };
int currentDirection = 0;
for (int i = 0; i < baseMap.Count(); i++)
{
    int pos;
    if ((pos = Array.IndexOf(baseMap[i], '^')) >= 0)
    {
        guard = (i, pos);
        guardStart = (i, pos);
    }
}
while (guard.Row >= 0 && guard.Col >= 0 && guard.Row < testMap.Count() && guard.Col < testMap[guard.Row].Length)
{
    testMap[guard.Row][guard.Col] = 'x';
    moveGuard();
}
Console.WriteLine("Part One:");
Console.WriteLine(testMap.Select(line => line.Count(x => x == 'x')).Sum());
Console.WriteLine("Part Two:");
int loopPoints = 0;
for (int i = 0; i < baseMap.Count(); i++)
{
    for (int j = 0; j < baseMap[i].Length; j++)
    {
        if (baseMap[i][j] == '.')
        {
            testMap = baseMap.ConvertAll(x => (char[]) x.Clone());
            testMap[i][j] = '#';
            testMap[guardStart.Row][guardStart.Col] = '.';
            guard = (guardStart.Row, guardStart.Col);
            currentDirection = 0;
            if (LoopCheck())
            {
                loopPoints++;
            }
        }
    }
}
Console.WriteLine(loopPoints);

int AdjustDirection(int direction)
{
    switch (direction)
    {
        case 0:
            if (guard.Row > 0)
            {
                if (testMap[guard.Row - 1][guard.Col] == '#')
                {
                    var newDirection = (direction + 1) % 4;
                    var nextDirection = AdjustDirection(newDirection);
                    return nextDirection;
                }
            }
            break;
        case 1:
            if (guard.Col < testMap[guard.Row].Length - 1)
            {
                if (testMap[guard.Row][guard.Col + 1] == '#')
                {
                    var newDirection = (direction + 1) % 4;
                    var nextDirection = AdjustDirection(newDirection);
                    return nextDirection;
                }
            }
            break;
        case 2:
            if (guard.Row < testMap.Count() - 1)
            {
                if (testMap[guard.Row + 1][guard.Col] == '#')
                {
                    var newDirection = (direction + 1) % 4;
                    var nextDirection = AdjustDirection(newDirection);
                    return nextDirection;
                }
            }
            break;
        case 3:
            if (guard.Col > 0)
            {
                if (testMap[guard.Row][guard.Col - 1] == '#')
                {
                    var newDirection = (direction + 1) % 4;
                    var nextDirection = AdjustDirection(newDirection);
                    return nextDirection;
                }
            }
            break;
        default:
            return direction;
    }
    return direction;
}

void moveGuard(){
    currentDirection = AdjustDirection(currentDirection);
    switch(currentDirection){
        case 0:
            guard.Row--;
            break;
        case 1:
            guard.Col++;
            break;
        case 2:
            guard.Row++;
            break;
        case 3:
            guard.Col--;
            break;
        default:
            return;
    }
}

bool LoopCheck()
{
    while (guard.Row >= 0 && guard.Col >= 0 && guard.Row < testMap.Count() && guard.Col < testMap[guard.Row].Length)
    {
        if (testMap[guard.Row][guard.Col] == guardDirections[currentDirection])
        {
            return true;
        }
        testMap[guard.Row][guard.Col] = guardDirections[currentDirection];
        moveGuard();
    }
    return false;
}