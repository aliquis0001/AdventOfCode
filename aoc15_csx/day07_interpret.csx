using System.Text.RegularExpressions;
using System.Threading;

#region Variables
Dictionary<string, ushort> Wires = new Dictionary<string, ushort>();
Dictionary<string, ManualResetEvent> WaitingWires = new Dictionary<string, ManualResetEvent>();
Regex MatchOpcode = new Regex(@"[A-Z]+");
Regex MatchOperand = new Regex(@"(?:[a-z]+|\d+)(?= )");
Regex MatchResult = new Regex(@"[a-z]+$");

Semaphore registers = new Semaphore(1, 1);
#endregion Variables

#region Display
Console.WriteLine("Part One:");
var ops = File.ReadLines("inputs/07.txt");
foreach (string op in ops)
{
    new Thread(() => Exec(op)).Start();
}
var aResult = Value("a");
Console.WriteLine(aResult);
Console.WriteLine("Part Two:");
Wires.Clear();
WaitingWires.Clear();
Assign(aResult,"b");
foreach (string op in ops)
{
    new Thread(() => Exec(op)).Start();
}
Console.WriteLine(Value("a"));
#endregion

#region Methods
void Exec(string operation)
{
    var opcode = MatchOpcode.Match(operation);
    var operand = MatchOperand.Match(operation);
    var result = MatchResult.Match(operation);
    if (opcode.Success)
    {
        switch (opcode.Value)
        {
            case "NOT":
                Not(operand.Value, result.Value);
                break;
            case "AND":
                And(operand.Value, operand.NextMatch().Value, result.Value);
                break;
            case "OR":
                Or(operand.Value, operand.NextMatch().Value, result.Value);
                break;
            case "LSHIFT":
                Lshift(operand.Value, operand.NextMatch().Value, result.Value);
                break;
            case "RSHIFT":
                Rshift(operand.Value, operand.NextMatch().Value, result.Value);
                break;
        }
    }
    else
        Assign(Value(operand.Value), result.Value);
}

void Not(string input, string output)
{
    ushort a = Value(input);
    Assign((ushort)(~a), output);
}
void And(string left, string right, string output)
{
    ushort a = Value(left);
    ushort b = Value(right);
    Assign((ushort)(a & b), output);
}
void Or(string left, string right, string output)
{
    var a = Value(left);
    var b = Value(right);
    Assign((ushort)(a | b), output);
}

void Lshift(string left, string right, string output)
{
    var a = Value(left);
    var b = Value(right);
    Assign((ushort)(a << b), output);
}

void Rshift(string left, string right, string output)
{
    var a = Value(left);
    var b = Value(right);
    Assign((ushort)(a >> b), output);
}

ushort Value(string value)
{
    if (ushort.TryParse(value, out var i))
    {
        return i;
    }
    registers.WaitOne();
        if (Wires.ContainsKey(value))
        {
            registers.Release();
            return Wires[value];
        }
        else if (!WaitingWires.ContainsKey(value))
        {
            WaitingWires[value] = new ManualResetEvent(false);
            registers.Release();
            WaitingWires[value].WaitOne();
            return Wires[value];
        }
        else
        {
            registers.Release();
            WaitingWires[value].WaitOne();
            return Wires[value];
        }
}

void Assign(ushort value, string wire)
{
    if(Wires.ContainsKey(wire)){
        return;
    }
    registers.WaitOne();
    Wires[wire] = value;
    if (WaitingWires.ContainsKey(wire))
    {
        WaitingWires[wire].Set();
    }
    else
    {
        WaitingWires[wire] = new ManualResetEvent(true);
    }
    registers.Release();
}
#endregion