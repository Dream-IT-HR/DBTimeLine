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
        private IDBTable BusinessDay(IDBSchema sch)
        {
            var rev = new DBRevision(new DateTime(2020, 4, 14), 0, eDBRevisionType.Create);

            var ret = DBMacros.AddDBTableID("BusinessDay", sch, new DBRevision(rev));

            ret.AddField("ShortName", DBMacros.DBFieldNazivDescriptor(false), new DBRevision(rev));
            ret.AddField("FullName", DBMacros.DBFieldNazivDescriptor(false), new DBRevision(rev));

            ret.AddConstraint(new DBUniqueConstraintDescriptor("ShortName"), new DBRevision(rev));
            ret.AddConstraint(new DBUniqueConstraintDescriptor("FullName"), new DBRevision(rev));

            return ret;
        }

        private string FillBusinessDays(IDBRevision sender, eDBType dBType)
        {
            return
@"WITH DaysCTE AS
(
    SELECT TOP 0 ID = NEWID(), ShortName = '', FullName = ''
UNION ALL SELECT ID = NEWID(), ShortName = 'Mon', FullName = 'Monday'
UNION ALL SELECT ID = NEWID(), ShortName = 'Tue', FullName = 'Tuesday'
UNION ALL SELECT ID = NEWID(), ShortName = 'Wed', FullName = 'Wednesday'
UNION ALL SELECT ID = NEWID(), ShortName = 'Thu', FullName = 'Thursday'
UNION ALL SELECT ID = NEWID(), ShortName = 'Fri', FullName = 'Friday'
UNION ALL SELECT ID = NEWID(), ShortName = 'Sat', FullName = 'Saturday'
UNION ALL SELECT ID = NEWID(), ShortName = 'Sun', FullName = 'Sunday'
)

INSERT INTO Flyinline.BusinessDay (ID, shortName, fullname)
SELECT 
	t.ID, t.ShortName, t.FullName
FROM 
	DaysCTE t
	LEFT JOIN Flyinline.BusinessDay d ON t.ShortName = d.ShortName AND t.FullName = d.FullName
WHERE 
	d.ID is null
";
        }
    }
}
