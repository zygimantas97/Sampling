﻿using Sampling.Model;

namespace Sampling.UnitTests;

public class MeasurementPickerTests
{
    [Theory]
    [AutoMockData]
    public void PickLastOrDefaultFromInterval_WhenMeasurementsNull_ReturnsNull(
        DateTime startOfInterval,
        DateTime endOfInterval,
        MeasurementPicker measurementPicker)
    {
        // Arrange
        IEnumerable<Measurement> measurements = null;

        // Act
        var pickedMeasurement = measurementPicker.PickLastOrDefaultFromInterval(
            measurements,
            startOfInterval,
            endOfInterval);

        // Assert
        pickedMeasurement.Should().BeNull();
    }

    [Theory]
    [AutoMockData]
    public void PickLastOrDefaultFromInterval_WhenMeasurementsEmpty_ReturnsNull(
        DateTime startOfInterval,
        DateTime endOfInterval,
        MeasurementPicker measurementPicker)
    {
        // Arrange
        var measurements = Enumerable.Empty<Measurement>();

        // Act
        var pickedMeasurement = measurementPicker.PickLastOrDefaultFromInterval(
            measurements,
            startOfInterval,
            endOfInterval);

        // Assert
        pickedMeasurement.Should().BeNull();
    }

    [Theory]
    [AutoMockData]
    public void PickLastOrDefaultFromInterval_WhenNoMeasurementsInInterval_ReturnsNull(
        MeasurementPicker measurementPicker)
    {
        // Arrange
        var startOfInterval = DateTime.UtcNow;
        var endOfInterval = startOfInterval.AddMinutes(1);
        var measurements = new List<Measurement>
        {
            MeasurementFactory.CreateMeasurementWithTime(startOfInterval.AddSeconds(-1)),
            MeasurementFactory.CreateMeasurementWithTime(endOfInterval.AddSeconds(1))
        };

        // Act
        var pickedMeasurement = measurementPicker.PickLastOrDefaultFromInterval(
            measurements,
            startOfInterval,
            endOfInterval);

        // Assert
        pickedMeasurement.Should().BeNull();
    }

    [Theory]
    [AutoMockData]
    public void PickLastOrDefaultFromInterval_WhenOneMeasurementsInInterval_ReturnsThatMeasurement(
        MeasurementPicker measurementPicker)
    {
        // Arrange
        var startOfInterval = DateTime.UtcNow;
        var endOfInterval = startOfInterval.AddMinutes(1);
        var measurementInInterval = MeasurementFactory.CreateMeasurementWithTime(startOfInterval.AddSeconds(1));
        var measurements = new List<Measurement>
        {
            measurementInInterval
        };
        var expectedPickedMeasurement =
            MeasurementFactory.CreateMeasurementWithTime(measurementInInterval, endOfInterval);

        // Act
        var pickedMeasurement = measurementPicker.PickLastOrDefaultFromInterval(
            measurements,
            startOfInterval,
            endOfInterval);

        // Assert
        pickedMeasurement.Should().NotBeNull();
        pickedMeasurement.Should().BeEquivalentTo(expectedPickedMeasurement);
    }

    [Theory]
    [AutoMockData]
    public void PickLastOrDefaultFromInterval_WhenSeveralMeasurementsInInterval_ReturnsLastMeasurement(
        MeasurementPicker measurementPicker)
    {
        // Arrange
        var startOfInterval = DateTime.UtcNow;
        var endOfInterval = startOfInterval.AddMinutes(1);
        var measurementJustAfterStartOfInterval = MeasurementFactory.CreateMeasurementWithTime(startOfInterval.AddSeconds(1));
        var measurementJustBeforeEndOfInterval = MeasurementFactory.CreateMeasurementWithTime(endOfInterval.AddSeconds(-1));
        var measurements = new List<Measurement>
        {
            measurementJustAfterStartOfInterval,
            measurementJustBeforeEndOfInterval
        };
        var expectedPickedMeasurement =
            MeasurementFactory.CreateMeasurementWithTime(measurementJustBeforeEndOfInterval, endOfInterval);

        // Act
        var pickedMeasurement = measurementPicker.PickLastOrDefaultFromInterval(
            measurements,
            startOfInterval,
            endOfInterval);

        // Assert
        pickedMeasurement.Should().NotBeNull();
        pickedMeasurement.Should().BeEquivalentTo(expectedPickedMeasurement);
    }

    [Theory]
    [AutoMockData]
    public void PickLastOrDefaultFromInterval_WhenSeveralMeasurementsInIntervalAndLastMatchesEndOfInterval_ReturnsLastMeasurementWhichMatchesEndOfInterval(
        MeasurementPicker measurementPicker)
    {
        // Arrange
        var startOfInterval = DateTime.UtcNow;
        var endOfInterval = startOfInterval.AddMinutes(1);
        var measurementJustAfterStartOfInterval = MeasurementFactory.CreateMeasurementWithTime(startOfInterval.AddSeconds(1));
        var measurementMatchingEndOfInterval = MeasurementFactory.CreateMeasurementWithTime(endOfInterval);
        var measurements = new List<Measurement>
        {
            measurementJustAfterStartOfInterval,
            measurementMatchingEndOfInterval
        };
        var expectedPickedMeasurement =
            MeasurementFactory.CreateMeasurementWithTime(measurementMatchingEndOfInterval, endOfInterval);

        // Act
        var pickedMeasurement = measurementPicker.PickLastOrDefaultFromInterval(
            measurements,
            startOfInterval,
            endOfInterval);

        // Assert
        pickedMeasurement.Should().NotBeNull();
        pickedMeasurement.Should().BeEquivalentTo(expectedPickedMeasurement);
    }

    [Theory]
    [AutoMockData]
    public void PickLastOrDefaultFromInterval_WhenSeveralMeasurementsInTwoIntervals_ReturnsLastMeasurementOfFirstInterval(
        MeasurementPicker measurementPicker)
    {
        // Arrange
        var startOfInterval = DateTime.UtcNow;
        var endOfInterval = startOfInterval.AddMinutes(1);
        var measurementJustAfterStartOfFirstInterval = MeasurementFactory.CreateMeasurementWithTime(startOfInterval.AddSeconds(1));
        var measurementJustBeforeEndOfFirstInterval = MeasurementFactory.CreateMeasurementWithTime(endOfInterval.AddSeconds(-1));
        var measurementJustAfterStartOfSecondInterval = MeasurementFactory.CreateMeasurementWithTime(endOfInterval.AddSeconds(1));
        var measurements = new List<Measurement>
        {
            measurementJustAfterStartOfFirstInterval,
            measurementJustBeforeEndOfFirstInterval,
            measurementJustAfterStartOfSecondInterval
        };
        var expectedPickedMeasurement =
            MeasurementFactory.CreateMeasurementWithTime(measurementJustBeforeEndOfFirstInterval, endOfInterval);

        // Act
        var pickedMeasurement = measurementPicker.PickLastOrDefaultFromInterval(
            measurements,
            startOfInterval,
            endOfInterval);

        // Assert
        pickedMeasurement.Should().NotBeNull();
        pickedMeasurement.Should().BeEquivalentTo(expectedPickedMeasurement);
    }
}