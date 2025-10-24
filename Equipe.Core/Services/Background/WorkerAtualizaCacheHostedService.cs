using System.Text;
using System.Text.Json;
using Equipe.Core.Models;
using Equipe.Infrastructure.Cache;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Equipe.Core.Services.Background
{
    public class WorkerAtualizaCacheHostedService: BackgroundService
    {
        private readonly ConnectionFactory _factory;
        private readonly IServiceProvider _serviceProvider;

        public WorkerAtualizaCacheHostedService(IServiceProvider serviceProvider)
        {
            _factory = new ConnectionFactory() { HostName = "localhost" }; // host do RabbitMQ
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var conexao = _factory.CreateConnection();
            var canal = conexao.CreateModel();

            canal.QueueDeclare(queue: "atualizar-cache", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumidor = new EventingBasicConsumer(canal);
            consumidor.Received += async (modelo, ea) =>
            {
                using var scope = _serviceProvider.CreateScope();
                var cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();

                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var equipe = JsonSerializer.Deserialize<EquipeDto>(json);

                if (equipe != null)
                {
                    var cacheKey = $"atleta_{equipe.NomeEquipe?.ToLower()}";
                    await cacheService.SetAsync(cacheKey, equipe, TimeSpan.FromMinutes(10));
                    Console.WriteLine($"[WORKER] Cache atualizado para {equipe.NomeEquipe}");
                }
            };

            canal.BasicConsume(queue: "atualizar-cache", autoAck: true, consumer: consumidor);

            Console.WriteLine("[WORKER] Worker iniciado. Aguardando mensagens...");
            return Task.CompletedTask;
        }
    }
}