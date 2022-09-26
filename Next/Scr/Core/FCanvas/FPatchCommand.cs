using System.Collections.Generic;

namespace SkySwordKill.Next.FCanvas;

public class FPatchCommand
{
    public string CmdType;
    public string CmdParams;

    private string[] paramArray;

    private string[] ParamArray
    {
        get
        {
            if (paramArray == null)
            {
                paramArray = CmdParams.Split('#');
            }

            return paramArray;
        }
    }

    public int GetParamInt(int index,int defaultValue = 0)
    {
        if (index >= ParamArray.Length)
        {
            return defaultValue;
        }
        return int.Parse(ParamArray[index]);
    }

    public string GetParamString(int index, string defaultValue = "")
    {
        if (index >= ParamArray.Length)
        {
            return defaultValue;
        }
        return ParamArray[index];
    }
}