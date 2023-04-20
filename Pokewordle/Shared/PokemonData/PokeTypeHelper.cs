using Microsoft.Extensions.Primitives;
using System.Collections.Immutable;

namespace Pokewordle.Shared.PokemonData
{
    public static class PokeTypeHelper
    {
        public enum PokemonType
        {
            Normal,
            Fire,
            Water,
            Electric,
            Grass,
            Ice,
            Fighting,
            Poison,
            Ground,
            Flying,
            Psychic,
            Bug,
            Rock,
            Ghost,
            Dragon,
            Dark,
            Steel,
            Fairy
        }

        static double[,] typeDamageMultipliers = new double[,]
        {
        // defender     Normal   Fire   Water  Electric  Grass  Ice   Fighting  Poison  Ground  Flying  Psychic Bug  Rock  Ghost Dragon  Dark  Steel  Fairy
        /* attacker */
        /* Normal   */ {    1,     1,     1,      1,      1,    1,      1,       1,      1,      1,      1,    1,    0.5,   0,    1,     1,     0.5,   1   },
        /* Fire     */ {    1,     0.5,   0.5,    1,      2,    2,      1,       1,      1,      1,      1,    2,    0.5,   1,    0.5,   1,     2,     1   },
        /* Water    */ {    1,     2,     0.5,    1,      0.5,  1,      1,       1,      2,      1,      1,    1,    2,     1,    1,     1,     1,     1   },
        /* Electric */ {    1,     1,     2,      0.5,    0.5,  1,      1,       1,      0,      2,      1,    1,    1,     1,    1,     1,     0.5,   1   },
        /* Grass    */ {    1,     0.5,   2,      1,      0.5,  1,      1,       0.5,    2,      0.5,    1,    0.5,  2,     1,    0.5,   1,     0.5,   1   },
        /* Ice      */ {    1,     0.5,   0.5,    1,      2,    0.5,    1,       1,      2,      2,      1,    1,    1,     1,    2,     1,     0.5,   1   },
        /* Fighting */ {    2,     1,     1,      1,      1,    2,      1,       0.5,    1,      0.5,    0.5,  0.5,  2,     0,    1,     2,     2,     0.5 },
        /* Poison   */ {    1,     1,     1,      1,      2,    1,      1,       0.5,    0.5,    1,      1,    1,    0.5,   0.5,  1,     1,     0.5,   2   },
        /* Ground   */ {    1,     2,     1,      2,      0.5,  1,      1,       2,      1,      0,      1,    0.5,  2,     1,    1,     1,     2,     1   },
        /* Flying   */ {    1,     1,     1,      0.5,    2,    1,      2,       1,      1,      1,      1,    2,    0.5,   1,    1,     1,     0.5,   1   },
        /* Psychic  */ {    1,     1,     1,      1,      1,    1,      2,       2,      1,      1,      0.5,  1,    1,     1,    1,     0,     0.5,   1   },
        /* Bug      */ {    1,     0.5,   1,      1,      2,    1,      0.5,     0.5,    1,      0.5,    2,    1,    1,     0.5,  1,     2,     0.5,   0.5 },
        /* Rock     */ {    1,     2,     1,      1,      1,    2,      0.5,     1,      0.5,    2,      1,    2,    1,     1,    1,     1,     0.5,   1   },
        /* Ghost    */ {    0,     1,     1,      1,      1,    1,      1,       1,      1,      1,      2,    1,    1,     2,    1,     0.5,   1,     1   },
        /* Dragon   */ {    1,     1,     1,      1,      1,    1,      1,       1,      1,      1,      1,    1,    1,     1,    2,     1,     0.5,   0   },
        /* Dark     */ {    1,     1,     1,      1,      1,    1,      0.5,     1,      1,      1,      2,    1,    1,     2,    1,     0.5,   1,     0.5 },
        /* Steel    */ {    1,     0.5,   0.5,    0.5,    1,    2,      1,       1,      1,      1,      1,    1,    2,     1,    1,     0.5,   1,     2   },
        /* Fairy    */ {    1,     0.5,   1,      1,      1,    1,      2,       0.5,    1,      1,      1,    1,    1,     1,    2,     2,     0.5,   1   },
        };

        public static readonly IImmutableList<string> AllTypes = ImmutableList.CreateRange(Enum.GetNames<PokemonType>().ToList().ConvertAll(str => str.ToLower()));
        public static readonly IImmutableDictionary<string, IImmutableList<string>> Weaknesses;
        public static readonly IImmutableDictionary<string, IImmutableList<string>> Effectives;
        public static readonly IImmutableDictionary<string, IImmutableList<string>> Resistances;
        public static readonly IImmutableDictionary<string, IImmutableList<string>> Immunities;
        public static readonly IImmutableDictionary<string, IImmutableList<string>> ResistancesAndImmunities;

