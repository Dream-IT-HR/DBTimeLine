using Framework.DBTimeLine;
using Framework.DBTimeLine.DBObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customizations.Flyinline
{
    public partial class FlyinlineModule
    {
        private IDBTable LineEmployee(IDBSchema sch)
        {
            var rev = new DBRevision(new DateTime(2020, 4, 14), 2, eDBRevisionType.Create);
            
            var ret = DBMacros.AddDBTableID("LineEmployee", sch, new DBRevision(rev));

            DBMacros.AddForeignKeyFieldID("LineID", false, ret, sch.Name + ".Line",
                    new DBRevision(rev));

            DBMacros.AddForeignKeyFieldID("EmployeeID", false, ret, sch.Name + ".UserDetail",
                    new DBRevision(rev));

            return ret;
        }
    }
}
