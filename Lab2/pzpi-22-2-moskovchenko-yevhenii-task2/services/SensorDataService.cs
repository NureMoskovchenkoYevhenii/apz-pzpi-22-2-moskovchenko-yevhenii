public class SensorDataService
{
    private readonly ISensorDataRepository _sensorDataRepository;
    private const decimal MinTemperature = 3m;
    private const decimal MaxTemperature = 5m;
    private const decimal MinHumidity = 85m;
    private const decimal MaxHumidity = 95m;

    public SensorDataService(ISensorDataRepository sensorDataRepository)
    {
        _sensorDataRepository = sensorDataRepository;
    }

    public void AddSensorData(SensorData sensorData)
    {
        SimulateAdjustmentActivation(sensorData, MinTemperature, MaxTemperature, MinHumidity, MaxHumidity);
        _sensorDataRepository.Add(sensorData);
    }

    public void SimulateAdjustmentActivation(SensorData sensorData, decimal minTemp, decimal maxTemp, decimal minHumidity, decimal maxHumidity)
    {
        bool isTemperatureAdjustmentNeeded = sensorData.Temperature < minTemp || sensorData.Temperature > maxTemp;
        bool isHumidityAdjustmentNeeded = sensorData.Humidity < minHumidity || sensorData.Humidity > maxHumidity;

        sensorData.IsTemperatureAdjustmentEnabled = isTemperatureAdjustmentNeeded;
        sensorData.IsHumidityAdjustmentEnabled = isHumidityAdjustmentNeeded;
    }
    public IEnumerable<SensorData> GetAllSensorData()
    {
        return _sensorDataRepository.GetAll();
    }

    public SensorData GetSensorDataById(int id)
    {
        return _sensorDataRepository.GetById(id);
    }

    public void UpdateSensorData(int id, SensorData updatedSensorData)
    {
        var sensorData = _sensorDataRepository.GetById(id);
        if (sensorData != null)
        {
            sensorData.Timestamp = updatedSensorData.Timestamp;
            sensorData.Temperature = updatedSensorData.Temperature;
            sensorData.Humidity = updatedSensorData.Humidity;
            _sensorDataRepository.Update(sensorData);
        }
    }

   

    public void DeleteSensorData(int id)
    {
        _sensorDataRepository.Delete(id);
    }
}
