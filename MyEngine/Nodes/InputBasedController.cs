using MyEngine.MyInput;

namespace MyEngine.Nodes;

public class InputBasedController<T> : Controller<T> where T : Node
{
    protected InputListener Input;

    protected InputBasedController(InputListener input)
        => Input = input;
}