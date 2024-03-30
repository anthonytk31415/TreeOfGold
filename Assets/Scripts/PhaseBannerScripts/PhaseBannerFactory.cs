using System;
using System.Collections.Generic;


public class PhaseBannerFactory {
    Dictionary<PhaseBanner, (string, string)> pathLookup = new Dictionary<PhaseBanner, (string, string)> 
    {
        {PhaseBanner.PlayerLose, ("Prefabs/PhaseBanner/PlayerLoseCanvas", "BackgroundPanel/PlayerLoseText")},
        {PhaseBanner.PlayerWin, ("Prefabs/PhaseBanner/PlayerWinCanvas", "BackgroundPanel/PlayerWinText")},
        {PhaseBanner.PlayerPhase, ("Prefabs/PhaseBanner/PlayerPhaseCanvas", "BackgroundPanel/PlayerPhaseText")},
        {PhaseBanner.EnemyPhase, ("Prefabs/PhaseBanner/EnemyPhaseCanvas", "BackgroundPanel/EnemyPhaseText")}
    };
    
    public PhaseBannerFactory(){

    }

    public string getCanvas(PhaseBanner phaseBanner){
        return pathLookup[phaseBanner].Item1; 
    }

    public  string getText(PhaseBanner phaseBanner){
        return pathLookup[phaseBanner].Item2; 
    }

}