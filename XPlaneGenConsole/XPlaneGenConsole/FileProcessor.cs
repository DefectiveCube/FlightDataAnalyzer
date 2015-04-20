using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XPlaneGenConsole
{
    public sealed class FileProcessor<T> where T : Datapoint<T>
    {
        public readonly string prefix;
        Task task;
        ConcurrentBag<Task> tasks;
        ConcurrentQueue<string> fileQueue = new ConcurrentQueue<string>();
        CancellationTokenSource CancelToken;

        public TaskStatus status { get; private set; }

        public FileProcessor(string path = "", CancellationTokenSource source = null)
        {
            prefix = typeof(T).Name.Replace("Datapoint", string.Empty);
            CancelToken = source == null ? new CancellationTokenSource() : source;
        }

        public void Cancel(TimeSpan ts = default(TimeSpan))
        {
            CancelToken.Cancel();
        }

        public void Process(string path, CancellationTokenSource token = default(CancellationTokenSource))
        {
            var hash = BitConverter.ToString(Hash.ComputeSHA1Hash(path)).Replace("-", string.Empty);
            var dir = Path.Combine(new[] { Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FlightDataAnalyzer", "data", hash });

            Debug.WriteLine(string.Format("Directory: {0}", path));
            Debug.WriteLine(string.Format("Hash: {0}", hash));

            // TODO: check cancel token

            Process(path, dir, token);
        }

        public void Process(string path, string directory, CancellationTokenSource token = default(CancellationTokenSource))
        {
            DirectoryInfo dirInfo = new FileInfo(path).Directory;

            // Make sure directory exists
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            else
            {
                bool hasIndexFile = (from f in dirInfo.EnumerateFiles(prefix + ".index.bin", SearchOption.TopDirectoryOnly)
                                     select f).Count() > 0;

                bool hasOutputFile = (from f in dirInfo.EnumerateFiles(prefix + ".output.bin", SearchOption.TopDirectoryOnly)
                                      select f).Count() > 0;

                if (hasIndexFile)
                {
                    Debug.WriteLine("Found index file");
                }

                if (hasOutputFile)
                {
                    Debug.WriteLine("Found output file");
                }

                if (hasIndexFile || hasOutputFile)
                {
                    return;
                }
            }

            // Read values from CSV into a datapoint type that uses primitive types for better performance

            //IEnumerable<T> result = new Reader<T>(File.OpenRead(path)).ReadAll();

            //Console.WriteLine(result.Count());
        }

        public void Add(string path)
        {
            fileQueue.Enqueue(path);
        }

        public void WaitAll()
        {
            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException ex)
            {
                foreach (var v in ex.InnerExceptions)
                {
                    if (v is TaskCanceledException)
                    {
                        Console.WriteLine("TaskCanceledException: Task {0}", ((TaskCanceledException)v).Task.Id);
                    }
                    else
                    {
                        Console.WriteLine("Exception: {0}", v.GetType().Name);
                        Console.WriteLine(v.StackTrace);
                    }
                }
            }

            foreach (var task in tasks)
            {
                switch (task.Status)
                {
                    case TaskStatus.RanToCompletion:
                    case TaskStatus.Faulted:
                    case TaskStatus.Canceled:
                    case TaskStatus.Created:
                    case TaskStatus.Running:
                        Console.WriteLine("[{0}] Task Id:{1} Status:{2}", prefix, task.Id, task.Status.ToString());
                        break;
                    default:
                        Console.WriteLine("Task is in unknown state");
                        break;
                }
            }
        }

        public void Start()
        {
            tasks = new ConcurrentBag<Task>();
            var path = "";

            while (fileQueue.Count > 0)
            {
                if (!fileQueue.TryDequeue(out path))
                {
                    break;
                }

                Console.WriteLine("[{0}] {1}", prefix, path);

                var task = Task.Factory.StartNew(() =>
                {
                    Process(path);
                });

                tasks.Add(task);
            }
        }
    }
}
