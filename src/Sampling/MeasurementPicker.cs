namespace Sampling;

public interface IMeasurementPicker
{
    Measurement? PickLastOrDefaultFromInterval(
        IEnumerable<Measurement> measurements,
        DateTime startOfInterval,
        DateTime endOfInterval);
}

public class MeasurementPicker
{
    public Measurement? PickLastOrDefaultFromInterval(
        IEnumerable<Measurement> measurements,
        DateTime startOfInterval,
        DateTime endOfInterval) =>
        measurements?.LastOrDefault(
            measurement => measurement.Time > startOfInterval && measurement.Time <= endOfInterval);
}