﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCLTesting
{
    public static class ExceptionAssert
    {
        public static T Throws<T>(Action action, string message = null) where T : Exception
        {
            string failMessage;
            try
            {
                action();
            }
            catch (T ex)
            {
                return ex;
            }
            catch (Exception ex)
            {
                failMessage = string.Format("Expected exception of type {0}, but caught exception of type {1}", typeof(T).FullName, ex.GetType().FullName);
                HandleFail("Throws", failMessage, message, ex);
                return null;
            }

            failMessage = string.Format("Expected exception of type {0}, but no exception was caught", typeof(T).FullName);
            HandleFail("Throws", failMessage, message);

            return null;
        }

        public static async Task<T> ThrowsAsync<T>(Func<Task> action, string message = null) where T : Exception
        {
            string failMessage;
            try
            {
                await action();
            }
            catch (T ex)
            {
                return ex;
            }
            catch (Exception ex)
            {
                failMessage = string.Format("Expected exception of type {0}, but caught exception of type {1}", typeof(T).FullName, ex.GetType().FullName);
                HandleFail("ThrowsAsync", failMessage, message, ex);
                return null;
            }

            failMessage = string.Format("Expected exception of type {0}, but no exception was caught", typeof(T).FullName);
            HandleFail("ThrowsAsync", failMessage, message);

            return null;
        }

        static void HandleFail(string assertName, string failMessage, string message, Exception innerException = null)
        {
            string finalMessage = "ExceptionAssert." + assertName + " failed.  " + failMessage;
            if (!string.IsNullOrEmpty(message))
            {
                message += "  " + message;
            }

            if (innerException == null)
            {
                throw new AssertFailedException(finalMessage);
            }
            else
            {
                throw new AssertFailedException(finalMessage, innerException);
            }
        }

        public class AssertFailedException : Exception
        {
            public AssertFailedException(string message)
                : base(message)
            {

            }

            public AssertFailedException(string message, Exception innerException)
                : base(message, innerException)
            {

            }
        }
    }

}
