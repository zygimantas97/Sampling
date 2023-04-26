namespace Sampling.UnitTests;

public class MeasurementFilterTests
{
    [Theory]
    [AutoMockData]
    public void FilterMeasurementsAfter_ReturnsMeasurementsAfterThresholdTime(
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
        var filteredMeasurements = measurementFilter.FilterMeasurementsAfter(measurements, thresholdTime);

        // Assert
        using var _ = new AssertionScope();
        filteredMeasurements.Should().NotBeNull();
        filteredMeasurements.Count().Should().Be(1);
        filteredMeasurements.Should().Contain(measurementAfterThresholdTime);
        filteredMeasurements.Should().NotContain(measurementOnThresholdTime);
        filteredMeasurements.Should().NotContain(measurementBeforeThresholdTime);
    }
}