namespace TCPChat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _Chats_Clients : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Chats",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        chatName = c.String(),
                        AdminId = c.String(),
                        AdminName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ClientObjects",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        userName = c.String(maxLength: 254, unicode: false),
                        userPass = c.String(maxLength: 254, unicode: false),
                        userLogin = c.String(maxLength: 254, unicode: false),
                        Usertype = c.String(maxLength: 254, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ClientObjectChats",
                c => new
                    {
                        ClientObject_Id = c.String(nullable: false, maxLength: 128),
                        Chat_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ClientObject_Id, t.Chat_Id })
                .ForeignKey("dbo.ClientObjects", t => t.ClientObject_Id, cascadeDelete: true)
                .ForeignKey("dbo.Chats", t => t.Chat_Id, cascadeDelete: true)
                .Index(t => t.ClientObject_Id)
                .Index(t => t.Chat_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClientObjectChats", "Chat_Id", "dbo.Chats");
            DropForeignKey("dbo.ClientObjectChats", "ClientObject_Id", "dbo.ClientObjects");
            DropIndex("dbo.ClientObjectChats", new[] { "Chat_Id" });
            DropIndex("dbo.ClientObjectChats", new[] { "ClientObject_Id" });
            DropTable("dbo.ClientObjectChats");
            DropTable("dbo.ClientObjects");
            DropTable("dbo.Chats");
        }
    }
}
