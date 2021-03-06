﻿using Raven.TestSuite.ClientWrapper._2_5_2750;
using Raven.TestSuite.Common.WrapperInterfaces;
using System;
using System.IO;
using System.Reflection;

namespace Raven.TestSuite.ClientWrapper.v2_5_2750
{
    public class Wrapper : MarshalByRefObject, IRavenClientWrapper, ITestUnitEnvironment
    {
        private Assembly assembly;
        private int databasePort;
        private string testSuiteRunningFolder;

        internal void LoadAssemblyAndSetUp(string clientDllPath, string testSuiteRunningFolder, int databasePort)
        {
            this.testSuiteRunningFolder = testSuiteRunningFolder;
            this.databasePort = databasePort;
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            this.assembly = Assembly.LoadFile(clientDllPath);
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender,
                                              ResolveEventArgs args)
        {
            var assemblyname = new AssemblyName(args.Name).Name;
            var assemblyFileName = Path.Combine(this.testSuiteRunningFolder, assemblyname + ".dll");
            var assembly = Assembly.LoadFrom(assemblyFileName);
            return assembly;
        }

        public string GetVersion()
        {
            return "2.5.2750";
        }

        public void Execute(Action<ITestUnitEnvironment> action)
        {
            action(this);
        }

        public void Execute(Action<IRavenClientWrapper> action)
        {
            action(this);
        }

        // ITestUnitEnvironment

        public IDocumentStoreWrapper CreateDocumentStore(string defaultDatabase)
        {
            var type = this.assembly.GetType("Raven.Client.Document.DocumentStore");
            var documentStore = Activator.CreateInstance(type) as Client.Document.DocumentStore;
            documentStore.Url = "http://localhost:" + this.databasePort;
            documentStore.DefaultDatabase = defaultDatabase;
            return new DocumentStoreWrapper(documentStore);
        }
    }
}
