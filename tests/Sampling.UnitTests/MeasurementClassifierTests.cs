using Sampling.Model;

namespace Sampling.UnitTests;

public class MeasurementClassifierTests
{
    [Theory]
    [AutoMockData]
    public void ClassifyByType_WhenMeasurementsNull_ReturnsNull(
        MeasurementClassifier measurementClassifier)
    {
        // Arrange
        IEnumerable<Measurement> measurements = null;

        // Act
        var measurementsAfterClassification = measurementClassifier.ClassifyByType(measurements);

        // Assert
        measurementsAfterClassification.Should().BeNull();
    }

    [Theory]
    [AutoMockData]
    public void ClassifyByType_WhenMeasurementsEmpty_ReturnsEmptyDictionary(
        MeasurementClassifier measurementClassifier)
    {
        // Arrange
        var measurements = Enumerable.Empty<Measurement>();

        // Act
        var measurementsAfterClassification = measurementClassifier.ClassifyByType(measurements);

        // Assert
        measurementsAfterClassification.Should().NotBeNull();
        measurementsAfterClassification.Should().BeEmpty();
    }

    [Theory]
    [AutoMockData]
    public void ClassifyByType_WhenMeasurementsNotEmpty_ReturnsMeasurementsClassifiedByType(
        MeasurementClassifier measurementClassifier)
    {
        // Arrange
        var temperatureMeasurements = MeasurementFactory.CreateMeasurementsWithType(MeasurementType.Temperature);
        var heartRateMeasurements = MeasurementFactory.CreateMeasurementsWithType(MeasurementType.HeartRate);
        var spO2Measurements = MeasurementFactory.CreateMeasurementsWithType(MeasurementType.SpO2);
        var measurements = temperatureMeasurements.Concat(heartRateMeasurements).Concat(spO2Measurements).Shuffle();

        // Act
        var measurementsAfterClassification = measurementClassifier.ClassifyByType(measurements);

        // Assert
        using var _ = new AssertionScope();
        measurementsAfterClassification.Should().NotBeNull();
        measurementsAfterClassification.Count().Should().Be(3);
        measurementsAfterClassification[MeasurementType.Temperature].Should().BeEquivalentTo(temperatureMeasurements);
        measurementsAfterClassification[MeasurementType.HeartRate].Should().BeEquivalentTo(heartRateMeasurements);
        measurementsAfterClassification[MeasurementType.SpO2].Should().BeEquivalentTo(spO2Measurements);
    }
}