using Sampling.Model;

namespace Sampling;

public interface IMeasurementSelector
{
    IEnumerable<Measurement> Select(
        IEnumerable<Measurement> measurements,
        DateTime startOfMeasurements);
}

public class MeasurementSelector
{
    private const int MeasurementIntervalInMinutes = 5;

    private readonly IMeasurementPicker _measurementPicker;

    public MeasurementSelector(IMeasurementPicker measurementPicker)
    {
        _measurementPicker = measurementPicker;
    }

    public IEnumerable<Measurement> Select(
        IEnumerable<Measurement> measurements,
        DateTime startOfMeasurements) =>
        measurements == null || measurements.Count() == 0
            ? Enumerable.Empty<Measurement>()
            : SelectMeasurementsFromNotEmptyEnumerable(measurements, startOfMeasurements);

    private IEnumerable<Measurement> SelectMeasurementsFromNotEmptyEnumerable(IEnumerable<Measurement> measurements, DateTime startOfMeasurements)
    {
        var selectedMeasurements = new List<Measurement>();

        var startOfInterval = startOfMeasurements;
        var endOfInterval = startOfInterval.AddMinutes(MeasurementIntervalInMinutes);
        var lastMeasurementTime = measurements.Max(measurement => measurement.Time);

        while (startOfInterval < lastMeasurementTime)
        {
            var measurement = _measurementPicker.PickLastOrDefaultFromInterval(
                measurements,
                startOfInterval,
                endOfInterval);

            AddSelectedMeasurement(measurement, selectedMeasurements);

            startOfInterval = endOfInterval;
            endOfInterval = endOfInterval.AddMinutes(MeasurementIntervalInMinutes);
        }

        return selectedMeasurements;
    }

    private static void AddSelectedMeasurement(Measurement? measurement, List<Measurement> selectedMeasurements)
    {
        if (measurement != null)
        {
            selectedMeasurements.Add(measurement);
        }
    }
}