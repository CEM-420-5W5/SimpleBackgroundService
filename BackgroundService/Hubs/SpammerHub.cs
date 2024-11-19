using BackgroundService.Data;
using BackgroundService.Models;
using BackgroundService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

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

        public async Task SetMessage(string texte)
        {
            // On garde ça simple, il ne peut y avoir qu'un seul message dans le système
            Message? message = await _dbContext.Message.FirstOrDefaultAsync();
            if (message != null)
            {
                message.Texte = texte;
            }
            else
            {
                Message m = new Message()
                {
                    Texte = texte
                };
                await _dbContext.AddAsync(m);
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
