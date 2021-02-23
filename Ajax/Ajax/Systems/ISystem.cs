namespace Ajax.Systems
{
    public interface ISystem
    {
        void OnAction(Objects.Entity entity);

        // Property signatures: 
        string Name
        {
            get;
        }
    }
}
