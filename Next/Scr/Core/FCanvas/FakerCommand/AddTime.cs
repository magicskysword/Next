using Fungus;

namespace SkySwordKill.Next.FCanvas.FakerCommand;

[FCommandBinder(typeof(Fungus.AddTime))]
public class AddTime : FCommand
{
    public int Year;
    public int Month;
    public int Day;
    public bool IsBalance;
    
    public override void ReadCommand(Command command)
    {
        base.ReadCommand(command);
        var cmdAddTime = (Fungus.AddTime)command;
        Year = cmdAddTime.Year;
        Month = cmdAddTime.Month;
        Day = cmdAddTime.Day;
        IsBalance = !cmdAddTime.IsNoJieSuan;
    }

    public override string GetSummary()
    {
        return $"增加时间 : {Year}年{Month}月{Day}日  结算:{IsBalance}";
    }
}