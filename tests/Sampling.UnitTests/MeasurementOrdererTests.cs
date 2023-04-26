namespace Sampling.UnitTests;

public class MeasurementOrdererTests
{
    [Theory]
    [AutoMockData]
    public void OrderByTimeAscending_ReturnsMeasurementsOrderedByTimeAscending(
        IEnumerable<Measurement> measurements,
        MeasurementOrderer measurementOrderer)
    {
        // Arrange
        var shuffledMeasurements = measurements.Shuffle();
        var orderedMeasurements = measurements.OrderBy(measurement => measurement.Time);

        // Act
        var measurementsAfterOrdering = measurementOrderer.OrderByTimeAscending(measurements);

        // Assert
        measurementsAfterOrdering.Should().NotBeNull();
        measurementsAfterOrdering.Should().BeEquivalentTo(orderedMeasurements, o => o.WithStrictOrdering());
    }
}