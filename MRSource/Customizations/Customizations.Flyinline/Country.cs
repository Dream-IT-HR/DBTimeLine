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
        private IDBTable Country(IDBSchema sch)
        {
            var rev = new DBRevision(new DateTime(2020, 4, 14), 0, eDBRevisionType.Create);

            var ret = DBMacros.AddDBTableID("Country", sch, new DBRevision(rev));

            ret.AddField("Name", DBMacros.DBFieldNazivDescriptor(false), new DBRevision(rev));

            return ret;
        }
    }
}
