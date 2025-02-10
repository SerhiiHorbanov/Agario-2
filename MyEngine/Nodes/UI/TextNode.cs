using MyEngine.Nodes.Graphics;
using SFML.Graphics;

namespace MyEngine.Nodes.UI;

public sealed class TextNode : RenderedNode
{
    public readonly Text MyText;

    public static Font DefaultFont = new("C:/Windows/Fonts/arial.ttf");

    public string Line
    {
        get => MyText.DisplayedString;
        set => MyText.DisplayedString = value;
    }
    
    private TextNode(RenderLayer layer) : base(layer)
    {
        MyText = new();
    }

    public static TextNode CreateTextNode()
    {
        TextNode result = new(RenderLayer.UILayer);

        result.MyText.Font = DefaultFont;
        
        return result;
    }

    public void UpdateLine(string newLine)
        => Line = newLine;
    
    public override void Draw(RenderTarget target, RenderStates states)
        => MyText.Draw(target, states);
}