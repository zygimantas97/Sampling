using AutoFixture;
using FluentAssertions;
using Objectivity.AutoFixture.XUnit2.AutoMoq.Attributes;

namespace Sampling.UnitTests;

public class MeasurementPickerTests
{
    [Theory]
    [AutoMockData]
    public void PickLastOrDefaultFromInterval_WhenNoMeasurementsAtAll_ReturnsNull(
        DateTime startOfInterval,
        DateTime endOfInterval,
        MeasurementPicker measurementPicker)
    {
        // Arrange
        IEnumerable<Measurement> measurements = Enumerable.Empty<Measurement>();

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
            CreateMeasurement(startOfInterval.AddSeconds(-1)),
            CreateMeasurement(endOfInterval.AddSeconds(1))
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
        var measurementInInterval = CreateMeasurement(startOfInterval.AddSeconds(1));
        var measurements = new List<Measurement>
        {
            measurementInInterval
        };

        // Act
        var pickedMeasurement = measurementPicker.PickLastOrDefaultFromInterval(
            measurements,
            startOfInterval,
            endOfInterval);

        // Assert
        pickedMeasurement.Should().NotBeNull();
        pickedMeasurement.Should().Be(measurementInInterval);
    }

    [Theory]
    [AutoMockData]
    public void PickLastOrDefaultFromInterval_WhenSeveralMeasurementsInInterval_ReturnsLastMeasurement(
        MeasurementPicker measurementPicker)
    {
        // Arrange
        var startOfInterval = DateTime.UtcNow;
        var endOfInterval = startOfInterval.AddMinutes(1);
        var measurementJustAfterStartOfInterval = CreateMeasurement(startOfInterval.AddSeconds(1));
        var measurementJustBeforeEndOfInterval = CreateMeasurement(endOfInterval.AddSeconds(-1));
        var measurements = new List<Measurement>
        {
            measurementJustAfterStartOfInterval,
            measurementJustBeforeEndOfInterval
        };

        // Act
        var pickedMeasurement = measurementPicker.PickLastOrDefaultFromInterval(
            measurements,
            startOfInterval,
            endOfInterval);

        // Assert
        pickedMeasurement.Should().NotBeNull();
        pickedMeasurement.Should().Be(measurementJustBeforeEndOfInterval);
    }

    [Theory]
    [AutoMockData]
    public void PickLastOrDefaultFromInterval_WhenSeveralMeasurementsInIntervalAndLastMatchesEndOfInterval_ReturnsLastMeasurementWhichMatchesEndOfInterval(
        MeasurementPicker measurementPicker)
    {
        // Arrange
        var startOfInterval = DateTime.UtcNow;
        var endOfInterval = startOfInterval.AddMinutes(1);
        var measurementJustAfterStartOfInterval = CreateMeasurement(startOfInterval.AddSeconds(1));
        var measurementMatchingEndOfInterval = CreateMeasurement(endOfInterval);
        var measurements = new List<Measurement>
        {
            measurementJustAfterStartOfInterval,
            measurementMatchingEndOfInterval
        };

        // Act
        var pickedMeasurement = measurementPicker.PickLastOrDefaultFromInterval(
            measurements,
            startOfInterval,
            endOfInterval);

        // Assert
        pickedMeasurement.Should().NotBeNull();
        pickedMeasurement.Should().Be(measurementMatchingEndOfInterval);
    }

    private Measurement CreateMeasurement(DateTime time)
    {
        var measurement = new Fixture().Create<Measurement>();
        measurement.Time = time;
        return measurement;
    }
}