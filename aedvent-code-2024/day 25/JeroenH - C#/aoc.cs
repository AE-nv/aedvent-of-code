var stats = new Stats();

var stream = new StreamReader("input.txt");
List<long> locks = [];
List<long> keys = [];

var buffer = new char[6*7+1];
while (!stream.EndOfStream)
{
    stream.ReadBlock(buffer);
    long value = 0;
    foreach (var c in buffer)
    {
        value = c switch 
        {
            '#' => (value << 1) | 1,
            '.' => value << 1,
            '\n' => value
        };
    }
    (buffer[0] switch {'#' => locks, '.' => keys}).Add(value);
}
stats.Report("Init");

int part1 = 0;
foreach (long l in locks) {
    foreach (long k in keys) {
        if ((l & k) == 0) {
            ++part1;
        }
    }
}

Console.WriteLine(part1);
stats.Report(1, part1);