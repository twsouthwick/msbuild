﻿//-----------------------------------------------------------------------
// <copyright file="OutOfProcTaskAppDomainWrapper.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>Class implementing an out-of-proc node for executing tasks inside an AppDomain.</summary>
//-----------------------------------------------------------------------
using System;
using Microsoft.Build.Shared;

namespace Microsoft.Build.CommandLine
{
    /// <summary>
    /// Class for executing a task in an AppDomain
    /// </summary>
#if !CORECLR
    [Serializable]
#endif
    internal class OutOfProcTaskAppDomainWrapper : OutOfProcTaskAppDomainWrapperBase
    {
        /// <summary>
        /// This is a stub for CLR2 in place of the OutOfProcTaskAppDomainWrapper class
        /// as used in CLR4 to support cancellation of ICancelable tasks.
        /// We provide a stub for CancelTask here so that the OutOfProcTaskHostNode
        /// that's shared by both the MSBuild.exe and MSBuildTaskHost.exe,
        /// can safely allow MSBuild.exe CLR4 Out-Of-Proc Task Host to call ICancelableTask.Cancel()
        /// </summary>
        /// <returns>False - Used by the OutOfProcTaskHostNode to determine if the task is ICancelable</returns>
        internal bool CancelTask()
        {
            // This method is a stub we will not do anything here.
            return false;
        }
    }
}
