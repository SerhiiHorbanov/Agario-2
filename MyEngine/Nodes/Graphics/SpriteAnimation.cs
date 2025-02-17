using MyEngine.Timed;

namespace MyEngine.Nodes.Graphics;

public class SpriteAnimation : Node
{
    private SequenceNode<Subtexture> _sequence;
    public SequenceNode<Subtexture> Sequence
    {
        get => _sequence;
        set
        {
            if (_sequence != null)
            {
                _sequence.Sequence.OnElementDue = null;
                DetachChild(_sequence);
            }
            
            value.Sequence.OnElementDue = ApplySubtexture;
            AdoptChild(value);
            _sequence = value;
        }
    }
    
    private SpriteNode _animatedSprite;
    public SpriteNode AnimatedSprite
    {
        get => _animatedSprite;
        set
        {
            Orphan();
            value.AdoptChild(this);
            _animatedSprite = value;
        }
    }

    private SpriteAnimation()
    { }
    
    public static SpriteAnimation AddAnimationTo(SpriteNode spriteNode, TimedSequence<Subtexture> animation)
    {
        SpriteAnimation result = new();

        result.Sequence = SequenceNode<Subtexture>.CreateSequenceNode(animation);
        result.AnimatedSprite = spriteNode;
        
        return result;
    }
    
    private void ApplySubtexture(Subtexture subtexture)
        => subtexture.ApplyTo(AnimatedSprite.Sprite);
}