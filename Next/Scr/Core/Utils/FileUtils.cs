using System;
using System.IO;
using UnityEngine;

namespace SkySwordKill.Next.Utils;

public class FileUtils
{
    public delegate void FileHandle(string virtualPath, string filePath);
    
    public static void DirectoryHandle(string rootPath, string dirPath, FileHandle fileHandle)
    {
        if(!Directory.Exists(dirPath))
            return;

        foreach (var directory in Directory.GetDirectories(dirPath))
        {
            var name = Path.GetFileNameWithoutExtension(directory);
            if (string.IsNullOrEmpty(rootPath))
            {
                DirectoryHandle($"{name}", directory, fileHandle);
            }
            else
            {
                DirectoryHandle($"{rootPath}/{name}", directory, fileHandle);
            }
            
        }

        foreach (var file in Directory.GetFiles(dirPath))
        {
            var fileName = Path.GetFileName(file);
                
            var cachePath = $"{rootPath}/{fileName}";
            fileHandle.Invoke(cachePath, file);
        }
    }
    
    public static string OpenDirectorySelector(string title, string directory)
    {
        FolderPicker folderPicker = new FolderPicker();
        folderPicker.Title = title;
        bool? flag = folderPicker.ShowDialog(IntPtr.Zero, false);
        if (flag == null || !flag.Value)
        {
            return directory;
        }
        string resultPath = folderPicker.ResultPath;
        return resultPath;
    }
}