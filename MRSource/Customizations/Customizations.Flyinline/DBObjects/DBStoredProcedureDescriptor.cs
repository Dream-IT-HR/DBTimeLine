﻿using Framework.DBTimeLine;

namespace Customizations.Flyinline.DBObjects
{
    public class DBStoredProcedureDescriptor : IDBObjectDescriptor
    {
        public IDBObject GetDBObjectInstance(IDBChained parent = null)
        {
            return new DBStoredProcedure() { Parent = parent, Descriptor = this };
        }

        public DBStoredProcedureDescriptor()
        {

        }

        public DBStoredProcedureDescriptor(IDBObjectDescriptor descriptor) : this()
        {

            Parameters = ((DBStoredProcedureDescriptor)descriptor).Parameters;
            Body = ((DBStoredProcedureDescriptor)descriptor).Body;
        }

        public string Parameters { get; set; }
        public string Body { get; set; }
        public bool ErrorHandling { get; set; }
    }
}
