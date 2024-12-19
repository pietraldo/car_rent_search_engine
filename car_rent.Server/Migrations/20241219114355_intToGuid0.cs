using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace car_rent.Server.Migrations
{
    public partial class intToGuid0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                -- Handle History Table Foreign Key and Index Dependencies
                ALTER TABLE dbo.History DROP CONSTRAINT FK_History_Offers_Offer_ID;
                ALTER TABLE dbo.History DROP CONSTRAINT FK_History_Companies_Company_ID;
                ALTER TABLE dbo.History DROP CONSTRAINT FK_History_AspNetUsers_User_ID;
                DROP INDEX IF EXISTS IX_History_Offer_ID ON dbo.History;
                DROP INDEX IF EXISTS IX_History_Company_ID ON dbo.History;
                DROP INDEX IF EXISTS IX_History_User_ID ON dbo.History;

                -- Drop default constraints on Offer_ID, Company_ID, Rent_ID, and User_ID
                DECLARE @ObjectName NVARCHAR(100);

                -- Rent_ID (History Table Primary Key)
                SELECT @ObjectName = OBJECT_NAME([default_object_id]) 
                FROM SYS.COLUMNS
                WHERE [object_id] = OBJECT_ID('[dbo].[History]') AND [name] = 'Rent_ID';
                IF @ObjectName IS NOT NULL EXEC('ALTER TABLE [dbo].[History] DROP CONSTRAINT ' + @ObjectName);

                -- Offers
                SELECT @ObjectName = OBJECT_NAME([default_object_id]) 
                FROM SYS.COLUMNS
                WHERE [object_id] = OBJECT_ID('[dbo].[Offers]') AND [name] = 'Offer_ID';
                IF @ObjectName IS NOT NULL EXEC('ALTER TABLE [dbo].[Offers] DROP CONSTRAINT ' + @ObjectName);

                -- Companies
                SELECT @ObjectName = OBJECT_NAME([default_object_id]) 
                FROM SYS.COLUMNS
                WHERE [object_id] = OBJECT_ID('[dbo].[Companies]') AND [name] = 'Company_ID';
                IF @ObjectName IS NOT NULL EXEC('ALTER TABLE [dbo].[Companies] DROP CONSTRAINT ' + @ObjectName);

                -- Drop primary key constraints
                ALTER TABLE dbo.History DROP CONSTRAINT PK_History;
                ALTER TABLE dbo.Offers DROP CONSTRAINT PK_Offers;
                ALTER TABLE dbo.Companies DROP CONSTRAINT PK_Companies;

                -- Drop the existing ID columns
                ALTER TABLE dbo.History DROP COLUMN Rent_ID;
                ALTER TABLE dbo.Offers DROP COLUMN Offer_ID;
                ALTER TABLE dbo.Companies DROP COLUMN Company_ID;

                -- Add new ID columns with uniqueidentifier type
                ALTER TABLE dbo.History
                ADD Rent_ID UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL;
                ALTER TABLE dbo.Offers
                ADD Offer_ID UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL;
                ALTER TABLE dbo.Companies
                ADD Company_ID UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL;

                -- Recreate primary key constraints
                ALTER TABLE dbo.History ADD CONSTRAINT PK_History PRIMARY KEY CLUSTERED (Rent_ID);
                ALTER TABLE dbo.Offers ADD CONSTRAINT PK_Offers PRIMARY KEY CLUSTERED (Offer_ID);
                ALTER TABLE dbo.Companies ADD CONSTRAINT PK_Companies PRIMARY KEY CLUSTERED (Company_ID);

                -- Recreate Foreign Key in History for Offers
                ALTER TABLE dbo.History DROP COLUMN Offer_ID;
                ALTER TABLE dbo.History
                ADD Offer_ID UNIQUEIDENTIFIER NOT NULL;

                ALTER TABLE dbo.History
                ADD CONSTRAINT FK_History_Offers_Offer_ID FOREIGN KEY (Offer_ID) 
                REFERENCES dbo.Offers (Offer_ID) ON DELETE CASCADE;

                -- Recreate Foreign Key in History for Companies
                ALTER TABLE dbo.History DROP COLUMN Company_ID;
                ALTER TABLE dbo.History
                ADD Company_ID UNIQUEIDENTIFIER NOT NULL;

                ALTER TABLE dbo.History
                ADD CONSTRAINT FK_History_Companies_Company_ID FOREIGN KEY (Company_ID) 
                REFERENCES dbo.Companies (Company_ID) ON DELETE CASCADE;

                -- Recreate Foreign Key in History for AspNetUsers
                ALTER TABLE dbo.History DROP COLUMN User_ID;
                ALTER TABLE dbo.History
                ADD User_ID UNIQUEIDENTIFIER NOT NULL;

                ALTER TABLE dbo.History
                ADD CONSTRAINT FK_History_AspNetUsers_User_ID FOREIGN KEY (User_ID) 
                REFERENCES dbo.AspNetUsers (Id) ON DELETE CASCADE;

                -- Recreate indexes
                CREATE INDEX IX_History_Offer_ID ON dbo.History(Offer_ID);
                CREATE INDEX IX_History_Company_ID ON dbo.History(Company_ID);
                CREATE INDEX IX_History_User_ID ON dbo.History(User_ID);
            ");
        }



        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                -- Revert Foreign Key and Index Dependencies
                ALTER TABLE dbo.History DROP CONSTRAINT FK_History_Offers_Offer_ID;
                ALTER TABLE dbo.History DROP CONSTRAINT FK_History_Companies_Company_ID;
                ALTER TABLE dbo.History DROP CONSTRAINT FK_History_AspNetUsers_User_ID;
                DROP INDEX IF EXISTS IX_History_Offer_ID ON dbo.History;
                DROP INDEX IF EXISTS IX_History_Company_ID ON dbo.History;
                DROP INDEX IF EXISTS IX_History_User_ID ON dbo.History;

                -- Drop primary key constraints
                ALTER TABLE dbo.History DROP CONSTRAINT PK_History;
                ALTER TABLE dbo.Offers DROP CONSTRAINT PK_Offers;
                ALTER TABLE dbo.Companies DROP CONSTRAINT PK_Companies;

                -- Drop the uniqueidentifier ID columns
                ALTER TABLE dbo.History DROP COLUMN Rent_ID;
                ALTER TABLE dbo.Offers DROP COLUMN Offer_ID;
                ALTER TABLE dbo.Companies DROP COLUMN Company_ID;

                -- Add back the original ID columns with int type
                ALTER TABLE dbo.History
                ADD Rent_ID INT IDENTITY(1, 1) NOT NULL;
                ALTER TABLE dbo.Offers
                ADD Offer_ID INT IDENTITY(1, 1) NOT NULL;
                ALTER TABLE dbo.Companies
                ADD Company_ID INT IDENTITY(1, 1) NOT NULL;

                -- Recreate primary key constraints
                ALTER TABLE dbo.History ADD CONSTRAINT PK_History PRIMARY KEY CLUSTERED (Rent_ID ASC);
                ALTER TABLE dbo.Offers ADD CONSTRAINT PK_Offers PRIMARY KEY CLUSTERED (Offer_ID ASC);
                ALTER TABLE dbo.Companies ADD CONSTRAINT PK_Companies PRIMARY KEY CLUSTERED (Company_ID ASC);

                -- Recreate Foreign Key in History for Offers
                ALTER TABLE dbo.History DROP COLUMN Offer_ID;
                ALTER TABLE dbo.History
                ADD Offer_ID INT NOT NULL;

                ALTER TABLE dbo.History
                ADD CONSTRAINT FK_History_Offers_Offer_ID FOREIGN KEY (Offer_ID) 
                REFERENCES dbo.Offers (Offer_ID) ON DELETE CASCADE;

                -- Recreate Foreign Key in History for Companies
                ALTER TABLE dbo.History DROP COLUMN Company_ID;
                ALTER TABLE dbo.History
                ADD Company_ID INT NOT NULL;

                ALTER TABLE dbo.History
                ADD CONSTRAINT FK_History_Companies_Company_ID FOREIGN KEY (Company_ID) 
                REFERENCES dbo.Companies (Company_ID) ON DELETE CASCADE;

                -- Recreate Foreign Key in History for AspNetUsers
                ALTER TABLE dbo.History DROP COLUMN User_ID;
                ALTER TABLE dbo.History
                ADD User_ID INT NOT NULL;

                ALTER TABLE dbo.History
                ADD CONSTRAINT FK_History_AspNetUsers_User_ID FOREIGN KEY (User_ID) 
                REFERENCES dbo.AspNetUsers (Id) ON DELETE CASCADE;

                -- Recreate indexes
                CREATE INDEX IX_History_Offer_ID ON dbo.History(Offer_ID);
                CREATE INDEX IX_History_Company_ID ON dbo.History(Company_ID);
                CREATE INDEX IX_History_User_ID ON dbo.History(User_ID);
            ");
       }

    }
}
