namespace SFLCC_Demo_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attendees",
                c => new
                    {
                        AttendeeId = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.AttendeeId);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        EventId = c.Int(nullable: false, identity: true),
                        EventName = c.String(),
                        EventStartDate = c.DateTime(nullable: false),
                        EventEndDate = c.DateTime(nullable: false),
                        Location_LocationId = c.Int(),
                        Attendee_AttendeeId = c.Int(),
                    })
                .PrimaryKey(t => t.EventId)
                .ForeignKey("dbo.Locations", t => t.Location_LocationId)
                .ForeignKey("dbo.Attendees", t => t.Attendee_AttendeeId)
                .Index(t => t.Location_LocationId)
                .Index(t => t.Attendee_AttendeeId);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationId = c.Int(nullable: false, identity: true),
                        LocationName = c.String(),
                        Address1 = c.String(),
                        Address2 = c.String(),
                        City = c.String(),
                        State = c.String(),
                        ZipCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LocationId);
            
            CreateTable(
                "dbo.Sessions",
                c => new
                    {
                        SessionId = c.Int(nullable: false, identity: true),
                        SessionName = c.String(),
                        SessionDescription = c.String(),
                        Slot_TrackSlotId = c.Int(),
                        Speaker_SpeakerId = c.Int(),
                        Attendee_AttendeeId = c.Int(),
                    })
                .PrimaryKey(t => t.SessionId)
                .ForeignKey("dbo.TrackSlots", t => t.Slot_TrackSlotId)
                .ForeignKey("dbo.Speakers", t => t.Speaker_SpeakerId)
                .ForeignKey("dbo.Attendees", t => t.Attendee_AttendeeId)
                .Index(t => t.Slot_TrackSlotId)
                .Index(t => t.Speaker_SpeakerId)
                .Index(t => t.Attendee_AttendeeId);
            
            CreateTable(
                "dbo.TrackSlots",
                c => new
                    {
                        TrackSlotId = c.Int(nullable: false, identity: true),
                        Slot_SessionSlotId = c.Int(),
                        Track_TrackId = c.Int(),
                    })
                .PrimaryKey(t => t.TrackSlotId)
                .ForeignKey("dbo.SessionSlots", t => t.Slot_SessionSlotId)
                .ForeignKey("dbo.Tracks", t => t.Track_TrackId)
                .Index(t => t.Slot_SessionSlotId)
                .Index(t => t.Track_TrackId);
            
            CreateTable(
                "dbo.SessionSlots",
                c => new
                    {
                        SessionSlotId = c.Int(nullable: false, identity: true),
                        SessionStart = c.DateTime(nullable: false),
                        SessionEnd = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SessionSlotId);
            
            CreateTable(
                "dbo.Tracks",
                c => new
                    {
                        TrackId = c.Int(nullable: false, identity: true),
                        TrackName = c.String(),
                        Event_EventId = c.Int(),
                        Room_RoomId = c.Int(),
                    })
                .PrimaryKey(t => t.TrackId)
                .ForeignKey("dbo.Events", t => t.Event_EventId)
                .ForeignKey("dbo.Rooms", t => t.Room_RoomId)
                .Index(t => t.Event_EventId)
                .Index(t => t.Room_RoomId);
            
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        RoomId = c.Int(nullable: false, identity: true),
                        RoomName = c.String(),
                        Floor = c.Int(nullable: false),
                        HandicapAccesible = c.Boolean(nullable: false),
                        Bulding_BuildingId = c.Int(),
                    })
                .PrimaryKey(t => t.RoomId)
                .ForeignKey("dbo.Buildings", t => t.Bulding_BuildingId)
                .Index(t => t.Bulding_BuildingId);
            
            CreateTable(
                "dbo.Buildings",
                c => new
                    {
                        BuildingId = c.Int(nullable: false, identity: true),
                        BuildingName = c.String(),
                        BuildingSortName = c.String(),
                        BuildingIdentifier = c.String(),
                        NumberOfFloors = c.Int(nullable: false),
                        HasElevator = c.Int(nullable: false),
                        HasChairLift = c.Int(nullable: false),
                        HandicapAccesible = c.Boolean(nullable: false),
                        Location_LocationId = c.Int(),
                    })
                .PrimaryKey(t => t.BuildingId)
                .ForeignKey("dbo.Locations", t => t.Location_LocationId)
                .Index(t => t.Location_LocationId);
            
            CreateTable(
                "dbo.Speakers",
                c => new
                    {
                        SpeakerId = c.Int(nullable: false, identity: true),
                        SpeakerName = c.String(),
                        SpeakerImage = c.String(),
                        Company = c.String(),
                        CompanyImage = c.String(),
                        SpeakerBio = c.String(),
                    })
                .PrimaryKey(t => t.SpeakerId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(maxLength: 255),
                        LastName = c.String(maxLength: 255),
                        Email = c.String(maxLength: 256),
                        ImageURL = c.String(),
                        Website = c.String(),
                        Facebook = c.String(),
                        Twitter = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Sessions", "Attendee_AttendeeId", "dbo.Attendees");
            DropForeignKey("dbo.Sessions", "Speaker_SpeakerId", "dbo.Speakers");
            DropForeignKey("dbo.Sessions", "Slot_TrackSlotId", "dbo.TrackSlots");
            DropForeignKey("dbo.TrackSlots", "Track_TrackId", "dbo.Tracks");
            DropForeignKey("dbo.Tracks", "Room_RoomId", "dbo.Rooms");
            DropForeignKey("dbo.Rooms", "Bulding_BuildingId", "dbo.Buildings");
            DropForeignKey("dbo.Buildings", "Location_LocationId", "dbo.Locations");
            DropForeignKey("dbo.Tracks", "Event_EventId", "dbo.Events");
            DropForeignKey("dbo.TrackSlots", "Slot_SessionSlotId", "dbo.SessionSlots");
            DropForeignKey("dbo.Events", "Attendee_AttendeeId", "dbo.Attendees");
            DropForeignKey("dbo.Events", "Location_LocationId", "dbo.Locations");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Buildings", new[] { "Location_LocationId" });
            DropIndex("dbo.Rooms", new[] { "Bulding_BuildingId" });
            DropIndex("dbo.Tracks", new[] { "Room_RoomId" });
            DropIndex("dbo.Tracks", new[] { "Event_EventId" });
            DropIndex("dbo.TrackSlots", new[] { "Track_TrackId" });
            DropIndex("dbo.TrackSlots", new[] { "Slot_SessionSlotId" });
            DropIndex("dbo.Sessions", new[] { "Attendee_AttendeeId" });
            DropIndex("dbo.Sessions", new[] { "Speaker_SpeakerId" });
            DropIndex("dbo.Sessions", new[] { "Slot_TrackSlotId" });
            DropIndex("dbo.Events", new[] { "Attendee_AttendeeId" });
            DropIndex("dbo.Events", new[] { "Location_LocationId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Speakers");
            DropTable("dbo.Buildings");
            DropTable("dbo.Rooms");
            DropTable("dbo.Tracks");
            DropTable("dbo.SessionSlots");
            DropTable("dbo.TrackSlots");
            DropTable("dbo.Sessions");
            DropTable("dbo.Locations");
            DropTable("dbo.Events");
            DropTable("dbo.Attendees");
        }
    }
}
