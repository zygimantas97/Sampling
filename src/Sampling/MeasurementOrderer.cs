using Sampling.Model;

namespace Sampling;

public interface IMeasurementOrderer
{
    IEnumerable<Measurement>? OrderByTimeAscending(IEnumerable<Measurement> measurements);
}

public class MeasurementOrderer : IMeasurementOrderer
{
    public IEnumerable<Measurement>? OrderByTimeAscending(IEnumerable<Measurement> measurements) =>
        measurements?.OrderBy(measurement => measurement.Time);
}