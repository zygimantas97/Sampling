namespace Sampling;

public interface IMeasurementFilter
{
    IEnumerable<Measurement> FilterMeasurementsAfter(
        IEnumerable<Measurement> measurements,
        DateTime thresholdTime);
}

public class MeasurementFilter : IMeasurementFilter
{
    public IEnumerable<Measurement> FilterMeasurementsAfter(
        IEnumerable<Measurement> measurements,
        DateTime thresholdTime) =>
        measurements.Where(measurement => measurement.Time > thresholdTime);
}