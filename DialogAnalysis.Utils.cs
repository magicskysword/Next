using System;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine.Events;

namespace SkySwordKill.Next
{
    public partial class DialogAnalysis
    {
        #region 字段



        #endregion

        #region 属性



        #endregion

        #region 回调方法



        #endregion

        #region 公共方法

        public static string AnalysisInlineScript(string text,DialogEnvironment env)
        {
            StringBuilder finallyText = new StringBuilder(text);
            Regex regex = new Regex(@"\[&(?<expression>[\s\S]*?)&]");
            
            var matches = regex.Matches(text);
            var evaluate = DialogAnalysis.GetEvaluate(env);
            foreach(Match match in matches)
            {
                var expression = match.Groups["expression"].Value;
                var getValue = evaluate.Evaluate(expression).ToString();
                finallyText.Replace(match.Value, getValue);
            }
            return finallyText.ToString();
        }

        public static void SetCharacter(int getNum)
        {
            var sayDialog = Fungus.SayDialog.GetSayDialog();
            var num = getNum;
            if (NpcJieSuanManager.inst.ImportantNpcBangDingDictionary.ContainsKey(num))
            {
                num = NpcJieSuanManager.inst.ImportantNpcBangDingDictionary[num];
            }
            sayDialog.SetCharacter(null,num);
            sayDialog.SetCharacterImage(null,num);
        }

        public static void Say(string text,Action callback)
        {
            var say = Fungus.SayDialog.GetSayDialog();
            say.SetActive(true);
            say.Say(text,true,true,true,true,
                false,null, ()=>
                {
                    callback?.Invoke();
                });
        }

        public static void ClearMenu()
        {
            var menuDialog = Fungus.MenuDialog.GetMenuDialog();
            menuDialog.Clear();
        }
        
        public static void AddMenu(string text,Action callback)
        {
            var menuDialog = Fungus.MenuDialog.GetMenuDialog();
            menuDialog.SetActive(true);

            void OptionAction()
            {
                menuDialog.StopAllCoroutines();
                menuDialog.Clear();
                menuDialog.HideSayDialog();
                menuDialog.SetActive(false);
                var say = Fungus.SayDialog.GetSayDialog();
                say.Stop();
                callback?.Invoke();
            }

            addOptionMethod.Value.Invoke(menuDialog, new object[] { text, true, false, (UnityAction)OptionAction });
        }

        public static void SetInt(string key,int value)
        {
            key = $"next_Int_{key}";
            Tools.instance.getPlayer().AvatarChengJiuData.SetField(key,value);
        }

        public static int GetInt(string key)
        {
            key = $"next_Int_{key}";
            var field = Tools.instance.getPlayer().AvatarChengJiuData.GetField(key);
            if (field == null || field.type != JSONObject.Type.NUMBER)
                return 0;
            return field.I;
        }
        
        public static void SetStr(string key,string value)
        {
            key = $"next_Str_{key}";
            Tools.instance.getPlayer().AvatarChengJiuData.SetField(key,value);
        }

        public static string GetStr(string key)
        {
            key = $"next_Str_{key}";
            var field = Tools.instance.getPlayer().AvatarChengJiuData.GetField(key);
            if (field == null || field.type != JSONObject.Type.STRING)
                return string.Empty;
            return field.Str;
        }

        #endregion

        #region 私有方法



        #endregion

        
    }
}