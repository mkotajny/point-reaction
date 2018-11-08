public class GameVersion
{
    public int FirstVersionNumber { get; private set; }
    public int SecondVersionNumber { get; private set; }
    public int ThirdVersionNumber { get; private set; }
    public int VersionValue { get; private set; }
    public string VersionString { get; private set; }

    public GameVersion(string versionString)
    {
        VersionString = versionString;
        int dotIndex = VersionString.IndexOf('.');
        int dotIndex2 = VersionString.IndexOf('.', dotIndex + 1);
        FirstVersionNumber = System.Convert.ToInt16(VersionString.Substring(0, dotIndex));
        SecondVersionNumber = System.Convert.ToInt16(VersionString.Substring(dotIndex + 1, dotIndex2 - dotIndex - 1));
        ThirdVersionNumber = System.Convert.ToInt16(VersionString.Substring(dotIndex2 + 1));
        VersionValue = FirstVersionNumber * 10000 + SecondVersionNumber * 100 + ThirdVersionNumber;
    }
}
