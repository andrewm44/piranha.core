using Piranha.AttributeBuilder;
using Piranha.Models;

namespace RazorWeb.Models;

[PageType(Title = "Directory archive", IsArchive = true)]
[ContentTypeRoute(Title = "Default", Route = "/directoryarchive")]
public class DirectoryArchive : Page<StandardArchive>
{
}
