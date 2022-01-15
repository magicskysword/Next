using System;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using Newtonsoft.Json.Linq;
using SkySwordKill.Next.Mod;
using SkySwordKill.Next.XiaoYeGUI;
using UnityEngine.SceneManagement;

namespace SkySwordKill.Next
{
    public partial class Main
    {
        #region 字段

        [NonSerialized]
        public bool isWinOpen;
        private Vector2 scrollRollLanguages = new Vector2(0, 0);
        private Vector2 scrollRollMods = new Vector2(0, 0);
        private Vector2 scrollRollDebugString = new Vector2(0, 0);
        private Vector2 scrollRollDebugInt = new Vector2(0, 0);

        private bool isGUIInit = false;
        private int toolbarSelected = 0;
        private Rect languageRect;
        private Rect winRect;

        private string inputEvent;
        private string testCommand;
        
        private GUIStyle labelTitleStyle;
        private GUIStyle labelLeftStyle;
        private GUIStyle labelMiddleStyle;
        private GUIStyle labelRightStyle;

        private bool isSelectedLanguage = false;
        
        private RayBlocker rayBlocker;

        #endregion

        #region 属性



        #endregion

        #region 回调方法

        private void Update()
        {
            if (Input.GetKeyDown(winKeyCode.Value))
            {
                isWinOpen = !isWinOpen;
            }
        }

        public void GUIInit()
        {
            if(isGUIInit)
                return;

            rayBlocker = RayBlocker.CreateRayBlock();
            
            languageRect = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 150, 400, 300);
            winRect = new Rect(W(0.1f), H(0.1f), W(0.8f), H(0.8f));
            
            labelTitleStyle = new GUIStyle("label")
            {
                fontSize = 36,
                alignment = TextAnchor.UpperCenter
            };
            labelLeftStyle = new GUIStyle("label")
            {
                alignment = TextAnchor.UpperLeft
            };
            labelMiddleStyle = new GUIStyle("label")
            {
                alignment = TextAnchor.UpperCenter
            };
            labelRightStyle = new GUIStyle("label")
            {
                alignment = TextAnchor.UpperRight
            };
            
            isGUIInit = true;
        }

        public void OnGUI()
        {
            GUIInit();

            var oldSkin = GUI.skin;
            GUI.skin = InterfaceMaker.CustomSkin;
            
            if (isSelectedLanguage || nextLanguage == null)
            {
                GUI.Window(20, languageRect, DrawLanguageSelectWindow, $"Next v{MOD_VERSION}");
                rayBlocker.SetSize(languageRect);
                rayBlocker.OpenBlocker();
            }
            else if (isWinOpen)
            {
                GUI.Window(50, winRect, DrawDebugWindow, $"Next v{MOD_VERSION}");
                rayBlocker.SetSize(winRect);
                rayBlocker.OpenBlocker();
            }
            else
            {
                rayBlocker.CloseBlocker();
            }

            GUI.skin = oldSkin;
        }

        #endregion

        #region 公共方法



        #endregion

        #region 私有方法

        private void DrawLanguageSelectWindow(int id)
        {
            if (NextLanguage.languages.Count == 0)
            {
                GUILayout.Label("请重写下载该Mod或下载Mod语言文件！",labelTitleStyle);
                GUILayout.Label("Please re-download the Mod or download the Mod language file！",labelTitleStyle);
                return;
            }
            
            GUILayout.Label("请选择Mod语言",labelMiddleStyle);
            GUILayout.Label("Choose Mod Language",labelMiddleStyle);
            
            scrollRollLanguages = GUILayout.BeginScrollView(scrollRollLanguages);
            {
                foreach (var pair in NextLanguage.languages)
                {
                    if (GUILayout.Button($"{pair.Key} ({pair.Value.LanguageName})"))
                    {
                        SelectLanguage(pair.Value);
                        isSelectedLanguage = false;
                    }
                }
            }
            GUILayout.EndScrollView();
        }

        private void DrawDebugWindow(int id)
        {
            DrawHeaderBar();

            switch (toolbarSelected)
            {
                case 0:
                    DrawModList();
                    break;
                case 1:
                    DrawDramaDebug();
                    break;
            }
            
            GUILayout.FlexibleSpace();

            DrawBottomBar();
        }

        private void DrawHeaderBar()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(string.Format("HeaderBar.HotkeyTip".I18N(),winKeyCode.Value.ToString()));
                // 该部分不简化是为了避免反复操作 debugMode.Value 属性
                var debugModeOpen = GUILayout.Toggle(debugMode.Value, "HeaderBar.DebugMode".I18N());
                if (debugModeOpen != debugMode.Value)
                    debugMode.Value = debugModeOpen;
                
                if (debugMode.Value)
                {
                    string[] toolbarList =
                    {
                        "HeaderBar.ModList".I18N(),
                        "HeaderBar.DramaDebug".I18N()
                    };
                    toolbarSelected = GUILayout.Toolbar(toolbarSelected, toolbarList);
                }
                else
                {
                    toolbarSelected = 0;
                }

