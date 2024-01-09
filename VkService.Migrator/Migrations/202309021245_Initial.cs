using FluentMigrator;

namespace VkService.Migrator.Migrations;

[Migration(1)]
public class Initial : Migration {
    
    public override void Up()
    {
        const string sql = 
            """
            CREATE TABLE IF NOT EXISTS Users 
            (
            	Id INTEGER,
            	Avatar Text,
            	Name Text,
            	PRIMARY KEY(Id)
            );

            CREATE TABLE IF NOT EXISTS Messages(
                Id Integer,
                Date DateTime,
                OwnerId Integer,
                RepostedFrom integer,
                Primary Key(Id, OwnerId) 
            );

            CREATE VIRTUAL TABLE IF NOT EXISTS messages_search USING fts5(Text);

            CREATE TABLE IF NOT EXISTS BannedGroups (
              Id Integer,
              PRIMARY KEY(Id) 
            );
            """;
        
        Execute.Sql(sql);
    }

    public override void Down()
    {
        
    }
}
