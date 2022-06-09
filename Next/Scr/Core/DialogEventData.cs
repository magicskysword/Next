using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace SkySwordKill.Next
{
    public class DialogCommand
    {
        public string Command { get; set; } 
        public string[] ParamList { get; set; } 

        public DialogEventData BindEventData { get; set; } 

        public bool IsEnd { get; set; } 

        public string RawCommand { get; set; } 

        public DialogCommand()
        {
            
        }

        public DialogCommand(string commandStr,DialogEventData bindEventData,DialogEnvironment env,bool isEnd)
        {
            RawCommand = commandStr;
            BindEventData = bindEventData;
            IsEnd = isEnd;
            var evaluateText = DialogAnalysis.AnalysisInlineScript(RawCommand, env);
            var strArr = evaluateText.Split('*');
            var posSharp = evaluateText.IndexOf('#');
            var posStar = evaluateText.IndexOf('*');
            
            // 确保第一个星号在井号前面
            if (strArr.Length >= 2 && (posStar < posSharp || posSharp == -1))
            {
                Command = strArr[0];
                var body = string.Join("*", strArr.Where((s, i) => i > 0));
                ParamList = body.Split('#');
            }
            else
            {
                Command = "";
                var body = strArr[0];
                ParamList = body.Split('#');
            }
        }

        public DialogCommand(string commandHead, string[] paramList, DialogEventData bindEventData,bool isEnd)
        {
            RawCommand = $"Call {commandHead}()";
            Command = commandHead;
            ParamList = paramList;
            BindEventData = bindEventData;
            IsEnd = isEnd;
        }

        public int GetInt(int index,int defaultValue = 0)
        {
            if (index >= ParamList.Length)
                return defaultValue;
            return Convert.ToInt32(ParamList[index]);
        }
        
        public float GetFloat(int index,float defaultValue = 0)
        {
            if (index >= ParamList.Length)
                return defaultValue;
            return Convert.ToSingle(ParamList[index]);
        }
        
        public bool GetBool(int index,bool defaultValue = false)
        {
            if (index >= ParamList.Length)
                return defaultValue;
            return Convert.ToInt32(ParamList[index]) != 0;
        }
        
        public string GetStr(int index,string defaultValue = "")
        {
            if (index >= ParamList.Length)
                return defaultValue;
            return ParamList[index];
        }
    }

    public class DialogOptionCommand
    {
        public string Option { get; set; }
        public string TagEvent { get; set; }
        public string Condition { get; set; } 

        public DialogOptionCommand()
        {
            
        }

        public DialogOptionCommand(string optionStr)
        {
            var body = optionStr.Split('#');
            Option = body[0];
            TagEvent = body.Length >= 2 ? body[1] : "";
            Condition = body.Length >= 3 ? body[2] : "";
        }
    }
    
    public class DialogEventData
    {
        #region 字段
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; } 
        [JsonProperty(PropertyName = "character")]
        public Dictionary<string, int> Character { get; set; } 
        [JsonProperty(PropertyName = "dialog")]
        public string[] Dialog { get; set; } 
        [JsonProperty(PropertyName = "option")]
        public string[] Option { get; set; } 

        #endregion

        #region 属性



        #endregion

        #region 回调方法



        #endregion

        #region 公共方法

        public DialogCommand GetDialogCommand(int index,DialogEnvironment env)
        {
            var rawText = Dialog[index];

            var isEnd = index == Dialog.Length - 1;
            var command = new DialogCommand(rawText, this, env, isEnd);

            return command;
        }

        public DialogOptionCommand[] GetOptionCommands()
        {
            var optionCommands = new DialogOptionCommand[Option.Length];
            for (int i = 0; i < optionCommands.Length; i++)
            {
                var curOption = new DialogOptionCommand(Option[i]);
                optionCommands[i] = curOption;
            }

            return optionCommands;
        }

        #endregion

        #region 私有方法



        #endregion


    }

    public class DialogEventRtData
    {
        public DialogEventData BindEventData { get; set; }
        public DialogEnvironment BindEnv { get; set; }
    }
}