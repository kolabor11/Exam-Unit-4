

using System;
using System.Collections.Generic;
using System.IO;

class LogFileParser
{
    public static Dictionary<string, (List<int> times, int fails)> ParseLogFile(string logFilePath)
    {
        var sessionData = new Dictionary<string, (List<int> times, int fails)>();
        string sessionName = Path.GetFileNameWithoutExtension(logFilePath);
        var times = new List<int>();
        int fails = 0;

        foreach (string line in File.ReadLines(logFilePath))
        {
            if (line.StartsWith("[TIME]"))
            {
                times.Add(int.Parse(line.Split(": ")[1]));
            }
            else if (line.StartsWith("[FAIL]"))
            {
                fails++;
            }
        }

        sessionData[sessionName] = (times, fails);
        return sessionData;
    }
}

class PhyscialGameSummary
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Error: No log file path provided.");
            Console.WriteLine("Usage: log-parser <log-file-path>");
            return;
        }

        string logFilePath = args[0];
        if (!File.Exists(logFilePath))
        {
            Console.WriteLine($"Error: Log file '{logFilePath}' not found.");
            return;
        }

        var sessionData = LogFileParser.ParseLogFile(logFilePath);

        if (sessionData.Count == 0)
        {
            Console.WriteLine("No data found in the log file.");
            return;
        }

        foreach (var session in sessionData)
        {
            Console.WriteLine($"Session: {session.Key}");
            Console.WriteLine($"Total time: {session.Value.times.Sum()} seconds");
            Console.WriteLine($"Average time: {session.Value.times.Average()} seconds");
            Console.WriteLine($"Total fails: {session.Value.fails}");
        }
    }
}
