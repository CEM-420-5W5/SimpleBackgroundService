using BackgroundService.Data;
using BackgroundService.Hubs;
using BackgroundService.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BackgroundService.Services
{
    public class Spammer : Microsoft.Extensions.Hosting.BackgroundService
    {
        public const int INITIAL_DELAY = 5 * 1000;
        
        private IHubContext<SpammerHub> _spammerHub;

        private IServiceScopeFactory _serviceScopeFactory;
        private int _delay = INITIAL_DELAY;

        public Spammer(IHubContext<SpammerHub> spammerHub, IServiceScopeFactory serviceScopeFactory)
        {
            _spammerHub = spammerHub;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Spam(CancellationToken stoppingToken)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                BackgroundServiceContext dbContext =
                    scope.ServiceProvider.GetRequiredService<BackgroundServiceContext>();

                List<Message> messages = await dbContext.Message.ToListAsync();

                if(messages.Count > 0)
                    await _spammerHub.Clients.All.SendAsync("Spam", messages.Select(m => m.Texte), stoppingToken);
            }
        }

        public void ChangeDelay(int delayInSeconds)
        {
            if(delayInSeconds > 0)
            {
                _delay = delayInSeconds * 1000;
            }
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_delay, stoppingToken);
                await Spam(stoppingToken);
            }
        }
    }
}
