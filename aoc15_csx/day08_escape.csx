using System.Text.RegularExpressions;
var Escapes = new Regex("""\\"|\\\\|\\x[0-9a-f]{2}""");
var strings = File.ReadLines("inputs/08.txt");
var baseLength = strings.Select(s => s.Length).Sum();
Console.WriteLine(baseLength - strings.Select(s => Escapes.Replace(s.Substring(1,s.Length-2),"1").Length).Sum());
Console.WriteLine(strings.Select(s => s.Replace("\\","\\\\").Replace("\"","\\\"").Length+2).Sum()-baseLength);