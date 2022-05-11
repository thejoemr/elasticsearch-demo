using indexador_worker;
using indexador_worker.IoC;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.ConfigureLocalServices();
    })
    .Build();

await host.RunAsync();
