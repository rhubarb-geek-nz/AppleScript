// Copyright (c) 2026 Roger Brown.
// Licensed under the MIT License.

using System;
using System.Collections;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml;

namespace RhubarbGeekNz.AppleScript
{
    public class Customer
    {
        public String Name { get; set; }
        public int Age { get; set; }
        public Customer(String n, int a)
        {
            Name = n;
            Age = a;
        }
    }

    [TestClass]
    public class TestInvokeAppleScript
    {
        const String testAppleScriptPath = "../../../../test.applescript";
        const String echoScript = """
                    on echo(x)
                        return x
                    end echo
                    """;
        const String multiplyNumbersScript =
                    """
                    on multiplyNumbers(x, y)
                        return x * y
                    end multiplyNumbers
                    """;

        const String echoSubroutineName = "echo";
        const String multiplySubroutineName = "multiplyNumbers";
        const String ScriptBlock = "ScriptBlock";
        const String ArgumentList = "ArgumentList";
        const String SubroutineName = "SubroutineName";
        const String InvokeAppleScript = "Invoke-AppleScript";

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
        public void TestFileInfo()
        {
            FileInfo f1 = new FileInfo(testAppleScriptPath);

            Assert.IsTrue(f1.Exists);

            String[] list = File.ReadAllLines(testAppleScriptPath);
            Assert.AreEqual("-- Copyright (c) 2026 Roger Brown.", list[0]);
        }

        [TestMethod]
        public void TestReturnNumberAsScriptBlock()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand(InvokeAppleScript).AddParameter(ScriptBlock, "return 42");

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                int result = (int)outputPipeline[0].BaseObject;

