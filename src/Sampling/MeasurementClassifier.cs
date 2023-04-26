namespace Sampling;

public interface IMeasurementClassifier
{
    IDictionary<MeasurementType, IEnumerable<Measurement>> ClassifyByType(IEnumerable<Measurement> measurements);
}

public class MeasurementClassifier : IMeasurementClassifier
{
    public IDictionary<MeasurementType, IEnumerable<Measurement>> ClassifyByType(IEnumerable<Measurement> measurements) =>
        measurements
            .GroupBy(measurement => measurement.Type)
            .ToDictionary(
                group => group.Key,
                group => group.AsEnumerable());
}