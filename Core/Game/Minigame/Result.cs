using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SpaceTraffic.Game.Minigame
{
    [DataContract]
    public class Result
    {
        [DataMember]
        public ResultState State { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public object ReturnValue { get; set; }

        public static Result createSuccessResult(string message, object returnValue)
        {
            return new Result
            {
                State = ResultState.SUCCESS,
                Message = message,
                ReturnValue = returnValue
            };
        }

        public static Result createSuccessResult(string message)
        {
            return new Result
            {
                State = ResultState.SUCCESS,
                Message = message,
                ReturnValue = null
            };
        }

        public static Result createFailureResult(string message, object returnValue)
        {
            return new Result
            {
                State = ResultState.FAILURE,
                Message = message,
                ReturnValue = returnValue
            };
        }

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

    [DataContract]
    public enum ResultState
    {
        [EnumMember]
        SUCCESS,

        [EnumMember]
        FAILURE
    }
}
