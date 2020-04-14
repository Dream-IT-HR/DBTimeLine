using System;
using Framework.DBTimeLine.DBObjects;
using Framework.DBTimeLine;
using System.Collections.Generic;
using Customizations.Flyinline.StoredProcedures;

namespace Customizations.Flyinline
{
    public partial class FlyinlineModule : DBModule
    {
        public override string ModuleKey
        {
            get
            {
                return "FlyinlineERM";
            }
        }
        public FlyinlineModule()
        {
            DefaultSchemaName = "Flyinline";
        }

        public override void CreateTimeLine()
        {
            var rev = new DBRevision(new DateTime(2016, 6, 10), 0, eDBRevisionType.Create);

            IDBSchema sch = AddSchema(DefaultSchemaName, new DBSchemaDescriptor());
            if (DefaultSchemaName != "dbo") sch.AddRevision(new DBRevision(rev));

            UserDetail(sch);
            
            Organization(sch);
            Line(sch);
            LineEmployee(sch);
            LineEmployeeInvited(sch);
            LineEmployeeAccepted(sch);
            Country(sch);
            LineLocation(sch);
            LinePhoto(sch);
            BusinessDay(sch);
            LineBusinessHour(sch);
            LineStatus(sch);

            ClearDbForTesting.Create(sch);

            Tasks(sch);
        }


        private IDBTable UserDetail(IDBSchema sch)
        {
            var rev = new DBRevision(new DateTime(2019, 9, 9), 0, eDBRevisionType.Create);
            var ret = DBMacros.AddDBTableID("UserDetail", sch, rev);

            ret.AddConstraint(new DBForeignKeyConstraintDescriptor(new List<string>() { "ID" }, "Common.Principal", new List<string>() { "ID" }), 
                new DBRevision(rev));

            ret.AddField("Username", new DBFieldDescriptor() { FieldType = new DBFieldTypeNvarchar(), Size = 512, Nullable = false },
                new DBRevision(rev));

            ret.AddConstraint(new DBUniqueConstraintDescriptor("Username"),
                new DBRevision(rev));

            ret.AddField("Email", new DBFieldDescriptor() { FieldType = new DBFieldTypeNvarchar(), Size = 512, Nullable = false },
                new DBRevision(rev));

            var fldFullName = ret.AddField("Fullname", new DBFieldDescriptor() { FieldType = new DBFieldTypeNvarchar(), Size = 512, Nullable = false },
                new DBRevision(rev));

            fldFullName.AddRevision(new DBRevision(new DateTime(2019, 10, 10), 0, eDBRevisionType.Delete));

            ret.AddField("FirstName", new DBFieldDescriptor() { FieldType = new DBFieldTypeNvarchar(), Size = 512, Nullable = false },
                new DBRevision(rev));

            ret.AddField("LastName", new DBFieldDescriptor() { FieldType = new DBFieldTypeNvarchar(), Size = 512, Nullable = false },
                new DBRevision(rev));


            var fldNickName = ret.AddField("Nickname", new DBFieldDescriptor() { FieldType = new DBFieldTypeNvarchar(), Size = 512, Nullable = false },
                new DBRevision(rev));

            fldNickName.AddRevision(new DBRevision(new DateTime(2019, 10, 10), 0, eDBRevisionType.Delete));

            return ret;
        }

        private void Tasks(IDBSchema sch)
        {
            sch.AddRevision(new DBRevision(new DateTime(2019, 9, 9), 0, eDBRevisionType.AlwaysExecuteTask, FillClaims));
            sch.AddRevision(new DBRevision(new DateTime(2019, 9, 10), 0, eDBRevisionType.AlwaysExecuteTask, FillRoles));
            sch.AddRevision(new DBRevision(new DateTime(2019, 9, 30), 0, eDBRevisionType.AlwaysExecuteTask, FillRolePermissions));

            sch.AddRevision(new DBRevision(new DateTime(2020, 4, 14), 3, eDBRevisionType.AlwaysExecuteTask, FillBusinessDays));
        }

        #region Tasks
        
        private string FillClaims(IDBRevision sender, eDBType dBType)
        {
            return
@"WITH ClaimsCTE AS
(
    SELECT TOP 0 ID = NEWID(), Name = ''
    UNION ALL SELECT ID = NEWID(), Name = 'Routes.HomePage.View'
)

INSERT INTO Common.Claim (ID, Name)
SELECT 
	claims.ID, claims.Name 
FROM 
	ClaimsCTE claims
	LEFT JOIN Common.Claim c ON claims.Name = c.Name
WHERE 
	c.ID is null
";
        }


        private string FillRoles(IDBRevision sender, eDBType dBType)
        {
            return
@"WITH RolesCTE AS
(
SELECT ID = NEWID(), Name = 'Client'
UNION ALL SELECT NEWID(), 'Admin'
UNION ALL SELECT NEWID(), 'BusinessOwner'
)

INSERT INTO Common.Role (ID, Name)
SELECT 
	t.ID, t.Name 
FROM 
	RolesCTE t
	LEFT JOIN Common.Role r ON t.Name = r.Name
WHERE 
	r.ID is null
";
        }


        private string FillRolePermissions(IDBRevision sender, eDBType dBType)
        {
            return
@"
;WITH 
RolesCTE AS
(
    SELECT RoleID = ID FROM Common.Role t WHERE t.Name IN ('Client', 'BusinessOwner')
),
ClaimsCTE AS
(
    SELECT ClaimID = ID FROM Common.Claim t WHERE t.Name IN ('Routes.HomePage.View')
)

INSERT INTO Common.RolePermission (ID, RoleID, ClaimID, CanExecute)
SELECT a.*
FROM
(
	SELECT ID = NEWID(), RoleID, ClaimID, CanExecute = 1
	FROM 
		RolesCTE r,
		ClaimsCTE c
) a
LEFT JOIN Common.RolePermission existing on a.RoleID = existing.RoleID and a.ClaimID = existing.ClaimID
WHERE existing.ID IS NULL
";
        }
        #endregion

    }
}
