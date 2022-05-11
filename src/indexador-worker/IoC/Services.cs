using Elasticsearch.Net;
using Nest;

namespace indexador_worker.IoC
{
    internal static class Services
    {
        public static void ConfigureLocalServices(this IServiceCollection services)
        {
            services.AddSingleton<IElasticClient>(s =>
            {
                string cloudId = "demo_local_workspace:dXMtY2VudHJhbDEuZ2NwLmNsb3VkLmVzLmlvJDkzMDBjMWUzZGU0NTQ1YTE4YzkxYWRjOTk1ZWRmNjcxJDRjMDRiYjdlYWI0OTQ0NjM4ODA2MTcwMjQzNzBjMWU5";
                string username = "elastic";
                string password = "mddCG6tqYBqVH3gHrY02a1iG";

                var settings = new ConnectionSettings(cloudId, new BasicAuthenticationCredentials(username, password));

                return new ElasticClient(settings.DefaultIndex("demo"));
            });
        }
    }
}
