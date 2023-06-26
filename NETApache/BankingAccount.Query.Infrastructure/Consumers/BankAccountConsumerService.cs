using Banking.Account.Query.Application.Contracts.Persistence;
using Banking.Account.Query.Application.Models;
using Banking.Account.Query.Domain;
using Banking.Cqrs.Core.Event;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Banking.Account.Query.Infrastructure.Consumers
{
    public class BankAccountConsumerService : IHostedService
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        public KafkaSettings KafkaSettings { get; }

        public BankAccountConsumerService(IServiceScopeFactory factory)
        {
            _bankAccountRepository = factory.CreateScope().ServiceProvider.GetRequiredService<IBankAccountRepository>();
            KafkaSettings = (factory.CreateScope().ServiceProvider.GetRequiredService<IOptions<KafkaSettings>>()).Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig()
            {
                GroupId = KafkaSettings.GroupId,
                BootstrapServers = $"{KafkaSettings.Hostname}:{KafkaSettings.Port}",
                AutoOffsetReset = AutoOffsetReset.Earliest,
            };

            try
            {
                using var consumerBuilder = new ConsumerBuilder<Ignore, string>(config).Build();

                var bankTopics = new string[]
                {
                    typeof(AccountOpenedEvent).Name,
                    typeof(AccountClosedEvent).Name,
                    typeof(FundsDepositedEvent).Name,
                    typeof (FundsWithDrawnEvent).Name,
                };

                consumerBuilder.Subscribe(bankTopics);
                var cancelToken = new CancellationTokenSource();

                try
                {
                    while (true)
                    {
                        var consumer = consumerBuilder.Consume(cancelToken.Token);

                        if (consumer.Topic == typeof(AccountOpenedEvent).Name)
                        {
                            var accountOpenedEvent = JsonSerializer.Deserialize<AccountOpenedEvent>(consumer.Message.Value);
                            var bankAccount = new BankAccount()
                            {
                                Identifier = accountOpenedEvent.Id,
                                AccountHolder = accountOpenedEvent.AccountHandler,
                                AccountType = accountOpenedEvent.AccountType,
                                Balance = accountOpenedEvent.OpeningBalance,
                                CreationDate = accountOpenedEvent.CreatedDate
                            };

                            await _bankAccountRepository.AddAsync(bankAccount);
                        }

                        if (consumer.Topic == typeof(AccountClosedEvent).Name)
                        {
                            var accountClosedEvent = JsonSerializer.Deserialize<AccountClosedEvent>(consumer.Message.Value);
                            await _bankAccountRepository.DeleteByIdentifier(accountClosedEvent.Id);
                        }

                        if (consumer.Topic == typeof(FundsDepositedEvent).Name)
                        {
                            var fundsDepositEvent = JsonSerializer.Deserialize<FundsDepositedEvent>(consumer.Message.Value);
                            var bankAccount = new BankAccount
                            {
                                Identifier = fundsDepositEvent.Id,
                                Balance = fundsDepositEvent.Amount,
                            };

                            await _bankAccountRepository.DepositBankAccountByIdentifier(bankAccount);
                        }

                        if (consumer.Topic == typeof(FundsWithDrawnEvent).Name)
                        {
                            var fundsDepositEvent = JsonSerializer.Deserialize<FundsWithDrawnEvent>(consumer.Message.Value);
                            var bankAccount = new BankAccount
                            {
                                Identifier = fundsDepositEvent.Id,
                                Balance = fundsDepositEvent.Amount,
                            };

                            await _bankAccountRepository.WithdrawnBankAccountByIdentifier(bankAccount);
                        }
                    }
                }
                catch (OperationCanceledException ex)
                {
                    consumerBuilder.Close();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            await Task.Delay(0);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(0);
        }
    }
}
