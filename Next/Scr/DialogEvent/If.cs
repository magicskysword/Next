using System;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("If")]
    public class If : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            var condition = command.GetStr(0);
            if (string.IsNullOrWhiteSpace(condition) || condition.Equals("true",StringComparison.OrdinalIgnoreCase))
            {
                var newCommand = string.Join("#", command.ParamList, 1, command.ParamList.Length - 1);
                var newCommandData = new DialogCommand(newCommand, command.BindEventData, env, command.IsEnd);
                DialogAnalysis.RunDialogEventCommand(newCommandData, env, callback);
            }
            else
            {
                callback?.Invoke();
            }
        }
    }
}