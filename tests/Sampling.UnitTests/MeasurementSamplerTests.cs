namespace Sampling.UnitTests;

public class MeasurementSamplerTests
{
    [Theory]
    [AutoMockData]
    public void Sample_ReturnsSelectedMeasurements(
        [Frozen] Mock<IMeasurementFilter> measurementFilterMock,
        [Frozen] Mock<IMeasurementClassifier> measurementClassifierMock,
        [Frozen] Mock<IMeasurementOrderer> measurementOrdererMock,
        [Frozen] Mock<IMeasurementSelector> measurementSelectorMock,
        IEnumerable<Measurement> filteredMeasurements,
        IEnumerable<Measurement> classifiedTemperatureMeasurements,
        IEnumerable<Measurement> classifiedHeartRateMeasurements,
        IEnumerable<Measurement> orderedTemperatureMeasurements,
        IEnumerable<Measurement> orderedHeartRateMeasurements,
        IEnumerable<Measurement> selectedTemperatureMeasurements,
        IEnumerable<Measurement> selectedHeartRateMeasurements,
        DateTime startOfSampling,
        IEnumerable<Measurement> measurements,
        MeasurementSampler measurementSampler)
    {
        // Arrange
        SetupMeasurementFilter(measurementFilterMock, filteredMeasurements, startOfSampling, measurements);
        SetupMeasurementClassifier(measurementClassifierMock, filteredMeasurements, classifiedTemperatureMeasurements, classifiedHeartRateMeasurements);
        SetupMeasurementOrderer(measurementOrdererMock, classifiedTemperatureMeasurements, classifiedHeartRateMeasurements, orderedTemperatureMeasurements, orderedHeartRateMeasurements);
        SetupMeasurementSelector(measurementSelectorMock, orderedTemperatureMeasurements, orderedHeartRateMeasurements, selectedTemperatureMeasurements, selectedHeartRateMeasurements, startOfSampling);

        // Act
        var sampledMeasurements = measurementSampler.Sample(startOfSampling, measurements);

        // Assert
        using var _ = new AssertionScope();
        sampledMeasurements.Should().NotBeNull();
        sampledMeasurements.Count.Should().Be(2);
        sampledMeasurements[MeasurementType.Temperature].Should().NotBeNull();
        sampledMeasurements[MeasurementType.Temperature].Should().BeSameAs(selectedTemperatureMeasurements);
        sampledMeasurements[MeasurementType.HeartRate].Should().NotBeNull();
        sampledMeasurements[MeasurementType.HeartRate].Should().BeSameAs(selectedHeartRateMeasurements);
    }

    private static void SetupMeasurementSelector(
        Mock<IMeasurementSelector> measurementSelectorMock,
        IEnumerable<Measurement> orderedTemperatureMeasurements,
        IEnumerable<Measurement> orderedHeartRateMeasurements,
        IEnumerable<Measurement> selectedTemperatureMeasurements,
        IEnumerable<Measurement> selectedHeartRateMeasurements,
        DateTime startOfSampling)
    {
        measurementSelectorMock
            .Setup(selector => selector.Select(orderedTemperatureMeasurements, startOfSampling))
            .Returns(selectedTemperatureMeasurements);
        measurementSelectorMock
            .Setup(selector => selector.Select(orderedHeartRateMeasurements, startOfSampling))
            .Returns(selectedHeartRateMeasurements);
    }

    private static void SetupMeasurementOrderer(
        Mock<IMeasurementOrderer> measurementOrdererMock,
        IEnumerable<Measurement> classifiedTemperatureMeasurements,
        IEnumerable<Measurement> classifiedHeartRateMeasurements,
        IEnumerable<Measurement> orderedTemperatureMeasurements,
        IEnumerable<Measurement> orderedHeartRateMeasurements)
    {
        measurementOrdererMock
            .Setup(orderer => orderer.OrderByTimeAscending(classifiedTemperatureMeasurements))
            .Returns(orderedTemperatureMeasurements);
        measurementOrdererMock
            .Setup(orderer => orderer.OrderByTimeAscending(classifiedHeartRateMeasurements))
            .Returns(orderedHeartRateMeasurements);
    }

    private static void SetupMeasurementClassifier(
        Mock<IMeasurementClassifier> measurementClassifierMock,
        IEnumerable<Measurement> filteredMeasurements,
        IEnumerable<Measurement> classifiedTemperatureMeasurements,
        IEnumerable<Measurement> classifiedHeartRateMeasurements)
    {
        measurementClassifierMock
            .Setup(classifier => classifier.ClassifyByType(filteredMeasurements))
            .Returns(new Dictionary<MeasurementType, IEnumerable<Measurement>>
            {
                { MeasurementType.Temperature, classifiedTemperatureMeasurements },
                { MeasurementType.HeartRate, classifiedHeartRateMeasurements }
            });
    }

    private static void SetupMeasurementFilter(
        Mock<IMeasurementFilter> measurementFilterMock,
        IEnumerable<Measurement> filteredMeasurements,
        DateTime startOfSampling,
        IEnumerable<Measurement> measurements)
    {
        measurementFilterMock
            .Setup(filter => filter.FilterMeasurementsAfter(measurements, startOfSampling))
            .Returns(filteredMeasurements);
    }
}