                Assert.AreEqual(42, result);
            }
        }

        [TestMethod]
        public void TestReturnNumberAsPipeline()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand(InvokeAppleScript);

                var outputPipeline = powerShell.Invoke(new String[] { "return 78" });

                Assert.AreEqual(1, outputPipeline.Count);

                int result = (int)outputPipeline[0].BaseObject;

                Assert.AreEqual(78, result);
            }
        }

        [TestMethod]
        public void TestMultiplyAsScriptBlock()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter(ScriptBlock, multiplyNumbersScript)
                    .AddParameter(ArgumentList, new String[] { "6", "9" })
                    .AddParameter(SubroutineName, multiplySubroutineName);

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                int result = (int)outputPipeline[0].BaseObject;

                Assert.AreEqual(54, result);
            }
        }

        [TestMethod]
        public void TestMultiplyAsWithIntegers()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter(ScriptBlock, multiplyNumbersScript)
                    .AddParameter(ArgumentList, new int[] { 6, 9 })
                    .AddParameter(SubroutineName, multiplySubroutineName);

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                int result = (int)outputPipeline[0].BaseObject;

                Assert.AreEqual(54, result);
            }
        }

        [TestMethod]
        public void TestEchoString()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter(ScriptBlock, echoScript)
                    .AddParameter(ArgumentList, new String[] { "foo" })
                    .AddParameter(SubroutineName, echoSubroutineName);

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                String result = (String)outputPipeline[0].BaseObject;

                Assert.AreEqual("foo", result);
            }
        }

        [TestMethod]
        public void TestEchoByte()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter(ScriptBlock, echoScript)
                    .AddParameter(ArgumentList, new byte[] { 97 })
                    .AddParameter(SubroutineName, echoSubroutineName);

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                int result = (int)outputPipeline[0].BaseObject;

                Assert.AreEqual(97, result);
            }
        }

        [TestMethod]
        public void TestEchoSByte()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter(ScriptBlock, echoScript)
                    .AddParameter(ArgumentList, new System.SByte[] { 97 })
                    .AddParameter(SubroutineName, echoSubroutineName);

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                int result = (int)outputPipeline[0].BaseObject;

                Assert.AreEqual(97, result);
            }
        }

        [TestMethod]
        public void TestEchoInt16()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter(ScriptBlock, echoScript)
                    .AddParameter(ArgumentList, new Int16[] { 97 })
                    .AddParameter(SubroutineName, echoSubroutineName);

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                int result = (int)outputPipeline[0].BaseObject;

                Assert.AreEqual(97, result);
            }
        }

        [TestMethod]
        public void TestEchoUInt16()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter(ScriptBlock, echoScript)
                    .AddParameter(ArgumentList, new UInt16[] { 97 })
                    .AddParameter(SubroutineName, echoSubroutineName);

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                int result = (int)outputPipeline[0].BaseObject;

                Assert.AreEqual(97, result);
            }
        }

        [TestMethod]
        public void TestEchoChar()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter(ScriptBlock, echoScript)
                    .AddParameter(ArgumentList, new char[] { 'A' })
                    .AddParameter(SubroutineName, echoSubroutineName);

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                String result = (String)outputPipeline[0].BaseObject;

                Assert.AreEqual("A", result);
            }
        }

        [TestMethod]
        public void TestEchoInt32()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter(ScriptBlock, echoScript)
                    .AddParameter(ArgumentList, new Int32[] { 97 })
                    .AddParameter(SubroutineName, echoSubroutineName);

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                int result = (int)outputPipeline[0].BaseObject;

                Assert.AreEqual(97, result);
            }
        }

        [TestMethod]
        public void TestEchoUInt32()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter(ScriptBlock, echoScript)
                    .AddParameter(ArgumentList, new UInt32[] { 97 })
                    .AddParameter(SubroutineName, echoSubroutineName);

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                int result = (int)outputPipeline[0].BaseObject;

                Assert.AreEqual(97, result);
            }
        }

        [TestMethod]
        public void TestEchoUInt64()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter(ScriptBlock, echoScript)
                    .AddParameter(ArgumentList, new UInt64[] { 97L })
                    .AddParameter(SubroutineName, echoSubroutineName);

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                int result = (int)outputPipeline[0].BaseObject;

                Assert.AreEqual(97, result);
            }
        }

        [TestMethod]
        public void TestEchoUInt128()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter(ScriptBlock, echoScript)
                    .AddParameter(ArgumentList, new UInt128[] { 97L })
                    .AddParameter(SubroutineName, echoSubroutineName);

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                int result = (int)outputPipeline[0].BaseObject;

                Assert.AreEqual(97, result);
            }
        }

        [TestMethod]
        public void TestEchoInt128()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter(ScriptBlock, echoScript)
                    .AddParameter(ArgumentList, new Int128[] { 97L })
                    .AddParameter(SubroutineName, echoSubroutineName);

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                int result = (int)outputPipeline[0].BaseObject;

                Assert.AreEqual(97, result);
            }
        }

        [TestMethod]
        public void TestEchoInt64()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter(ScriptBlock, echoScript)
                    .AddParameter(ArgumentList, new Int64[] { 97L })
                    .AddParameter(SubroutineName, echoSubroutineName);

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                int result = (int)outputPipeline[0].BaseObject;

                Assert.AreEqual(97, result);
            }
        }

        [TestMethod]
        public void TestEchoIntPtr()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter(ScriptBlock, echoScript)
                    .AddParameter(ArgumentList, new IntPtr[] { new IntPtr(97) })
                    .AddParameter(SubroutineName, echoSubroutineName);

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                int result = (int)outputPipeline[0].BaseObject;

                Assert.AreEqual(97, result);
            }
        }

        [TestMethod]
        public void TestEchoBoolTrue()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter(ScriptBlock, echoScript)
                    .AddParameter(ArgumentList, new bool[] { true })
                    .AddParameter(SubroutineName, echoSubroutineName);

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                bool result = (bool)outputPipeline[0].BaseObject;

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void TestEchoBoolFalse()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter(ScriptBlock, echoScript)
                    .AddParameter(ArgumentList, new bool[] { false })
                    .AddParameter(SubroutineName, echoSubroutineName);

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                bool result = (bool)outputPipeline[0].BaseObject;

                Assert.IsFalse(result);
            }
        }

        [TestMethod]
        public void TestEchoDouble()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                double pi = 3.14159;
                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter(ScriptBlock, echoScript)
                    .AddParameter(ArgumentList, new double[] { pi })
                    .AddParameter(SubroutineName, echoSubroutineName);

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                double result = (double)outputPipeline[0].BaseObject;

                Assert.AreEqual(pi, result);
            }
        }

        [TestMethod]
        public void TestEchoFloat()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                float pi = 3.14F;
                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter(ScriptBlock, echoScript)
                    .AddParameter(ArgumentList, new float[] { pi })
                    .AddParameter(SubroutineName, echoSubroutineName);

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                double result = (double)outputPipeline[0].BaseObject;

                Assert.AreEqual(pi, result);
            }
        }

        [TestMethod]
        public void TestEchoDecimal()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                double pi = 3.14159;
                decimal pid = (decimal)pi;
                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter(ScriptBlock, echoScript)
                    .AddParameter(ArgumentList, new decimal[] { pid })
                    .AddParameter(SubroutineName, echoSubroutineName);

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                double result = (double)outputPipeline[0].BaseObject;

                Assert.AreEqual(pi, result);
            }
        }

        [TestMethod]
        public void TestReturnArray()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter(ScriptBlock, "return [4,5,6]");

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                object[] result = (object[])outputPipeline[0].BaseObject;

                Assert.AreEqual(3, result.Length);
                Assert.AreEqual(4, (int)result[0]);
                Assert.AreEqual(5, (int)result[1]);
                Assert.AreEqual(6, (int)result[2]);
            }
        }

        [TestMethod]
        public void TestEchoArray()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter(ScriptBlock, echoScript)
                    .AddParameter(SubroutineName, echoSubroutineName)
                    .AddParameter(ArgumentList, new int[][] { new int[] { 7, 8, 9 } });

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                object[] result = (object[])outputPipeline[0].BaseObject;

                Assert.AreEqual(3, result.Length);
                Assert.AreEqual(7, (int)result[0]);
                Assert.AreEqual(8, (int)result[1]);
                Assert.AreEqual(9, (int)result[2]);
            }
        }

        [TestMethod]
        public void TestEchoHashtable()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                Hashtable table = new Hashtable()
                {
                    {"Name","Dennis"},
                    {"Age",37}
                };

                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter(ScriptBlock, echoScript)
                    .AddParameter(SubroutineName, echoSubroutineName)
                    .AddParameter(ArgumentList, table);

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                IDictionary result = (IDictionary)outputPipeline[0].BaseObject;

                Assert.AreEqual("Dennis", (String)result["Name"]);
                Assert.AreEqual(37, (int)result["Age"]);
            }
        }

        [TestMethod]
        public void TestEchoCustomer()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter(ScriptBlock, echoScript)
                    .AddParameter(SubroutineName, echoSubroutineName)
                    .AddParameter(ArgumentList, new Customer("Dennis", 37));

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                IDictionary result = (IDictionary)outputPipeline[0].BaseObject;

                Assert.AreEqual("Dennis", (String)result["Name"]);
                Assert.AreEqual(37, (int)result["Age"]);
            }
        }

        [TestMethod]
        public void TestEchoDateTime()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                DateTime now = DateTime.Now.ToUniversalTime();

                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter(ScriptBlock, echoScript)
                    .AddParameter(ArgumentList, new DateTime[] { now })
                    .AddParameter(SubroutineName, echoSubroutineName);

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                DateTime result = (DateTime)outputPipeline[0].BaseObject;

                var dateTimeOffset = result.Subtract(now);

                Assert.IsTrue(dateTimeOffset.TotalSeconds < 2.0 && dateTimeOffset.TotalSeconds > -2.0);
            }
        }

        [TestMethod]
        public void TestEchoFileUrl()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                Uri uri = new Uri("http://www.contoso.com/");

                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter(ScriptBlock, echoScript)
                    .AddParameter(ArgumentList, new Uri[] { uri })
                    .AddParameter(SubroutineName, echoSubroutineName);

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                Uri result = (Uri)outputPipeline[0].BaseObject;

                Assert.AreEqual(uri, result);
            }
        }

        [TestMethod]
        public void TestEchoHashTable()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                Hashtable hashtable = new Hashtable();
                hashtable.Add("foo", "bar");

                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter(ScriptBlock, echoScript)
                    .AddParameter(ArgumentList, new object[] { hashtable })
                    .AddParameter(SubroutineName, echoSubroutineName);

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                Hashtable result = (Hashtable)outputPipeline[0].BaseObject;

                Assert.AreEqual(1, result.Count);
                Assert.AreEqual("bar", result["foo"]);
            }
        }

        [TestMethod]
        public void TestEchoPSCustomObject()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                PSObject hashtable = new PSObject();
                PSNoteProperty noteProperty = new PSNoteProperty("foo", "bar");
                hashtable.Properties.Add(noteProperty);

                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter(ScriptBlock, echoScript)
                    .AddParameter(ArgumentList, new object[] { hashtable })
                    .AddParameter(SubroutineName, echoSubroutineName);

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                Hashtable result = (Hashtable)outputPipeline[0].BaseObject;

                Assert.AreEqual(1, result.Count);
                Assert.AreEqual("bar", result["foo"]);
            }
        }

        [TestMethod]
        public void TestEchoWithFileInfoAsFileInfo()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter("FileInfo", new FileInfo(testAppleScriptPath))
                    .AddParameter(ArgumentList, new object[] { "foo" })
                    .AddParameter(SubroutineName, echoSubroutineName);

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                String result = (String)outputPipeline[0].BaseObject;

                Assert.AreEqual("foo", result);
            }
        }

        [TestMethod]
        public void TestEchoWithFileInfoAsString()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter("FileInfo", testAppleScriptPath)
                    .AddParameter(ArgumentList, new object[] { "foo" })
                    .AddParameter(SubroutineName, echoSubroutineName);

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                String result = (String)outputPipeline[0].BaseObject;

                Assert.AreEqual("foo", result);
            }
        }

        [TestMethod]
        public void TestEchoWithFileInfoAsArguments()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand(InvokeAppleScript)
                    .AddArgument(new FileInfo(testAppleScriptPath))
                    .AddArgument(echoSubroutineName)
                    .AddArgument(new object[] { "foo" });

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                String result = (String)outputPipeline[0].BaseObject;

                Assert.AreEqual("foo", result);
            }
        }

        [TestMethod]
        public void TestEchoWithFileInfoAndArgumentListAsInput()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand(InvokeAppleScript)
                    .AddArgument(new FileInfo(testAppleScriptPath))
                    .AddArgument(echoSubroutineName);

                var outputPipeline = powerShell.Invoke(new object[] { "foo", "bar" });

                Assert.AreEqual(2, outputPipeline.Count);

                Assert.AreEqual("foo", (String)outputPipeline[0].BaseObject);
                Assert.AreEqual("bar", (String)outputPipeline[1].BaseObject);
            }
        }
        [TestMethod]
        public void TestEchoWithFileReadAsArguments()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand(InvokeAppleScript)
                    .AddArgument(File.ReadAllText(testAppleScriptPath))
                    .AddArgument(echoSubroutineName)
                    .AddArgument(new object[] { "foo" });

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                String result = (String)outputPipeline[0].BaseObject;

                Assert.AreEqual("foo", result);
            }
        }

        [TestMethod]
        public void TestOnRunWithFileReadAsInputNoArguments()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand(InvokeAppleScript);
                var outputPipeline = powerShell.Invoke(new String[] { File.ReadAllText(testAppleScriptPath) });

                Assert.AreEqual(1, outputPipeline.Count);

                String result = (String)outputPipeline[0].BaseObject;

                Assert.AreEqual("Hello World", result);
            }
        }

        [TestMethod]
        public void TestHelloWorldWithFileReadAsInputWithHelloMustFail()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                bool wasCaught = false;
                try
                {
                    powerShell.AddCommand(InvokeAppleScript).AddArgument("hello");
                    var outputPipeline = powerShell.Invoke(new String[] { File.ReadAllText(testAppleScriptPath) });
                }
                catch (ActionPreferenceStopException)
                {
                    wasCaught = true;
                }

                Assert.IsTrue(wasCaught, "Must be an exception to catch");
            }
        }

        [TestMethod]
        public void TestHelloWorldWithFileReadAsInputWithSubroutineNameHello()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand(InvokeAppleScript).AddParameter(SubroutineName, "hello");

                var outputPipeline = powerShell.Invoke(new String[] { File.ReadAllText(testAppleScriptPath) });

                Assert.AreEqual(1, outputPipeline.Count);

                String result = (String)outputPipeline[0].BaseObject;

                Assert.AreEqual("Hello", result);
            }
        }

        [TestMethod]
        public void TestEchoWithUriAsArguments()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                FileInfo info = new FileInfo(testAppleScriptPath);
                Uri uri = new Uri("file://" + info.FullName);
                powerShell.AddCommand(InvokeAppleScript)
                    .AddArgument(uri)
                    .AddArgument(echoSubroutineName)
                    .AddArgument(new object[] { "foo" });

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                String result = (String)outputPipeline[0].BaseObject;

                Assert.AreEqual("foo", result);
            }
        }

        [TestMethod]
        public void TestEchoErrorWithArgumentsMustThrow1708()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                int? errorNumber = null;

                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter(ScriptBlock, """
                    on run argv
                        return argv
                    end run
                    """)
                    .AddParameter(ArgumentList, new object[] { "foo" })
                    .AddParameter(SubroutineName, "run");

                try
                {
                    powerShell.Invoke();
                }
                catch (ActionPreferenceStopException ex)
                {
                    AppleScriptException appleScriptException = (AppleScriptException)ex.ErrorRecord.Exception;

                    errorNumber = appleScriptException.HResult;
                }

                Assert.IsTrue(errorNumber.HasValue);
                Assert.AreEqual(-1708, errorNumber);
            }
        }

        [TestMethod]
        public void TestEchoErrorWithScriptMustThrow2753()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                int? errorNumber = null;

                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter(ScriptBlock, "foo");

                try
                {
                    powerShell.Invoke();
                }
                catch (ActionPreferenceStopException ex)
                {
                    AppleScriptException appleScriptException = (AppleScriptException)ex.ErrorRecord.Exception;

                    errorNumber = appleScriptException.HResult;
                }

                Assert.IsTrue(errorNumber.HasValue);
                Assert.AreEqual(-2753, errorNumber);
            }
        }

        [TestMethod]
        public void TestEchoFileInfo()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                FileInfo fileInfo = new FileInfo(testAppleScriptPath);
                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter(ScriptBlock, echoScript)
                    .AddParameter(ArgumentList, new FileInfo[] { fileInfo })
                    .AddParameter(SubroutineName, echoSubroutineName);

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                Uri result = (Uri)outputPipeline[0].BaseObject;

                Assert.AreEqual("file://" + fileInfo.FullName, result.ToString());
            }
        }

        [TestMethod]
        public void TestEchoJson()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                JsonNode node = JsonNode.Parse("42");

                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter(ScriptBlock, echoScript)
                    .AddParameter(ArgumentList, new JsonNode[] { node })
                    .AddParameter(SubroutineName, echoSubroutineName);

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                String result = (String)outputPipeline[0].BaseObject;

                Assert.AreEqual("42", result);
            }
        }

        [TestMethod]
        public void TestEchoXml()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml("<doc/>");

                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter(ScriptBlock, echoScript)
                    .AddParameter(ArgumentList, new XmlNode[] { xml })
                    .AddParameter(SubroutineName, echoSubroutineName);

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                String result = (String)outputPipeline[0].BaseObject;

                Assert.AreEqual("<doc />", result);
            }
        }

        [TestMethod]
        public void TestEchoCharArray()
        {
            using (PowerShell powerShell = PowerShell.Create(initialSessionState))
            {
                powerShell.AddCommand(InvokeAppleScript)
                    .AddParameter(ScriptBlock, echoScript)
                    .AddParameter(ArgumentList, new char[][] { "foo".ToCharArray() })
                    .AddParameter(SubroutineName, echoSubroutineName);

                var outputPipeline = powerShell.Invoke();

                Assert.AreEqual(1, outputPipeline.Count);

                String result = (String)outputPipeline[0].BaseObject;

                Assert.AreEqual("foo", result);
            }
        }
    }
}
