using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TTSResponseData
{
        public string audio_id;
        public int sample_rate;
        public double duration;
        public string type;
        public List<Viseme> viseme;
        public string speaker_name;
        public double inference_time;

    [Serializable]
    public class Viseme
    {
        public string phoneme;
        public double duration;
        public double offset;
        public int id;
        public string label;
    }
}