        static PokeTypeHelper()
        {
            var weaknessesDictBuilder = ImmutableDictionary.CreateBuilder<string, IImmutableList<string>>();
            var effectivesDictBuilder = ImmutableDictionary.CreateBuilder<string, IImmutableList<string>>();
            var resistancesDictBuilder = ImmutableDictionary.CreateBuilder<string, IImmutableList<string>>();
            var immunitiesDictBuilder = ImmutableDictionary.CreateBuilder<string, IImmutableList<string>>();
            var resistancesimmunitiesDictBuilder = ImmutableDictionary.CreateBuilder<string, IImmutableList<string>>();
            foreach (string type in AllTypes)
            {
                GetTypeRelations(type,
                    out IImmutableList<string> weaknesses,
                    out IImmutableList<string> effectives,
                    out IImmutableList<string> resistances,
                    out IImmutableList<string> immunities);
                weaknessesDictBuilder.Add(type, weaknesses);
                effectivesDictBuilder.Add(type, effectives);
                resistancesDictBuilder.Add(type, resistances);
                immunitiesDictBuilder.Add(type, immunities);
                resistancesimmunitiesDictBuilder.Add(type, resistances.Union(immunities).ToImmutableList());
            }
            Weaknesses = weaknessesDictBuilder.ToImmutable();
            Effectives = effectivesDictBuilder.ToImmutable();
            Resistances = resistancesDictBuilder.ToImmutable();
            Immunities = immunitiesDictBuilder.ToImmutable();
            ResistancesAndImmunities = resistancesimmunitiesDictBuilder.ToImmutable();
        }

        public static double GetMultiplier(string targetType, string attackType)
        {
            int targetIndex = AllTypes.IndexOf(targetType);
            int attackIndex = AllTypes.IndexOf(attackType);
            if (targetIndex >= 0 && attackIndex >= 0)
            {
                return typeDamageMultipliers[attackIndex, targetIndex];
            }
            throw new ArgumentException($"At least one of those was not a valid type: targetType:\"{targetType}\", attackType:\"{attackType}\".");
        }

        public static double GetMultiplier(string targetType1, string targetType2, string attackType)
        {
            int target1Index = AllTypes.IndexOf(targetType1);
            int target2Index = AllTypes.IndexOf(targetType2);
            int attackIndex = AllTypes.IndexOf(attackType);
            if (target1Index >= 0 && target2Index >= 0 && attackIndex >= 0)
            {
                return typeDamageMultipliers[attackIndex, target1Index] * typeDamageMultipliers[attackIndex, target2Index];
            }
            throw new ArgumentException($"At least one of those was not a valid type: targetType1:\"{targetType1}\", targetType2:\"{targetType2}\", attackType:\"{attackType}\".");
        }

        public static List<string> GetWeaknesses(string type1, string type2)
        {
            List<string> result = new();
            foreach(string attackType in AllTypes)
            {
                if (GetMultiplier(type1, type2, attackType) > 1d)
                {
                    result.Add(attackType);
                }
            }
            return result;
        }

        public static List<string> GetEffectives(string type1, string type2)
        {
            List<string> result = new();
            foreach (string attackType in AllTypes)
            {
                if (GetMultiplier(type1, type2, attackType) == 1d)
                {
                    result.Add(attackType);
                }
            }
            return result;
        }

        public static List<string> GetResistances(string type1, string type2)
        {
            List<string> result = new();
            foreach (string attackType in AllTypes)
            {
                double multiplier = GetMultiplier(type1, type2, attackType);
                if (0 < multiplier && multiplier < 1d)
                {
                    result.Add(attackType);
                }
            }
            return result;
        }

        public static List<string> GetImmunities(string type1, string type2)
        {
            List<string> result = new();
            foreach (string attackType in AllTypes)
            {
                if (GetMultiplier(type1, type2, attackType) == 0d)
                {
                    result.Add(attackType);
                }
            }
            return result;
        }

        public static List<string> GetResistancesAndImmunities(string type1, string type2)
        {
            List<string> result = new();
            foreach (string attackType in AllTypes)
            {
                if (GetMultiplier(type1, type2, attackType) < 1d)
                {
                    result.Add(attackType);
                }
            }
            return result;
        }

        private static void GetTypeRelations(string targetType,
            out IImmutableList<string> weaknesses,
            out IImmutableList<string> effectives,
            out IImmutableList<string> resistances,
            out IImmutableList<string> immunities
            )
        {
            ImmutableList<string>.Builder weaknessBuilder = ImmutableList.CreateBuilder<string>();
            ImmutableList<string>.Builder effectiveBuilder = ImmutableList.CreateBuilder<string>();
            ImmutableList<string>.Builder resistanceBuilder = ImmutableList.CreateBuilder<string>();
            ImmutableList<string>.Builder immunityBuilder = ImmutableList.CreateBuilder<string>();
            foreach(string attackType in AllTypes)
            {
                double multiplier = GetMultiplier(targetType, attackType);
                switch(multiplier)
                {
                    case 0: immunityBuilder.Add(attackType); break;
                    case 0.5: resistanceBuilder.Add(attackType); break;
                    case 1: effectiveBuilder.Add(attackType); break;
                    case 2: weaknessBuilder.Add(attackType); break;
                }
            }

            weaknesses = weaknessBuilder.ToImmutable();
            effectives = effectiveBuilder.ToImmutable();
            resistances = resistanceBuilder.ToImmutable();
            immunities = immunityBuilder.ToImmutable();
        }
    }

}
