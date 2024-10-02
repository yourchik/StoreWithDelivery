using Delivery.Infrastructure.Services.Implementations.Scheduler.Jobs;
using Delivery.Infrastructure.Services.Interfaces.Scheduler;
using Delivery.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Delivery.Infrastructure.Services.Implementations.Scheduler;

public class JobSchedulerService
(IHangFireService hangFireService, 
    ILogger<JobSchedulerService> logger, 
    IOptions<HangfireCronSettings> cronSettings)
{
    public void RegisterJobs()
    {
        hangFireService.Execute<GetNewOrdersJob>(
            job => job.RunAsync(default),
            cronSettings.Value.FetchNewOrdersJob
        );
    }
}