                GUILayout.FlexibleSpace();

                
                if (debugMode.Value)
                {
                    var inMainScene = SceneManager.GetActiveScene().name == "MainMenu";
                    GUI.enabled = inMainScene;
                    
                    if(!inMainScene)
                        GUILayout.Label("HeaderBar.ReloadModTip".I18N());
                    
                    if (GUILayout.Button("HeaderBar.ReloadMod".I18N()))
                    {
                        ModManager.ReloadAllMod();
                    }

                    GUI.enabled = true;
                    
                    if (GUILayout.Button("HeaderBar.ExportBase".I18N()))
                    {
                        ModManager.GenerateBaseData();
                    }
                }
                
                // 该部分不简化是为了避免反复操作 openInStart.Value 属性
                bool isPop = GUILayout.Toggle(openInStart.Value, "HeaderBar.OpenInStart".I18N());
                if (isPop != openInStart.Value)
                    openInStart.Value = isPop;

                if (GUILayout.Button("HeaderBar.Close".I18N()))
                    isWinOpen = false;
            }
            GUILayout.EndHorizontal();
        }

        private void DrawBottomBar()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label($"{"Misc.CurrentLanguage".I18N()} : {nextLanguage.LanguageName}");
                if (GUILayout.Button("Language"))
                {
                    NextLanguage.InitLanguage();
                    nextLanguage = null;
                }
            }
            GUILayout.EndHorizontal();
        }

        private void DrawModList()
        {
            GUILayout.Label("Mod.List".I18N(),labelTitleStyle);
            scrollRollMods = GUILayout.BeginScrollView(scrollRollMods, false, true,
                new GUILayoutOption[]
                {
                    GUILayout.MinHeight(winRect.height * 0.8f-40)
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
                
                var modName = modConfig.Name ?? "Mod.Unknown".I18N();
                var modAuthor = modConfig.Author ?? "Mod.Unknown".I18N();
                var modVersion = modConfig.Version ?? "Mod.Unknown".I18N();
                var modDescription = modConfig.Description ?? "Mod.Unknown".I18N();
                
                var modDir = modConfig.Path ?? "Mod.Unknown".I18N();
                
                var modState = modConfig.State; 
                
                GUI.color = Color.white;
                GUI.Label(infoRect,$"{"Mod.Name".I18N()}: {modName}",labelLeftStyle);
                GUI.Label(infoRect,$"{"Mod.Author".I18N()}: {modAuthor}",labelMiddleStyle);
                GUI.Label(infoRect,$"{"Mod.Version".I18N()}: {modVersion}",labelRightStyle);
                
                GUI.Label(descRect,$"{"Mod.Description".I18N()}: {modDescription}");
                
                GUI.Label(dirRect,$"{"Mod.Directory".I18N()}: {modDir}");


                switch (modState)
                {
                    case ModState.Unload:
                        GUI.color = Color.gray;
                        GUI.Label(stateRect,"Mod.Load.Unload".I18N());
                        break;
                    case ModState.Loading:
                        GUI.color = Color.white;
                        GUI.Label(stateRect,"Mod.Load.Loading".I18N());
                        break;
                    case ModState.LoadSuccess:
                        GUI.color = Color.cyan;
                        GUI.Label(stateRect,"Mod.Load.Success".I18N());
                        break;
                    case ModState.LoadFail:
                        GUI.color = Color.red;
                        GUI.Label(stateRect,"Mod.Load.Fail".I18N());
                        break;
                }

                GUI.color = oldColor;
            }
            GUILayout.EndScrollView();
        }

        private void DrawDramaDebug()
        {
            GUILayout.Label("DramaDebug.Title".I18N(),labelTitleStyle);
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
                    GUILayout.Label("DramaDebug.DramaID".I18N());
                    inputEvent = GUILayout.TextArea(inputEvent);
                    if (GUILayout.Button("DramaDebug.Run".I18N()))
                    {
                        DialogAnalysis.StartDialogEvent(inputEvent);
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(20);
                GUILayout.Label("DramaDebug.DramaCommandDebug".I18N());
                testCommand = GUILayout.TextArea(testCommand, new GUILayoutOption[]
                {
                    GUILayout.MinHeight(debugArea1.height - 200)
                });
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("DramaDebug.Run".I18N()))
                    {
                        if(!string.IsNullOrEmpty(testCommand))
                            DialogAnalysis.StartTestDialogEvent(testCommand);
                    }

                    if (GUILayout.Button("DramaDebug.CopyRawData".I18N()))
                    {
                        GUIUtility.systemCopyBuffer = testCommand;
                    }

                    if (GUILayout.Button("DramaDebug.CopyJsonData".I18N()))
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
                GUILayout.Label("DramaDebug.IntVariableDebugWin".I18N());
                scrollRollDebugInt = GUILayout.BeginScrollView(scrollRollDebugInt, false, true,
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
                        GUI.Label(infoRect,$"{pair.Key}",labelLeftStyle);
                        GUI.Label(infoRect,$"{pair.Value}",labelRightStyle);
                    }
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndArea();
            
            GUILayout.BeginArea(debugArea3);
            {
                GUI.Box(debugArea3,"");
                GUILayout.Label("DramaDebug.StrVariableDebugWin".I18N());
                scrollRollDebugString = GUILayout.BeginScrollView(scrollRollDebugString, false, true,
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
                        GUI.Label(infoRect,$"{pair.Key}",labelLeftStyle);
                        GUI.Label(infoRect,$"{pair.Value}",labelRightStyle);
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