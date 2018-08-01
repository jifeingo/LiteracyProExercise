using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using LiteracyPro.Biz.Data;

namespace LiteracyPro.Biz.Operations
{
    public abstract class Operation
    {
        private LiteracyProEntities _dbContext = null;
        protected LiteracyProEntities DB
        {
            get { return _dbContext ?? (_dbContext = new LiteracyProEntities()); }
        }

        protected Operation()
        {
        }

        /// <summary>
        /// Constructor to use when a database connection has already been established.
        /// For example, this may be a sub-operation of an outer.
        /// </summary>
        /// <param name="db"></param>
        protected Operation(LiteracyProEntities db)
        {
            _dbContext = db;
        }

        protected abstract string DisplayName { get; }

        /// <summary>
        /// Perform any instantiation for the operation.
        /// </summary>
        /// <returns>bool indicating success or failure</returns>
        protected virtual List<ResultMessage> InstantiateImpl()
        {
            return new List<ResultMessage>();
        }


        /// <summary>
        /// Perform validation of the inputModel object prior to the execution phase.
        /// </summary>
        /// <returns>bool indicating success or failure</returns>
        protected abstract List<ResultMessage> ValidateImpl();
        

        /// <summary>
        /// Perform the operation (usually database operation) on the object.
        /// </summary>
        /// <returns>A list of validation messages.  Empty or Null indicates successful execution</returns>
        protected abstract List<ResultMessage> ExecuteImpl();
        

        /// <summary>
        /// Any code required to run after the database save has occurred.
        /// For example, returning back the identity value may occur here.
        /// </summary>
        protected virtual List<ResultMessage> PostSaveImpl()
        {
            return new List<ResultMessage>();
        }


        /// <summary>
        /// Responsible for performing the data-store save opeartions in the
        /// correct order.
        /// </summary>
        protected virtual List<ResultMessage> SaveImpl()
        {
            var result = new List<ResultMessage>();


            if (_dbContext != null)
            {
                _dbContext.SaveChanges();
            }

            return new List<ResultMessage>();
        }


        /// <summary>
        /// Method responsible for returning the successful result
        /// It is virtual so that the generic versions of this class 
        /// can return a more specific operation result with an updated OutputModel
        /// </summary>
        protected virtual OperationResult GetOperationResult(List<ResultMessage> messages)
        {
            return new OperationResult(messages.ToArray());
        }

        public OperationResult Execute()
        {
            var resultMessages = new List<ResultMessage>();

            //Instiantiate OutputModel
            try
            {
                var validationMessages = InstantiateImpl();

                if (validationMessages != null && validationMessages.Any(m => ResultMessage.FailMessageTypes.Contains(m.Type)))
                {
                    return new OperationResult(validationMessages.ToArray());
                }

                resultMessages.AddRange(validationMessages);

            }
            catch (Exception ex)
            {
                return new OperationResult(ex.ToResultMessage(OperationSection.Instantiation, "An unexpected error has occurred - {0}", DisplayName));
            }


            //Validate
            try
            {
                var validationMessages = ValidateImpl();

                if (validationMessages != null && validationMessages.Any(m=>ResultMessage.FailMessageTypes.Contains(m.Type)))
                {
                    return new OperationResult(validationMessages.ToArray());
                }

                resultMessages.AddRange(validationMessages);
            }
            catch (Exception ex)
            {
                return new OperationResult(ex.ToResultMessage(OperationSection.Validation, "An unexpected error has occurred - {0}", DisplayName));
            }


            //Execute Logic
            try
            {
                var executionMessages = ExecuteImpl();

                if (executionMessages != null && executionMessages.Any(m => ResultMessage.FailMessageTypes.Contains(m.Type)))
                {
                    return new OperationResult(executionMessages.ToArray());
                }

                resultMessages.AddRange(executionMessages);
            }
            catch (Exception ex)
            {
                return new OperationResult(ex.ToResultMessage(OperationSection.Execution, "An unexpected error has occurred - {0}", DisplayName));
            }


            //Execute Save
            try
            {
                var saveMessages = SaveImpl();

                if (saveMessages != null && saveMessages.Any(m => ResultMessage.FailMessageTypes.Contains(m.Type)))
                {
                    return new OperationResult(saveMessages.ToArray());
                }

                resultMessages.AddRange(saveMessages);

            }
            catch (Exception ex)
            {
                return new OperationResult(ex.ToResultMessage(OperationSection.Save, "An unexpected error has occurred - {0}", DisplayName));
            }


            //Any logic required after save
            try
            {
                var postSaveMessages = PostSaveImpl();

                if (postSaveMessages != null && postSaveMessages.Any(m => ResultMessage.FailMessageTypes.Contains(m.Type)))
                {
					//This call differs from the others in that a inputModel has been saved, so it can be returned.
                    return GetOperationResult(postSaveMessages);
                }

                resultMessages.AddRange(postSaveMessages);
            }
            catch (Exception ex)
            {
                return new OperationResult(ex.ToResultMessage(OperationSection.PostSave, "An unexpected error has occurred - {0}", DisplayName));
            }


            return GetOperationResult(resultMessages);
        }
    }

