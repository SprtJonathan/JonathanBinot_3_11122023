# Projet 3 : Testez l'implémentation d'une nouvelle fonctionnalité .NET

## Prérequis :
MSSQL Developer 2019 ou Express 2019 doit être installé avec Microsoft SQL Server Management Studio (SSMS).

MSSQL : https://www.microsoft.com/fr-fr/sql-server/sql-server-downloads

SSMS : https://docs.microsoft.com/fr-fr/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16

*Remarque : les versions antérieures de MSSQL Server devraient fonctionner sans problèmes, mais elles n’ont pas été testées.

*Dans le projet P3AddNewFunctionalityDotNetCore, ouvrez le fichier appsettings.json.*

Vous avez la section ConnectionStrings qui définit les chaînes de connexion pour les 2 bases de données utilisées dans cette application.

      "ConnectionStrings":
      {
        "P3Referential": "Server=.;Database=P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a;Trusted_Connection=True;MultipleActiveResultSets=true",
        "P3Identity": "Server=.;Database=Identity;Trusted_Connection=True;MultipleActiveResultSets=true"
      }

**P3Referential** - chaîne de connexion à la base de données de l’application.

**P3Identity** - chaîne de connexion à la base de données des utilisateurs. Il s’agit d’une base de données indépendante, car une organisation utilise généralement plusieurs applications différentes. Au lieu
de laisser chaque application définir ses propres utilisateurs (et plusieurs identifiants et mots de passe différents pour un seul utilisateur), une organisation peut disposer d’une base de données utilisateur unique et utiliser des autorisations et des rôles pour définir les applications auxquelles l’utilisateur a accès. De cette façon, un utilisateur peut accéder à toutes les applications de l’organisation avec le même identifiant et le même mot de passe, mais il n’aura accès qu’aux bases de données et aux actions définies dans la base de données des utilisateurs.

Il existe des versions différentes de MSSQL (veuillez utiliser MSSQL pour ce projet et non une autre base de données). Lorsque vous configurez le serveur de base de données, diverses options modifient la configuration de sorte que les chaînes de connexion définies peuvent ne pas fonctionner.

Les chaînes de connexion définies dans le projet sont configurées pour MSSQL Server Standard 2019. L’installation n’a pas créé de nom d’instance, le serveur est donc simplement désigné par « . », qui désigne l’instance par défaut de MSSQL Server fonctionnant sur la machine actuelle. Pendant l’installation, c’est l’utilisateur intégré de Windows qui est configuré dans le serveur MSSQL par défaut.

Si vous avez installé MSSQL Express, la valeur à utiliser pour Server est très probablement .\SQLEXPRESS. Donc votre chaîne de connexion P3Referential serait :

    "P3Referential": "Server=.\SQLEXPRESS;Database=P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a;Trusted_Connection=True;MultipleActiveResultSets=true"
  
Si vous avez des difficultés à vous connecter, essayez d’abord de vous connecter à l’aide de Microsoft SQL Server Management Studio (assurez-vous que le type d’authentification est « Authentification Windows »), ou consultez le site https://sqlserver-help.com/2011/06/19/help-whats-my-sql-server-name/.
Si le problème persiste, demandez de l’aide à votre mentor.

## Points de vigilance :
Ne pas modifier la configuration de la base de données SQL, que ce soit via SSMS ou dans ces fichiers ci-dessous :
- IdentitySeedData.cs
- P3Referential.cs
- SeedData.cs
- les fichiers qui se trouvent dans le dossier “Migrations” (avec structure du nom de fichier Une date_Un nom, par exemple 20190127080942_InitialMigration.cs
  
