namespace cr_mono.Core.GameLogic
{
    internal class GameContext
    {
        // anything that will be needed regularly (or at all?) across the game scene
        // examples would be player info, progression, quests, map state, (current subscene?)
        // this way, info can persist across subscenes
        //  can also use this for load/save functionality?
        // therefore, when making a new one, I guess I would generate the world here?
        // that, or I do it as is, and just set the values here

        // stored info will be classes (playerinfo, worldInfo etc.) i think
        internal GameContext() 
        {
            //
        }
    }
}
