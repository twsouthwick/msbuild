﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;

using Microsoft.Build.Shared;

namespace Microsoft.Build.CommandLine
{
    /// <summary>
    /// This exception is used to flag (syntax) errors in command line switches passed to the application.
    /// </summary>
#if !CORECLR
    [Serializable]
#endif
    internal sealed class CommandLineSwitchException : Exception
    {
        /// <summary>
        /// This constructor initializes the exception message.
        /// </summary>
        /// <param name="message"></param>
        private CommandLineSwitchException
        (
            string message
        ) :
            base(message)
        {
            // do nothing
        }

        /// <summary>
        /// This constructor initializes the exception message and saves the command line argument containing the switch error.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="commandLineArg"></param>
        private CommandLineSwitchException
        (
            string message,
            string commandLineArg
        ) :
            this(message)
        {
            _commandLineArg = commandLineArg;
        }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        private CommandLineSwitchException
        (
            SerializationInfo info,
            StreamingContext context
        ) :
            base(info, context)

        {
            ErrorUtilities.VerifyThrowArgumentNull(info, "info");

            _commandLineArg = info.GetString("commandLineArg");
        }

        /// <summary>
        /// Gets the error message and the invalid switch, or only the error message if no invalid switch is set.
        /// </summary>
        public override string Message
        {
            get
            {
                if (_commandLineArg == null)
                {
                    return base.Message;
                }
                else
                {
                    return base.Message + Environment.NewLine + ResourceUtilities.FormatResourceString("InvalidSwitchIndicator", _commandLineArg);
                }
            }
        }

        /// <summary>
        /// Gets the invalid switch that caused the exception.
        /// </summary>
        /// <value>Can be null.</value>
        internal string CommandLineArg
        {
            get
            {
                return _commandLineArg;
            }
        }

        // the invalid switch causing this exception
        private string _commandLineArg;

        /// <summary>
        /// Serialize the contents of the class.
        /// </summary>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("commandLineArg", _commandLineArg, typeof(string));
        }

        /// <summary>
        /// Throws the exception if the specified condition is not met.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="messageResourceName"></param>
        /// <param name="commandLineArg"></param>
        internal static void VerifyThrow(bool condition, string messageResourceName, string commandLineArg)
        {
            if (!condition)
            {
                Throw(messageResourceName, commandLineArg);
            }
#if DEBUG
            else
            {
                ResourceUtilities.VerifyResourceStringExists(messageResourceName);
            }
#endif
        }

        /// <summary>
        /// Throws the exception using the given message and the command line argument containing the switch error.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="messageResourceName"></param>
        /// <param name="commandLineArg"></param>
        internal static void Throw(string messageResourceName, string commandLineArg)
        {
            Throw(messageResourceName, commandLineArg, String.Empty);
        }

        /// <summary>
        /// Throws the exception using the given message and the command line argument containing the switch error.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="messageResourceName"></param>
        /// <param name="messageArgs"></param>
        internal static void Throw(string messageResourceName, string commandLineArg, params string[] messageArgs)
        {
            string errorMessage = ResourceUtilities.FormatResourceString(messageResourceName, messageArgs);

            ErrorUtilities.VerifyThrow(errorMessage != null, "The resource string must exist.");

            throw new CommandLineSwitchException(errorMessage, commandLineArg);
        }
    }
}
