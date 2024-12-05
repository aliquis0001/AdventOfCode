#region Setup
bool secDone = false;
List<IEnumerable<int>> secOne = new();
List<List<int>> secTwo = new();
var lines = File.ReadLines("input");

foreach (string line in lines)
{
    if (string.IsNullOrEmpty(line)){
        secDone = true;
        continue;
    }
    if(secDone) 
        secTwo.Add(line.Split(",").Select(num => Int32.Parse(num)).ToList());
    else
        secOne.Add(line.Split("|").Select(num => Int32.Parse(num)));
}

var ruleset = secOne.GroupBy(rule => rule.First(), rule => rule.Last());
CustomOrderer orderer = new CustomOrderer(ruleset);
#endregion

#region Display
Console.WriteLine("Part One:")
var valid = secTwo.Where(entry => Validate(entry)).Select(entry =>entry[(entry.Count())/2]).Sum();
Console.WriteLine(valid);
Console.WriteLine("Part Two:");
var corrected = secTwo.Where(entry => !Validate(entry)).Select(x => {x.Sort(new CustomOrderer(ruleset)); return x[x.Count()/2];}).Sum();
Console.WriteLine(corrected);
#endregion

#region Methods
bool Validate(List<int> pages){
    return pages.Select((page, index) => !ruleset.Any(rule => rule.Key==page)||ruleset.First(rule => rule.Key==page)
                                                .All(lateValue => Bounds(pages.IndexOf(lateValue), index)))
                    .All(x=>x);
}

bool Bounds(int value, int min){
    return value<0 || value>min;
}

#endregion
class CustomOrderer: IComparer<int>
{
    public IEnumerable<IGrouping<int,int>> ruleset;
    public CustomOrderer(IEnumerable<IGrouping<int,int>> rules){
        this.ruleset = rules;
    }
    public int Compare(int x, int y){
        IGrouping<int, int> group;
        if((group=ruleset.FirstOrDefault(rule => rule.Key==x))!=null &&group.Contains(y))
        {
            return -1;
        }
        if((group=ruleset.FirstOrDefault(rule => rule.Key==y))!=null &&group.Contains(x))
        {
            return 1;
        }
        return 0;
    }
}