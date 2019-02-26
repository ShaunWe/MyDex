using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MyDex
{
    class Program
    {
        // MySQL Database Connection String
        static string cs = @"server=0.0.0.0;userid=dbremoteuser;password=password;database=MyDex;port=8889";

        static void Main(string[] args)
        {
            /*
             * Shaun Wehe
             * This app is designed to pull various info about Pokemon and to allow the creation of teams
             * complete with natures, items, abilities, and stats.
             */
            string inputLine;
            string menu = "1. Review Pokedex\n" +
                "2. Search\n" +
                "3. Filter\n" +
                "4. Teams\n" +
                "5. Moves\n" +
                "6. Options\n" +
                "7. Exit";
            int inputInt;
            bool programRunning = true;

            while (programRunning)
            {
                Console.Clear();
                Console.WriteLine("--- MyDex ---");
                Console.WriteLine(menu);
                Console.Write("\n Selection: ");
                inputLine = Console.ReadLine().ToLower();

                switch (inputLine)
                {
                    case "1":
                    case "review pokedex":
                        ReviewPokedex();
                        break;
                    case "2":
                    case "search":
                        Search();
                        break;
                    case "3":
                    case "filter":
                        Filter();
                        break;
                    case "4":
                    case "teams":
                        Teams();
                        break;
                    case "5":
                    case "moves":
                        Moves();
                        break;
                    case "6":
                    case "options":
                        Options();
                        break;
                    case "7":
                    case "exit":
                        programRunning = false;
                        break;
                    default:
                        Console.WriteLine("Entry not recognized...");
                        Utility.KeyToProceed();
                        break;
                }
            }
        }

        public static void ReviewPokedex()
        {
            using (MySqlConnection conn = new MySqlConnection(cs))
            {
                conn.Open();
                MySqlDataReader rdr = null;

                string stm = "SELECT NationalDexNumber, Name, Type1, Type2, Description, Height, Weight, Generation, Favorite, Sound FROM Pokemon";

                MySqlCommand cmd = new MySqlCommand(stm, conn);
                rdr = cmd.ExecuteReader();
                Console.Clear();
                int infoBreaker = 0;
                while (rdr.Read())
                {
                    infoBreaker++;
                    Console.WriteLine($"Dex Number: {rdr["NationalDexNumber"]}\n" +
                        $"Name: {rdr["Name"]}");
                    if (rdr["Type2"] as string == "Null")
                    {
                        Console.WriteLine($"Type: {rdr["Type1"]}");
                    }
                    else
                    {
                        Console.WriteLine($"Types: {rdr["Type1"]}\t{rdr["Type2"]}");
                    }
                    Console.WriteLine($"Height: {rdr["Height"]}\tWeight: {rdr["Weight"]}\n" +
                        $"Description: {rdr["Description"]}\n" +
                        $"Generation: {rdr["Generation"]}");
                    if (rdr["Favorite"] as string == "TRUE")
                    {
                        Console.WriteLine($"Favorite: Yes\tSound: {rdr["Sound"]}");
                    }
                    else
                    {
                        Console.WriteLine($"Favorite: No\tSound: {rdr["Sound"]}");
                    }
                    if (infoBreaker >= 3)
                    {
                        Utility.KeyToProceed();
                        infoBreaker = 0;
                    }
                }
            }
        }

        public static void Search()
        {
            Console.WriteLine("Search function currently under construction");
            Utility.KeyToProceed();
        }

        public static void Filter()
        {
            Console.WriteLine("Filter function currently under construction");
            Utility.KeyToProceed();
        }

        public static void Teams()
        {

        }

        public static void Moves()
        {
            using (MySqlConnection conn = new MySqlConnection(cs))
            {
                conn.Open();
                MySqlDataReader rdr = null;

                string stm = "SELECT Name, Power, Accuracy, PP, Type1, Type2, Phys-Spec-Stat, Generation FROM Move";

                MySqlCommand cmd = new MySqlCommand(stm, conn);
                rdr = cmd.ExecuteReader();
                Console.Clear();
                int infoBreaker = 0;
                while (rdr.Read())
                {
                    infoBreaker++;
                    Console.WriteLine($"Name: {rdr["Name"]}\tGeneration: {rdr["Generation"]}\n" +
                        $"Power: {rdr["Power"]}\tAccuracy: {rdr["Accuracy"]}");
                    if (rdr["Phys-Spec-Stat"] as string == "Phys")
                    {
                        Console.WriteLine($"PP: {rdr["PP"]}\tPhysical Move");
                    }
                    else if (rdr["Phys-Spec-Stat"] as string == "Spec")
                    {
                        Console.WriteLine($"PP: {rdr["PP"]}\tSpecial Move");
                    }
                    else
                    {
                        Console.WriteLine($"PP: {rdr["PP"]}\tStatus Move");
                    }
                    if (!(rdr["Type2"] as string == "Null"))
                    {
                        Console.WriteLine($"Types: {rdr["Type1"]}\t{rdr["Type2"]}");
                    }
                    else
                    {
                        Console.WriteLine($"Type: {rdr["Type1"]}");
                    }
                    if (infoBreaker >= 3)
                    {
                        Utility.KeyToProceed();
                        infoBreaker = 0;
                    }
                }
            }
        }

        public static void Options()
        {

        }

        public static PokeType DetermineType(string typeString)
        {
            PokeType typeValue = PokeType.NULL;
            string testString = typeString.ToLower();
            switch (testString)
            {
                case "normal":
                    typeValue = PokeType.NORMAL;
                    break;
                case "fire":
                    typeValue = PokeType.FIRE;
                    break;
                case "water":
                    typeValue = PokeType.WATER;
                    break;
                case "grass":
                    typeValue = PokeType.GRASS;
                    break;
                case "electric":
                    typeValue = PokeType.ELECTRIC;
                    break;
                case "flying":
                    typeValue = PokeType.FLYING;
                    break;
                case "bug":
                    typeValue = PokeType.BUG;
                    break;
                case "poison":
                    typeValue = PokeType.POISON;
                    break;
                case "psychic":
                    typeValue = PokeType.PSYCHIC;
                    break;
                case "fighting":
                    typeValue = PokeType.FIGHTING;
                    break;
                case "ice":
                    typeValue = PokeType.ICE;
                    break;
                case "dragon":
                    typeValue = PokeType.DRAGON;
                    break;
                case "rock":
                    typeValue = PokeType.ROCK;
                    break;
                case "ground":
                    typeValue = PokeType.GROUND;
                    break;
                case "ghost":
                    typeValue = PokeType.GHOST;
                    break;
                case "dark":
                    typeValue = PokeType.DARK;
                    break;
                case "steel":
                    typeValue = PokeType.STEEL;
                    break;
                case "fairy":
                    typeValue = PokeType.FAIRY;
                    break;
                default:
                    typeValue = PokeType.NULL;
                    break;
            }
            return typeValue;
        }

        public static PokeStat DetermineStatType(string typeString)
        {
            PokeStat statValue = PokeStat.NONE;
            string testString = typeString.ToLower();
            switch (testString)
            {
                case "hp":
                    statValue = PokeStat.HP;
                    break;
                case "attack":
                case "att":
                    statValue = PokeStat.ATTACK;
                    break;
                case "defense":
                case "def":
                    statValue = PokeStat.DEFENSE;
                    break;
                case "spattack":
                case "special attack":
                case "spa":
                    statValue = PokeStat.SPATTACK;
                    break;
                case "spdefense":
                case "special defense":
                case "spd":
                    statValue = PokeStat.SPDEFENSE;
                    break;
                case "speed":
                case "spe":
                    statValue = PokeStat.SPEED;
                    break;
                default:
                    statValue = PokeStat.NONE;
                    break;
            }
            return statValue;
        }

    }

    enum PokeType
    {
        NULL,
        NORMAL,
        FIRE,
        WATER,
        GRASS,
        ELECTRIC,
        FLYING,
        BUG,
        POISON,
        PSYCHIC,
        FIGHTING,
        ICE,
        DRAGON,
        ROCK,
        GROUND,
        GHOST,
        DARK,
        STEEL,
        FAIRY
    }

    enum PokeStat
    {
        NONE,
        HP,
        ATTACK,
        DEFENSE,
        SPATTACK,
        SPDEFENSE,
        SPEED
    }
}
