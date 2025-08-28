using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASW.RemoteViewing.Shared.Dto.RemoteUser;

public class RemoteUserDto
{
    public Guid Id { get; set; } 
    public Guid PlaceId { get; set; }
    public string PlaceName { get; set; } = string.Empty;
    public Guid UsersId { get; set; }
    public string Login { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string FullFIO { get; set; } = string.Empty;
    public string ReductionFIO { get; set; } = string.Empty;
    public Guid ModifiedId { get; set; } = Guid.Empty;
    public bool IsRemoved { get; set; }
}
