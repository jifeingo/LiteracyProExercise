using System.Collections.Generic;
using System.Linq;

namespace LiteracyPro.Biz.Operations
{
    public class OperationResult
    {
        public bool Successful
        {
            get
            {
                return Messages != null && !Messages.Any(m => ResultMessage.FailMessageTypes.Contains(m.Type));
            }
        }

        public bool HasExceptions
        {
            get
            {
                return Messages != null && Messages.Any(m => m.Type == ResultMessage.MessageType.Exception);
            }
        }

        public bool HasErrors
        {
            get
            {
                return Messages != null && Messages.Any(m => m.Type == ResultMessage.MessageType.Error);
            }
        }


        public List<ResultMessage> Messages { get; private set; }

        public OperationResult()
        {
            Messages = new List<ResultMessage>();
        }

        public OperationResult(params ResultMessage[] messages) : this()
        {
            Messages.AddRange(messages);
        }
	}

    public class OperationResult<TOutputModel> : OperationResult 
        where TOutputModel : class, new()
    {
        public TOutputModel Model { get; private set; }

        public OperationResult(TOutputModel model, params ResultMessage[] messages) : base(messages)
        {
            Model = model;
        }

		/// <summary>
		/// Allows deserialization
		/// </summary>
	    public OperationResult() : base()
	    {
	    }
    }

    public class OperationResult<TOutputModel, TViewData> : OperationResult<TOutputModel>
        where TOutputModel : class, new()
		where TViewData: class, new()
	{

        public TViewData ViewData { get; private set; }

        public OperationResult(TOutputModel model, TViewData viewData, params ResultMessage[] messages) 
            : base(model, messages)
        {
            ViewData = viewData;
        }

		public OperationResult() : base()
		{
			
		}
    }
}
