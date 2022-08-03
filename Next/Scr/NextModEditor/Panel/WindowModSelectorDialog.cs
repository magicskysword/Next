using System;
using System.Collections.Generic;
using System.IO;
using FairyGUI;
using SkySwordKill.Next.FGUI.Dialog;
using script.Steam;
using SkySwordKill.Next;
using SkySwordKill.Next.Extension;
using SkySwordKill.Next.FGUI;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextEditor.Mod;
using SkySwordKill.NextFGUI.NextCore;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

namespace SkySwordKill.NextEditor.Panel
{
    [Flags]
    public enum ModFilter
    {
        None =       0b00000000,
        Local =      0b00000001,
        Workshop =   0b00000010,
    }
    
    public delegate void OnSelectMod(ModWorkshop mod);
    
    public class WindowModSelectorDialog : WindowDialogBase
    {
        private WindowModSelectorDialog() : base("NextCore", "WinModSelectorDialog")
        {
            
        }

        public static WindowModSelectorDialog CreateDialog(string title, ModFilter filter, List<TableInfo> tableInfos, OnSelectMod onConfirm)
        {
            var window = new WindowModSelectorDialog();
            window.Title = title;
            window.Filter = filter;
            window.OnConfirm = onConfirm;
            window.TableInfos.AddRange(tableInfos);
            
            window.Show();
            return window;
        }
        
        public string Title { get; set; }
        public OnSelectMod OnConfirm { get; set; }
        public ModFilter Filter { get; set; }
        public List<ModWorkshop> Mods { get; } = new List<ModWorkshop>();
        public List<TableInfo> TableInfos { get; } = new List<TableInfo>();
        public CtlTableList TableList { get; private set; }
        public CtlPropertyInspector Inspector { get; private set; }
        public UI_WinModSelectorDialog SelectorDialog => contentPane as UI_WinModSelectorDialog;
        

        protected override void OnInit()
        {
            base.OnInit();
            
            TableList = new CtlTableList(SelectorDialog.m_table);
            Inspector = new CtlPropertyInspector(SelectorDialog.m_inspector);
            
            if(Filter.HasFlag(ModFilter.Local))
            {
                AddLocalMods();
            }
            if(Filter.HasFlag(ModFilter.Workshop))
            {
                AddWorkshopMods();
            }
            
            TableList.SetClickItem(OnClickModItem);
            TableList.BindTable(TableInfos, new TableDataList<ModWorkshop>(Mods));
            
            SelectorDialog.m_btnConfirm.onClick.Add(OnClickConfirm);
            SelectorDialog.m_btnConfirm.enabled = false;
            SelectorDialog.m_frame.m_closeButton.onClick.Add(OnClickCancel);
            
            Main.LogInfo($"初始化完成，共有{Mods.Count}个模组");
        }
        
        private void OnClickConfirm(EventContext context)
        {
            OnConfirm?.Invoke(Mods[TableList.SelectedIndex]);
            Hide();
        }
        
        private void OnClickCancel()
        {
            Hide();
        }

        private void OnClickModItem(int index, object o)
        {
            var modInfo = (ModWorkshop)o;
            ShowModInfo(modInfo);
        }
        
        private void ShowModInfo(ModWorkshop mod)
        {
            Inspector.Clear();
            SelectorDialog.m_btnConfirm.enabled = mod != null;
            if (mod != null)
            {
                var modInfo = mod.ModInfo;
                Inspector.AddDrawer(new CtlTextDrawer(string.Format($"Mod名称 : {modInfo.Title}")));
                Inspector.AddDrawer(new CtlTextDrawer(string.Format($"Mod地址 : {mod.Path}")));
                Inspector.AddDrawer(new CtlTextDrawer(string.Format($"Mod介绍 : {modInfo.Des}")));
            }
        }

        private void AddLocalMods()
        {
            foreach (var dir in Directory.GetDirectories(Main.PathLocalModsDir.Value))
            {
                string path = dir;
                Main.LogInfo($"检查模组：{path}");
                if (Directory.Exists(path))
                {
                    try
                    {
                        Mods.Add(ModEditorManager.I.LoadWorkshop(path));
                    }
                    catch (Exception message)
                    {
                        Main.LogError($"读取Mod配置文件失败：{path}\n{message}");
                    }
                }
            }
        }
        
        private void AddWorkshopMods()
        {
            var modNum = SteamUGC.GetNumSubscribedItems();
            var pvecPublishedFileID = new PublishedFileId_t[modNum];
            int subscribedItems = (int) SteamUGC.GetSubscribedItems(pvecPublishedFileID, modNum);
            var subscribeModList = new List<ulong>();
            foreach (PublishedFileId_t publishedFileIdT in pvecPublishedFileID)
                subscribeModList.Add(publishedFileIdT.m_PublishedFileId);
            foreach (ulong num in subscribeModList)
            {
                string path = $"{WorkshopTool.WorkshopRootPath}/{num}";
                Main.LogInfo($"检查模组：{path}");
                if (Directory.Exists(path) && File.Exists($"{path}/Mod.bin"))
                {
                    try
                    {
                        Mods.Add(ModEditorManager.I.LoadWorkshop(path));
                    }
                    catch (Exception message)
                    {
                        Main.LogError($"读取Mod配置文件失败：{path}\n{message}");
                    }
                }
            }
        }
    }
}