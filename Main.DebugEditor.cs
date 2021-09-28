using System;
using UnityEngine;
using Newtonsoft.Json.Linq;

namespace SkySwordKill.Next
{
    public partial class Main
    {
        #region 字段

        public bool isOpen;
        public Vector2 scrollRoll = new Vector2(0, 0);

        private bool isGUIInit = false;
        
        private GUIStyle titleStyle;
        private GUIStyle leftStyle;
        private GUIStyle middleStyle;
        private GUIStyle rightStyle;
        
        #endregion

        #region 属性



        #endregion

        #region 回调方法

        private void Update()
        {
            if (Input.GetKeyDown(debugKeyCode.Value))
                isOpen = !isOpen;
        }

        public void GUIInit()
        {
            if(isGUIInit)
                return;
            titleStyle = new GUIStyle("label")
            {
                fontSize = 36,
                alignment = TextAnchor.UpperCenter
            };
            leftStyle = new GUIStyle("label")
            {
                alignment = TextAnchor.UpperLeft
            };
            middleStyle = new GUIStyle("label")
            {
                alignment = TextAnchor.UpperCenter
            };
            rightStyle = new GUIStyle("label")
            {
                alignment = TextAnchor.UpperRight
            };
            
            isGUIInit = true;
        }

        public void OnGUI()
        {
            if (isOpen)
            {
                GUIInit();
                GUI.Window(50, new Rect(W(0.1f), H(0.1f), W(0.8f), H(0.8f)),
                    DrawDebugWindow, "Next 调试窗口");
            }
        }

        public void DrawDebugWindow(int id)
        {
            GUILayout.Label("Mod列表",titleStyle);
            
            GUILayout.BeginScrollView(scrollRoll, false, true,
                new GUILayoutOption[]
                {
                    GUILayout.MinHeight(H(0.4f))
                });
            foreach (var modConfig in DataPatcher.modConfigs)
            {
                var oldColor = GUI.color;
                
                GUILayout.Box("",new GUILayoutOption[]
                {
                    GUILayout.MinHeight(100)
                });
                var rect = GUILayoutUtility.GetLastRect();
                var infoRect = new Rect(rect)
                {
                    x = rect.x + 10,
                    y = rect.y + 10,
                    height = 20,
                    width = rect.width - 20
                };
                var descRect = new Rect(rect)
                {
                    x = rect.x + 10,
                    y = rect.y + 30,
                    height = 20,
                    width = rect.width - 20
                };
                var dirRect = new Rect(rect)
                {
                    x = rect.x + 10,
                    y = rect.y + 50,
                    height = 20,
                    width = rect.width - 20
                };
                var stateRect = new Rect(rect)
                {
                    x = rect.x + 10,
                    y = rect.y + 70,
                    height = 20,
                    width = rect.width - 20
                };
                
                var modName = modConfig.GetValue("Name")?.Value<string>() ?? "未知";
                var modAuthor = modConfig.GetValue("Author")?.Value<string>() ?? "未知";
                var modVersion = modConfig.GetValue("Version")?.Value<string>() ?? "未知";
                var modDescription = modConfig.GetValue("Description")?.Value<string>() ?? "无";
                
                var modDir = modConfig.GetValue("Dir")?.Value<string>() ?? "未知";
                
                var modState = modConfig.GetValue("success")?.Value<bool>() == true; 
                
                GUI.color = Color.white;
                GUI.Label(infoRect,$"名称：{modName}",leftStyle);
                GUI.Label(infoRect,$"作者：{modAuthor}",middleStyle);
                GUI.Label(infoRect,$"版本：{modVersion}",rightStyle);
                
                GUI.Label(descRect,$"描述：{modDescription}");
                
                GUI.Label(dirRect,$"目录：{modDir}");

                
                if (modState)
                {
                    GUI.color = Color.cyan;
                    GUI.Label(stateRect,$"加载成功。");
                }
                else
                {
                    GUI.color = Color.red;
                    GUI.Label(stateRect,$"加载失败！");
                }

                GUI.color = oldColor;
            }
            GUILayout.EndScrollView();
        }

        #endregion

        #region 公共方法



        #endregion

        #region 私有方法

        private float W(float size)
        {
            return Screen.width * size;
        }
        
        private float H(float size)
        {
            return Screen.height * size;
        }

        #endregion

        
    }
}