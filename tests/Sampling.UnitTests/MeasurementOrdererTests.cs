using FluentAssertions;
using Objectivity.AutoFixture.XUnit2.AutoMoq.Attributes;

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
        var shuffledMeasurements = measurements.OrderBy(_ => Guid.NewGuid());
        var orderedMeasurements = measurements.OrderBy(measurement => measurement.Time);

        // Act
        var measurementsAfterOrdering = measurementOrderer.OrderByTimeAscending(measurements);

        // Assert
        measurementsAfterOrdering.Should().BeEquivalentTo(orderedMeasurements, o => o.WithStrictOrdering());
    }
}