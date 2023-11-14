using System.Diagnostics;

namespace EncryptAddition.Analysis.Utils
{
    /// <summary>
    /// Contains overloaded Profile method for calculating execution time of functions.
    /// </summary>
    public static class Profiling
    {
        /// <summary>
        /// Accurately measures execution time for the provided function and stores any return 
        /// value from the function.
        /// If the number of iterations is specified and it is greater that 1,
        /// the function will be called multiple times and the average execution time will be returned.
        /// Due to the function being called multiple times, care should be taken to ensure that the function
        /// does not have any side effects.
        /// </summary>
        /// <returns>The average execution time of the function in milliseconds.</returns>
        /// <param name="action">Reference to the function that is to be benchmarked</param>
        /// <param name="returnValue">Reference to a variable which will be used to store the result of the callback</param>
        /// <param name="iterations">Number of times to execute the function</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the number of iterations is negative.</exception>
        public static double Profile<T>(Func<T> action, out T returnValue, int iterations = 1)
        {
            if (iterations < 1)
                throw new ArgumentOutOfRangeException(nameof(iterations), "The number of iterations must be greater than 0.");

            // Ensure that the process is at the highest priority to prevent instability
            // from other processes.
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            // Call the function to reduce any overhead that could be created by JIT
            returnValue = action();

            // Instantiate the stopwatch object
            var watch = new Stopwatch();

            // Clean up the garbage collector
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // Start the stopwatch
            if (iterations == 1)
            {
                watch.Start();
                action();
                watch.Stop();
                return watch.Elapsed.TotalMilliseconds;
            }
            else
            {
                watch.Start();
                for (int i = 0; i < iterations; i++)
                {
                    action();
                }
                watch.Stop();

                // Calculate mean running time
                double executionTime = watch.Elapsed.TotalMilliseconds / iterations;

                return executionTime;
            }
        }

        /// <summary>
        /// Accurately measures execution time for the provided function and discards return values.
        /// If the number of iterations is specified and it is greater that 1,
        /// the function will be called multiple times and the average execution time will be returned.
        /// Due to the function being called multiple times, care should be taken to ensure that the function
        /// does not have any side effects.
        /// </summary>
        /// <returns>The average execution time of the function in milliseconds.</returns>
        /// <param name="action">Reference to the function that is to be benchmarked</param>
        /// <param name="iterations">Number of times to execute the function</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the number of iterations is negative.</exception>
        public static double Profile(Action action, int iterations = 1)
        {
            if (iterations < 1)
                throw new ArgumentOutOfRangeException(nameof(iterations), "The number of iterations must be greater than 0.");

            // Ensure that the process is at the highest priority to prevent instability
            // from other processes.
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            // Call the function to reduce any overhead that could be created by JIT
            action();

            // Instantiate the stopwatch object
            var watch = new Stopwatch();

            // Clean up the garbage collector
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // Start the stopwatch
            if (iterations == 1)
            {
                watch.Start();
                action();
                watch.Stop();
                return watch.Elapsed.TotalMilliseconds;
            }
            else
            {
                watch.Start();
                for (int i = 0; i < iterations; i++)
                {
                    action();
                }
                watch.Stop();

                // Calculate mean running time
                double executionTime = watch.Elapsed.TotalMilliseconds / iterations;

                return executionTime;
            }
        }
    }
}
