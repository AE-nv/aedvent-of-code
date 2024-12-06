class Stats
{
    Stopwatch sw = Stopwatch.StartNew();
    long bytes = GC.GetTotalAllocatedBytes();

    public void Report(string label)
    {
        Console.WriteLine($"{label,-18} took {FormatTime(sw.Elapsed),-6} and allocated {FormatBytes(GC.GetTotalAllocatedBytes() - bytes), 6}");
        sw.Restart();
        bytes = GC.GetTotalAllocatedBytes();
    }
    public void Report<T>(int part, T value)
    {
        Report($"Part {part} = {value}");
    }
    static string FormatBytes(long b)
    {
        double bytes = b;
        string[] sizes = ["B", "KB", "MB", "GB", "TB"];
        int n = 0;
        while (bytes >= 1024 && n < sizes.Length - 1)
        {
            n++;
            bytes /= 1024;
        }
        return $"{bytes:0} {sizes[n]}";
    }
    static string FormatTime(TimeSpan timespan) => timespan switch
    {
        { TotalHours: > 1 } ts => $@"{ts:hh\:mm\:ss}",
        { TotalMinutes: > 1 } ts => $@"{ts:mm\:ss}",
        { TotalSeconds: > 3 } ts => $"{ts.TotalSeconds,3:0}  s",
        { TotalSeconds: > 1 } ts => $@"{ts,3:s\.f}  s",
        { TotalMilliseconds: > 1 } ts => $"{ts.TotalMilliseconds,3:0} ms",
        { TotalMicroseconds: > 1 } ts => $"{ts.TotalMicroseconds,3:0} μs",
        TimeSpan ts => $"{ts.TotalNanoseconds,3:0} ns"
    };
}