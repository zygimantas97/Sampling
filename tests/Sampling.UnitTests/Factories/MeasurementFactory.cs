using AutoFixture;

namespace Sampling.UnitTests.Factories;

internal static class MeasurementFactory
{
    public static Measurement CreateMeasurementWithTime(DateTime time)
    {
        var measurement = new Fixture().Create<Measurement>();
        measurement.Time = time;
        return measurement;
    }

    public static IEnumerable<Measurement> CreateMeasurementsWithType(MeasurementType type)
    {
        var measurements = new Fixture().Create<List<Measurement>>();
        measurements.ForEach(measurement => measurement.Type = type);
        return measurements;
    }
}