using Sampling.Model;

namespace Sampling.UnitTests;

public class MeasurementFilterTests
{
    [Theory]
    [AutoMockData]
    public void FilterMeasurementsAfter_WhenMeasurementsNull_ReturnsNull(
        DateTime thresholdTime,
        MeasurementFilter measurementFilter)
    {
        // Arrange
        IEnumerable<Measurement> measurements = null;

        // Act
        var measurementsAfterFiltering = measurementFilter.FilterMeasurementsAfter(measurements, thresholdTime);

        // Assert
        measurementsAfterFiltering.Should().BeNull();
    }

    [Theory]
    [AutoMockData]
    public void FilterMeasurementsAfter_WhenMeasurementsEmpty_ReturnsEmptyEnumerable(
        DateTime thresholdTime,
        MeasurementFilter measurementFilter)
    {
        // Arrange
        var measurements = Enumerable.Empty<Measurement>();

        // Act
        var measurementsAfterFiltering = measurementFilter.FilterMeasurementsAfter(measurements, thresholdTime);

        // Assert
        measurementsAfterFiltering.Should().NotBeNull();
        measurementsAfterFiltering.Should().BeEmpty();
    }

    [Theory]
    [AutoMockData]
    public void FilterMeasurementsAfter_WhenMeasurementsNotEmpty_ReturnsMeasurementsAfterThresholdTime(
        DateTime thresholdTime,
        MeasurementFilter measurementFilter)
    {
        // Arrange
        var measurementBeforeThresholdTime = MeasurementFactory.CreateMeasurementWithTime(thresholdTime.AddSeconds(-1));
        var measurementOnThresholdTime = MeasurementFactory.CreateMeasurementWithTime(thresholdTime);
        var measurementAfterThresholdTime = MeasurementFactory.CreateMeasurementWithTime(thresholdTime.AddSeconds(1));
        var measurements = new List<Measurement>()
            { measurementBeforeThresholdTime, measurementOnThresholdTime, measurementAfterThresholdTime };

        // Act
        var measurementsAfterFiltering = measurementFilter.FilterMeasurementsAfter(measurements, thresholdTime);

        // Assert
        using var _ = new AssertionScope();
        measurementsAfterFiltering.Should().NotBeNull();
        measurementsAfterFiltering.Count().Should().Be(1);
        measurementsAfterFiltering.Should().Contain(measurementAfterThresholdTime);
        measurementsAfterFiltering.Should().NotContain(measurementOnThresholdTime);
        measurementsAfterFiltering.Should().NotContain(measurementBeforeThresholdTime);
    }
}