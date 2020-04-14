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
        private IDBTable LineStatus(IDBSchema sch)
        {
            var rev = new DBRevision(new DateTime(2020, 4, 14), 2, eDBRevisionType.Create);

            var ret = DBMacros.AddDBTableID("LineStatus", sch, new DBRevision(rev));

            ret.AddConstraint(new DBForeignKeyConstraintDescriptor(new List<string>() { "ID" }, sch.Name + ".Line", new List<string>() { "ID" }),
                new DBRevision(rev));

            ret.AddField("IsReadyForWork", DBMacros.DBFieldBitDescriptor(false),
                new DBRevision(rev));

            return ret;
        }
    }
}
