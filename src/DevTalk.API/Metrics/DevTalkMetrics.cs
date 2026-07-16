using System.Diagnostics.Metrics;

namespace DevTalk.API.Metrics;

public static class DevTalkMetrics
{
    public static readonly Meter Meter = new("DevTalk", "1.0.0");

    public static readonly Counter<long> RequestCounter =
        Meter.CreateCounter<long>(
            "devtalk.requests.count",
            description: "Number of DevTalk requests");
}
