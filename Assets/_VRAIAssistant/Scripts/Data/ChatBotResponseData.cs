using System;
using System.Collections.Generic;

[Serializable]
public class ChatBotResponseData
{
    public string success;
    public Data data;
    public string message;
    public string code;

    [Serializable]
    public class Data
    {
        public string output;
        public string created_at;
        public List<Reference> refrence = new List<Reference>();
        public string username;
    }
 
    [Serializable]
    public class Reference
    {
        public string sentence;
        public string source;
    }
}

