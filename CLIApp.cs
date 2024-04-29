

using System;
using System.Collections.Generic;

class LogFileParser
{
    public Dictionary<string, (List<int> times, int fails)> ParseLogFile(string logFilePath)
    {
        var sessionData = new Dictionary<string, (List<int> times, int fails)>();
        string sessionName = Path.GetFileNameWithoutExtension(logFilePath);
        List<int> times = new List<int>();
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
