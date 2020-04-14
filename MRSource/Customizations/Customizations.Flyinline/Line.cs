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
        private IDBTable Line(IDBSchema sch)
        {
            var rev = new DBRevision(new DateTime(2020, 4, 14), 1, eDBRevisionType.Create);
            
            var ret = DBMacros.AddDBTableID("Line", sch, new DBRevision(rev));

            var fldName = ret.AddField("Name", DBMacros.DBFieldNazivDescriptor(false), 
                new DBRevision(rev));

            fldName.AddRevision(new DBRevision(new DateTime(2020, 4, 14), 2, eDBRevisionType.Delete));

            ret.AddField("Title", DBMacros.DBFieldNazivDescriptor(false),
                new DBRevision(rev));
            
            ret.AddField("Description", DBMacros.DBFieldLongTextDescriptor(false),
                new DBRevision(rev));

            DBMacros.AddForeignKeyFieldID("OrganizationID", false, ret, sch.Name + ".Organization",
                    new DBRevision(rev));

            return ret;
        }

        
    }
}
