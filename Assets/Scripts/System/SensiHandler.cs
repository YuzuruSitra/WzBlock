
public class SensiHandler
{
    // ƒVƒ“ƒOƒ‹ƒgƒ“
    private static SensiHandler instance;
    public static SensiHandler Instance => instance ?? (instance = new SensiHandler());
    private PlayDataIO _playDataIO;
    private int _sensitivity;
    public int Sensitivity => _sensitivity;

    private SensiHandler()
    {
        _playDataIO = PlayDataIO.Instance;
        _sensitivity = _playDataIO.LoadSensitivity();
    }

    public void ChangeSensitivity(int value)
    {
        _sensitivity = value;
        _playDataIO.SaveSensitivity(value);
    }
}
