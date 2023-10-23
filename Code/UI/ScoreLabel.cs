using Godot;

namespace Code.UI;

public partial class ScoreLabel : Label
{
    private int _score = 0;

    public void Set(int valueToSet)
    {
        _score = valueToSet;
        Text = $"Score: {_score}";
    }

    public void Increase()
    {
        _score++;
        Text = $"Score: {_score}";
    }
}
