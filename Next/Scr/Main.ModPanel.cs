﻿using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using Newtonsoft.Json.Linq;
using SkySwordKill.Next.Extension;
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
        private Vector2 scrollRollModInfo = new Vector2(0, 0);
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
        
        private GUIStyle modToggleStyle;
        private GUIStyle modInfoStyle;

        private bool isSelectedLanguage = false;
        
        private RayBlocker rayBlocker;

        private int curModSelectedIndex = 0;

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
            languageRect = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 150, 400, 300);
            winRect = new Rect(W(0.1f), H(0.1f), W(0.8f), H(0.8f));
            
            if(isGUIInit)
                return;

            rayBlocker = RayBlocker.CreateRayBlock();
            
            labelTitleStyle = new GUIStyle("label")
            {
                fontSize = 36,
                alignment = TextAnchor.UpperCenter
            };
            labelLeftStyle = new GUIStyle("label")
            {
                alignment = TextAnchor.UpperLeft,
            };
            modToggleStyle = new GUIStyle(InterfaceMaker.CustomSkin.button)
            {
                fontSize = 20,
                alignment = TextAnchor.UpperLeft,
                contentOffset = new Vector2(5,0),
            };
            modInfoStyle = new GUIStyle(InterfaceMaker.CustomSkin.box)
            {
                alignment = TextAnchor.UpperLeft,
                contentOffset = new Vector2(5,0),
                wordWrap = true
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
                GUILayout.Window(20, languageRect, DrawLanguageSelectWindow, $"Next v{MOD_VERSION}");
                rayBlocker.SetSize(languageRect);
                rayBlocker.OpenBlocker();
            }
            else if (isWinOpen)
            {
                GUILayout.Window(50, winRect, DrawDebugWindow, $"Next v{MOD_VERSION}");
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
            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                if (debugMode.Value)
                {
                    if (GUILayout.Button("HeaderBar.ExportBase".I18N()))
                    {
                        ModManager.GenerateBaseData();
                    }
                }
                var inMainScene = SceneManager.GetActiveScene().name == "MainMenu";
                if(!inMainScene)
                    GUILayout.Label("HeaderBar.ReloadModTip".I18N());
                GUI.enabled = inMainScene;
                if (GUILayout.Button("HeaderBar.ReloadMod".I18N()))
                {
                    ModManager.ReloadAllMod();
                }
                GUI.enabled = true;
                if (GUILayout.Button("HeaderBar.ModFolder".I18N()))
                {
                    System.Diagnostics.Process.Start(Main.pathModsDir.Value);
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.Label("Mod.List".I18N(),labelTitleStyle);
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical();
                {
                    scrollRollMods = GUILayout.BeginScrollView(scrollRollMods, false, false, 
                        GUILayout.MinHeight(winRect.height * 0.8f-60),
                        GUILayout.MinWidth(winRect.width * 0.7f-40),
                        GUILayout.MaxWidth(winRect.width * 0.7f-40));
                    {
                        var mods = ModManager.modConfigs.Select(config =>
                        {
                            var configName = config.Name ?? "Mod.Unknown".I18N();
                            var modSetting = Main.Instance.nextModSetting.GetOrCreateModSetting(config);
                            
                            string modEnable = modSetting.enable ? "☑" : "□";

                            string modState = string.Empty;
                            string colorCode = string.Empty;
                            switch (config.State)
                            {
                                case ModState.Unload:
                                    modState = "Mod.Load.Unload".I18N();
                                    colorCode = "#000000";
                                    break;
                                case ModState.Disable:
                                    modState = "Mod.Load.Disable".I18N();
                                    colorCode = "#808080";
                                    break;
                                case ModState.Loading:
                                    modState = "Mod.Load.Loading".I18N();
                                    colorCode = "#000000";
                                    break;
                                case ModState.LoadSuccess:
                                    modState = "Mod.Load.Success".I18N();
                                    colorCode = "#00FFFF";
                                    break;
                                case ModState.LoadFail:
                                    modState = "Mod.Load.Fail".I18N();
                                    colorCode = "#FF0000";
                                    break;
                            }
                            
                            return $"{modEnable} [<color={colorCode}>{modState}</color>]  {configName} ({Path.GetFileNameWithoutExtension(config.Path)})";
                        }).ToArray();
                        
                        curModSelectedIndex = GUILayout.SelectionGrid(curModSelectedIndex, mods, 1, modToggleStyle);
                    }
                    GUILayout.EndScrollView();
                    
                    GUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("Mod.Panel.MoveToTop".I18N()))
                        {
                            ModManager.ModMoveToTop(ref curModSelectedIndex);
                        }

                        if (GUILayout.Button("Mod.Panel.MoveUp".I18N()))
                        {
                            ModManager.ModMoveUp(ref curModSelectedIndex);
                        }

                        if (GUILayout.Button("Mod.Panel.MoveDown".I18N()))
                        {
                            ModManager.ModMoveDown(ref curModSelectedIndex);
                        }

                        if (GUILayout.Button("Mod.Panel.MoveToBottom".I18N()))
                        {
                            ModManager.ModMoveToBottom(ref curModSelectedIndex);
                        }
                        
                        GUILayout.Space(20);
                        var modEnable = ModManager.ModGetEnable(curModSelectedIndex);
                        var btnText = modEnable ? "Mod.Panel.DisableMod".I18N() : "Mod.Panel.EnableMod".I18N();
                        if (GUILayout.Button(btnText))
                        {
                            ModManager.ModSetEnable(curModSelectedIndex,!modEnable);
                        }
                    }
                    GUILayout.EndHorizontal();

                    if (ModManager.ModDataDirty)
                    {
                        GUILayout.Label($"Mod.Panel.DataDirtyTip".I18N());
                    }
                }
                GUILayout.EndVertical();
                
                var rect = GUILayoutUtility.GetRect(GUIContent.none, GUI.skin.box,
                    GUILayout.MinHeight(winRect.height * 0.7f-70));
                if (ModManager.TryGetModConfig(curModSelectedIndex, out var curMod))
                {
                    var modPath = curMod.Path ?? "Mod.Unknown".I18N();
                        
                    var modName = curMod.Name ?? "Mod.Unknown".I18N();
                    var modAuthor = curMod.Author ?? "Mod.Unknown".I18N();
                    var modVersion = curMod.Version ?? "Mod.Unknown".I18N();
                    var modDesc = curMod.Description ?? "Mod.Unknown".I18N();

                    scrollRollModInfo = GUILayout.BeginScrollView(scrollRollModInfo);
                    {
                        GUILayout.BeginVertical();
                        {
                            GUILayout.Label($"{"Mod.Name".I18N()} : {modName}",modInfoStyle);
                            GUILayout.Space(20);
                            GUILayout.Label($"{"Mod.Author".I18N()} : {modAuthor}",modInfoStyle);
                            GUILayout.Label($"{"Mod.Version".I18N()} : {modVersion}",modInfoStyle);
                            GUILayout.Label($"{"Mod.Description".I18N()} : {modDesc}",modInfoStyle);
                            GUILayout.Space(20);
                            GUILayout.Label($"{"Mod.Directory".I18N()} : {modPath}",modInfoStyle);
                        }
                        GUILayout.EndVertical();
                    }
                    GUILayout.EndScrollView();
                }
            }
            GUILayout.EndHorizontal();
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