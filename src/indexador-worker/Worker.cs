using indexador_worker.Model;
using Nest;

namespace indexador_worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IElasticClient _esClient;

        public Worker(ILogger<Worker> logger, IElasticClient elasticClient)
        {
            _logger = logger;
            _esClient = elasticClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                var index = "products";

                var product = new Product
                {
                    ProductId = 1,
                    ProductName = "Demo Product",
                    Description = $"Product demo description",
                };

                //  Search the document to see if this exists
                var response = _esClient.Search<Product>(s => s.Index(index).Query(q => q.MatchAll()));
                if (!response.Documents.Any(x => x.ProductId == 1))
                {
                    //  Insert product detail document to index  
                    var responseRequest = await _esClient.IndexAsync(new IndexRequest<Product>(product, index), stoppingToken);
                }
                else
                {
                    //  Search the document to see if this exists
                    var item = response.Hits.FirstOrDefault(h => h.Source.ProductId == 1);
                    var searchId = item?.Id;
                    var responseRequest = await _esClient.UpdateAsync<Product>(searchId, doc =>
                    {
                        return doc.Index(index).Doc(new Product
                        {
                            ProductId = product.ProductId,
                            ProductName = product.ProductName,
                            Description = $"{product.Description} updated"
                        });
                    }, stoppingToken);
                }


                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}