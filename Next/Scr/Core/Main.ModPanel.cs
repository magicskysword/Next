using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using FairyGUI;
using JSONClass;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.Next.Extension;
using SkySwordKill.Next.FGUI;
using SkySwordKill.Next.I18N;
using SkySwordKill.Next.Mod;
using SkySwordKill.Next.ModGUI;
using SkySwordKill.Next.StaticFace;
using SkySwordKill.Next.XiaoYeGUI;
using SkySwordKill.NextModEditor.Panel;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SkySwordKill.Next
{
    public partial class Main
    {
        #region 字段

        private bool _isWinOpen;
        private Vector2 scrollRollLanguages = new Vector2(0, 0);
        private Vector2 scrollRollMods = new Vector2(0, 0);
        private Vector2 scrollRollModInfo = new Vector2(0, 0);
        private Vector2 scrollRollDebugString = new Vector2(0, 0);
        private Vector2 scrollRollDebugInt = new Vector2(0, 0);

        private Vector2 scrollRollDebugNpcs;
        private Vector2 scrollRollDebugNpcInfo;

        private bool isGUIInit;
        private int toolbarSelected;
        private Rect languageRect;
        private Rect winRect;

        private string inputEvent;
        private string testCommand;
        private string inputNpcSearch;

        private string inputPlayerFaceJson;

        private GUIStyle labelTitleStyle;
        private GUIStyle labelLeftStyle;
        private GUIStyle labelMiddleStyle;
        private GUIStyle labelRightStyle;

        private GUIStyle infoStyle;

        private GUIStyle modToggleStyle;
        private GUIStyle modInfoExceptionStyle;

        private bool isSelectedLanguage;

        private RayBlocker rayBlocker;

        private int curNpcSelectedIndex;
        private int curNpcSelectedPage;
        private int countEachPage = 20;
        private SearchNpcData curNpcData;

        private List<SearchNpcData> npcDataList = new List<SearchNpcData>();
        private string[] npcDataListName = new string[0];

        #endregion

        #region 属性

        #endregion

        #region 回调方法

        private void Update()
        {
            if (Input.GetKeyDown(WinKeyCode.Value))
            {
                _isWinOpen = !_isWinOpen;
            }


            if (rayBlocker == null)
                return;
            if (isSelectedLanguage || CurrentLanguage == null)
            {
                rayBlocker.SetSize(languageRect);
                rayBlocker.OpenBlocker();
            }
            else if (_isWinOpen)
            {
                rayBlocker.SetSize(winRect);
                rayBlocker.OpenBlocker();
            }
            else
            {
                rayBlocker.CloseBlocker();
            }
        }

        public void GUIInit()
        {
            languageRect = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 150, 400, 300);
            winRect = new Rect(W(0.1f), H(0.1f), W(0.8f), H(0.8f));

            if (isGUIInit)
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
                contentOffset = new Vector2(5, 0),
            };

            infoStyle = new GUIStyle(InterfaceMaker.CustomSkin.box)
            {
                alignment = TextAnchor.UpperLeft,
                contentOffset = new Vector2(5, 0),
                wordWrap = true
            };

            modInfoExceptionStyle = new GUIStyle(InterfaceMaker.CustomSkin.box)
            {
                alignment = TextAnchor.UpperLeft,
                contentOffset = new Vector2(5, 0),
                wordWrap = true,
                normal =
                {
                    textColor = Color.red
                }
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

            try
            {
                OnDrawModPanel();
            }
            catch (ArgumentException argumentException)
            {
                LogWarning("UI绘制异常");
                LogWarning(argumentException);
            }
            catch (Exception e)
            {
                LogError(e);
            }
        }

        #endregion

        #region 公共方法

        #endregion

        #region 私有方法

        private void OnDrawModPanel()
        {
            var oldSkin = GUI.skin;
            GUI.skin = InterfaceMaker.CustomSkin;

            if (isSelectedLanguage || CurrentLanguage == null)
            {
                GUILayout.Window(20, languageRect, DrawLanguageSelectWindow, $"Next v{MOD_VERSION}");
            }
            else if (_isWinOpen)
            {
                GUILayout.Window(50, winRect, DrawDebugWindow, $"Next v{MOD_VERSION}");
            }

            GUI.skin = oldSkin;
        }

        private void DrawLanguageSelectWindow(int id)
        {
            if (NextLanguage.Languages.Count == 0)
            {
                GUILayout.Label("请重写下载该Mod或下载Mod语言文件！", labelTitleStyle);
                GUILayout.Label("Please re-download the Mod or download the Mod language file！", labelTitleStyle);
                return;
            }

            GUILayout.Label("请选择Mod语言", labelMiddleStyle);
            GUILayout.Label("Choose Mod Language", labelMiddleStyle);

            scrollRollLanguages = GUILayout.BeginScrollView(scrollRollLanguages);
            {
                foreach (var language in NextLanguage.Languages)
                {
                    if (GUILayout.Button(language.Config.LanguageName))
                    {
                        SelectLanguage(language);
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
                // case 0:
                //     DrawModList();
                //     break;
                case 0:
                    DrawDramaDebug();
                    break;
                case 1:
                    DrawNpcDebug();
                    break;
                case 2:
                    DrawMiscDebug();
                    break;
            }

            GUILayout.FlexibleSpace();

            DrawBottomBar();
        }

        private void DrawHeaderBar()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(string.Format("HeaderBar.HotkeyTip".I18N(), WinKeyCode.Value.ToString()));
                // 该部分不简化是为了避免反复操作 debugMode.Value 属性
                var debugModeOpen = GUILayout.Toggle(DebugMode.Value, "HeaderBar.DebugMode".I18N());
                if (debugModeOpen != DebugMode.Value)
                    DebugMode.Value = debugModeOpen;

                if (DebugMode.Value)
                {
                    string[] toolbarList =
                    {
                        //"HeaderBar.ModList".I18N(),
                        "HeaderBar.DramaDebug".I18N(),
                        "HeaderBar.NpcDebug".I18N(),
                        "HeaderBar.MiscDebug".I18N()
                    };
                    toolbarSelected = GUILayout.Toolbar(toolbarSelected, toolbarList);
                }
                else
                {
                    toolbarSelected = 0;
                }

                GUILayout.FlexibleSpace();

                if (GUILayout.Button("HeaderBar.Close".I18N()))
                    _isWinOpen = false;
            }
            GUILayout.EndHorizontal();
        }

        private void DrawBottomBar()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label($"{"Misc.CurrentLanguage".I18N()} : {CurrentLanguage.Config.LanguageName}");
                if (GUILayout.Button("Language"))
                {
                    NextLanguage.InitLanguage();
                    CurrentLanguage = null;
                }
            }
            GUILayout.EndHorizontal();
        }

        /*
        private void DrawModList()
        {
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("HeaderBar.CheckUpdate".I18N()))
                    Updater.CheckVersion();
                if (Updater.IsChecking)
                {
                    GUILayout.Label("HeaderBar.CheckUpdating".I18N());
                }
                else if (!Updater.CheckSuccess)
                {
                    GUILayout.Label("HeaderBar.CheckUpdateFailure".I18N());
                }
                else
                {
                    if (Updater.HasNewVersion)
                    {
                        GUILayout.Label(string.Format("HeaderBar.CheckUpdateHasNewVersion".I18N(),
                            Updater.CurVersionStr, Updater.NewVersionStr));
                    }
                    else
                    {
                        GUILayout.Label("HeaderBar.CheckUpdateIsNewVersion".I18N());
                    }
                }

                GUILayout.FlexibleSpace();
                if (DebugMode.Value)
                {
                    if (GUILayout.Button("HeaderBar.ExportBase".I18N()))
                    {
                        Directory.CreateDirectory(PathExportOutputDir.Value);
                        ModManager.GenerateBaseData(() =>
                        {
                            Process.Start(PathExportOutputDir.Value);
                        });
                    }
                }

                var inMainScene = SceneManager.GetActiveScene().name == "MainMenu";
                if (!inMainScene)
                    GUILayout.Label("HeaderBar.ReloadModTip".I18N());
                GUI.enabled = inMainScene;
                if (GUILayout.Button("HeaderBar.ReloadMod".I18N()))
                {
                    ModManager.ReloadAllMod();
                }

                GUI.enabled = true;
                if (GUILayout.Button("HeaderBar.ModBaseFolder".I18N()))
                {
                    Directory.CreateDirectory(PathExportOutputDir.Value);
                    Process.Start(PathExportOutputDir.Value);
                }
            }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("HeaderBar.Github".I18N()))
                {
                    Process.Start(Updater.WebGitHubUrl);
                }
                if (GUILayout.Button("HeaderBar.3dmBBS".I18N()))
                {
                    Process.Start(Updater.Web3dmBBSUrl);
                }
                if (GUILayout.Button("HeaderBar.3dmModSite".I18N()))
                {
                    Process.Start(Updater.Web3dmModSiteUrl);
                }
                if (GUILayout.Button("HeaderBar.BWiki".I18N()))
                {
                    Process.Start(Updater.WebBWikiUrl);
                }
                
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
            
            GUILayout.Label("Mod.List".I18N(), labelTitleStyle);
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical();
                {
                    scrollRollMods = GUILayout.BeginScrollView(scrollRollMods, false, false,
                        GUILayout.MinHeight(winRect.height * 0.7f - 100),
                        GUILayout.MinWidth(winRect.width * 0.6f - 40),
                        GUILayout.MaxWidth(winRect.width * 0.6f - 40));
                    {
                        var mods = ModManager.modGroups.Select(config =>
                        {
                            var configName = config.Name ?? "Mod.Unknown".I18N();
                            var modSetting = I.NextModSetting.GetOrCreateModSetting(config);

                            string modEnable = modSetting.enable ? "☑" : "□";

                            return
                                $"{modEnable} [{config.GetModStateDescription()}]  {configName} ({Path.GetFileNameWithoutExtension(config.Path)})";
                        }).ToArray();

                        curModSelectedIndex = GUILayout.SelectionGrid(curModSelectedIndex, mods, 1, modToggleStyle);
                    }
                    GUILayout.EndScrollView();

                    if (ModManager.ModDataDirty)
                    {
                        GUILayout.Label("Mod.Panel.DataDirtyTip".I18N());
                    }
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                {
                    var modEnable = ModManager.ModGetEnable(curModSelectedIndex);
                    var btnText = modEnable ? "Mod.Panel.DisableMod".I18N() : "Mod.Panel.EnableMod".I18N();
                    if (GUILayout.Button(btnText, GUILayout.MinHeight(40)))
                    {
                        ModManager.ModSetEnable(curModSelectedIndex, !modEnable);
                    }

                    GUILayout.Space(20);

                    if (GUILayout.Button("Mod.Panel.EnableAllMod".I18N()))
                    {
                        for (int i = 0; i < ModManager.modGroups.Count; i++)
                        {
                            ModManager.ModSetEnable(i, true);
                        }
                    }

                    if (GUILayout.Button("Mod.Panel.DisableAllMod".I18N()))
                    {
                        for (int i = 0; i < ModManager.modGroups.Count; i++)
                        {
                            ModManager.ModSetEnable(i, false);
                        }
                    }

                    GUILayout.Space(40);

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
                }
                GUILayout.EndVertical();

                var rect = GUILayoutUtility.GetRect(GUIContent.none, GUI.skin.box,
                    GUILayout.MinHeight(winRect.height * 0.7f - 70));
                GUILayout.Space(5);
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
                            GUILayout.Label($"{"Mod.Name".I18N()} : {modName}", infoStyle);
                            GUILayout.Space(15);
                            GUILayout.Label($"{"Mod.State".I18N()} : {curMod.GetModStateDescription()}", infoStyle);
                            if (curMod.Exception != null)
                            {
                                GUILayout.Label(
                                    $"{"Mod.Exception".I18N()} : {curMod.Exception.Message} \n\nException : {curMod.Exception}",
                                    modInfoExceptionStyle);
                                GUILayout.Space(15);
                            }

                            GUILayout.Label($"{"Mod.Author".I18N()} : {modAuthor}", infoStyle);
                            GUILayout.Label($"{"Mod.Version".I18N()} : {modVersion}", infoStyle);
                            GUILayout.Label($"{"Mod.Description".I18N()} : {modDesc}", infoStyle);
                            GUILayout.Space(15);
                            GUILayout.Label($"{"Mod.Directory".I18N()} : {modPath}", infoStyle);
                        }
                        GUILayout.EndVertical();
                    }
                    GUILayout.EndScrollView();
                }
            }
            GUILayout.EndHorizontal();
        }
         */

        private void DrawDramaDebug()
        {
            GUILayout.Label("DramaDebug.Title".I18N(), labelTitleStyle);

            if (Tools.instance == null || Tools.instance.getPlayer() == null)
            {
                GUILayout.Label("Mod.GameNotStart".I18N(), labelTitleStyle);
                return;
            }

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
                    GUILayout.Label(
                        $"当前状态：{(DialogAnalysis.IsRunningEvent ? "运行中" : "未运行")} 当前队列事件数量：{DialogAnalysis.EventQueue.Count}");
                    if (GUILayout.Button("重置事件状态"))
                    {
                        DialogAnalysis.CancelEvent();
                    }
                }
                GUILayout.EndHorizontal();


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
                testCommand = GUILayout.TextArea(testCommand, GUILayout.MinHeight(debugArea1.height - 200));
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("DramaDebug.Run".I18N()))
                    {
                        if (!string.IsNullOrEmpty(testCommand))
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
                            .Where(str => !string.IsNullOrWhiteSpace(str))
                            .ToArray()).ToString(Formatting.Indented);
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();

            GUILayout.BeginArea(debugArea2);
            {
                GUI.Box(debugArea2, "");
                GUILayout.Label("DramaDebug.IntVariableDebugWin".I18N());
                scrollRollDebugInt = GUILayout.BeginScrollView(scrollRollDebugInt, false, true,
                    GUILayout.MinHeight(debugArea2.height - 50));
                {
                    foreach (var pair in DialogAnalysis.GetAllInt())
                    {
                        GUILayout.Box("", GUILayout.MinHeight(26));
                        var rect = GUILayoutUtility.GetLastRect();
                        var infoRect = new Rect(rect)
                        {
                            x = rect.x + 10,
                            height = 24,
                            width = rect.width - 20
                        };
                        GUI.Label(infoRect, $"{pair.Key}", labelLeftStyle);
                        GUI.Label(infoRect, $"{pair.Value}", labelRightStyle);
                    }
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndArea();

            GUILayout.BeginArea(debugArea3);
            {
                GUI.Box(debugArea3, "");
                GUILayout.Label("DramaDebug.StrVariableDebugWin".I18N());
                scrollRollDebugString = GUILayout.BeginScrollView(scrollRollDebugString, false, true,
                    GUILayout.MinHeight(debugArea3.height - 50));
                {
                    foreach (var pair in DialogAnalysis.GetAllStr())
                    {
                        GUILayout.Box("", GUILayout.MinHeight(26));
                        var rect = GUILayoutUtility.GetLastRect();
                        var infoRect = new Rect(rect)
                        {
                            x = rect.x + 10,
                            height = 24,
                            width = rect.width - 20
                        };
                        GUI.Label(infoRect, $"{pair.Key}", labelLeftStyle);
                        if (pair.Value.Length > 15)
                            GUI.Label(infoRect, $"{pair.Value.Substring(0, 15)}...", labelRightStyle);
                        else
                            GUI.Label(infoRect, $"{pair.Value}", labelRightStyle);
                    }
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }

        private void DrawNpcDebug()
        {
            GUILayout.Label("NpcDebug.Title".I18N(), labelTitleStyle);

            if (Tools.instance == null || Tools.instance.getPlayer() == null)
            {
                GUILayout.Label("Mod.GameNotStart".I18N(), labelTitleStyle);
                return;
            }

            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical(GUILayout.MinWidth(winRect.width * 0.2f));
                {
                    GUILayout.BeginHorizontal();
                    {
                        inputNpcSearch =
                            GUILayout.TextField(inputNpcSearch, GUILayout.MinWidth(winRect.width * 0.2f - 100));
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("NpcDebug.Search".I18N()))
                        {
                            SearchNpc(inputNpcSearch);
                        }
                    }
                    GUILayout.EndHorizontal();

                    var maxPage = (npcDataListName.Length - 1) / countEachPage;
                    GUILayout.BeginHorizontal();
                    {
                        var oldState = GUI.enabled;

                        GUI.enabled = curNpcSelectedPage > 0;
                        if (GUILayout.Button("NpcDebug.PgUp".I18N()))
                        {
                            curNpcSelectedPage -= 1;
                        }

                        GUI.enabled = curNpcSelectedPage < maxPage;
                        if (GUILayout.Button("NpcDebug.PgDn".I18N()))
                        {
                            curNpcSelectedPage += 1;
                        }

                        GUI.enabled = oldState;
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.Label(
                        string.Format("NpcDebug.PageInfo".I18N(), curNpcSelectedPage + 1, maxPage + 1));

                    scrollRollDebugNpcs = GUILayout.BeginScrollView(scrollRollDebugNpcs, false, false);
                    {
                        var nameArray = npcDataListName.Where(
                            (_, index) => index >= curNpcSelectedPage * countEachPage &&
                                          index < (curNpcSelectedPage + 1) * countEachPage).ToArray();
                        curNpcSelectedIndex = GUILayout.SelectionGrid(curNpcSelectedIndex, nameArray, 1);
                    }
                    GUILayout.EndScrollView();
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                {
                    GUILayout.Space(5);
                    var npcData = GetCurSelectNpcData();
                    if (npcData != null)
                    {
                        if (curNpcData != npcData)
                        {
                            try
                            {
                                npcData.Refresh();
                            }
                            catch (Exception e)
                            {
                                LogError(string.Format("NpcDebug.NpcMissError".I18N(), npcData.ID));
                                LogError(e);
                                SearchNpc(null);
                                goto EndNpc;
                            }
                        }

                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.BeginVertical();
                            {
                                scrollRollDebugNpcInfo =
                                    GUILayout.BeginScrollView(scrollRollDebugNpcInfo, false, false);
                                {
                                    GUILayout.Label(string.Format("NpcDebug.Info.ID".I18N(), npcData.ID), infoStyle);
                                    if (npcData.ImportantID > 0)
                                    {
                                        GUILayout.Label(
                                            string.Format("NpcDebug.Info.BindingID".I18N(), npcData.ImportantID),
                                            infoStyle);
                                    }
                                    else
                                    {
                                        GUILayout.Label("NpcDebug.Info.NotBinding".I18N(), infoStyle);
                                    }

                                    GUILayout.Space(16);
                                    GUILayout.Label(string.Format("NpcDebug.Info.Name".I18N(), npcData.Name),
                                        infoStyle);
                                    GUILayout.Label(string.Format("NpcDebug.Info.Title".I18N(), npcData.Title),
                                        infoStyle);
                                    GUILayout.Label(
                                        string.Format("NpcDebug.Info.School".I18N(),
                                            DialogAnalysis.GetSchoolName(npcData.School.ToString())), infoStyle);
                                    GUILayout.Label(
                                        string.Format("NpcDebug.Info.Location".I18N(), npcData.LocationName),
                                        infoStyle);

                                    GUILayout.Space(16);
                                    GUILayout.Label(
                                        string.Format("NpcDebug.Info.Level".I18N(),
                                            DialogAnalysis.GetLevelName(npcData.Level)), infoStyle);
                                    GUILayout.Label(
                                        string.Format("NpcDebug.Info.Gender".I18N(),
                                            DialogAnalysis.GetGenderName(npcData.Gender)), infoStyle);
                                    GUILayout.Label(string.Format("NpcDebug.Info.Age".I18N(), npcData.AgeYear),
                                        infoStyle);
                                    GUILayout.Label(string.Format("NpcDebug.Info.Life".I18N(), npcData.Life),
                                        infoStyle);
                                    if (npcData.IsCouple)
                                        GUILayout.Label("NpcDebug.Info.Relation.Couple".I18N(), infoStyle);
                                    if (npcData.IsTeacher)
                                        GUILayout.Label("NpcDebug.Info.Relation.Teacher".I18N(), infoStyle);
                                    if (npcData.IsStudent)
                                        GUILayout.Label("NpcDebug.Info.Relation.Student".I18N(), infoStyle);
                                    if (npcData.IsBrother)
                                        GUILayout.Label("NpcDebug.Info.Relation.Brother".I18N(), infoStyle);

                                    GUILayout.Space(16);
                                    GUILayout.Label(string.Format("NpcDebug.Info.Sprite".I18N(), npcData.Sprite),
                                        infoStyle);
                                }
                                GUILayout.EndScrollView();
                            }
                            GUILayout.EndVertical();

                            GUILayout.BeginVertical(GUILayout.MinWidth(winRect.width * 0.2f));
                            {
                                if (GUILayout.Button("NpcDebug.Info.Func.Refresh".I18N()))
                                {
                                    try
                                    {
                                        curNpcData.Refresh();
                                    }
                                    catch (Exception e)
                                    {
                                        LogError(string.Format("NpcDebug.NpcMissError".I18N(), npcData.ID));
                                        LogError(e);
                                        SearchNpc(null);
                                    }
                                }

                                GUILayout.Space(16);
                                if (GUILayout.Button("NpcDebug.Info.Func.ForceInteract".I18N()))
                                {
                                    DialogAnalysis.NpcForceInteract(npcData.ID);
                                }

                                GUILayout.Space(16);
                                if (GUILayout.Button("NpcDebug.Info.Func.ExportFaceInfo".I18N()))
                                {
                                    var faceInfo = DialogAnalysis.GetNpcFaceInfo(npcData.ID);
                                    if (faceInfo != null)
                                    {
                                        GUIUtility.systemCopyBuffer =
                                            JsonConvert.SerializeObject(faceInfo, Formatting.Indented);
                                        LogInfo(string.Format("NpcDebug.Info.Func.ExportFaceInfo.Success".I18N(),
                                            npcData.ID, npcData.Name));
                                    }
                                    else
                                    {
                                        LogError(string.Format("NpcDebug.Info.Func.ExportFaceInfo.Failure".I18N(),
                                            npcData.ID, npcData.Name));
                                    }
                                }
                            }
                            GUILayout.EndVertical();
                        }
                        GUILayout.EndHorizontal();
                    }

                    EndNpc:

                    curNpcData = npcData;
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }

        private void DrawMiscDebug()
        {
            GUILayout.Label("Misc.Title".I18N(), labelTitleStyle);

            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical(GUILayout.MaxWidth(winRect.width * 0.4f));
                {
                    GUILayout.Label("Misc.PlayerFace".I18N());
                    inputPlayerFaceJson = GUILayout.TextArea(inputPlayerFaceJson,
                        GUILayout.MinHeight(winRect.height * 0.4f));
                    GUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button($"Misc.ExportPlayerFace".I18N()))
                        {
                            var faceInfo = DialogAnalysis.GetNpcFaceInfo(1);
                            inputPlayerFaceJson = JsonConvert.SerializeObject(faceInfo, Formatting.Indented);
                        }

                        if (GUILayout.Button($"Misc.ImportPlayerFace".I18N()))
                        {
                            var faceInfo = JsonConvert.DeserializeObject<CustomStaticFaceInfo>(inputPlayerFaceJson);
                            DialogAnalysis.SetPlayerFaceData(faceInfo);
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                {
                    if (GUILayout.Button("HeaderBar.ExportBase".I18N()))
                    {
                        _isWinOpen = false;
                        Directory.CreateDirectory(PathExportOutputDir.Value);
                        ModManager.GenerateBaseData(() =>
                        {
                            Process.Start(PathExportOutputDir.Value);
                        });
                    }
                    
                    if (GUILayout.Button("导出当前场景Fungus".I18NTodo()))
                    {
                        _isWinOpen = false;
                        Directory.CreateDirectory(PathExportOutputDir.Value);
                        ModManager.GenerateCurrentSceneFungusData(() =>
                        {
                            Process.Start(PathExportOutputDir.Value);
                        });
                    }
                    
                    GUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("清空Lua已载入文件"))
                        {
                            Main.I._luaManager.ClearCache();
                        }

                        if (GUILayout.Button("重载Lua虚拟机"))
                        {
                            Main.I._luaManager.Reset();
                            DialogAnalysis.CancelEvent();
                        }
                    }
                    GUILayout.EndHorizontal();
                    

                    if (GUILayout.Button("NextMod面板"))
                    {
                        var window = new ModMainWindow();
                        window.Show();
                    }
                    //
                    //
                    if (GUILayout.Button("Next编辑器（预览版）"))
                    {
                        var window = new ModEditorMainPanel();
                        window.Show();
                        _isWinOpen = false;
                    }

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("UI缩放比例");
                        
                        var scaler = Stage.inst.gameObject.GetComponent<UIContentScaler>();
                        var factory = scaler.constantScaleFactor;
                        var newFactory = GUILayout.HorizontalSlider(factory, 0.8f, 1.2f);
                        if(Math.Abs(factory - newFactory) > 0.01f)
                            GRoot.inst.SetContentScaleFactor(newFactory);
                        
                        GUILayout.Label(newFactory.ToString(CultureInfo.InvariantCulture));
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.Label("请先设置好缩放比例，再打开编辑器。注：过大的缩放比例可能导致部分界面显示异常。");
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }

        private void SearchNpc(string searchFilter)
        {
            curNpcSelectedIndex = -1;
            curNpcSelectedPage = 0;
            npcDataList.Clear();

            bool noFilter = string.IsNullOrWhiteSpace(searchFilter);

            foreach (var jsonObject in jsonData.instance.AvatarJsonData.list)
            {
                var npcId = jsonObject["id"].I;
                if (npcId < 20000)
                    continue;
                var npcData = new SearchNpcData();
                npcData.ID = npcId;

                npcData.Refresh();

                if (noFilter || npcData.FilterCheck(searchFilter))
                    npcDataList.Add(npcData);
            }

            npcDataListName = npcDataList.Select(data => data.ToString()).ToArray();
        }


        private SearchNpcData GetCurSelectNpcData()
        {
            if (curNpcSelectedIndex < 0 || curNpcSelectedIndex >= countEachPage)
                return null;

            var index = curNpcSelectedPage * countEachPage + curNpcSelectedIndex;
            if (index >= 0 && index < npcDataList.Count)
                return npcDataList[index];
            return null;
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