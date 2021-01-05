using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracingApplication
{
    [EventSource(Name = "EventSourceSample", Guid = "45FFF0E2-7198-4E4F- 9FC3-DF6934680096")]
    [CustomAttribute("MyMessage", false)]
    class SampleEventSource : EventSource
    {

        public class Keywords
        {
            public const EventKeywords Database = (EventKeywords)1;
            public const EventKeywords Calculation = (EventKeywords)2;
        }


        public class Tasks
        {
            public const EventTask CalculationTask = (EventTask)1;
        }


        public SampleEventSource(string name): base(name)
        {

        }


        [Event(1, Opcode = EventOpcode.Start, Level = EventLevel.Verbose)]
        public void Startup() => WriteEvent(1);

        [Event(2, Opcode = EventOpcode.Info, Keywords = Keywords.Calculation, Level = EventLevel.Verbose, Message = "{0}")]
        public void CalculationService(string method) => WriteEvent(2, method);

        [Event(3, Opcode = EventOpcode.Info, Task = Tasks.CalculationTask, Level = EventLevel.Verbose, Keywords = Keywords.Database)]
        public void DatabaseService(string message, bool success) => WriteEvent(3, message, success);

    }

    class CustomAttribute: Attribute
    {
        public CustomAttribute(string message, bool error)
        {
            Message = message;
            IsError = error;
        }

        public string Message { get; }
        public bool IsError { get; }
    }
}
