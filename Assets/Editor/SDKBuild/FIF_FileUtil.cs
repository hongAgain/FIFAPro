using System.IO;

public static class FIF_FileUtil
{
    public static void MoveFolderTo(string directorySource, string directoryTarget)
    {
        if (!Directory.Exists(directorySource))
        {
            Directory.CreateDirectory(directorySource);
        }
        if (!Directory.Exists(directoryTarget))
        {
            Directory.CreateDirectory(directoryTarget);
        }

        DirectoryInfo sourceDirInfo = new DirectoryInfo(directorySource);
        FileInfo[] files = sourceDirInfo.GetFiles("*.*");

        foreach (FileInfo file in files)
        {
            if (File.Exists(Path.Combine(directoryTarget, file.Name)))
            {
                if (File.Exists(Path.Combine(directoryTarget, file.Name + ".bak")))
                {
                    File.Delete(Path.Combine(directoryTarget, file.Name + ".bak"));
                }
                File.Move(Path.Combine(directoryTarget, file.Name), Path.Combine(directoryTarget, file.Name + ".bak"));
            }
            file.MoveTo(Path.Combine(directoryTarget, file.Name));
        }

        DirectoryInfo[] directoryInfoArray = sourceDirInfo.GetDirectories();
        foreach (DirectoryInfo dir in directoryInfoArray)
        {
            MoveFolderTo(Path.Combine(directorySource, dir.Name), Path.Combine(directoryTarget, dir.Name));
        }

        Directory.Delete(directorySource);
    }

    public static void MoveFileTo(string from, string to)
    {
        FileInfo file = new FileInfo(from);
        try
		{
			file.MoveTo(to);
		}
		catch (System.Exception)
		{
			
		}
    }

    public static void CopyFolderTo(string directorySource, string directoryTarget)
    {
        if (!Directory.Exists(directoryTarget))
        {
            Directory.CreateDirectory(directoryTarget);
        }
        if (!Directory.Exists(directorySource))
        {
            Directory.CreateDirectory(directorySource);
        }

        DirectoryInfo directoryInfo = new DirectoryInfo(directorySource);
        FileInfo[] files = directoryInfo.GetFiles();

        foreach (FileInfo file in files)
        {
            file.CopyTo(Path.Combine(directoryTarget, file.Name), true);
        }

        DirectoryInfo[] directoryInfoArray = directoryInfo.GetDirectories();
        foreach (DirectoryInfo dir in directoryInfoArray)
        {
            CopyFolderTo(Path.Combine(directorySource, dir.Name), Path.Combine(directoryTarget, dir.Name));
        }
    }

    public static void CopyFileTo(string from, string to)
    {
        FileInfo file = new FileInfo(from);
        FileInfo toFile = new FileInfo(to);
        if (toFile.Directory.Exists == false)
        {
            Directory.CreateDirectory(toFile.DirectoryName);
        }

        file.CopyTo(to, true);
    }
}