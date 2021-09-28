using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SkySwordKill.Next
{
    public class DialogCommand
    {
        public string command;
        public string[] paramList;
        
        public string charID;
        public string say;

        public DialogEventData bindEventData;

        public bool isEnd;

        public string rawCommand;
    }

    public class DialogOptionCommand
    {
        public string option;
        public string tagEvent;
    }
    
    public class DialogEventData
    {
        #region 字段

        public string id;
        public Dictionary<string, int> character;
        public string[] dialog;
        public string[] option;

        #endregion

        #region 属性



        #endregion

        #region 回调方法



        #endregion

        #region 公共方法

        public DialogCommand GetDialogCommand(int index)
        {
            var str = dialog[index];
            
            var command = new DialogCommand();
            command.bindEventData = this;
            command.isEnd = index == dialog.Length - 1;
            command.rawCommand = str;
            
            var strArr = str.Split('*');
            if (strArr.Length >= 2)
            {
                command.command = strArr[0];
                var body = string.Join("*", strArr.Where((s, i) => i > 0));
                command.paramList = body.Split('#');
            }
            else
            {
                command.command = "";
                var body = strArr[0];
                command.paramList = body.Split('#');

                if (command.paramList.Length >= 2)
                {
                    command.charID = command.paramList[0];
                    command.say = command.paramList[1];
                }
            }

            return command;
        }

        public DialogOptionCommand[] GetOptionCommands()
        {
            var optionCommands = new DialogOptionCommand[option.Length];
            for (int i = 0; i < optionCommands.Length; i++)
            {
                var curOption = new DialogOptionCommand();
                var body = option[i].Split('#');
                curOption.option = body[0];
                if (body.Length >= 2)
                    curOption.tagEvent = body[1];
                else
                    curOption.tagEvent = "";
                optionCommands[i] = curOption;
            }

            return optionCommands;
        }

        #endregion

        #region 私有方法



        #endregion


    }
}