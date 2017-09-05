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
