using System.IO;
using FairyGUI;
using SkySwordKill.Next.FGUI;
using SkySwordKill.Next.FGUI.DialogBuilder;
using SkySwordKill.Next.Utils;
using SkySwordKill.NextFGUI.NextCore;
using SkySwordKill.NextModEditor.Mod;
using UnityEngine;

namespace SkySwordKill.Next.NextModEditor.Window;

public class WindowCreateABProjectDialog : WindowDialogBase
{
    public WindowCreateABProjectDialog() : base("NextCore", "WinCreateABMod")
    {
    }

    public UI_WinCreateABMod MainView => contentPane as UI_WinCreateABMod;
    public GTextInput InputProjectName { get; set; }
    public GTextInput InputProjectPath { get; set; }
    public GTextInput InputOutputABPath { get; set; }
    
    protected override void OnInit()
    {
        base.OnInit();
        
        var mainView = MainView;
        mainView.m_btnOk.onClick.Add(OnClickOk);
        mainView.m_btnCancle.onClick.Add(OnClickCancel);
        mainView.m_frame.m_closeButton.onClick.Add(OnClickCancel);
        mainView.m_btnEditProjectPath.onClick.Add(OnClickEditProjectPath);
        mainView.m_btnEditOutputABPath.onClick.Add(OnClickEditOutputABPath);
        
        InputProjectName = mainView.m_inputABName.GetChild("inContent").asTextInput;
        InputProjectPath = mainView.m_inputProjectPath.GetChild("inContent").asTextInput;
        InputOutputABPath = mainView.m_inputExportPath.GetChild("inContent").asTextInput;
    }

    private void OnClickEditOutputABPath()
    {
        InputOutputABPath.text = FileUtils.OpenDirectorySelector("选择输出目录", InputOutputABPath.text);
    }

    private void OnClickEditProjectPath()
    {
        InputProjectPath.text = FileUtils.OpenDirectorySelector("选择Unity工程目录", InputProjectPath.text);
    }

    protected override void OnKeyDown(EventContext context)
    {
        base.OnKeyDown(context);
        
        if (context.inputEvent.keyCode == KeyCode.Escape)
        {
            OnClickCancel();
        }
    }
    
    private void OnClickCancel()
    {
        Hide();
    }

    private void OnClickOk()
    {
        CreateABProjectStep1();
    }
    
    private void CreateABProjectStep1()
    {
        // 如果目录为空
        if(string.IsNullOrEmpty(InputProjectPath.text))
        {
            new WindowConfirmDialogBuilder()
                .SetTitle("错误")
                .SetContent("Unity工程目录不能为空")
                .SetCanCancel(false)
                .Build()
                .Show();
            return;
        }
        
        if(string.IsNullOrEmpty(InputOutputABPath.text))
        {
            new WindowConfirmDialogBuilder()
                .SetTitle("错误")
                .SetContent("输出目录不能为空")
                .SetCanCancel(false)
                .Build()
                .Show();
            return;
        }
        
        if(string.IsNullOrEmpty(InputProjectName.text))
        {
            new WindowConfirmDialogBuilder()
                .SetTitle("错误")
                .SetContent("AB包名称不能为空")
                .SetCanCancel(false)
                .Build()
                .Show();
            return;
        }
        
        // 如果目录里没有文件
        if(Directory.Exists(InputProjectPath.text) && Directory.GetFiles(InputProjectPath.text).Length > 0)
        {
            new WindowConfirmDialogBuilder()
                .SetTitle("提示")
                .SetContent("目标目录不为空，是否继续创建？")
                .SetCanCancel(true)
                .SetOnConfirm(CreateABProjectStep2)
                .Build()
                .Show();
            return;
        }
        
        CreateABProjectStep2();
    }

    private void CreateABProjectStep2()
    {
        // 如果输出目录不为AssetBundles
        var dirName = Path.GetFileNameWithoutExtension(InputOutputABPath.text);
        if(dirName != "AssetBundles")
        {
            new WindowConfirmDialogBuilder()
                .SetTitle("提示")
                .SetContent("输出目录不为AssetBundles，可能导致打包AB包不能正确读取，是否继续创建？")
                .SetCanCancel(true)
                .SetOnConfirm(CreateABProjectStep3)
                .Build()
                .Show();
            return;
        }
        
        CreateABProjectStep3();
    }

    private void CreateABProjectStep3()
    {
        var projectName = InputProjectName.text;
        var projectDir = InputProjectPath.text;
        var exportDir = InputOutputABPath.text;
        
        ModUtils.CreateUnityABTemplateProject(projectName, projectDir, exportDir);
        Hide();
        
        new WindowConfirmDialogBuilder()
            .SetTitle("提示")
            .SetContent("创建成功，是否打开Unity工程目录？")
            .SetCanCancel(true)
            .SetOnConfirm(() =>
            {
                Application.OpenURL(projectDir);
            })
            .Build()
            .Show();
    }
}