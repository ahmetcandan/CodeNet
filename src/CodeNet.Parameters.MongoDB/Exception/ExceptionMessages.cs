﻿using CodeNet.Core.Models;

namespace CodeNet.Parameters.MongoDB.Exception
{
    internal static class ExceptionMessages
    {
        public static ExceptionMessage NotFoundGroup { get { return new ExceptionMessage("PR001", "Not found parameter group (Id: {0})."); } }
        public static ExceptionMessage NotFoundDefaultValue { get { return new ExceptionMessage("PR002", "Not found default value (Id: {0})."); } }
        public static ExceptionMessage DefaultParameterMoreThanOne { get { return new ExceptionMessage("PR003", "The default value cannot be more than one."); } }

        public static ExceptionMessage UseParams(this ExceptionMessage exceptionMessage, params string[] args) => new(exceptionMessage.Code, string.Format(exceptionMessage.Message, args));
    }
}
