
using _2023_GC_A2_Partiel_POO.Level_2;
using NUnit.Framework;
using System;

namespace _2023_GC_A2_Partiel_POO.Tests.Level_2
{
    public class FightMoreTests
    {
        // Tu as probablement remarqué qu'il y a encore beaucoup de code qui n'a pas été testé ...
        // À présent c'est à toi de créer les TU sur le reste et de les implémenter

        // Ce que tu peux ajouter:
        // - Ajouter davantage de sécurité sur les tests apportés
        // - un heal ne régénère pas plus que les HP Max
        // - si on abaisse les HPMax les HP courant doivent suivre si c'est au dessus de la nouvelle valeur
        // - ajouter un equipement qui rend les attaques prioritaires puis l'enlever et voir que l'attaque n'est plus prioritaire etc)
        // - Le support des status (sleep et burn) qui font des effets à la fin du tour et/ou empeche le pkmn d'agir
        // - Gérer la notion de force/faiblesse avec les différentes attaques à disposition (skills.cs)
        // - Cumuler les force/faiblesses en ajoutant un type pour l'équipement qui rendrait plus sensible/résistant à un type


        //Tests Types
        [Test]
        public void GetRightFactor()
        {
            Assert.That(TypeResolver.GetFactor(TYPE.NORMAL, TYPE.FIRE), Is.EqualTo(1f));
            Assert.That(TypeResolver.GetFactor(TYPE.NORMAL, TYPE.NORMAL), Is.EqualTo(1f));
            Assert.That(TypeResolver.GetFactor(TYPE.FIRE, TYPE.FIRE), Is.EqualTo(1f));
            Assert.That(TypeResolver.GetFactor(TYPE.GRASS, TYPE.FIRE), Is.EqualTo(0.8f));
            Assert.That(TypeResolver.GetFactor(TYPE.FIRE, TYPE.GRASS), Is.EqualTo(1.2f));
            Assert.That(TypeResolver.GetFactor(TYPE.WATER, TYPE.GRASS), Is.EqualTo(0.8f));
        }

        [Test]
        public void CharacterReceiveAttackFromNormalType()
        {
            Character pikachu = new Character(180, 60, 20, 70, TYPE.NORMAL);
            Character grookey = new Character(300, 30, 40, 50, TYPE.GRASS);
            var grookeyOldHeath = grookey.CurrentHealth;
            Punch punch = new Punch();

            grookey.ReceiveAttack(pikachu, punch);

            Assert.That(grookey.CurrentHealth,
                Is.EqualTo((int)(grookeyOldHeath - (punch.Power + pikachu.Attack - grookey.Defense) * TypeResolver.GetFactor(punch.Type, grookey.BaseType))));
            //300 - (70 + 60 - 40) * 1;
        }

        [Test]
        public void CharacterReceiveAttackFromSameType()
        {
            Character bulbasaur = new Character(180, 40, 20, 70, TYPE.GRASS);
            Character grookey = new Character(300, 30, 40, 50, TYPE.GRASS);
            var grookeyOldHeath = grookey.CurrentHealth;
            MagicalGrass magical = new MagicalGrass();

            grookey.ReceiveAttack(bulbasaur, magical);

            Assert.That(grookey.CurrentHealth,
                Is.EqualTo((int)(grookeyOldHeath - (magical.Power + bulbasaur.Attack - grookey.Defense) * TypeResolver.GetFactor(magical.Type, grookey.BaseType))));
            //300 - (70 + 40 - 40) * 1;
        }

        [Test]
        public void CharacterReceiveAttackFromVulnerableType()
        {
            Character scorbunny = new Character(180, 50, 20, 70, TYPE.FIRE);
            Character grookey = new Character(300, 30, 40, 50, TYPE.GRASS);
            var grookeyOldHeath = grookey.CurrentHealth;
            FireBall fireBall = new FireBall();

            grookey.ReceiveAttack(scorbunny, fireBall);

            Assert.That(grookey.CurrentHealth,
                Is.EqualTo((int)(grookeyOldHeath - (fireBall.Power + scorbunny.Attack - grookey.Defense) * TypeResolver.GetFactor(fireBall.Type, grookey.BaseType))));
            //300 - (50 + 50 - 40) * 1.2;
        }

        [Test]
        public void CharacterReceiveAttackFromResistType()
        {
            Character sobble = new Character(180, 40, 20, 70, TYPE.WATER);
            Character grookey = new Character(300, 30, 40, 50, TYPE.GRASS);
            var grookeyOldHeath = grookey.CurrentHealth;
            WaterBlouBlou w = new WaterBlouBlou();

            grookey.ReceiveAttack(sobble, w);

            Assert.That(grookey.CurrentHealth,
                Is.EqualTo((int)(grookeyOldHeath - (w.Power + sobble.Attack - grookey.Defense) * TypeResolver.GetFactor(w.Type, grookey.BaseType))));
            //300 - (20 + 40 - 40) * 0.8;
        }

        //Test Status
        [Test]
        public void ApplyStatus()
        {
            Character scorbunny = new Character(180, 50, 20, 70, TYPE.FIRE);
            Character grookey = new Character(300, 30, 40, 50, TYPE.GRASS);
            FireBall fireBall = new FireBall();
            MagicalGrass magical = new MagicalGrass();

            Fight f = new Fight(scorbunny, grookey);
            f.ExecuteTurn(fireBall, magical);

            Assert.That(scorbunny.CurrentStatus.GetType(), Is.EqualTo(StatusEffect.GetNewStatusEffect(magical.Status).GetType()));
            Assert.That(grookey.CurrentStatus.GetType(), Is.EqualTo(StatusEffect.GetNewStatusEffect(fireBall.Status).GetType()));
        }

        [Test]
        public void ReplaceStatus()
        {
            Character mew = new Character(180, 60, 20, 70, TYPE.NORMAL);
            Character grookey = new Character(300, 30, 40, 50, TYPE.GRASS);
            FireBall f = new FireBall();
            MagicalGrass m = new MagicalGrass();

            grookey.ReceiveAttack(mew, f);

            Assert.That(grookey.CurrentStatus.GetType(), Is.EqualTo(StatusEffect.GetNewStatusEffect(f.Status).GetType()));

            grookey.ReceiveAttack(mew, m);

            Assert.That(grookey.CurrentStatus.GetType(), Is.EqualTo(StatusEffect.GetNewStatusEffect(m.Status).GetType()));
        }
    }
}
