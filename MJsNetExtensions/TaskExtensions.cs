#pragma warning disable S125
namespace MJsNetExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;


    /// <summary>
    /// Summary description for TaskExtensions
    /// </summary>
    public static class TaskExtensions
    {
        #region SafeWaitAllAndLogPotentialExceptions

        /// <summary>
        /// Safe Wait All <see cref="Task"/>s and Log Potential Exceptions.
        /// </summary>
        /// <param name="tasksToWait">Optional. Can be null or empty. All <see cref="Task"/>s to wait for their finish.</param>
        /// <param name="logError">Mandatory error logging <see cref="Action"/></param>
        public static void SafeWaitAllAndLogPotentialExceptions(this IEnumerable<Task> tasksToWait, Action<string, Exception> logError)
        {
            SafeWaitAllAndLogPotentialExceptions(tasksToWait, logError, null);
        }

        /// <summary>
        /// Safe Wait All <see cref="Task"/>s and Log Potential Exceptions.
        /// </summary>
        /// <param name="tasksToWait">Optional. Can be null or empty. All <see cref="Task"/>s to wait for their finish.</param>
        /// <param name="logError">Mandatory error logging <see cref="Action"/></param>
        /// <param name="onError">Optional. Can be null. <see cref="Action"/> which will be executed only if there was an Exception on any of the <paramref name="tasksToWait"/>.
        /// It will be executed after all Exceptions of all Tasks are logged.</param>
        public static void SafeWaitAllAndLogPotentialExceptions(this IEnumerable<Task> tasksToWait, Action<string, Exception> logError, Action onError)
        {
            Throw.IfNull(logError, nameof(logError));

            int cntTasks = 0;
            Exception catchedEx = null;

            try
            {
                Task[] tasksToWaitPurified = (tasksToWait ?? [])
                    .Where(it => it != null)
                    .ToArray();

                cntTasks = tasksToWaitPurified.Length;

                if (cntTasks > 0)
                {
                    Task.WaitAll(tasksToWaitPurified);
                }

                // if we got so far without an exception, then quit here!
                return;
            }
            catch (AggregateException aex)
            {
                catchedEx = aex;
                logError($"ERROR waiting for end of all {cntTasks} started tasks. All partial exceptions follow.", aex);

                // !! DO NOT THROW HERE !!
            }
            catch (Exception ex)
            {
                // last resort catch handler, if something goes unexpectedly wrong.
                // This catch handler is introduced to prevent an exception to tear the complete application (service) down, if the exception is not handled (on time).
                catchedEx = ex;
                logError($"Unhandled ERROR Waiting for End of All {cntTasks} started tasks.", ex);

                // !! DO NOT THROW HERE !!
            }

            try
            {
                if (catchedEx is AggregateException catchedAex)
                {
                    ReadOnlyCollection<Exception> exceptionCollection = catchedAex.Flatten().InnerExceptions;
                    int ii = 0;
                    foreach (Exception ex in exceptionCollection)
                    {
                        logError($"Partial Exception {++ii} of {exceptionCollection.Count} while waiting for end of all {cntTasks} started tasks.", ex);
                    }
                }

                onError?.Invoke();
            }
            catch (Exception ex)
            {
                // last resort catch handler, if something goes unexpectedly wrong.
                // This catch handler is introduced to prevent an exception to tear the complete application (service) down, if the exception is not handled (on time).
                logError($"Unhandled ERROR while Handling Catched Exception after Waiting for End of All {cntTasks} started tasks.", ex);

                // !! DO NOT THROW HERE !!
            }
        }
        #endregion SafeWaitAllAndLogPotentialExceptions

        #region IgnoreExceptionsOnNoWaitedTask, WithThrowOnCancellation and HandleAntecedentState
        //private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// A helper for catching and "handling" (ignoring) not handled exception in forked threads, allowing "fire and forget" strrategy of calling <see cref="TaskFactory.StartNew(Action)"/> and simmilar.
        /// </summary>
        /// <param name="task"></param>
        public static void IgnoreExceptionsOnNoWaitedTask(this Task task)
        {
            // Joe Albahari - PART 5: PARALLEL PROGRAMMING
            // http://www.albahari.com/threading/part5.aspx
            task?.ContinueWith(
                antecedent =>
                {
                    var ignore = antecedent.Exception;
                    Debug.WriteLine(ignore);
                    //Logger.Warn("IgnoreExceptionsOnNoWaitedTask", antecedent.Exception);

                },
                CancellationToken.None,
                TaskContinuationOptions.OnlyOnFaulted,
                TaskScheduler.Default
              );
        }

        /// <summary>
        /// A helper for catching and "handling" (ignoring) not handled exception in forked threads, allowing "fire and forget" strrategy of calling <see cref="TaskFactory.StartNew(Action)"/> and simmilar.
        /// </summary>
        /// <param name="task"></param>
        public static void IgnoreExceptionsOnNoWaitedTask<T>(this Task<T> task)
        {
            // Joe Albahari - PART 5: PARALLEL PROGRAMMING
            // http://www.albahari.com/threading/part5.aspx
            task?.ContinueWith(
                antecedent =>
                {
                    var ignore = antecedent.Exception;
                    Debug.WriteLine(ignore);
                    //Logger.Warn("IgnoreExceptionsOnNoWaitedTask", antecedent.Exception);
                    return default(T);
                },
                CancellationToken.None,
                TaskContinuationOptions.OnlyOnFaulted,
                TaskScheduler.Default

              );
        }

        /// <summary>
        /// A helper for explicitely throwing an exception upon calncelation in a chainf of forking a thread and using continuations.
        /// Not enough tested yet.
        /// </summary>
        /// <param name="task"></param>
        /// <param name="cancellationToken"></param>
        public static Task WithThrowOnCancellation(this Task task, CancellationToken cancellationToken)
        {
            return task == null || task.IsCompleted // fast-path optimization
                ? task
                : task.ContinueWith(
                        antecedent => antecedent.HandleAntecedentState(false),
                        cancellationToken,
                        TaskContinuationOptions.ExecuteSynchronously,
                        TaskScheduler.Default
                    );
        }

        /// <summary>
        /// A helper for explicitely throwing an exception upon calncelation in a chainf of forking a thread and using continuations.
        /// Not enough tested yet.
        /// </summary>
        /// <param name="task"></param>
        /// <param name="cancellationToken"></param>
        public static Task<T> WithThrowOnCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            return task == null || task.IsCompleted // fast-path optimization
                ? task
                : task.ContinueWith(
                        antecedent => antecedent.HandleAntecedentState<T>(false),
                        cancellationToken,
                        TaskContinuationOptions.ExecuteSynchronously,
                        TaskScheduler.Default
                    );
        }

        #region OBSOLETE DISABLED WithSilentFinishOnCancellation() methods left as a reminder not to use something like this!
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        //[Obsolete("Usage not recommended. It is dangerous due to the questionable user code handling of the cancelation situation after await.")]
        //private static Task WithSilentFinishOnCancellation(this Task task, CancellationToken cancellationToken)
        //{
        //    return task == null || task.IsCompleted // fast-path optimization
        //        ? task
        //        : task.ContinueWith(
        //                antecedent => antecedent.HandleAntecedentState(false),
        //                cancellationToken,
        //                TaskContinuationOptions.ExecuteSynchronously,
        //                TaskScheduler.Default
        //            )
        //            .ContinueWith(
        //                antecedent => antecedent.HandleAntecedentState(true)
        //                //, TaskContinuationOptions.OnlyOnCanceled  <--  if switched on, it breaks the execution of the caller's await in a good case with a TaskCanceledException after .Net check:
        //                //                                               TaskAwaiter.GetResult() -> TaskAwaiter.HandleNonSuccessAndDebuggerNotification() -> TaskAwaiter.ThrowForNonSuccess() 
        //            );
        //}

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        //[Obsolete("Usage not recommended. It is dangerous due to the questionable user code handling of the cancelation situation after await.")]
        //private static Task<T> WithSilentFinishOnCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
        //{
        //    return task == null || task.IsCompleted // fast-path optimization
        //        ? task
        //        : task.ContinueWith(
        //                antecedent => antecedent.HandleAntecedentState<T>(false),
        //                cancellationToken,
        //                TaskContinuationOptions.ExecuteSynchronously,
        //                TaskScheduler.Default
        //            )
        //            .ContinueWith(
        //                antecedent => antecedent.HandleAntecedentState<T>(true)
        //                //, TaskContinuationOptions.OnlyOnCanceled  <--  if switched on, it breaks the execution of the caller's await in a good case with a TaskCanceledException after .Net check:
        //                //                                               TaskAwaiter.GetResult() -> TaskAwaiter.HandleNonSuccessAndDebuggerNotification() -> TaskAwaiter.ThrowForNonSuccess() 
        //            );
        //}

        #endregion OBSOLETE DISABLED WithSilentFinishOnCancellation() methods left as a reminder not to use something like this!

        /// <summary>
        /// Helper method used to implement <see cref="TaskExtensions.WithThrowOnCancellation(Task, CancellationToken)"/>.
        /// Not enough tested yet.
        /// </summary>
        /// <param name="antecedent"></param>
        /// <param name="silentFinishOnCancellationException"></param>
        public static void HandleAntecedentState(this Task antecedent, bool silentFinishOnCancellationException)
        {
            if (antecedent == null)
            {
                // ignore
            }
            else if (antecedent.IsCanceled)
            {
                //Logger.Warn("Canceling (a)wait as requested...");
            }
            else if (antecedent.IsFaulted) // || antecedent.Exception != null)
            {
                var aeFlat = antecedent.Exception?.Flatten(); //NOTE: that this access to antecedent.Exception causes to mark the exception as handled and thus the whole App does not get torn down.
                if (silentFinishOnCancellationException &&
                    aeFlat.InnerException != null &&
                    aeFlat.InnerException is TaskCanceledException
                    )
                {
                    //Logger.Warn("Canceling (a)wait as requested ...");
                }
                else
                {
                    //Logger.Error("Exceptopn propagation in WithLogAndFinishOnCancellation()", antecedent.Exception);
                    // Joe Albahari - PART 5: PARALLEL PROGRAMMING
                    // http://www.albahari.com/threading/part5.aspx
                    // A safe pattern is to rethrow antecedent exceptions. As long as the continuation is Waited upon, the exception will be propagated and rethrown to the Waiter:
                    throw antecedent.Exception;    // Continue processing...
                }
            }
            else if (antecedent.IsCompleted)
            {
                //Logger.Debug("Normal processing...");
            }
        }

        /// <summary>
        /// Helper method used to implement <see cref="TaskExtensions.WithThrowOnCancellation{T}(Task{T}, CancellationToken)"/>.
        /// Not enough tested yet.
        /// </summary>
        /// <param name="antecedent"></param>
        /// <param name="silentFinishOnCancellationException"></param>
        public static T HandleAntecedentState<T>(this Task<T> antecedent, bool silentFinishOnCancellationException)
        {
            if (antecedent == null)
            {
                // ignore
            }
            else if (antecedent.IsCanceled)
            {
                //Logger.Warn("Canceling (a)wait as requested...");
            }
            else if (antecedent.IsFaulted) // || antecedent.Exception != null)
            {
                var aeFlat = antecedent.Exception?.Flatten(); //NOTE: that this access to antecedent.Exception causes to mark the exception as handled and thus the whole App does not get torn down.
                if (silentFinishOnCancellationException && aeFlat?.InnerException is TaskCanceledException)
                {
                    //Logger.Warn("Canceling (a)wait as requested ...");
                }
                else
                {
                    //Logger.Error("Exceptopn propagation in WithLogAndFinishOnCancellation()", antecedent.Exception);
                    // Joe Albahari - PART 5: PARALLEL PROGRAMMING
                    // http://www.albahari.com/threading/part5.aspx
                    // A safe pattern is to rethrow antecedent exceptions. As long as the continuation is Waited upon, the exception will be propagated and rethrown to the Waiter:
                    throw antecedent.Exception;    // Continue processing...
                }
            }
            else if (antecedent.IsCompleted)
            {
                //Logger.Debug("Normal processing...");
                return antecedent.GetAwaiter().GetResult();
            }

            return default(T);
        }
        #endregion IgnoreExceptionsOnNoWaitedTask, WithThrowOnCancellation and HandleAntecedentState

    }
}
