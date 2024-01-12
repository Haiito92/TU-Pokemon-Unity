
using System;

namespace _2023_GC_A2_Partiel_POO.Level_2
{
    /// <summary>
    /// Définition des types dans le jeu
    /// </summary>
    public enum TYPE { NORMAL, WATER, FIRE, GRASS }

    public class TypeResolver
    {
        static float _normalFactor = 1f;
        static float _resistFactor = 0.8f;
        static float _vulnerableFactor = 1.2f;

        /// <summary>
        /// Récupère le facteur multiplicateur pour la résolution des résistances/faiblesses
        /// WATER faible contre GRASS, resiste contre FIRE
        /// FIRE faible contre WATER, resiste contre GRASS
        /// GRASS faible contre FIRE, resiste contre WATER
        /// </summary>
        /// <param name="attacker">Type de l'attaque (le skill)</param>
        /// <param name="receiver">Type de la cible</param>
        /// <returns>
        /// Normal returns 1 if attacker or receiver
        /// 0.8 if resist
        /// 1.0 if same type
        /// 1.2 if vulnerable
        /// </returns>
        public static float GetFactor(TYPE attacker, TYPE receiver)
        {
            if (attacker == receiver) return _normalFactor;
            if (attacker == TYPE.NORMAL || receiver == TYPE.NORMAL) return _normalFactor;
            switch (receiver)
            {
                case TYPE.WATER:
                    switch (attacker)
                    {
                        case TYPE.GRASS:
                            return _vulnerableFactor;
                        case TYPE.FIRE:
                            return _resistFactor;
                    }
                    break;
                case TYPE.FIRE:
                    switch (attacker)
                    {
                        case TYPE.WATER:
                            return _vulnerableFactor;
                        case TYPE.GRASS:
                            return _resistFactor;
                    }
                    break;
                case TYPE.GRASS:
                    switch (attacker)
                    {
                        case TYPE.FIRE:
                            return _vulnerableFactor;
                        case TYPE.WATER:
                            return _resistFactor;
                    }
                    break;
            }
            return -1;
        }

    }
}
