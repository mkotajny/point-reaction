using System.Collections.Generic;

public static class ConvertionTools  {

    public static int GestIntFromString(string stringValue, int valueOnFail = 0)
    {
        int intValue;
        try { int.TryParse(stringValue, out intValue); }
        catch { intValue = valueOnFail; }

        return intValue;
    }
}
