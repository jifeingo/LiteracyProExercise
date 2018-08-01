using System;
using System.Collections.Generic;

namespace LiteracyPro.Biz.Operations
{
	public static class OperationExtensions
	{
		public static ResultMessage ToResultMessage(this Exception ex, OperationSection area, string templateString, params object[] replacements)
		{
			return ResultMessage.Exception(area, ex, templateString, replacements);
		}

	}

	public class ResultMessage
    {

		/// <summary>
		/// Just a simple static to get the list quickly.
		/// </summary>
		/// <returns></returns>
	    public static List<ResultMessage> NewList()
	    {
		    return new List<ResultMessage>();
	    }

        public enum MessageType
        {
            Debug = 0,                  //Informational Messages
            Warning = 1,                //Warning feedback to caller
            Error = 2,                  //A data condition ocurred that prevented execution.
            Exception = 3,              //A hard-error has occurred.
            Feedback = 4                //Result information feedback to caller.
        }

        public static MessageType[] FailMessageTypes
        {
            get { return new[] { MessageType.Error, MessageType.Exception }; }
        }


        /// <summary>
        /// Used to indicate the execution was halted due to an unforseen problem
        /// </summary>
        public static ResultMessage Exception(OperationSection source, Exception ex, string publicMessageTemplate, params object[] replacements)
        {
            return Exception(source, ex.ToString(), publicMessageTemplate, replacements);
        }

        /// <summary>
        /// Used to indicate the execution was halted due to an unforseen problem
        /// </summary>
        public static ResultMessage Exception(OperationSection source, string internalMessage, string publicMessageTemplate, params object[] replacements)
        {
            return new ResultMessage(source, MessageType.Exception, string.Format(publicMessageTemplate, replacements), internalMessage);
        }

        /// <summary>
        /// Used to return caution/warning messages back to the caller.
        /// </summary>
        public static ResultMessage Warning(OperationSection source, string internalMessage, string publicMessageTemplate, params object[] replacements)
        {
            return new ResultMessage(source, MessageType.Warning, string.Format(publicMessageTemplate, replacements), internalMessage);
        }

        /// <summary>
        /// Debug messages that may help support understand
        /// </summary>
        public static ResultMessage Debug(OperationSection source, string internalMessage, string publicMessageTemplate, params object[] replacements)
        {
            return new ResultMessage(source, MessageType.Debug, string.Format(publicMessageTemplate, replacements), internalMessage);
        }

        /// <summary>
        /// Used to indicate the reason why execution was halted based on a condition
        /// This should not be used to record unexpected errors
        /// </summary>
        public static ResultMessage Error(OperationSection source, string internalMessage, string publicMessageTemplate, params object[] replacements)
        {
            return new ResultMessage(source, MessageType.Error, string.Format(publicMessageTemplate, replacements), internalMessage);
        }

        /// <summary>
        /// Provides visible feedback to the caller.  This may be information that you would display on a web page.
        /// This should not be used to record unexpected errors
        /// </summary>
        public static ResultMessage Feedback(OperationSection source, string internalMessage, string publicMessageTemplate, params object[] replacements)
        {
            return new ResultMessage(source, MessageType.Feedback, string.Format(publicMessageTemplate, replacements), internalMessage);
        }


        public MessageType Type { get; private set; }
        public string TypeName => Enum.GetName(typeof(MessageType), Type);

	    public OperationSection Source { get; private set; }
        public string SourceName => Enum.GetName(typeof(OperationSection), Source);

	    public string PublicMessage { get; private set; }

        public string InternalMessage { get; private set; }

        private ResultMessage(OperationSection source, MessageType type, string publicMessage, string internalMessage = null)
        {
            Type = type;
            Source = source;
            InternalMessage = internalMessage;
            PublicMessage = publicMessage;
        }
    }
}
