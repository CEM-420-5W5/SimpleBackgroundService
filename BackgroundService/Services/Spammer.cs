using BackgroundService.Data;
using BackgroundService.Hubs;
using BackgroundService.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BackgroundService.Services
{
    public class Spammer : Microsoft.Extensions.Hosting.BackgroundService
    {
        public const int DELAY = 30 * 1000;
        
        private IHubContext<SpammerHub> _spammerHub;

        private IServiceScopeFactory _serviceScopeFactory;

        public Spammer(IHubContext<SpammerHub> spammerHub, IServiceScopeFactory serviceScopeFactory)
        {
            _spammerHub = spammerHub;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task EndRound(CancellationToken stoppingToken)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                BackgroundServiceContext dbContext =
                    scope.ServiceProvider.GetRequiredService<BackgroundServiceContext>();

                // TODO: Mettre à jour et sauvegarder le nbWinds des joueurs

                Message? message = await dbContext.Message.FirstOrDefaultAsync();

                await _spammerHub.Clients.All.SendAsync("Spam", message, stoppingToken);
            }
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(DELAY, stoppingToken);
                await EndRound(stoppingToken);
            }
        }
    }
}
