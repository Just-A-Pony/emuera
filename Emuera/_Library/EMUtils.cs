using System.IO;

namespace MinorShift.Emuera.EvilMask
{
    internal sealed class Utils
    {
        public static string GetValidPath(string path)
        {
            path =  path.Replace('/', '\\').Replace("..\\", "");
            if (Path.GetPathRoot(path) != string.Empty) return null;
            return path;
        }
        public static void LoadLangFile()
        {

        }
    }
}
