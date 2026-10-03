// Copyright (c) 2026 Roger Brown.
// Licensed under the MIT License.

#if NETCOREAPP
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif
using System;
using System.Collections;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Reflection;

namespace RhubarbGeekNz.AppleScript
{
    [TestClass]
    public class TestInvokeAppleScript
    {
        readonly InitialSessionState initialSessionState = InitialSessionState.CreateDefault();
        public TestInvokeAppleScript()
        {
            foreach (Type t in new Type[] {
                typeof(InvokeAppleScript)
            })
            {
                CmdletAttribute ca = t.GetCustomAttribute<CmdletAttribute>();

                if (ca == null) throw new NullReferenceException();

                initialSessionState.Commands.Add(new SessionStateCmdletEntry($"{ca.VerbName}-{ca.NounName}", t, ca.HelpUri));
            }

            initialSessionState.Variables.Add(new SessionStateVariableEntry("ErrorActionPreference", ActionPreference.Stop, "Stop action"));
        }

        [TestMethod]
        public async Task TestReturnNumberAsScriptBlock()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand("Invoke-AppleScript").AddParameter("ScriptBlock","return 42");

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                int result = (int)outputPipeline[0].BaseObject;

                Assert.AreEqual(42,result);
            }
        }

        [TestMethod]
        public async Task TestReturnNumberAsPipeline()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand("Invoke-AppleScript");

                var outputPipeline = powerShell.Invoke(new String[]{"return 78"});

                Assert.AreEqual(1, outputPipeline.Count);

                int result = (int)outputPipeline[0].BaseObject;

                Assert.AreEqual(78,result);
            }
        }

        [TestMethod]
        public async Task TestMultiplyAsScriptBlock()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand("Invoke-AppleScript")
                    .AddParameter("ScriptBlock",
                    """
                    on multiplyNumbers(x, y)
                        return x * y
                    end multiplyNumbers
                    """)
                    .AddParameter("ArgumentList",new String[]{"6","9"})
                    .AddParameter("SubroutineName","multiplyNumbers");

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                int result = (int)outputPipeline[0].BaseObject;

                Assert.AreEqual(54,result);
            }
        }

        [TestMethod]
        public async Task TestMultiplyAsWithIntegers()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand("Invoke-AppleScript")
                    .AddParameter("ScriptBlock",
                    """
                    on multiplyNumbers(x, y)
                        return x * y
                    end multiplyNumbers
                    """)
                    .AddParameter("ArgumentList",new int[]{6,9})
                    .AddParameter("SubroutineName","multiplyNumbers");

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                int result = (int)outputPipeline[0].BaseObject;

                Assert.AreEqual(54,result);
            }
        }

        [TestMethod]
        public async Task TestEchoString()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand("Invoke-AppleScript")
                    .AddParameter("ScriptBlock",
                    """
                    on echo(x)
                        return x
                    end echo
                    """)
                    .AddParameter("ArgumentList",new String[]{"foo"})
                    .AddParameter("SubroutineName","echo");

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                String result = (String)outputPipeline[0].BaseObject;

                Assert.AreEqual("foo",result);
            }
        }

        [TestMethod]
        public async Task TestEchoInt32()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand("Invoke-AppleScript")
                    .AddParameter("ScriptBlock",
                    """
                    on echo(x)
                        return x
                    end echo
                    """)
                    .AddParameter("ArgumentList",new int[]{97})
                    .AddParameter("SubroutineName","echo");

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                int result = (int)outputPipeline[0].BaseObject;

                Assert.AreEqual(97,result);
            }
        }

        [TestMethod]
        public async Task TestEchoBool()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand("Invoke-AppleScript")
                    .AddParameter("ScriptBlock",
                    """
                    on echo(x)
                        return x
                    end echo
                    """)
                    .AddParameter("ArgumentList",new bool[]{true})
                    .AddParameter("SubroutineName","echo");

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                bool result = (bool)outputPipeline[0].BaseObject;

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public async Task TestEchoDouble()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                double pi=3.14159;
                powerShell.AddCommand("Invoke-AppleScript")
                    .AddParameter("ScriptBlock",
                    """
                    on echo(x)
                        return x
                    end echo
                    """)
                    .AddParameter("ArgumentList",new double[]{pi})
                    .AddParameter("SubroutineName","echo");

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                double result = (double)outputPipeline[0].BaseObject;

                Assert.AreEqual(pi,result);
            }
        }

        [TestMethod]
        public async Task TestEchoDateTime()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                DateTime now=DateTime.Now.ToUniversalTime();

                powerShell.AddCommand("Invoke-AppleScript")
                    .AddParameter("ScriptBlock",
                    """
                    on echo(x)
                        return x
                    end echo
                    """)
                    .AddParameter("ArgumentList",new DateTime[]{now})
                    .AddParameter("SubroutineName","echo");

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                DateTime result = (DateTime)outputPipeline[0].BaseObject;

                var dateTimeOffset=result.Subtract(now);

                Assert.IsTrue(dateTimeOffset.TotalSeconds < 2.0 && dateTimeOffset.TotalSeconds > -2.0);
            }
        }

        [TestMethod]
        public async Task TestEchoFileUrl()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                Uri uri = new Uri("http://www.contoso.com/");

                powerShell.AddCommand("Invoke-AppleScript")
                    .AddParameter("ScriptBlock",
                    """
                    on echo(x)
                        return x
                    end echo
                    """)
                    .AddParameter("ArgumentList",new Uri[]{uri})
                    .AddParameter("SubroutineName","echo");

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                Uri result = (Uri)outputPipeline[0].BaseObject;

                Assert.AreEqual(uri,result);
            }
        }

        [TestMethod]
        public async Task TestEchoHashTable()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                Hashtable hashtable=new Hashtable();
                hashtable.Add("foo","bar");

                powerShell.AddCommand("Invoke-AppleScript")
                    .AddParameter("ScriptBlock",
                    """
                    on echo(x)
                        return x
                    end echo
                    """)
                    .AddParameter("ArgumentList",new object[]{hashtable})
                    .AddParameter("SubroutineName","echo");

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                Hashtable result = (Hashtable)outputPipeline[0].BaseObject;

                Assert.AreEqual(1,result.Count);
                Assert.AreEqual("bar",result["foo"]);
            }
        }

        [TestMethod]
        public async Task TestEchoPSCustomObject()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                PSObject hashtable=new PSObject();
                PSNoteProperty noteProperty=new PSNoteProperty("foo","bar");
                hashtable.Properties.Add(noteProperty);

                powerShell.AddCommand("Invoke-AppleScript")
                    .AddParameter("ScriptBlock",
                    """
                    on echo(x)
                        return x
                    end echo
                    """)
                    .AddParameter("ArgumentList",new object[]{hashtable})
                    .AddParameter("SubroutineName","echo");

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                Hashtable result = (Hashtable)outputPipeline[0].BaseObject;

                Assert.AreEqual(1,result.Count);
                Assert.AreEqual("bar",result["foo"]);
            }
        }

        [TestMethod]
        public async Task TestEchoErrorWithArguments()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                int ?errorNumber=null;

                powerShell.AddCommand("Invoke-AppleScript")
                    .AddParameter("ScriptBlock","""
                    on run argv
                        return argv
                    end run
                    """)
                    .AddParameter("ArgumentList",new object[]{"foo"})
                    .AddParameter("SubroutineName","run");

                try
                {
                    powerShell.Invoke();      
                }
                catch (ActionPreferenceStopException ex)
                {
                    AppleScriptException appleScriptException=(AppleScriptException)ex.ErrorRecord.Exception;

                    errorNumber=appleScriptException.HResult;
                }

                Assert.IsTrue(errorNumber.HasValue);
                Assert.AreEqual(-1708,errorNumber);
            }
        }

        [TestMethod]
        public async Task TestEchoErrorWithScript()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                int ? errorNumber=null;

                powerShell.AddCommand("Invoke-AppleScript")
                    .AddParameter("ScriptBlock","foo");

                try
                {
                    powerShell.Invoke();
                    
                }
                catch (ActionPreferenceStopException ex)
                {
                    AppleScriptException appleScriptException=(AppleScriptException)ex.ErrorRecord.Exception;

                    errorNumber=appleScriptException.HResult;
                }

                Assert.IsTrue(errorNumber.HasValue);
                Assert.AreEqual(-2753,errorNumber);
            }
        }
    }
}
