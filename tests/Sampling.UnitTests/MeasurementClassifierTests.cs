using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Objectivity.AutoFixture.XUnit2.AutoMoq.Attributes;
using Sampling.UnitTests.Factories;
using Sampling.UnitTests.TestExtensions;

namespace Sampling.UnitTests;

public class MeasurementClassifierTests
{
    [Theory]
    [AutoMockData]
    public void ClassifyByType_ReturnsMeasurementsClassifiedByType(
        uint countOfMeasurements,
        MeasurementClassifier measurementClassifier)
    {
        // Arrange
        var temperatureMeasurements = MeasurementFactory.CreateMeasurementsWithType(MeasurementType.Temperature);
        var heartRateMeasurements = MeasurementFactory.CreateMeasurementsWithType(MeasurementType.HeartRate);
        var spO2Measurements = MeasurementFactory.CreateMeasurementsWithType(MeasurementType.SpO2);
        var measurements = temperatureMeasurements.Concat(heartRateMeasurements).Concat(spO2Measurements).Shuffle();

        // Act
        var measurementsAfterClasification = measurementClassifier.ClassifyByType(measurements);

        // Assert
        using var _ = new AssertionScope();
        measurementsAfterClasification.Should().NotBeNull();
        measurementsAfterClasification.Count().Should().Be(3);
        measurementsAfterClasification[MeasurementType.Temperature].Should().BeEquivalentTo(temperatureMeasurements);
        measurementsAfterClasification[MeasurementType.HeartRate].Should().BeEquivalentTo(heartRateMeasurements);
        measurementsAfterClasification[MeasurementType.SpO2].Should().BeEquivalentTo(spO2Measurements);
    }
}