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
        private IDBTable LineLocation(IDBSchema sch)
        {
            var rev = new DBRevision(new DateTime(2020, 4, 14), 2, eDBRevisionType.Create);

            var ret = DBMacros.AddDBTableID("LineLocation", sch, new DBRevision(rev));

            ret.AddField("Longitude", new DBFieldDescriptor() { Size = 18, Precision = 6, FieldType = new DBFieldTypeDecimal(), DefaultValue = "0", Nullable = false },
                new DBRevision(rev));

            ret.AddField("Latitude", new DBFieldDescriptor() { Size = 18, Precision = 6, FieldType = new DBFieldTypeDecimal(), DefaultValue = "0", Nullable = false },
                new DBRevision(rev));

            DBMacros.AddForeignKeyFieldID("CountryID", false, ret, sch.Name + ".Country",
                    new DBRevision(rev));

            ret.AddField("StreetAddress", DBMacros.DBFieldNazivDescriptor(false),
                new DBRevision(rev));

            ret.AddField("City", DBMacros.DBFieldNazivDescriptor(false),
                new DBRevision(rev));

            ret.AddField("State", DBMacros.DBFieldNazivDescriptor(true),
                new DBRevision(rev));

            ret.AddField("ZipCode", DBMacros.DBFieldNazivDescriptor(true),
                new DBRevision(rev));

            ret.AddConstraint(new DBForeignKeyConstraintDescriptor(new List<string>() { "ID" }, sch.Name + ".Line", new List<string>() { "ID" }),
                new DBRevision(rev));

            return ret;
        }
    }
}
