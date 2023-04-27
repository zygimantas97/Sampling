namespace Sampling.UnitTests;

public class MeasurementSamplerTests
{
    [Theory]
    [AutoMockData]
    public void Sample(
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
        measurementFilterMock
            .Setup(filter => filter.FilterMeasurementsAfter(measurements, startOfSampling))
            .Returns(filteredMeasurements);
        measurementClassifierMock
            .Setup(classifier => classifier.ClassifyByType(filteredMeasurements))
            .Returns(new Dictionary<MeasurementType, IEnumerable<Measurement>>
            {
                { MeasurementType.Temperature, classifiedTemperatureMeasurements },
                { MeasurementType.HeartRate, classifiedHeartRateMeasurements }
            });
        measurementOrdererMock
            .Setup(orderer => orderer.OrderByTimeAscending(classifiedTemperatureMeasurements))
            .Returns(orderedTemperatureMeasurements);
        measurementOrdererMock
            .Setup(orderer => orderer.OrderByTimeAscending(classifiedHeartRateMeasurements))
            .Returns(orderedHeartRateMeasurements);
        measurementSelectorMock
            .Setup(selector => selector.SelectMeasurements(orderedTemperatureMeasurements, startOfSampling))
            .Returns(selectedTemperatureMeasurements);
        measurementSelectorMock
            .Setup(selector => selector.SelectMeasurements(orderedHeartRateMeasurements, startOfSampling))
            .Returns(selectedHeartRateMeasurements);

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
}