var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("user", secret: true);
var password = builder.AddParameter("password", secret: true);

var db = builder.AddPostgres("db", username, password)
    .WithEnvironment("POSTGRES_DB", "Postgres")
    .WithDataVolume();

var postgres = db.AddDatabase("Postgres");

var webapi = builder.AddProject<Projects.WebApi>("webapi")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
    // .WithEnvironment("AWS:Sqs:ServiceUrl", localstack.GetEndpoint("http"))
    .WithReference(postgres)
    .WithExternalHttpEndpoints();

// builder.AddNpmApp("webapp", "../webapp", "dev")
//     .WithReference(webapi)
//     .WithEnvironment("BROWSER", "none") // Disable opening browser on npm start
//     .WithHttpEndpoint(env: "PORT", port: 3000)
//     .WithExternalHttpEndpoints()
//     .PublishAsDockerFile();

// var localstack = builder
//     .AddContainer("localstack", "localstack/localstack")
//     .WithHttpEndpoint(port: 4566, targetPort: 4566, "http")
//     .WithEnvironment("AWS_DEFAULT_REGION", "us-east-1")
//     .WithEnvironment("AWS_ACCESS_KEY_ID", "key")
//     .WithEnvironment("AWS_SECRET_ACCESS_KEY", "secret")
//     .WithEnvironment("SERVICES", "sqs,s3")
//     .WithEnvironment("DOCKER_HOST", "unix:///var/run/docker.sock")
//     .WithEnvironment("PERSISTENCE", "1")
//     .WithBindMount("../../infra/localstack/volume", "/var/lib/localstack", isReadOnly: false)
//     .WithBindMount("../../infra/localstack/aws", "/etc/localstack/init/ready.d", isReadOnly: true);

builder.Build().Run();