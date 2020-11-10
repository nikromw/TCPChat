namespace TCPChat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _ChatClient : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ClientParamChats", "Chat_Id", "dbo.Chats");
            DropIndex("dbo.ClientParamChats", new[] { "Chat_Id" });
            DropPrimaryKey("dbo.Chats");
            DropPrimaryKey("dbo.ClientParamChats");
            AlterColumn("dbo.Chats", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.ClientParamChats", "Chat_Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Chats", "Id");
            AddPrimaryKey("dbo.ClientParamChats", new[] { "ClientParam_Id", "Chat_Id" });
            CreateIndex("dbo.ClientParamChats", "Chat_Id");
            AddForeignKey("dbo.ClientParamChats", "Chat_Id", "dbo.Chats", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClientParamChats", "Chat_Id", "dbo.Chats");
            DropIndex("dbo.ClientParamChats", new[] { "Chat_Id" });
            DropPrimaryKey("dbo.ClientParamChats");
            DropPrimaryKey("dbo.Chats");
            AlterColumn("dbo.ClientParamChats", "Chat_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Chats", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.ClientParamChats", new[] { "ClientParam_Id", "Chat_Id" });
            AddPrimaryKey("dbo.Chats", "Id");
            CreateIndex("dbo.ClientParamChats", "Chat_Id");
            AddForeignKey("dbo.ClientParamChats", "Chat_Id", "dbo.Chats", "Id", cascadeDelete: true);
        }
    }
}
