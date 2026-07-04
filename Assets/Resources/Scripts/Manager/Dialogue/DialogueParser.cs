using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{
    public struct ParsedLine
    {
        public string Action;
        public string Detail;
        public string BGM;
        public string Result;
        public int LineNum;
    }

    public List<ParsedLine> Parse(string tsvFile)
    {
        List<ParsedLine> parsedLines = new List<ParsedLine>();
        string[] lines = tsvFile.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            if (string.IsNullOrWhiteSpace(line)) continue;

            List<string> parts = new List<string>(line.Split('	'));

            while (parts.Count < 11)
            {
                parts.Add("");
            }

            for (int j = 0; j < parts.Count; j++)
            {
                parts[j] = parts[j].Trim();
            }

            string action = parts[0]; //첫번째 열(행동)을 action변수에 저장.
            string detail = parts[1];
            string bgm = parts[2];
            string result = parts[3];

            ParsedLine parsedLine = new ParsedLine
            {
                Action = action,
                Detail = detail,
                BGM = bgm,
                Result = result,
                LineNum = i
            };

            parsedLines.Add(parsedLine);
        }
        Debug.Log(parsedLines.Count);
        return parsedLines;
    }
}
