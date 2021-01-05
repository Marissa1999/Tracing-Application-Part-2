using System;
using System.Diagnostics.Tracing;
using TracingApplication;

namespace Tracing
{

    class Program
    {
        static void Main(string[] args)
        {
            //-1- Instantiate the EventSourceSample class.
            SampleEventSource sampleEventSource = new SampleEventSource("VanierCollege");

            /*
            //-1- Instantiate the EventSource class.
            EventSource sampleEventSource = new EventSource("VanierEventSource");
            */

            //-1- Instantiate the listener.
            MyEventListener listener = new MyEventListener();

            //-1- Add the event source to the listener.
            listener.EnableEvents(sampleEventSource, EventLevel.Verbose);

            //-2- Generate a GUID in order to use it in the logging session.
            Console.WriteLine($"Log Guid: {sampleEventSource.Guid}");
            Console.WriteLine($"Name: {sampleEventSource.Name}");

            /*
            //-3- Create a log event to mention that the application started.
            sampleEventSource.Write("Startup", new EventSourceOptions { Level = EventLevel.Verbose }, new { Info = "Application Started" });
            */

            // -3- Call the sample event source to start the application.
            sampleEventSource.Startup();

            /*
            //-4- Mark the log with a call to calculate.
            sampleEventSource.Write("Action", new { Info = $"Calling Calculate" });
            */

            //-4- Mark the log with a service call to the Calculate method.
            sampleEventSource.CalculationService("Calculate");
            int result = Calculate(10, 2);

            /*
            //-5- Mark the end of the calculation action.
            sampleEventSource.Write("Action Complete", new { Info = $"Completed Call to Calculate, Result: {result}" });
            */

            //-5- Mark the end of the calculation action by calling the database service.
            sampleEventSource.DatabaseService("Action Complete", true);

            //-6- Mark the end of the application.
            sampleEventSource.Write("Complete", new { Info = "Application Done" });
            Console.WriteLine("Completed, press any key to exit.");
            Console.ReadLine();

            //-7- Need to cleanup.
            sampleEventSource.Dispose();
        }

        static int Calculate(int a, int b)
        {
            return a / b;
        }
    }

    public class MyEventListener : EventListener
    {       
        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            Console.WriteLine($"Created: {eventSource.Name} {eventSource.Guid}");
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            Console.WriteLine($"Event ID: {eventData.EventId} Source: {eventData.EventSource.Name}");

            foreach (var payload in eventData.Payload)
            {
                Console.WriteLine($"\t {payload}");
            }
        }
    }


}
