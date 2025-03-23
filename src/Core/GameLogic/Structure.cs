namespace cr_mono.Core.GameLogic
{
    internal class Structure
    {
        internal bool isCleared;
        internal int id;

        internal Structure(int id) 
        {
            this.isCleared = false;
            this.id = id;
        }
    }
}
