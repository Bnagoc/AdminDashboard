namespace Api.Data
{
    public class Seed
    {
        private RandomGenerator _random = new();

        private int clientsCount = 3;
        private int paymentsCount = 5;
        private int ratesCount = 1;
        private int defaultRateValue = 10;

        public async Task CheckAndFillWithDefaultEntytiesDatabase(IServiceProvider serviceProvider)
        {
            using var di = serviceProvider.CreateScope();

            await InitAdmin(di.ServiceProvider);
            await FillClients(di.ServiceProvider);
            await FillRates(di.ServiceProvider);
            await FillPayments(di.ServiceProvider);
        }

        private async Task InitAdmin(IServiceProvider service)
        {
            var repository = service.GetRequiredService<AppDbContext>();

            if (!repository.Users.Any())
            {
                await repository.Users.AddAsync(new User
                {
                    Email = "admin@mirra.dev",
                    Password = "admin123"
                });

                await repository.SaveChangesAsync();

            }
        }

        private async Task FillClients(IServiceProvider service)
        {
            var repository = service.GetRequiredService<AppDbContext>();

            if (!repository.Clients.Any())
            {
                for (int i = 1; i <= clientsCount; i++) 
                {
                    var clientName = $"{nameof(Client)} #{i}";
                    var clientEmail = $"{nameof(Client)}{i}@gmail.com";
                    var clientBalance = i * _random.GenerateRandomNumber(100);

                    await repository.Clients.AddAsync(new Client
                    {
                        Name = clientName,
                        Email = clientEmail,
                        Balance = clientBalance
                    });
                }

                await repository.SaveChangesAsync();

            }

        }
        private async Task FillRates(IServiceProvider service)
        {
            var repository = service.GetRequiredService<AppDbContext>();

            if (!repository.Rates.Any())
            {
                for (int i = 1; i <= ratesCount; i++)
                {
                    await repository.Rates.AddAsync(new Rate
                    {
                        Value = defaultRateValue, 
                    });
                }

                await repository.SaveChangesAsync();

            }

        }
        private async Task FillPayments(IServiceProvider service)
        {
            var repository = service.GetRequiredService<AppDbContext>();

            if (!repository.Payments.Any())
            {
                for (int i = 1; i <= paymentsCount;) 
                {
                    var client = await repository.Clients.FindAsync(_random.GenerateRandomNumber(clientsCount));
                    if (client is null)
                    {
                        continue;
                    }
                    var rate = await repository.Rates
                        .OrderByDescending(r => r.CreatedAtUtc)
                        .LastAsync();

                    var paymentAmount = _random.GenerateRandomNumber(((int)client.Balance));
                    if (paymentAmount == 0)
                    {
                        continue;
                    }

                    await repository.Payments.AddAsync(new Payment
                    {
                        Amount = paymentAmount,
                        ClientId = client.Id,
                        RateId = rate.Id,
                    });

                    client.Balance -= paymentAmount;

                    i++;
                }

                await repository.SaveChangesAsync();
            }

        }

        
    }
}
