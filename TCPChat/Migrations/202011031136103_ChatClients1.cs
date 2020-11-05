namespace TCPChat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChatClients1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClientParamChats",
                c => new
                    {
                        ClientParam_Id = c.Int(nullable: false),
                        Chat_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ClientParam_Id, t.Chat_Id })
                .ForeignKey("dbo.ClientParams", t => t.ClientParam_Id, cascadeDelete: true)
                .ForeignKey("dbo.Chats", t => t.Chat_Id, cascadeDelete: true)
                .Index(t => t.ClientParam_Id)
                .Index(t => t.Chat_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClientParamChats", "Chat_Id", "dbo.Chats");
            DropForeignKey("dbo.ClientParamChats", "ClientParam_Id", "dbo.ClientParams");
            DropIndex("dbo.ClientParamChats", new[] { "Chat_Id" });
            DropIndex("dbo.ClientParamChats", new[] { "ClientParam_Id" });
            DropTable("dbo.ClientParamChats");
        }
    }
}
