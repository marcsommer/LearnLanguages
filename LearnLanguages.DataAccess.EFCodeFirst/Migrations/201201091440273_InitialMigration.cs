namespace LearnLanguages.DataAccess.EFCodeFirst.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "PhraseDtoes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Text = c.String(),
                        LanguageId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "LanguageDtoes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("LanguageDtoes");
            DropTable("PhraseDtoes");
        }
    }
}
