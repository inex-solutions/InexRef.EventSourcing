#region Copyright & License
// The MIT License (MIT)
// 
// Copyright 2017 INEX Solutions Ltd
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

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