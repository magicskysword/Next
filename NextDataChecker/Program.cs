// See https://aka.ms/new-console-template for more information

using SkySwordKill.NextDataChecker;

var dataRootPath = "../../Next/NextConfig/language/schinese/Editor/";
var seidCheckers = new List<DataSeidMetaChecker>()
{
    new DataSeidMetaChecker("BuffSeidJson", dataRootPath + "Meta/BuffSeidMeta.json"),
    new DataSeidMetaChecker("SkillSeidJson", dataRootPath + "Meta/SkillSeidMeta.json"),
    new DataSeidMetaChecker("CrateAvatarSeid", dataRootPath + "Meta/CreateAvatarSeidMeta.json"),
    new DataSeidMetaChecker("ItemsSeidJsonData", dataRootPath + "Meta/ItemUseSeidMeta.json"),
    new DataSeidMetaChecker("EquipSeidJsonData", dataRootPath + "Meta/ItemEquipSeidMeta.json"),
};


foreach (var checker in seidCheckers)
{
    checker.Check();
}

var totalError = seidCheckers.Sum(c => c.ErrorCount);
if (totalError == 0)
{
    Console.WriteLine("未发现错误，检查程序结束。");
    return 0;
}


Console.Write($"共发现{totalError}个错误，是否要对数据进行修复？(y/n)");
var c = Console.ReadKey();
if (c.KeyChar != 'y')
{
    return 0;
}
Console.Write("\n");

foreach (var checker in seidCheckers)
{
    checker.Repair();
}

return 0;

public static class Logger
{
    public static void Info(object o)
    {
        var oldColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(o);
        Console.ForegroundColor = oldColor;
    }
    
    public static void Warning(object o)
    {
        var oldColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(o);
        Console.ForegroundColor = oldColor;
    }
    
    public static void Error(object o)
    {
        var oldColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(o);
        Console.ForegroundColor = oldColor;
    }
}