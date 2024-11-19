using BackgroundService.Data;
using BackgroundService.Models;
using BackgroundService.Services;
using Microsoft.AspNetCore.SignalR;

namespace BackgroundService.Hubs
{
    public class SpammerHub : Hub
    {
        private Spammer _spammer;
        private BackgroundServiceContext _dbContext;

        public SpammerHub(Spammer spammer, BackgroundServiceContext backgroundServiceContext)
        {
            _spammer = spammer;
            _dbContext = backgroundServiceContext;
        }

        public async Task ClearAllMessages()
        {
            _dbContext.RemoveRange(_dbContext.Message);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddMessage(string texte)
        {
            Message m = new Message()
            {
                Texte = texte
            };
            await _dbContext.AddAsync(m);
            await _dbContext.SaveChangesAsync();
        }

        public async Task ChangeDelay(int delayInSeconds)
        {
            _spammer.ChangeDelay(delayInSeconds);
        }
    }
}
