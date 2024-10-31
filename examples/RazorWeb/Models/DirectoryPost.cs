using Piranha.AttributeBuilder;
using Piranha.Models;

namespace RazorWeb.Models;

[PostType(Title = "Directory post")]
[ContentTypeRoute(Title = "Default", Route = "/directorypost")]
public class DirectoryPost  : Post<DirectoryPost>
{
}
