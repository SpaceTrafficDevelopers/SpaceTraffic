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
using System.Runtime.Serialization;
using System.Text;

namespace SpaceTraffic.Game.Minigame
{
    /// <summary>
    /// Result of any minigame action or minigame manager action.
    /// </summary>
    [DataContract]
    public class Result
    {
        /// <summary>
        /// Result state.
        /// </summary>
        [DataMember]
        public ResultState State { get; set; }

        /// <summary>
        /// Result message.
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// Object with return value.
        /// </summary>
        [DataMember]
        public object ReturnValue { get; set; }

        /// <summary>
        /// Static factory method for create success result.
        /// </summary>
        /// <param name="message">result message</param>
        /// <param name="returnValue">return value</param>
        /// <returns>Success result instance.</returns>
        public static Result createSuccessResult(string message, object returnValue)
        {
            return new Result
            {
                State = ResultState.SUCCESS,
                Message = message,
                ReturnValue = returnValue
            };
        }

        /// <summary>
        /// Static factory method for create success result. (return value is null)
        /// </summary>
        /// <param name="message">result message</param>
        /// <returns>Success result instance.</returns>
        public static Result createSuccessResult(string message)
        {
            return new Result
            {
                State = ResultState.SUCCESS,
                Message = message,
                ReturnValue = null
            };
        }

        /// <summary>
        /// Static factory method for create failure result.
        /// </summary>
        /// <param name="message">result message</param>
        /// <param name="returnValue">return value</param>
        /// <returns>Failure result instance.</returns>
        public static Result createFailureResult(string message, object returnValue)
        {
            return new Result
            {
                State = ResultState.FAILURE,
                Message = message,
                ReturnValue = returnValue
            };
        }

        /// <summary>
        /// Static factory method for create failure result. (return value is null)
        /// </summary>
        /// <param name="message">result message</param>
        /// <returns>Failure result instance.</returns>
        public static Result createFailureResult(string message)
        {
            return new Result
            {
                State = ResultState.FAILURE,
                Message = message,
                ReturnValue = null
            };
        }
    }

    /// <summary>
    /// Result state enumeration.
    /// </summary>
    [DataContract]
    public enum ResultState
    {
        //mapping on int is because javascript cannot determine which member is which

        /// <summary>
        /// Success type
        /// </summary>
        [EnumMember]
        SUCCESS = 1,

        /// <summary>
        /// Failure type
        /// </summary>
        [EnumMember]
        FAILURE = 0
    }
}
