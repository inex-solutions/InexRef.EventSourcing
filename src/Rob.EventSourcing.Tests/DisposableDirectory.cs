#region Copyright & License
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
    /// The disposable directory is a directory that is automatically deleted on disposable.
    /// </summary>
    public class DisposableDirectory : IDisposable
    {
        private readonly bool _throwOnFailedCleanup;

        private readonly DirectoryInfo _wrappedDirectory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableDirectory"/> class.
        /// </summary>
        public DisposableDirectory()
            : this(true)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableDirectory"/> class.
        /// </summary>
        /// <param name="throwOnFailedCleanup">True to throw n exception if cleanup fails, otherwise false.</param>
        public DisposableDirectory(bool throwOnFailedCleanup)
            : this(Guid.NewGuid().ToString(), throwOnFailedCleanup)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableDirectory"/> class.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="throwOnFailedCleanup">if set to <c>true</c> [throw on failed cleanup].</param>
        public DisposableDirectory(string directoryPath, bool throwOnFailedCleanup)
        {
            if (directoryPath == null) throw new ArgumentNullException(nameof(directoryPath));

            _throwOnFailedCleanup = throwOnFailedCleanup;
            _wrappedDirectory = new DirectoryInfo(directoryPath);

            if (!_wrappedDirectory.Exists)
            {
                _wrappedDirectory.Create();
            }
            _wrappedDirectory.Refresh();
        }

        /// <summary>
        /// Gets the full path of the directory 
        /// </summary>
        public string FullName => _wrappedDirectory.FullName;

        /// <summary>
        /// Causes the directory to be deleted.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _wrappedDirectory.Refresh();
                if (_wrappedDirectory.Exists)
                {
                    try
                    {
                        FileFinder.FindFile("*.*", _wrappedDirectory, true, info => info.Attributes &= ~FileAttributes.ReadOnly);
                        _wrappedDirectory.Delete(true);
                        _wrappedDirectory.Refresh();
                    }
                    catch (IOException)
                    {
                        if (_throwOnFailedCleanup) throw;
                    }
                    catch (UnauthorizedAccessException)
                    {
                        if (_throwOnFailedCleanup) throw;
                    }
                }
            }
        }
    }
}
