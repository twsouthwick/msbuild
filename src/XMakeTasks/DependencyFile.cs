﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

using Microsoft.Build.Shared;

namespace Microsoft.Build.Tasks
{
    /// <remarks>
    /// Represents a single input to a compilation-style task.
    /// Keeps track of timestamp for later comparison.
    /// </remarks>
#if !CORECLR
    [Serializable]
#endif
    internal class DependencyFile
    {
        // Filename
        private string _filename;

        // Date and time the file was last modified           
        private DateTime _lastModified;

        // Whether the file exists or not.
        private bool _exists = false;

        /// <summary>
        /// The name of the file.
        /// </summary>
        /// <value></value>
        internal string FileName
        {
            get { return _filename; }
        }

        /// <summary>
        /// The last-modified timestamp when the class was instantiated.
        /// </summary>
        /// <value></value>
        internal DateTime LastModified
        {
            get { return _lastModified; }
        }

        /// <summary>
        /// Returns true if the file existed when this class was instantiated.
        /// </summary>
        /// <value></value>
        internal bool Exists
        {
            get { return _exists; }
        }

        /// <summary>
        /// Construct.
        /// </summary>
        /// <param name="filename">The file name.</param>
        internal DependencyFile(string filename)
        {
            _filename = FileUtilities.FixFilePath(filename);

            if (File.Exists(FileName))
            {
                _lastModified = File.GetLastWriteTime(FileName);
                _exists = true;
            }
            else
            {
                _exists = false;
            }
        }

        /// <summary>
        /// Checks whether the file has changed since the last time a timestamp was recorded.
        /// </summary>
        /// <returns></returns>
        internal bool HasFileChanged()
        {
            FileInfo info = FileUtilities.GetFileInfoNoThrow(_filename);

            // Obviously if the file no longer exists then we are not up to date.
            if (info == null || !info.Exists)
            {
                return true;
            }

            // Check the saved timestamp against the current timestamp.
            // If they are different then obviously we are out of date.
            DateTime curLastModified = info.LastWriteTime;
            if (curLastModified != _lastModified)
            {
                return true;
            }

            // All checks passed -- the info should still be up to date.
            return false;
        }
    }
}