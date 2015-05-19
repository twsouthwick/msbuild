﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.Build.Tasks
{
    /// <summary>
    /// A parameter passed to the task was invalid.
    /// Currently used by the RAR task.
    /// ArgumentException was not used because it does not have a property for ActualValue.
    /// ArgumentOutOfRangeException does, but it appends its own message to yours.
    /// </summary>
#if !CORECLR
    [Serializable]
#endif
    internal sealed class InvalidParameterValueException : Exception
    {
        /// <summary>
        /// Don't allow default construction.
        /// </summary>
        private InvalidParameterValueException()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        internal InvalidParameterValueException(string paramName, string actualValue, string message)
            : this(paramName, actualValue, message, null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        internal InvalidParameterValueException(string paramName, string actualValue, string message, Exception innerException)
            : base(message, innerException)
        {
            ParamName = paramName;
            ActualValue = actualValue;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private InvalidParameterValueException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        private string _paramName;

        /// <summary>
        /// The name of the parameter.
        /// </summary>
        public string ParamName
        {
            get { return _paramName; }
            set { _paramName = value; }
        }

        private string _actualValue;

        /// <summary>
        /// The value supplied, that was bad.
        /// </summary>
        public string ActualValue
        {
            get { return _actualValue; }
            set { _actualValue = value; }
        }
    }
}
