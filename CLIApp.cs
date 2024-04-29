using System;
using System.Collections.Generic;
using System.IO;

class LogFileParser
{
    public static Dictionary<string, (List<int> times, int fails)> ParseLogFile(string logFilePath)
    {
        var sessionData = new Dictionary<string, (List<int> times, int fails)>();

        try
        {
            using var reader = new StreamReader(logFilePath);
            string sessionName = string.Empty;
            var times = new List<int>();
            int fails = 0;

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split(':');
                if (parts.Length >= 2)
                {
                    string code = parts[0].Trim();
                    string logEntry = parts[1].Trim();

                    if (logEntry.StartsWith("[START]"))
                    {
                        sessionName = code;
                        times = new List<int>();
                        fails = 0;
                    }
                    else if (logEntry.StartsWith("[TIME]"))
                    {
                        if (int.TryParse(logEntry.Split(": ")[1], out int time))
                        {
                            times.Add(time);
                        }
                    }
                    else if (logEntry.StartsWith("[FAIL]"))
                    {
                        fails++;
                    }
                    else if (logEntry.StartsWith("[END]"))
                    {
                        if (sessionName != null)
                        {
                            sessionData.Add(sessionName, (times, fails));
                            sessionName = null;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing log file: {ex.Message}");
        }

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
            double averageTime = session.Value.times.Average();
            Console.WriteLine($"Average time: {averageTime:F2} seconds");

            Console.WriteLine($"Total fails: {session.Value.fails}");
        }
    }
}