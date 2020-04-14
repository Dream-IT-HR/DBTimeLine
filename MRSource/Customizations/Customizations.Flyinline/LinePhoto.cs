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
        private IDBTable LinePhoto(IDBSchema sch)
        {
            var rev = new DBRevision(new DateTime(2020, 4, 14), 2, eDBRevisionType.Create);

            var ret = DBMacros.AddDBTableID("LinePhoto", sch, new DBRevision(rev));

            DBMacros.AddForeignKeyFieldID("LineID", false, ret, sch.Name + ".Line",
                    new DBRevision(rev));

            ret.AddField("PhotoName", DBMacros.DBFieldNazivDescriptor(false),
                    new DBRevision(rev));

            ret.AddField("PhotoUrl", DBMacros.DBFieldLongTextDescriptor(false),
                    new DBRevision(rev));

            return ret;
        }
    }
}
