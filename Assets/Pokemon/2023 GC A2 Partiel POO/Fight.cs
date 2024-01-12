
using System;

namespace _2023_GC_A2_Partiel_POO.Level_2
{
    public class Fight
    {
        public Fight(Character character1, Character character2)
        {
            if (character1 == null || character2 == null) throw new ArgumentNullException();
            Character1 = character1;
            Character2 = character2;
        }

        public Character Character1 { get; }
        public Character Character2 { get; }
        /// <summary>
        /// Est-ce la condition de victoire/défaite a été rencontré ?
        /// </summary>
        public bool IsFightFinished => !Character1.IsAlive || !Character2.IsAlive;

        /// <summary>
        /// Jouer l'enchainement des attaques. Attention à bien gérer l'ordre des attaques par apport à la speed des personnages
        /// </summary>
        /// <param name="skillFromCharacter1">L'attaque selectionné par le joueur 1</param>
        /// <param name="skillFromCharacter2">L'attaque selectionné par le joueur 2</param>
        /// <exception cref="ArgumentNullException">si une des deux attaques est null</exception>
        public void ExecuteTurn(Skill skillFromCharacter1, Skill skillFromCharacter2)
        {
            Character fastest, slowest;
            Skill fromFastest, fromSlowest;
            if (Character1.Speed > Character2.Speed)
            {
                fastest = Character1;
                slowest = Character2;
                fromFastest = skillFromCharacter1;
                fromSlowest = skillFromCharacter2;
            }
            else
            {
                fastest = Character2;
                slowest = Character1;
                fromFastest = skillFromCharacter2;
                fromSlowest = skillFromCharacter1;
            }

            slowest.ReceiveAttack(fastest, fromFastest);

            if (slowest.IsAlive) fastest.ReceiveAttack(slowest, fromSlowest);


            if (fastest.IsAlive) fastest.CurrentStatus.EndTurn();
            if (slowest.IsAlive) slowest.CurrentStatus.EndTurn();
        }

    }
}
