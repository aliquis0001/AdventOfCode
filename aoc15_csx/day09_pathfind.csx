using System.Collections;
List<List<int>> matrix = new();
string currentTown = "";
int startIdx = -1;
int destIdx;
matrix.Add(new List<int>());
foreach (string[] line in File.ReadLines("inputs/09.txt").Select(x=>x.Split(" "))){
    if(line[0]!=currentTown){
        currentTown = line[0];
        startIdx++;
        matrix[startIdx].Add(0);
        destIdx = startIdx+1;
    }
    matrix[startIdx].Add(Int32.Parse(line[line.Length - 1]));
    if(matrix.Count==destIdx){
        matrix.Add(new List<int>());
    }
    matrix[destIdx].Add(Int32.Parse(line[line.Length - 1]));
    destIdx++;
}
matrix.Last().Add(0);

Dictionary<List<int>, int> Checked = new Dictionary<List<int>, int>(new ListComparer());
foreach (var line in matrix){
    foreach (var i in line){
        Console.Write(i.ToString("D3")+", ");
    }
    Console.WriteLine();
}

Console.WriteLine(fish(Enumerable.Range(0,matrix.Count).ToList()));
foreach(var i in Checked){
   // Console.WriteLine(i.ToString());
}

int fish(List<int> visits){
    string indent = string.Concat(Enumerable.Repeat("\t",matrix.Count()-visits.Count()));
    Console.WriteLine(indent+"Visiting: "+BetterPrint(visits)+"{");
    if(visits.Count()==2){
        Console.WriteLine(indent+$"\tDistance: {matrix[visits[0]][visits[1]]}");
        Console.WriteLine(indent+"}");
        return matrix[visits[0]][visits[1]];
    }
    else if(Checked.ContainsKey(visits))
    {
        Console.WriteLine(indent+$"\tCached: {Checked[visits]}");
        Console.WriteLine(indent+"}");
        return Checked[visits];
    }
    else{
        int min = int.MaxValue;
        for(int i = 1; i<visits.Count(); i++){
            var targets = new List<int>(visits);
            targets.RemoveAt(i);
            targets[0]=visits[i];
            min = Math.Min(min, fish(targets)+matrix[visits[0]][visits[i]]);
        }
        Console.WriteLine(indent+$"\tCalculated: {min}");
        Console.WriteLine(indent+"}");
        Checked.Add(visits,min);
        return min;
    }
}

string BetterPrint(List<int> list){
    string output = "";
    foreach(int i in list){
        output+=i;
        output+=", ";
    }
    return output;
}
sealed class ListComparer : EqualityComparer<List<int>>
{
  public override bool Equals(List<int> x, List<int> y)
    => StructuralComparisons.StructuralEqualityComparer.Equals(x?.ToArray(), y?.ToArray());

  public override int GetHashCode(List<int> x)
    => StructuralComparisons.StructuralEqualityComparer.GetHashCode(x?.ToArray());
}