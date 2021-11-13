using System;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using Newtonsoft.Json.Linq;

namespace SkySwordKill.Next
{
    public partial class Main
    {
        #region 字段

        public bool isWinOpen;
        private Vector2 scrollRoll = new Vector2(0, 0);

        private bool isGUIInit = false;
        private int toolbarSelected = 0;
        private Rect winRect;

        private string inputEvent;
        private string testCommand;
        
        private GUIStyle titleStyle;
        private GUIStyle leftStyle;
        private GUIStyle middleStyle;
        private GUIStyle rightStyle;

        private string[] toolbarList =
        {
            "Mod列表",
            "剧情调试"
        };
        
        #endregion

        #region 属性



        #endregion

        #region 回调方法

        private void Update()
        {
            if (Input.GetKeyDown(debugKeyCode.Value))
            {
                isWinOpen = !isWinOpen;
                winRect = new Rect(W(0.1f), H(0.1f), W(0.8f), H(0.8f));
            }
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
            if(!isGUIInit)
                GUIInit();
            if (isWinOpen)
            {
                GUI.Window(50, winRect, DrawDebugWindow, $"Next v{MOD_VERSION}");
            }
        }

        #endregion

        #region 公共方法



        #endregion

        #region 私有方法

        private void DrawDebugWindow(int id)
        {
            GUILayout.BeginHorizontal();
            {
                if (debugMode.Value)
                {
                    GUILayout.Label($"按 {debugKeyCode.Value.ToString()} 开关此窗口。当前模式：调试模式（调试模式可从ModConfig开关）");
                    toolbarSelected = GUILayout.Toolbar(toolbarSelected, toolbarList);
                }
                else
                {
                    GUILayout.Label($"按 {debugKeyCode.Value.ToString()} 开关此窗口。当前模式：普通模式（调试模式可从ModConfig开关）");
                    toolbarSelected = 0;
                }

                GUILayout.FlexibleSpace();

                
                if (debugMode.Value)
                {
                    if (GUILayout.Button("重置Base生成（重启游戏生效）"))
                    {
                        gameVersion.Value = "";
                    }
                }
                // 该部分不简化是为了避免反复操作 openInStart.Value 属性
                bool isPop = openInStart.Value;
                isPop = GUILayout.Toggle(isPop, "游戏启动时弹出");
                if (isPop != openInStart.Value)
                    openInStart.Value = isPop;

                if (GUILayout.Button("关闭"))
                    isWinOpen = false;
            }
            GUILayout.EndHorizontal();

            switch (toolbarSelected)
            {
                case 0:
                    DrawModList();
                    break;
                case 1:
                    DrawEventDebug();
                    break;
            }
        }

        private void DrawModList()
        {
            GUILayout.Label("Mod列表",titleStyle);
            GUILayout.BeginScrollView(scrollRoll, false, true,
                new GUILayoutOption[]
                {
                    GUILayout.MinHeight(winRect.height * 0.8f)
                });
            foreach (var modConfig in ModManager.modConfigs)
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
                
                var modName = modConfig.Name ?? "未知";
                var modAuthor = modConfig.Author ?? "未知";
                var modVersion = modConfig.Version ?? "未知";
                var modDescription = modConfig.Description ?? "无";
                
                var modDir = modConfig.Path ?? "未知";
                
                var modState = modConfig.Success; 
                
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

        private void DrawEventDebug()
        {
            GUILayout.Label("剧情调试",titleStyle);
            var lastRect = GUILayoutUtility.GetLastRect();
            var lastHeight = lastRect.y + lastRect.height;
            var debugArea1 = new Rect(winRect)
            {
                x = 20,
                y = 20 + 100,
                width = winRect.width / 2 - 20,
                height = winRect.height - (10 + 100)
            };
            
            var debugArea2 = new Rect(winRect)
            {
                x = winRect.width / 2 + 20,
                y = 20 + 100,
                width = winRect.width / 4 - 20,
                height = winRect.height - (10 + 150)
            };
            
            var debugArea3 = new Rect(winRect)
            {
                x = winRect.width / 4 * 3,
                y = 20 + 100,
                width = winRect.width / 4 - 20,
                height = winRect.height - (10 + 150)
            };
            
            GUILayout.BeginArea(debugArea1);
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("剧情ID");
                    inputEvent = GUILayout.TextArea(inputEvent);
                    if (GUILayout.Button("执行"))
                    {
                        DialogAnalysis.StartDialogEvent(inputEvent);
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(20);
                GUILayout.Label("剧情指令调试");
                testCommand = GUILayout.TextArea(testCommand, new GUILayoutOption[]
                {
                    GUILayout.MinHeight(debugArea1.height - 200)
                });
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("运行"))
                    {
                        if(!string.IsNullOrEmpty(testCommand))
                            DialogAnalysis.StartTestDialogEvent(testCommand);
                    }

                    if (GUILayout.Button("复制原数据"))
                    {
                        GUIUtility.systemCopyBuffer = testCommand;
                    }

                    if (GUILayout.Button("复制Json数据"))
                    {
                        GUIUtility.systemCopyBuffer = JArray.FromObject(testCommand
                            .Split('\n')
                            .Where(str=>!string.IsNullOrWhiteSpace(str))
                            .ToArray()).ToString(Formatting.Indented);
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
            
            GUILayout.BeginArea(debugArea2);
            {
                GUI.Box(debugArea2,"");
                GUILayout.Label("Int变量调试窗口");
                GUILayout.BeginScrollView(scrollRoll, false, true,
                    new GUILayoutOption[]
                    {
                        GUILayout.MinHeight(debugArea2.height - 50)
                    });
                {
                    foreach (var pair in DialogAnalysis.GetAllInt())
                    {
                        GUILayout.Box("",new GUILayoutOption[]
                        {
                            GUILayout.MinHeight(26)
                        });
                        var rect = GUILayoutUtility.GetLastRect();
                        var infoRect = new Rect(rect)
                        {
                            x = rect.x + 10,
                            height = 24,
                            width = rect.width - 20
                        };
                        GUI.Label(infoRect,$"{pair.Key}",leftStyle);
                        GUI.Label(infoRect,$"{pair.Value}",rightStyle);
                    }
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndArea();
            
            GUILayout.BeginArea(debugArea3);
            {
                GUI.Box(debugArea3,"");
                GUILayout.Label("String变量调试窗口");
                GUILayout.BeginScrollView(scrollRoll, false, true,
                    new GUILayoutOption[]
                    {
                        GUILayout.MinHeight(debugArea3.height - 50)
                    });
                {
                    foreach (var pair in DialogAnalysis.GetAllStr())
                    {
                        GUILayout.Box("",new GUILayoutOption[]
                        {
                            GUILayout.MinHeight(26)
                        });
                        var rect = GUILayoutUtility.GetLastRect();
                        var infoRect = new Rect(rect)
                        {
                            x = rect.x + 10,
                            height = 24,
                            width = rect.width - 20
                        };
                        GUI.Label(infoRect,$"{pair.Key}",leftStyle);
                        GUI.Label(infoRect,$"{pair.Value}",rightStyle);
                    }
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }

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