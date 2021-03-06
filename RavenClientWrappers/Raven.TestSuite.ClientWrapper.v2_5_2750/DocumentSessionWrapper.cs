﻿using Raven.Client;
using Raven.TestSuite.Common.WrapperInterfaces;
using System;
using System.Linq;

namespace Raven.TestSuite.ClientWrapper.v2_5_2750
{
    public class DocumentSessionWrapper : MarshalByRefObject, IDocumentSessionWrapper
    {
        private IDocumentSession documentSession;

        internal DocumentSessionWrapper(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;
        }

        public IOrderedQueryable<T> Query<T>()
        {
            return this.documentSession.Query<T>();
        }

        public void Dispose()
        {
            if (this.documentSession != null)
            {
                this.documentSession.Dispose();
            }
        }

        public void Store(dynamic entity)
        {
            this.documentSession.Store(entity);
        }

        public void SaveChanges()
        {
            this.documentSession.SaveChanges();
        }

        public T Load<T>(ValueType id)
        {
            return this.documentSession.Load<T>(id);
        }
    }
}
