using UnityEngine;
using System.Collections;
using System.IO;
public class Function
{
    public static void del(string dir)
    {
        if (Directory.Exists(dir))
        {
            DeleteFolder(dir);
        }
        else if (File.Exists(dir))
        {
            File.Delete(dir);
        }

    }


    public static void DeleteFolder(string dir)
    {
        if (!Directory.Exists(dir))
            return;
        string[] str = Directory.GetFileSystemEntries(dir);
        foreach (string d in str)
        {
            Debug.Log(d);
            if (File.Exists(d))
            {
                FileInfo fi = new FileInfo(d);
                if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                    fi.Attributes = FileAttributes.Normal;
                File.Delete(d);
            }
            else
            {
                DirectoryInfo d1 = new DirectoryInfo(d);

                DirectoryInfo[] d2 = d1.GetDirectories();
                if (d2.Length != 0)
                {
                    DeleteFolder(d1.FullName);////递归删除子文件夹
                }

                FileInfo[] f = d1.GetFiles();
                if (f.Length != 0)
                {
                    DeleteFolder(d1.FullName);////递归删除子文件夹
                }

                Directory.Delete(d);
            }
        }
    }

    public static void MoveDirectory(string sourcePath, string destinationPath)
    {
        CopyDirectory(sourcePath, destinationPath);
        DeleteFolder(sourcePath);
    }


    public static void CopyDirectory(string sourcePath, string destinationPath, string filter = "")
    {
        DirectoryInfo info = new DirectoryInfo(sourcePath);
        Directory.CreateDirectory(destinationPath);
        foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
        {
            string destName = Path.Combine(destinationPath, fsi.Name);
            if (fsi is System.IO.FileInfo)
            {
                if (filter == "" || fsi.FullName.EndsWith(filter))
                {
                    if (File.Exists(destName))
                        File.Delete(destName);
                    File.Copy(fsi.FullName, destName, true);
                }
            }
            else
            {
                if (!Directory.Exists(destName))
                    Directory.CreateDirectory(destName);
                CopyDirectory(fsi.FullName, destName);
            }
        }
    }
}