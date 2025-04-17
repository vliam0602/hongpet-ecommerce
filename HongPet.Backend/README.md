# HongPet - Backend

## EF migration code first
- To apply the latest migrations to physical database, run this command (from <strong>.\src folder</strong>)
    ```
    dotnet ef database update -s HongPet.WebApi -p HongPet.Migrators.MSSQL
    ```

- To add new migrations, run this command (from <strong>.\src folder</strong>)
    ```
    dotnet ef migrations add MigrationName -s HongPet.WebApi -p HongPet.Migrators.MSSQL
    ```
## Tech stack
- Src: ASP.Net Api 9, EF Core, MSSQL Server, Swagger/OpenApi,
- Test: xUnit, Moq, AutoFixture,
