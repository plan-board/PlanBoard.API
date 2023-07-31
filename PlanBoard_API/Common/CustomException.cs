using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlanBoard_API.Common
{
    public class CustomException : Exception
    {
        public CustomException()
        {
        }

        public CustomException(string message)
            : base(message)
        {
        }

        public CustomException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}