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
        private IDBTable LineEmployeeInvited(IDBSchema sch)
        {
            var rev = new DBRevision(new DateTime(2020, 4, 14), 3, eDBRevisionType.Create);

            var ret = DBMacros.AddDBTableID("LineEmployeeInvited", sch, new DBRevision(rev));

            ret.AddConstraint(new DBForeignKeyConstraintDescriptor(new List<string>() { "ID" }, sch.Name + ".LineEmployee", new List<string>() { "ID" }), 
                new DBRevision(rev));

            ret.AddField("InviteSentOn", DBMacros.DBFieldDateTimeDescriptor(false),
                new DBRevision(rev));
            
            ret.AddField("InviteEmail", DBMacros.DBFieldNazivDescriptor(false),
                new DBRevision(rev));

            return ret;
        }
    }
}
