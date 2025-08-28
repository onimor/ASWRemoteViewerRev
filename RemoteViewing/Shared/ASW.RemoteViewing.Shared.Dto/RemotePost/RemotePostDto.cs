namespace ASW.RemoteViewing.Shared.Dto.RemotePost;

public class RemotePostDto
{
    public Guid Id { get; set; }
    public Guid PlaceId { get; set; }
    public string PlaceName { get; set; } = string.Empty;
    public Guid PostId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsAutomaticMode { get; set; }
    public string OperatingMode { get; set; } = string.Empty;
    public bool IsAutomaticDeviceModeOff { get; set; }
    public bool IsRegisteringEmptyPassage { get; set; }
    public double ThresholdBeginningWeighing { get; set; } = 0;
    public int DelayEmptyScales { get; set; }
    public int TerminalNumber { get; set; }
    public bool IsTcp { get; set; }
    public string TCPIP { get; set; } = string.Empty;
    public int TCPPort { get; set; }
    public string ComName { get; set; } = string.Empty;
    public int ComSpeed { get; set; }
    public bool IsWeighingByAxes { get; set; }
    public bool IsOnlyAxes { get; set; }
    public string ServerIP { get; set; } = string.Empty;
    public int ServerPort { get; set; }
    public string ClientIP { get; set; } = string.Empty;
    public int ClientPort { get; set; } 
}
