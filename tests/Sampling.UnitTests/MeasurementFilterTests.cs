﻿using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Objectivity.AutoFixture.XUnit2.AutoMoq.Attributes;

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
        var measurementBeforeThresholdTime = CreateMeasurement(thresholdTime.AddSeconds(-1));
        var measurementOnThresholdTime = CreateMeasurement(thresholdTime);
        var measurementAfterThresholdTime = CreateMeasurement(thresholdTime.AddSeconds(1));
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

    private Measurement CreateMeasurement(DateTime time)
    {
        var measurement = new Fixture().Create<Measurement>();
        measurement.Time = time;
        return measurement;
    }
}