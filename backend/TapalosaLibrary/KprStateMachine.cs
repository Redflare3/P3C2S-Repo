using System;
using System.Collections.Generic;

namespace TapalosaCommon.Models
{
    public enum KprState
    {
        MenungguPersetujuan,
        Disetujui,
        Ditolak,
        Dibatalkan
    }

    public enum KprTrigger
    {
        Setujui,
        Tolak,
        Batalkan
    }

    public static class KprStateMachine
    {
        private static readonly Dictionary<(KprState, KprTrigger), KprState> Transitions =
            new Dictionary<(KprState, KprTrigger), KprState>
            {
                { (KprState.MenungguPersetujuan, KprTrigger.Setujui), KprState.Disetujui },
                { (KprState.MenungguPersetujuan, KprTrigger.Tolak), KprState.Ditolak },
                { (KprState.MenungguPersetujuan, KprTrigger.Batalkan), KprState.Dibatalkan }
            };

        public static bool TryTransition(string currentStateStr, string triggerStr, out string nextStateStr)
        {
            nextStateStr = currentStateStr;

            string cleanState = currentStateStr.Replace(" ", "").Trim();
            string cleanTrigger = triggerStr.Replace(" ", "").Trim();

            if (!Enum.TryParse(cleanState, true, out KprState currentState) ||
                !Enum.TryParse(cleanTrigger, true, out KprTrigger trigger))
            {
                return false;
            }

            if (Transitions.TryGetValue((currentState, trigger), out KprState nextState))
            {
                nextStateStr = nextState.ToString();
                return true;
            }

            return false;
        }
    }
}
