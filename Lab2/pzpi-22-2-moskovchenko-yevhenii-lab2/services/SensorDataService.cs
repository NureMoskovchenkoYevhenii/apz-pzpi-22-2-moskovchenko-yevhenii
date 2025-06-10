using Microsoft.Extensions.Localization;

public class SensorDataService
{
    private readonly ISensorDataRepository _sensorDataRepository;
    private readonly IStringLocalizer<SharedResources> _localizer;
    private const decimal MinTemperature = 3m;
    private const decimal MaxTemperature = 5m;
    private const decimal MinHumidity = 85m;
    private const decimal MaxHumidity = 95m;

    public SensorDataService(ISensorDataRepository sensorDataRepository, IStringLocalizer<SharedResources> localizer)
    {
        _sensorDataRepository = sensorDataRepository;
        _localizer = localizer;
    }

    public async Task AddSensorDataAsync(SensorData sensorData)
    {
        SimulateAdjustmentActivation(sensorData, MinTemperature, MaxTemperature, MinHumidity, MaxHumidity);
        await _sensorDataRepository.AddAsync(sensorData);
    }

    public void SimulateAdjustmentActivation(SensorData sensorData, decimal minTemp, decimal maxTemp, decimal minHumidity, decimal maxHumidity)
    {
        bool isTemperatureAdjustmentNeeded = sensorData.Temperature < minTemp || sensorData.Temperature > maxTemp;
        bool isHumidityAdjustmentNeeded = sensorData.Humidity < minHumidity || sensorData.Humidity > maxHumidity;
        sensorData.IsTemperatureAdjustmentEnabled = isTemperatureAdjustmentNeeded;
        sensorData.IsHumidityAdjustmentEnabled = isHumidityAdjustmentNeeded;
    }

    public async Task<IEnumerable<SensorData>> GetAllSensorDataAsync()
    {
        return await _sensorDataRepository.GetAllAsync();
    }

    public async Task<SensorData> GetSensorDataByIdAsync(int id)
    {
        return await _sensorDataRepository.GetByIdAsync(id);
    }

    public async Task UpdateSensorDataAsync(int id, SensorData updatedSensorData)
    {
        var sensorData = await _sensorDataRepository.GetByIdAsync(id);
        if (sensorData != null)
        {
            sensorData.Timestamp = updatedSensorData.Timestamp;
            sensorData.Temperature = updatedSensorData.Temperature;
            sensorData.Humidity = updatedSensorData.Humidity;
            // Логіку SimulateAdjustmentActivation можна застосувати і при оновленні
            SimulateAdjustmentActivation(sensorData, MinTemperature, MaxTemperature, MinHumidity, MaxHumidity);
            await _sensorDataRepository.UpdateAsync(sensorData);
        }
    }

    public async Task DeleteSensorDataAsync(int id)
    {
        await _sensorDataRepository.DeleteAsync(id);
    }
}