using MassTransit;
using EmailService;

var builder = WebApplication.CreateBuilder(args);

//for datetime with time zone issue
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

//for db context

builder.Services.AddScoped<IDataContext, PostgresDataContext>();

// for message bus
builder.Services.AddMassTransit(x =>
            {
           
                x.AddConsumer<QueryConsumer>();

                x.SetKebabCaseEndpointNameFormatter();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(builder.Configuration["RabbitMq:host"],"/",h=>{
                        h.Username(builder.Configuration["RabbitMq:user"]);
                        h.Password(builder.Configuration["RabbitMq:password"]);
                    });
                    cfg.ConfigureEndpoints(context);
                });


                x.AddRequestClient<DataServiceContract>();
            

            }).AddMassTransitHostedService();




// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
