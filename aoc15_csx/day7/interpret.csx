using System.Text.RegularExpressions;
using System.Threading;

#region Variables
Dictionary<string, ushort> Wires = new Dictionary<string, ushort>();
Dictionary<string, ManualResetEvent> WaitingWires = new Dictionary<string, ManualResetEvent>();
Regex MatchOpcode = new Regex(@"[A-Z]+");
Regex MatchOperand = new Regex(@"(?:[a-z]+|\d+)(?= )");
Regex MatchResult = new Regex(@"[a-z]+$");
#endregion Variables

#region Display
var ops = File.ReadLines("input");
foreach (string op in ops){
    Exec(op);
}
Console.WriteLine($"value of a: {Wires["a"]}");
#endregion

#region Methods
async Task Exec(string operation)
{
    var opcode = MatchOpcode.Match(operation);
    var operand = MatchOperand.Match(operation);
    var result = MatchResult.Match(operation);
    if (opcode.Success)
    {
        switch (opcode.Value)
        {
            case "NOT":
                await Not(operand.Value, result.Value);
                break;
            case "AND":
                await And(operand.Value, operand.NextMatch().Value, result.Value);
                break;
            case "OR":
                await Or(operand.Value, operand.NextMatch().Value, result.Value);
                break;
            case "LSHIFT":
                await Lshift(operand.Value, operand.NextMatch().Value, result.Value);
                break;
            case "RSHIFT":
                await Rshift(operand.Value, operand.NextMatch().Value, result.Value);
                break;
        }
    }
    else
        Assign(await Value(operand.Value), result.Value);
}

async Task Not(string input, string output)
{
    Console.WriteLine($"NOT    | {input}");
    ushort a = await Value(input);
    Assign((ushort)(~a), output);
}
async Task And(string left, string right, string output)
{
    Console.WriteLine($"AND    | {left} \t| {right}");
    ushort a = await Value(left);
    ushort b = await Value(right);
    Assign((ushort)(a & b), output);
}
async Task Or(string left, string right, string output)
{
    Console.WriteLine($"OR     | {left} \t| {right}");
    var a = await Value(left);
    var b = await Value(right);
    Assign((ushort)(a | b), output);
}

async Task Lshift(string left, string right, string output)
{
    Console.WriteLine($"LSHIFT | {left} \t| {right}");
    var a = await Value(left);
    var b = await Value(right);
    Assign((ushort)(a << b), output);
}

async Task Rshift(string left, string right, string output)
{
    Console.WriteLine($"RSHIFT | {left} \t| {right}");
    var a = await Value(left);
    var b = await Value(right);
    Assign((ushort)(a >> b), output);
}

async Task<ushort> Value(string value)
{
    if (ushort.TryParse(value, out var i))
    {
        return i;
    }
    else if (Wires.ContainsKey(value))
    {
        return Wires[value];
    }
    else if (WaitingWires.ContainsKey(value)){
        Console.WriteLine($"WAIT   | {value}");
        WaitingWires[value].WaitOne();
        return Wires[value];
    }
    else
    {
        Console.WriteLine($"WAIT   | {value}");
        WaitingWires[value] = new ManualResetEvent(false);
        WaitingWires[value].WaitOne();
        return Wires[value];
    }
}

void Assign(ushort value, string wire)
{
    Wires[wire] = value;
    if (WaitingWires.ContainsKey(wire))
    {
        WaitingWires[wire].Set();
    }
}
#endregion