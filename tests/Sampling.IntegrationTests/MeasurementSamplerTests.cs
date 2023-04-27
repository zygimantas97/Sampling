using FluentAssertions;
using Objectivity.AutoFixture.XUnit2.AutoMoq.Attributes;
using Sampling.Model;

namespace Sampling.IntegrationTests;

public class MeasurementSamplerTests
{
    private readonly MeasurementSampler _measurementSampler;

    public MeasurementSamplerTests()
    {
        var measurementFilter = new MeasurementFilter();
        var measurementClassifier = new MeasurementClassifier();
        var measurementOrderer = new MeasurementOrderer();
        var measurementPicker = new MeasurementPicker();
        var measurementSelector = new MeasurementSelector(measurementPicker);

        _measurementSampler = new MeasurementSampler(
            measurementFilter,
            measurementClassifier,
            measurementOrderer,
            measurementSelector);
    }

    [Theory]
    [AutoMockData]
    public void Sample_WhenMeasurementsNull_ReturnsNull(
        DateTime startOfSampling)
    {
        // Arrange
        IEnumerable<Measurement> measurements = null;

        // Act
        var measurementsAfterSampling = _measurementSampler.Sample(startOfSampling, measurements);

        // Assert
        measurementsAfterSampling.Should().BeNull();
    }

    [Theory]
    [AutoMockData]
    public void Sample_WhenMeasurementsEmpty_ReturnsEmptyDictionary(
        DateTime startOfSampling)
    {
        // Arrange
        var measurements = Enumerable.Empty<Measurement>();

        // Act
        var measurementsAfterSampling = _measurementSampler.Sample(startOfSampling, measurements);

        // Assert
        measurementsAfterSampling.Should().NotBeNull();
        measurementsAfterSampling.Should().BeEmpty();
    }

    [Fact]
    public void Sample_WhenMeasurementsNotEmpty_ReturnsSampledMeasurements()
    {
        // Arrange
        var startOfSampling = new DateTime(2017, 1, 3, 10, 0, 0);
        var inputMeasurements = CreateInputMeasurements();
        var expectedSampledMeasurements = CreateOutputMeasurements();

        // Act
        var measurementsAfterSampling = _measurementSampler.Sample(startOfSampling, inputMeasurements);

        // Assert
        measurementsAfterSampling.Should().NotBeNull();
        measurementsAfterSampling.Should().BeEquivalentTo(expectedSampledMeasurements);
    }

    private IEnumerable<Measurement> CreateInputMeasurements() => new List<Measurement>
    {
        new Measurement
        {
            Time = new DateTime(2017, 1, 3, 10, 4, 45),
            Type = MeasurementType.Temperature,
            Value = 35.79
        },
        new Measurement
        {
            Time = new DateTime(2017, 1, 3, 10, 1, 18),
            Type = MeasurementType.SpO2,
            Value = 98.78
        },
        new Measurement
        {
            Time = new DateTime(2017, 1, 3, 10, 9, 7),
            Type = MeasurementType.Temperature,
            Value = 35.01
        },
        new Measurement
        {
            Time = new DateTime(2017, 1, 3, 10, 3, 34),
            Type = MeasurementType.SpO2,
            Value = 96.49
        },
        new Measurement
        {
            Time = new DateTime(2017, 1, 3, 10, 2, 1),
            Type = MeasurementType.Temperature,
            Value = 35.82
        },
        new Measurement
        {
            Time = new DateTime(2017, 1, 3, 10, 5, 0),
            Type = MeasurementType.SpO2,
            Value = 97.17
        },
        new Measurement
        {
            Time = new DateTime(2017, 1, 3, 10, 5, 1),
            Type = MeasurementType.SpO2,
            Value = 95.08
        }
    };

    private IDictionary<MeasurementType, IEnumerable<Measurement>> CreateOutputMeasurements() =>
        new Dictionary<MeasurementType, IEnumerable<Measurement>>
        {
            {
                MeasurementType.Temperature,
                new List<Measurement>
                {
                    new Measurement
                    {
                        Time = new DateTime(2017, 1, 3, 10, 5, 0),
                        Type = MeasurementType.Temperature,
                        Value = 35.79
                    },
                    new Measurement
                    {
                        Time = new DateTime(2017, 1, 3, 10, 10, 0),
                        Type = MeasurementType.Temperature,
                        Value = 35.01
                    }
                }
            },
            {
                MeasurementType.SpO2,
                new List<Measurement>
                {
                    new Measurement
                    {
                        Time = new DateTime(2017, 1, 3, 10, 5, 0),
                        Type = MeasurementType.SpO2,
                        Value = 97.17
                    },
                    new Measurement
                    {
                        Time = new DateTime(2017, 1, 3, 10, 10, 0),
                        Type = MeasurementType.SpO2,
                        Value = 95.08
                    }
                }
            }
        };
}