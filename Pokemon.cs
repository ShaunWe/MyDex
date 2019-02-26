using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDex
{
    class Pokemon
    {
        int _number, _item, _ability, _nature, _level;
        string _name;
        int[] _moves = new int[4];
        int[] _baseStats = new int[6];
        int[] _IVStats = new int[6];
        int[] _EVStats = new int[6];
        PokeType _type1;
        PokeType _type2;

        public Pokemon(int number, string name, PokeType type1, PokeType type2, int[] baseStats)
        {
            _number = number;
            _name = name;
            _type1 = type1;
            _type2 = type2;
            _baseStats = baseStats;
        }

        public int Number
        {
            get { return _number; }
        }

        public string Name
        {
            get { return _name; }
        }

        public PokeType Type1
        {
            get { return _type1; }
        }

        public PokeType Type2
        {
            get { return _type2; }
        }

        public int[] BaseStats
        {
            get { return _baseStats; }
        }

        public int[] Moves
        {
            get { return _moves; }
            set { _moves = value; }
        }

        public int[] IVStats
        {
            get { return _IVStats; }
            set { _IVStats = value; }
        }

        public int[] EVStats
        {
            get { return _EVStats; }
            set { _EVStats = value; }
        }

        public int Item
        {
            get { return _item; }
            set { _item = value; }
        }

        public int Ability
        {
            get { return _ability; }
            set { _ability = value; }
        }

        public int Nature
        {
            get { return _nature; }
            set { _nature = value; }
        }

        public int Level
        {
            get { return _level; }
            set { _level = value; }
        }

        public int GetStat (PokeStat statWanted)
        {
            if (statWanted == PokeStat.NONE)
            {
                Console.WriteLine("Invalid stat value.");
                Utility.KeyToProceed();
                return 0;
            }

            int index = (int)statWanted - 1;
            double transValue = 0;
            int statValue = 0;

            if (index == 0)
            {
                statValue = (int)(Math.Floor((((2 * _baseStats[index]) + _IVStats[index] + Math.Floor(_EVStats[index] / 4.0)) * _level) / 100) + _level + 10);
            }
            else
            {
                transValue = Math.Floor((((2 * _baseStats[index]) + _IVStats[index] + Math.Floor(_EVStats[index] / 4.0)) * _level) / 100) + 5;
                if (!(GetStatIncreased() == GetStatDecreased()))
                {
                    if ((int)GetStatIncreased()-1 == index)
                    {
                        transValue *= 1.1;
                    }
                    if ((int)GetStatDecreased() - 1 == index)
                    {
                        transValue *= 0.9;
                    }
                }
                statValue = (int)Math.Floor(transValue);
            }
            return statValue;
        }

        public PokeStat GetStatIncreased()
        {
            PokeStat statIncreased = PokeStat.NONE;
            switch (_nature)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    statIncreased = PokeStat.ATTACK;
                    break;
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                    statIncreased = PokeStat.DEFENSE;
                    break;
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                    statIncreased = PokeStat.SPEED;
                    break;
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                    statIncreased = PokeStat.SPATTACK;
                    break;
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                    statIncreased = PokeStat.SPDEFENSE;
                    break;
            }

            return statIncreased;
        }

        public PokeStat GetStatDecreased()
        {
            PokeStat statDecreased = PokeStat.NONE;
            switch (_nature)
            {
                case 1:
                case 6:
                case 11:
                case 16:
                case 21:
                    statDecreased = PokeStat.ATTACK;
                    break;
                case 2:
                case 7:
                case 12:
                case 17:
                case 22:
                    statDecreased = PokeStat.DEFENSE;
                    break;
                case 3:
                case 8:
                case 13:
                case 18:
                case 23:
                    statDecreased = PokeStat.SPEED;
                    break;
                case 4:
                case 9:
                case 14:
                case 19:
                case 24:
                    statDecreased = PokeStat.SPATTACK;
                    break;
                case 5:
                case 10:
                case 15:
                case 20:
                case 25:
                    statDecreased = PokeStat.SPDEFENSE;
                    break;
            }
            return statDecreased;
        }
    }
}
