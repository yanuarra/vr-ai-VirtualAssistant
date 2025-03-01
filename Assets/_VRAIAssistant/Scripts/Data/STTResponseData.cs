using System;
using System.Collections.Generic;

[Serializable]
public class STTResponseData 
{
    public string task;
    public string language;
    public double duration;
    public string text;
    public List<Segment> segments;
    public float temperature;
    public float avg_logprob;
    [Serializable]
    public class Segment
    {
        public int id;
        public string text;
        public double start;
        public double end;
        public List<int> tokens = new List<int>();
        public List<Words> words = new List<Words>();
        public double temperature;
        public double avg_logprob;
    }
    [Serializable]
    public class Words
    {
        public string word;
        public double start;
        public double end;
        public int t_dtw;
        public double probability;
    }
}
