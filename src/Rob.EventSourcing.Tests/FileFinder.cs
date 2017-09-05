using System;
using System.IO;

namespace Rob.EventSourcing.Tests
{
    /// <summary>
    /// Locates files matching the search pattern in the specified directory and calls the supplied callback when each matching file is located.
    /// </summary>
    public static class FileFinder
    {
        /// <summary>
        /// Locates files in the specified directory and calls the supplied callback when each matching file is located.
        /// </summary>
        /// <param name="directory">Directory in which to look for files.</param>
        /// <param name="recurse">True to recurse into sub directories, otherwise false.</param>
        /// <param name="onFileFoundCallback">Callback to call on each matching file</param>
        public static void FindFile(string directory, bool recurse, Action<FileInfo> onFileFoundCallback)
        {
            FindFile("*", new DirectoryInfo(directory), recurse, onFileFoundCallback);
        }

        /// <summary>
        /// Locates files matching the search pattern in the specified directory and calls the supplied callback when each matching file is located.
        /// </summary>
        /// <param name="searchPattern">Search pattern of file(s) to find, e.g. *.txt or *.*</param>
        /// <param name="directory">Directory in which to look for files.</param>
        /// <param name="recurse">True to recurse into sub directories, otherwise false.</param>
        /// <param name="onFileFoundCallback">Callback to call on each matching file</param>
        public static void FindFile(string searchPattern, string directory, bool recurse, Action<FileInfo> onFileFoundCallback)
        {
            FindFile(searchPattern, new DirectoryInfo(directory), recurse, onFileFoundCallback);
        }

        /// <summary>
        /// Locates files matching the search pattern in the specified directory and calls the supplied callback when each matching file is located.
        /// </summary>
        /// <param name="searchPattern">Search pattern of file(s) to find, e.g. *.txt or *.*</param>
        /// <param name="directory">Directory in which to look for files.</param>
        /// <param name="recurse">True to recurse into sub directories, otherwise false.</param>
        /// <param name="onFileFoundCallback">Callback to call on each matching file</param>
        public static void FindFile(string searchPattern, DirectoryInfo directory, bool recurse, Action<FileInfo> onFileFoundCallback)
        {
            if (recurse)
            {
                foreach (var subDirectory in directory.GetDirectories())
                {
                    FindFile(searchPattern, subDirectory, recurse, onFileFoundCallback);
                }
            }

            foreach (var file in directory.GetFiles(searchPattern))
            {
                onFileFoundCallback(file);
            }
        }
    }
}