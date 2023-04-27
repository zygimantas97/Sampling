using Sampling.Model;

namespace Sampling.UnitTests;

public class MeasurementOrdererTests
{
    [Theory]
    [AutoMockData]
    public void OrderByTimeAscending_WhenMeasurementsNull_ReturnsNull(
        MeasurementOrderer measurementOrderer)
    {
        // Arrange
        IEnumerable<Measurement> measurements = null;

        // Act
        var measurementsAfterOrdering = measurementOrderer.OrderByTimeAscending(measurements);

        // Assert
        measurementsAfterOrdering.Should().BeNull();
    }

    [Theory]
    [AutoMockData]
    public void OrderByTimeAscending_WhenMeasurementsEmpty_ReturnsEmptyEnumerable(
        MeasurementOrderer measurementOrderer)
    {
        // Arrange
        var measurements = Enumerable.Empty<Measurement>();

        // Act
        var measurementsAfterOrdering = measurementOrderer.OrderByTimeAscending(measurements);

        // Assert
        measurementsAfterOrdering.Should().NotBeNull();
        measurementsAfterOrdering.Should().BeEmpty();
    }

    [Theory]
    [AutoMockData]
    public void OrderByTimeAscending_WhenMeasurementsNotEmpty_ReturnsMeasurementsOrderedByTimeAscending(
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