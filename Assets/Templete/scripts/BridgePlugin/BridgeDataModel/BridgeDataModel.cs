using System;

[Serializable]
public class CloseAppClass
{
    public bool generateCsvReport;
    public string action;
}

[Serializable]
public class StartAppClass
{
    public int[] settings;
    public string sessionId;
}