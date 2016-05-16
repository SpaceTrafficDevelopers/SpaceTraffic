/**
Copyright 2010 FAV ZCU

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

	http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

**/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpaceTraffic.GameUi.GameServerClient;
using System.Web.Script.Serialization;
using Microsoft.CSharp.RuntimeBinder;
using System.IO;
using SpaceTraffic.GameUi.Controllers.AjaxHandlers;


namespace SpaceTraffic.GameUi.Controllers
{
	/* comes in */
	public class RequestObject
	{
		public string requestId = "unknown";
		public string relatedObject;
		public object data;
		public int repeatEvery;
	}

	/* this is send back */
	public class DataResponse
	{
		public string requestId;
		public object data;
	}

	/* json class when error occures*/
	public class ErrorResponse {

		public string requestId;
		public string error;
	}
	
	/**
	 * This controller process ajax requests, creates utility object and sends it's response back
	 */
	public class AjaxController : AbstractController
	{

		private Dictionary<String, IAjaxHandleable> handlers = new Dictionary<String, IAjaxHandleable>();

		//
		// POST: /Ajax/
		[HttpPost]
		public JsonResult Index()
		{
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			String postData = new System.IO.StreamReader(Request.InputStream).ReadToEnd();/* reading POST data*/
			List<RequestObject> requestObjects = serializer.Deserialize<List<RequestObject>>(postData);/* parsing json data */
			List<object> response = new List<object>();

			foreach (RequestObject requestObject in requestObjects)
			{
				response.Add(handleRequestObject(requestObject));
			}

			JsonResult result = Json(response, JsonRequestBehavior.AllowGet);
			return result;
		}


		/// <summary>
		/// For given requestObject creates and calls relatedObject with interface IAjaxHandleable
		/// </summary>
		/// <param name="requestObject">The request object.</param>
		/// <returns>ErrorObject or specific IAjaxHandleable object</returns>
		private object handleRequestObject(RequestObject requestObject)
		{
			try
			{
				IAjaxHandleable handler;
				if (handlers.ContainsKey(requestObject.relatedObject)) {
					handler = handlers[requestObject.relatedObject];
				}else{/* is not in list - we have to create it */
					handler = createHandleObject(requestObject);
					if (handler == null)
					{/* if error */
						return createErrorObject(requestObject.requestId, String.Format(
							"ERROR: AjaxHandler class with name: {0} does not implements IAjaxHandleable.",
							requestObject.relatedObject
						));
					}
					handlers[requestObject.relatedObject] = handler;
				}				
				
				DataResponse response = new DataResponse();
				response.requestId = requestObject.requestId;
				response.data = handler.handleRequest(requestObject.data, this);
				return response;
			}
			catch (NullReferenceException)
			{
				return createErrorObject("unknown", "ERROR: Missing property: requestId, relatedObject or data.");
			}
			catch (FileNotFoundException e)
			{
				return createErrorObject(requestObject.requestId, "ERROR: IAjaxHandleable file with name " + requestObject.relatedObject + " was not found in SpaceTraffic.GameUi.Controllers.AjaxHandlers namespace");
			}
			catch (Exception e)
			{
				return createErrorObject(requestObject.requestId, "ERROR: Thrown exception during creating handling object:" + e.Message);
			}
		}


		/// <summary>
		/// Creates the object which can handle specific request.
		/// </summary>
		/// <param name="requestObject">The request object.</param>
		/// <returns></returns>
		/// <exception cref="System.IO.FileNotFoundException"></exception>
		private IAjaxHandleable createHandleObject(RequestObject requestObject)
		{
			Type handleType = Type.GetType("SpaceTraffic.GameUi.Controllers.AjaxHandlers." + requestObject.relatedObject);
			if (handleType == null)
			{/* if error */
				throw new FileNotFoundException();
			}
			IAjaxHandleable handler = Activator.CreateInstance(handleType) as IAjaxHandleable;/* creates object with given class name */
			return handler;
		}

		private ErrorResponse createErrorObject(string requestId, string errorMessage) {
			ErrorResponse error = new ErrorResponse();
			error.requestId = requestId;
			error.error = errorMessage;
			return error;
		}


	}
}