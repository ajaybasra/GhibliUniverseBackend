namespace GhibliUniverse.Core.Utils;

public static class Configuration
{
    public static string GetDbConnectionString()
    {
        var root = AppDomain.CurrentDomain.BaseDirectory;
        var localEnvironmentFilePath = root + "/Utils/env.local";
        var isInDockerCompose = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOCKER_COMPOSE"));

        if (!isInDockerCompose && File.Exists(localEnvironmentFilePath))
        {
            DotEnv.Load(localEnvironmentFilePath);
        }

        var dbName = Environment.GetEnvironmentVariable("DB_NAME");
        var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
        var port = Environment.GetEnvironmentVariable("DB_PORT");
        var username = Environment.GetEnvironmentVariable("DB_USER");
        var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

        return $"Database={dbName};Host={dbHost};Username={username};Password={password};Port={port}";
    }
}