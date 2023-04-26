using Quartz;
using Stocks.Services;
using Stocks.WebService.Jobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();
builder.Services.AddStocksServices();

builder.Services.AddTransient<StocksDiffJob>();

// Configure Quartz jobs
builder.Services.Configure<QuartzOptions>(options =>
{
    options.Scheduling.IgnoreDuplicates = true; // default: false
    options.Scheduling.OverWriteExistingData = true; // default: true
});
builder.Services.AddQuartz(q =>
{
    q.SchedulerId = "StocksScheduler";
    q.UseMicrosoftDependencyInjectionJobFactory();
    q.UseSimpleTypeLoader();
    q.UseInMemoryStore();
    q.UseDefaultThreadPool(tp =>
    {
        tp.MaxConcurrency = 1;
    });

    q.ScheduleJob<StocksDiffJob>(trigger => trigger
        .WithIdentity("Stocks Diff Job")
        .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(5)))
        .WithDailyTimeIntervalSchedule(x => x.WithInterval(1, IntervalUnit.Day))
        .WithDescription("Creates a difference between stocks in two dates and sends an email")
    );
});
builder.Services.AddQuartzHostedService(options =>
{
    options.WaitForJobsToComplete = true;
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.Run();
