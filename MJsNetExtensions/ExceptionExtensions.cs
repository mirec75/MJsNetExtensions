namespace MJsNetExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Summary description for ExceptionExtensions
    /// </summary>
    public static class ExceptionExtensions
    {
        #region API - Public Methods

        /// <summary>
        /// Returns a flattened list of all exceptions from the whole exception hierarchy 
        /// starting and including the top-level exception <paramref name="ex"/>, down through all the inner exceptions. 
        /// </summary>
        /// <param name="ex">Optional. Can be null. The exception to flatten.</param>
        public static IEnumerable<Exception> Flatten(this Exception ex)
        {
            // return an empty sequence if the provided exception is null
            if (ex == null) { yield break; }

            // return self
            yield return ex;

            // Handle AggregateException:
            if (ex is AggregateException aex)
            {
                foreach (Exception innerException in (IEnumerable<Exception>)aex.Flatten()?.InnerExceptions)
                {
                    // recurse an optionally null InnerException:
                    foreach (Exception innerExOfInnerEx in innerException.Flatten())
                    {
                        yield return innerExOfInnerEx;
                    }
                }
            }
            else
            {
                // recurse an optionally null InnerException:
                foreach (Exception innerExOfInnerEx in ex.InnerException.Flatten())
                {
                    yield return innerExOfInnerEx;
                }
            }
        }

        /// <summary>
        /// Returns a flattened list of all exception <see cref="Exception.Message"/>-s from the whole exception hierarchy 
        /// starting from the top-level exception down through all the inner exceptions. 
        /// </summary>
        /// <param name="ex">Optional. Can be null. The exception to get messages from.</param>
        public static IEnumerable<string> GetMessages(this Exception ex)
        {
            // return an empty sequence if the provided exception is null
            if (ex == null) { yield break; }

            // iterate flattened Exceptions.
            //NOTE: the flattened exceptions contains self (== ex)!
            foreach (Exception innerException in ex.Flatten())
            {
                if (innerException != null)
                {
                    yield return innerException.Message;
                }
            }
        }

        /// <summary>
        /// Returns a single concantenated string of all <see cref="Exception.Message"/>-s of all exceptions inside the hierarchy 
        /// starting from the top-level exception down through all the inner exceptions. 
        /// The <see cref="Exception.Message"/>-s will be concatenated (joined) using a default separator: " --> ".
        /// </summary>
        /// <param name="ex">Optional. Can be null. The exception to get messages from.</param>
        public static string JoinMessages(this Exception ex)
        {
            return ex.JoinMessages(" --> ");
        }

        /// <summary>
        /// Returns a single concantenated string of all <see cref="Exception.Message"/>-s of all exceptions inside the hierarchy 
        /// starting from the top-level exception down through all the inner exceptions. 
        /// The <see cref="Exception.Message"/>-s will be concatenated (joined) using <paramref name="separator"/>.
        /// </summary>
        /// <param name="ex">Optional. Can be null. The exception to get messages from.</param>
        /// <param name="separator">The string to use as a separator. separator is included in the returned string only if values has more than one element.</param>
        public static string JoinMessages(this Exception ex, string separator)
        {
            // return an empty sequence if the provided exception is null
            if (ex == null) { return null; }

            return string.Join(separator, ex.GetMessages());
        }

        /// <summary>
        /// Returns a flattened list of all exception "<see cref="Exception.GetType()"/>.Name: <see cref="Exception.Message"/>" from the whole exception hierarchy 
        /// starting from the top-level exception down through all the inner exceptions. 
        /// </summary>
        /// <param name="ex">Optional. Can be null. The exception to get messages from.</param>
        public static IEnumerable<string> GetMessagesWithTypes(this Exception ex)
        {
            // return an empty sequence if the provided exception is null
            if (ex == null) { yield break; }

            // iterate flattened Exceptions.
            //NOTE: the flattened exceptions contains self (== ex)!
            foreach (Exception innerException in ex.Flatten())
            {
                if (innerException != null)
                {
                    yield return $"{innerException.GetType().Name}: {innerException.Message}";
                }
            }
        }

        /// <summary>
        /// Returns a single concantenated string of all "<see cref="Exception.GetType()"/>.Name: <see cref="Exception.Message"/>"-s of all exceptions inside the hierarchy 
        /// starting from the top-level exception down through all the inner exceptions. 
        /// The <see cref="Exception.Message"/>-s will be concatenated (joined) using a default separator: " --> ".
        /// </summary>
        /// <param name="ex">Optional. Can be null. The exception to get messages from.</param>
        public static string JoinMessagesWithTypes(this Exception ex)
        {
            return ex.JoinMessagesWithTypes(" --> ");
        }

        /// <summary>
        /// Returns a single concantenated string of all "<see cref="Exception.GetType()"/>.Name: <see cref="Exception.Message"/>"-s of all exceptions inside the hierarchy 
        /// starting from the top-level exception down through all the inner exceptions. 
        /// The <see cref="Exception.Message"/>-s will be concatenated (joined) using <paramref name="separator"/>.
        /// </summary>
        /// <param name="ex">Optional. Can be null. The exception to get messages from.</param>
        /// <param name="separator">The string to use as a separator. separator is included in the returned string only if values has more than one element.</param>
        public static string JoinMessagesWithTypes(this Exception ex, string separator)
        {
            // return an empty sequence if the provided exception is null
            if (ex == null) { return null; }

            return string.Join(separator, ex.GetMessagesWithTypes());
        }
        #endregion API - Public Methods
    }
}
