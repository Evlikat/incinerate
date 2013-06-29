using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NeuroIncinerate.Neuro
{
    [TestFixture]
    class LimitedProcessHistoryTest
    {
        class ProcessAction : IProcessAction
        {
            public ProcessAction(string eventName)
            {
                this.EventName = eventName;
            }

            public string EventName
            {
                get;
                private set;
            }
        }

        [Test]
        [TestCase(3)]
        [TestCase(20000)]
        public void TestCycle(int limit)
        {
            LimitedProcessHistory instance = new LimitedProcessHistory(new WinPID(1234, "testProcess"), limit);
            for (int i = 0; i < limit; i++)
            {
                instance.Add(new ProcessAction("Event" + i));
            }
            instance.Add(new ProcessAction("EventN"));
            for (int i = 0; i < limit - 1; i++)
            {
                Assert.AreEqual("Event" + (i + 1), instance[i].EventName);
            }
            Assert.AreEqual("EventN", instance[limit - 1].EventName);
        }
    }
}
