namespace FrostyEditor.Services.Implementation.Mock;

public class DesignLoggingService : LoggingService
{
    public DesignLoggingService()
    {
        LogWarning("WarningMessage. Test. Test. Test. 1234");
        LogWarning("WarningMessage. Test. Test. Test. 1234");
        LogInfo("InfoMessage. Test. Test. Test. 1234");
        LogWarning("WarningMessage. Test. Test. Test. 1234");
        LogWarning("WarningMessage. Test. Test. Test. 1234");
        LogInfo("InfoMessage. Test. Test. Test. 1234");
        LogInfo("InfoMessage. Test. Test. Test. 1234");
        LogError("ErrorMessage. Test. Test. Test. 1234");
        LogInfo("InfoMessage. Test. Test. Test. 1234");
        LogInfo("InfoMessage. Test. Test. Test. 1234");
        LogError("ErrorMessage. Test. Test. Test. 1234");
        LogError("ErrorMessage. Test. Test. Test. 1234");
        LogWarning("WarningMessage. Test. Test. Test. 1234");
        LogError("ErrorMessage. Test. Test. Test. 1234");
        LogError("ErrorMessage. Test. Test. Test. 1234");
        LogInfo("InfoMessage. Test. Test. Test. 1234");
        LogError("ErrorMessage. Test. Test. Test. 1234");
    }
}