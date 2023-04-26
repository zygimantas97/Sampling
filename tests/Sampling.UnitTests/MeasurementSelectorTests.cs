﻿using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using Objectivity.AutoFixture.XUnit2.AutoMoq.Attributes;

namespace Sampling.UnitTests;

public class MeasurementSelectorTests
{
    [Theory]
    [AutoMockData]
    public void SelectMeasurements_WhenMeasurementsNull_ReturnsEmptyEnumerable(
        DateTime startOfMeasurements,
        MeasurementSelector measurementSelector)
    {
        // Arrange
        IEnumerable<Measurement> measurements = null;

        // Act
        var selectedMeasurements = measurementSelector.SelectMeasurements(
            measurements,
            startOfMeasurements);

        // Assert
        selectedMeasurements.Should().NotBeNull();
        selectedMeasurements.Should().BeEmpty();
    }

    [Theory]
    [AutoMockData]
    public void SelectMeasurements_WhenNoMeasurementsAtAll_ReturnsEmptyEnumerable(
        DateTime startOfMeasurements,
        MeasurementSelector measurementSelector)
    {
        // Arrange
        var measurements = Enumerable.Empty<Measurement>();

        // Act
        var selectedMeasurements = measurementSelector.SelectMeasurements(
            measurements,
            startOfMeasurements);

        // Assert
        selectedMeasurements.Should().NotBeNull();
        selectedMeasurements.Should().BeEmpty();
    }

    [Theory]
    [AutoMockData]
    public void SelectMeasurements_WhenLastMeasurementIsBeforeStartOfMeasurementsTime_ReturnsEmptyEnumerable(
        DateTime startOfMeasurements,
        MeasurementSelector measurementSelector)
    {
        // Arrange
        var measurements = new List<Measurement> { CreateMeasurement(startOfMeasurements.AddSeconds(-1)) };

        // Act
        var selectedMeasurements = measurementSelector.SelectMeasurements(
            measurements,
            startOfMeasurements);

        // Assert
        selectedMeasurements.Should().NotBeNull();
        selectedMeasurements.Should().BeEmpty();
    }

    [Theory]
    [AutoMockData]
    public void SelectMeasurements_WhenPickedMeasurementNull_ReturnsEmptyEnumerable(
        [Frozen] Mock<IMeasurementPicker> measurementPickerMock,
        DateTime startOfMeasurements,
        MeasurementSelector measurementSelector)
    {
        // Arrange
        var measurements = new List<Measurement> { CreateMeasurement(startOfMeasurements.AddSeconds(1)) };
        measurementPickerMock
            .Setup(picker => picker.PickLastOrDefaultFromInterval(
                measurements,
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>()))
            .Returns((Measurement)null);

        // Act
        var selectedMeasurements = measurementSelector.SelectMeasurements(
            measurements,
            startOfMeasurements);

        // Assert
        selectedMeasurements.Should().NotBeNull();
        selectedMeasurements.Should().BeEmpty();
    }

    [Theory]
    [AutoMockData]
    public void SelectMeasurements_WhenLastMeasurementTimeIsJustBeforeEndOfFirstInterval_ReturnsOnePickedMeasurement(
        [Frozen] Mock<IMeasurementPicker> measurementPickerMock,
        Measurement pickedMeasurement,
        DateTime startOfMeasurements,
        MeasurementSelector measurementSelector)
    {
        // Arrange
        var measurementJustAfterStartOfFirstInterval = CreateMeasurement(startOfMeasurements.AddSeconds(1));
        var measurementJustBeforeEndOfFirstInterval = CreateMeasurement(startOfMeasurements.AddMinutes(5).AddSeconds(-1));
        var measurements = new List<Measurement> { measurementJustAfterStartOfFirstInterval, measurementJustBeforeEndOfFirstInterval };
        measurementPickerMock
            .Setup(picker => picker.PickLastOrDefaultFromInterval(
                measurements,
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>()))
            .Returns(pickedMeasurement);

        // Act
        var selectedMeasurements = measurementSelector.SelectMeasurements(
            measurements,
            startOfMeasurements);

        // Assert
        using var _ = new AssertionScope();
        selectedMeasurements.Should().NotBeNull();
        selectedMeasurements.Count().Should().Be(1);
        selectedMeasurements.Should().OnlyContain(measurement => measurement == pickedMeasurement);
    }

    [Theory]
    [AutoMockData]
    public void SelectMeasurements_WhenLastMeasurementTimeMatchesEndOfFirstInterval_ReturnsOnePickedMeasurement(
        [Frozen] Mock<IMeasurementPicker> measurementPickerMock,
        Measurement pickedMeasurement,
        DateTime startOfMeasurements,
        MeasurementSelector measurementSelector)
    {
        // Arrange
        var measurementJustAfterStartOfFirstInterval = CreateMeasurement(startOfMeasurements.AddSeconds(1));
        var measurementMatchingEndOfFirstInterval = CreateMeasurement(startOfMeasurements.AddMinutes(5));
        var measurements = new List<Measurement> { measurementJustAfterStartOfFirstInterval, measurementMatchingEndOfFirstInterval };
        measurementPickerMock
            .Setup(picker => picker.PickLastOrDefaultFromInterval(
                measurements,
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>()))
            .Returns(pickedMeasurement);

        // Act
        var selectedMeasurements = measurementSelector.SelectMeasurements(
            measurements,
            startOfMeasurements);

        // Assert
        using var _ = new AssertionScope();
        selectedMeasurements.Should().NotBeNull();
        selectedMeasurements.Count().Should().Be(1);
        selectedMeasurements.Should().OnlyContain(measurement => measurement == pickedMeasurement);
    }

    [Theory]
    [AutoMockData]
    public void SelectMeasurements_WhenLastMeasurementTimeIsJustAfterEndOfFirstInterval_ReturnsTwoPickedMeasurement(
        [Frozen] Mock<IMeasurementPicker> measurementPickerMock,
        Measurement pickedMeasurement,
        DateTime startOfMeasurements,
        MeasurementSelector measurementSelector)
    {
        // Arrange
        var measurementJustAfterStartOfFirstInterval = CreateMeasurement(startOfMeasurements.AddSeconds(1));
        var measurementJustAfterEndOfFirstInterval = CreateMeasurement(startOfMeasurements.AddMinutes(5).AddSeconds(1));
        var measurements = new List<Measurement> { measurementJustAfterStartOfFirstInterval, measurementJustAfterEndOfFirstInterval };
        measurementPickerMock
            .Setup(picker => picker.PickLastOrDefaultFromInterval(
                measurements,
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>()))
            .Returns(pickedMeasurement);

        // Act
        var selectedMeasurements = measurementSelector.SelectMeasurements(
            measurements,
            startOfMeasurements);

        // Assert
        using var _ = new AssertionScope();
        selectedMeasurements.Should().NotBeNull();
        selectedMeasurements.Count().Should().Be(2);
        selectedMeasurements.Should().OnlyContain(measurement => measurement == pickedMeasurement);
    }

    private Measurement CreateMeasurement(DateTime time)
    {
        var measurement = new Fixture().Create<Measurement>();
        measurement.Time = time;
        return measurement;
    }
}