    /// <summary>
    /// Used for operations that do not have an input or output model
    /// </summary>
    public abstract class OperationNoModel : Operation
    {
        protected OperationNoModel() : base()
        {
        }

    }

	public abstract class Operation<TOutputModel> : Operation
		where TOutputModel : class, new()
	{
		private TOutputModel _outputModel = null;
		public TOutputModel OutputModel => _outputModel;


		protected void SetOuptutModel(TOutputModel model)
		{
			_outputModel = model;
		}

		protected Operation() : base()
		{

		}

		protected override OperationResult GetOperationResult(List<ResultMessage> messages)
		{
			return new OperationResult<TOutputModel>(OutputModel, messages.ToArray());
		}
	}


	public abstract class Operation<TOutputModel, TViewData> : Operation<TOutputModel>
		where TOutputModel : class, new()
		where TViewData : class, new()
	{

		private TViewData _viewData = null;
		public TViewData ViewData => _viewData ?? (_viewData = new TViewData());

		protected void SetViewData(TViewData viewData)
		{
			_viewData = viewData;
		}

		protected Operation() : base()
		{

		}

		protected override OperationResult GetOperationResult(List<ResultMessage> messages)
		{
			return new OperationResult<TOutputModel, TViewData>(OutputModel, ViewData, messages.ToArray());
		}

	}

	
    public abstract class OperationFromJSON<TInputModel, TOutputModel> : Operation<TOutputModel>
        where TInputModel : class, new()
		where TOutputModel : class, new()
	{


	    private TInputModel _inputModel = null;
	    public TInputModel InputModel => _inputModel ?? (_inputModel = new TInputModel());

	    protected void SetInputModel(TInputModel inputModel)
	    {
		    _inputModel = inputModel;
	    }


		private string _inputModelJSON { get; set; }

        protected OperationFromJSON(string jsonInputModel) : base()
        {
            _inputModelJSON = jsonInputModel;
        }

        protected override List<ResultMessage> InstantiateImpl()
        {
            var result = base.InstantiateImpl();
            
            try
            {
                var model = JsonConvert.DeserializeObject<TInputModel>(_inputModelJSON);
                SetInputModel(model);
            }
            catch(Exception ex)
            {
                result.Add(ex.ToResultMessage(OperationSection.Instantiation, "Failed to deserialize inputModel"));
            }
            
            return result;
        }
    }


    public abstract class OperationFromJSON<TInputModel, TOutputModel, TViewData> : Operation<TOutputModel, TViewData>
	    where TInputModel : class, new()
	    where TOutputModel : class, new()
		where TViewData : class, new()
    {

	    private TInputModel _inputModel = null;
	    public TInputModel InputModel => _inputModel ?? (_inputModel = new TInputModel());

	    protected void SetInputModel(TInputModel inputModel)
	    {
		    _inputModel = inputModel;
	    }

		private string _modelJSON { get; set; }

        protected OperationFromJSON(string jsonModel) : base()
        {
            _modelJSON = jsonModel;
        }

        protected OperationFromJSON(string jsonModel, TViewData viewData) : this(jsonModel)
        {
            SetViewData(viewData);
        }

        protected override List<ResultMessage> InstantiateImpl()
        {
            var result = base.InstantiateImpl();

            try
            {
                var model = JsonConvert.DeserializeObject<TInputModel>(_modelJSON);
                SetInputModel(model);
            }
            catch (Exception ex)
            {
                result.Add(ex.ToResultMessage(OperationSection.Instantiation, "Failed to deserialize inputModel"));
            }

            return result;
        }

    }

}

