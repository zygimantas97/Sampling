using Sampling.Model;

namespace Sampling;

public interface IMeasurementPicker
{
    Measurement? PickLastOrDefaultFromInterval(
        IEnumerable<Measurement> measurements,
        DateTime startOfInterval,
        DateTime endOfInterval);
}

public class MeasurementPicker : IMeasurementPicker
{
    public Measurement? PickLastOrDefaultFromInterval(
        IEnumerable<Measurement> measurements,
        DateTime startOfInterval,
        DateTime endOfInterval)
    {
        var measurement = measurements?.LastOrDefault(
            measurement => measurement.Time > startOfInterval && measurement.Time <= endOfInterval);

        return measurement != null ? CreatePickedMeasurement(endOfInterval, measurement) : null;
    }

    private static Measurement? CreatePickedMeasurement(DateTime endOfInterval, Measurement measurement) => new Measurement
    {
        Time = endOfInterval,
        Type = measurement.Type,
        Value = measurement.Value
    };
}