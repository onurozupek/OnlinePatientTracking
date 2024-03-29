﻿using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using AppointmentSystemMicroservice.Entities;
using Newtonsoft.Json;
using ManagementMicroservice.Entities;
using ManagementMicroservice.Repositories;

namespace ManagementMicroservice.Services;

public class MessageListener : BackgroundService
{
    private readonly ILogger<MessageListener> _logger;
    private readonly List<string> _queueNames;
    private readonly IGenericRepository<Doctor> _genericRepository;

    public MessageListener(ILogger<MessageListener> logger, GenericRepository<Doctor> genericRepository)
    {
        _logger = logger;
        _genericRepository = genericRepository;
        _queueNames = new List<string> { "appointmentCreated", "appointmentRemoved" }; // Kuyruk isimlerini buraya ekleyin
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "user",
            Password = "password",
            VirtualHost = "/",
            DispatchConsumersAsync = true
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        foreach (var queueName in _queueNames)
        {
            channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation("Received message from queue {0}: {1}", queueName, message);
                ProcessMessage(queueName,message); // Handle the received message here
                await Task.Yield();
            };

            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    private async void ProcessMessage(string sender, string message)
    {
        var deserializedMessage = JsonConvert.DeserializeObject<Appointment>(message);
        if (deserializedMessage != null)
        {
            if (sender == "appointmentCreated")
            {
                var entity = await _genericRepository.GetByIdAsync(deserializedMessage.DoctorId);
                if (entity != null)
                {
                    entity.Appointments++;
                    _genericRepository.Update(entity);
                }
            }
            if (sender == "appointmentRemoved")
            {
                if (deserializedMessage.DoctorId != 0)
                {
                    var entity = await _genericRepository.GetByIdAsync(deserializedMessage.DoctorId);
                    if (entity != null)
                    {
                        entity.Appointments--;
                        _genericRepository.Update(entity);
                    }
                }
            }
        }
    }
}