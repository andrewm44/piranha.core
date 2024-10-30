using Piranha.AttributeBuilder;
using Piranha.Extend.Fields;
using Piranha.Extend;
using Piranha.Models;

namespace RazorWeb.Models;

[PageType(Title = "Directory Entry")]
public class DirectoryEntry  : Page<DirectoryEntry>
{
    [Field(Title = "Main Content")]
    public TextField Body { get; set; }

    [Field(Title = "Header Image")]
    public ImageField HeaderImage { get; set; }
}
