namespace Sampling.UnitTests;

public interface IMeasurementSampler
{
    IDictionary<MeasurementType, IEnumerable<Measurement>> Sample(
        DateTime startOfSampling,
        IEnumerable<Measurement> measurements);
}

public class MeasurementSampler
{
    private readonly IMeasurementFilter _measurementFilter;
    private readonly IMeasurementClassifier _measurementClassifier;
    private readonly IMeasurementOrderer _measurementOrderer;
    private readonly IMeasurementSelector _measurementSelector;

    public MeasurementSampler(
        IMeasurementFilter measurementFilter,
        IMeasurementClassifier measurementClassifier,
        IMeasurementOrderer measurementOrderer,
        IMeasurementSelector measurementSelector)
    {
        _measurementFilter = measurementFilter;
        _measurementClassifier = measurementClassifier;
        _measurementOrderer = measurementOrderer;
        _measurementSelector = measurementSelector;
    }

    public IDictionary<MeasurementType, IEnumerable<Measurement>> Sample(
        DateTime startOfSampling,
        IEnumerable<Measurement> measurements)
    {
        var filteredMeasurements = _measurementFilter.FilterMeasurementsAfter(measurements, startOfSampling);
        var classifiedMeasurements = _measurementClassifier.ClassifyByType(filteredMeasurements);
        var orderedMeasurements = classifiedMeasurements.ToDictionary(
            group => group.Key,
            group => _measurementOrderer.OrderByTimeAscending(group.Value));
        var selectedMeasurements = orderedMeasurements.ToDictionary(
            group => group.Key,
            group => _measurementSelector.SelectMeasurements(group.Value, startOfSampling));
        return selectedMeasurements;
    }
}