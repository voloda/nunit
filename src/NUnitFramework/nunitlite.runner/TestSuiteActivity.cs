// ***********************************************************************
// Copyright (c) 2012 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Reflection;
using Android.App;
using Android.Widget;
using Android.OS;
using NUnit.Common;
using NUnit.Framework;
using NUnit.Framework.Api;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnitLite.Runner;
using NUnitLite.Runner.Android;

namespace nunitlite.runnerandroid
{
	[Activity (Label = "nunitlite.runner", MainLauncher = true, Icon = "@drawable/icon")]
	public class TestSuiteActivity : Activity
    {
        private ITestAssemblyRunner runner;
        private ExtendedTextWriter writer;

		protected override void OnCreate (Bundle bundle)
		{
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.TestSuite);

            runner = new NUnitTestAssemblyRunner(new DefaultTestAssemblyBuilder());
            
            AddTest(Assembly.GetExecutingAssembly());

		    var editText = FindViewById<EditText>(Resource.Id.textOutput);
            writer = new ExtendedTextWriter(new TextBlockWriter(editText));

			// Get our button from the layout resource,
			// and attach an event to it
            var button = FindViewById<Button>(Resource.Id.buttonRun);
			
			button.Click += delegate {
                TextUI.WriteHeader(this.writer);
                TextUI.WriteRuntimeEnvironment(this.writer);
                RunOnUiThread(ExecuteTests);
			};
		}

	    protected virtual bool AddTest(Assembly testAssembly)
	    {
            return runner.Load(testAssembly, new Dictionary<string, string>()) != null;
	    }

        private void ExecuteTests()
        {
            ITestResult result = runner.Run(TestListener.NULL, TestFilter.Empty);
            var reporter = new ResultReporter(result, writer, false);

            reporter.ReportResults();

            //ResultSummary summary = reporter.Summary;

            //this.Total.Text = summary.TestCount.ToString();
            //this.Failures.Text = summary.FailureCount.ToString();
            //this.Errors.Text = summary.ErrorCount.ToString();
            //var notRunTotal = summary.SkipCount + summary.InvalidCount + summary.IgnoreCount;
            //this.NotRun.Text = notRunTotal.ToString();
            //this.Passed.Text = summary.PassCount.ToString();
            //this.Inconclusive.Text = summary.InconclusiveCount.ToString();

            //this.Notice.Visibility = Visibility.Collapsed;
        }
	}

    [TestFixture]
    public class TestsSample
    {

        [SetUp]
        public void Setup() { }


        [TearDown]
        public void Tear() { }

        [Test]
        public void Pass()
        {
            Console.WriteLine("test1");
            Assert.True(true);
        }

        [Test]
        public void Fail()
        {
            Assert.False(true);
        }

        [Test]
        [Ignore("another time")]
        public void Ignore()
        {
            Assert.True(false);
        }

        [Test]
        public void Inconclusive()
        {
            Assert.Inconclusive("Inconclusive");
        }
    }
}


