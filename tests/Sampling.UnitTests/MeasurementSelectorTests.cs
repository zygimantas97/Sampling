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
        var selectedMeasurements = measurementSelector.Select(
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
        var selectedMeasurements = measurementSelector.Select(
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
        var measurementJustBeforeStartOfFirstInterval =
            MeasurementFactory.CreateMeasurementWithTime(startOfMeasurements.AddSeconds(-1));
        var measurements = new List<Measurement> { measurementJustBeforeStartOfFirstInterval };

        // Act
        var selectedMeasurements = measurementSelector.Select(
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
        var measurementJustAfterStartOfFirstInterval = MeasurementFactory.CreateMeasurementWithTime(startOfMeasurements.AddSeconds(1));
        var measurements = new List<Measurement> { measurementJustAfterStartOfFirstInterval };
        measurementPickerMock
            .Setup(picker => picker.PickLastOrDefaultFromInterval(
                measurements,
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>()))
            .Returns((Measurement)null);

        // Act
        var selectedMeasurements = measurementSelector.Select(
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
        var measurementJustAfterStartOfFirstInterval = MeasurementFactory.CreateMeasurementWithTime(startOfMeasurements.AddSeconds(1));
        var measurementJustBeforeEndOfFirstInterval = MeasurementFactory.CreateMeasurementWithTime(startOfMeasurements.AddMinutes(5).AddSeconds(-1));
        var measurements = new List<Measurement> { measurementJustAfterStartOfFirstInterval, measurementJustBeforeEndOfFirstInterval };
        measurementPickerMock
            .Setup(picker => picker.PickLastOrDefaultFromInterval(
                measurements,
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>()))
            .Returns(pickedMeasurement);

        // Act
        var selectedMeasurements = measurementSelector.Select(
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
        var measurementJustAfterStartOfFirstInterval = MeasurementFactory.CreateMeasurementWithTime(startOfMeasurements.AddSeconds(1));
        var measurementMatchingEndOfFirstInterval = MeasurementFactory.CreateMeasurementWithTime(startOfMeasurements.AddMinutes(5));
        var measurements = new List<Measurement> { measurementJustAfterStartOfFirstInterval, measurementMatchingEndOfFirstInterval };
        measurementPickerMock
            .Setup(picker => picker.PickLastOrDefaultFromInterval(
                measurements,
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>()))
            .Returns(pickedMeasurement);

        // Act
        var selectedMeasurements = measurementSelector.Select(
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
        var measurementJustAfterStartOfFirstInterval = MeasurementFactory.CreateMeasurementWithTime(startOfMeasurements.AddSeconds(1));
        var measurementJustAfterEndOfFirstInterval = MeasurementFactory.CreateMeasurementWithTime(startOfMeasurements.AddMinutes(5).AddSeconds(1));
        var measurements = new List<Measurement> { measurementJustAfterStartOfFirstInterval, measurementJustAfterEndOfFirstInterval };
        measurementPickerMock
            .Setup(picker => picker.PickLastOrDefaultFromInterval(
                measurements,
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>()))
            .Returns(pickedMeasurement);

        // Act
        var selectedMeasurements = measurementSelector.Select(
            measurements,
            startOfMeasurements);

        // Assert
        using var _ = new AssertionScope();
        selectedMeasurements.Should().NotBeNull();
        selectedMeasurements.Count().Should().Be(2);
        selectedMeasurements.Should().OnlyContain(measurement => measurement == pickedMeasurement);
    }
}