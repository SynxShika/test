public interface ITrappable
{
    bool IsTrapped { get; }
    void Trap(Bubble bubble);
    void Release();
    void Defeat();
}
