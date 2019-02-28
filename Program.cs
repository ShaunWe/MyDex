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
        static string cs = @"server=10.0.0.101;userid=dbremoteuser;password=password;database=MyDex;port=8889";

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
                Console.Write("\nSelection: ");
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
                    Console.WriteLine();
                    if (infoBreaker >= 3)
                    {
                        Utility.KeyToProceed();
                        infoBreaker = 0;
                        Console.Clear();
                    }
                }
                Utility.KeyToProceed();
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
            //Gather the teams that are currently in the database
            string inputLine;
            int teamSelection = 0;
            bool teamSelectionMade = false;
            Dictionary<int, string> listedTeams = new Dictionary<int, string>();
            using (MySqlConnection conn = new MySqlConnection(cs))
            {
                conn.Open();
                MySqlDataReader rdr = null;

                string stm = "SELECT TeamID, TeamName FROM Teams";

                string inputID = "", inputName = "";
                int ID = 0;
                MySqlCommand cmd = new MySqlCommand(stm, conn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    inputName = rdr["TeamName"] as string;
                    inputID = rdr["TeamID"] as string;
                    while (!int.TryParse(inputID, out ID))
                    {
                        Console.WriteLine($"ID not recognized.\n" +
                            $"Please input the number seen here: {rdr["TeamID"]}");
                        inputID = Console.ReadLine();
                    }
                    listedTeams.Add(ID, inputName);
                }
            }

            while (!teamSelectionMade)
            {
                //Display all available teams
                Console.Clear();
                foreach (KeyValuePair<int, string> kvp in listedTeams)
                {
                    Console.WriteLine($"{kvp.Key}. {kvp.Value}");
                }
                //Add options for back and add options
                Console.WriteLine($"{listedTeams.Count + 1}. Add new team");
                Console.WriteLine($"{listedTeams.Count + 2}. Back");
                Console.Write("\nSelection: ");

                inputLine = Console.ReadLine().ToLower();
                //Adding new team
                if (inputLine == $"{listedTeams.Count + 1}" || inputLine == "add new team")
                {
                    inputLine = "";
                    Console.Clear();
                    Console.Write("What is the name of the new team? ");
                    inputLine = Console.ReadLine();
                    while (string.IsNullOrWhiteSpace(inputLine))
                    {
                        Console.WriteLine("There needs to be a team name. Please enter a team name for the new team.");
                        Console.Write("Team: ");
                        inputLine = Console.ReadLine();
                    }
                    teamSelection = AddNewTeam(inputLine);
                    teamSelectionMade = true;
                }
                //Going back
                if (inputLine == $"{listedTeams.Count + 2}" || inputLine == "back")
                {
                    return;
                }

                foreach (KeyValuePair<int, string> kvp in listedTeams)
                {
                    if (inputLine == $"{kvp.Key}" || inputLine == $"{kvp.Value.ToLower()}")
                    {
                        teamSelection = kvp.Key;
                        teamSelectionMade = true;
                    }
                }

                if (!teamSelectionMade)
                {
                    Console.WriteLine("Entry not recognized...");
                    Utility.KeyToProceed();
                }
            }

            TeamScreen(teamSelection);
        }

        public static int AddNewTeam(string teamName)
        {
            int teamID = 0;
            using (MySqlConnection conn = new MySqlConnection(cs))
            {
                conn.Open();
                MySqlDataReader rdr = null;

                string stm = "INSERT INTO Teams (TeamName) VALUES (@teamName)";

                MySqlCommand cmd = new MySqlCommand(stm, conn);
                cmd.Parameters.AddWithValue("@teamName", teamName);

                rdr = cmd.ExecuteReader();

                //Get teamID of newly created team to return
                stm = "SELECT TeamID, TeamName FROM Teams WHERE TeamName = @teamName";

                MySqlConnection conn2 = new MySqlConnection(cs);
                conn2.Open();
                MySqlCommand cmd2 = new MySqlCommand(stm, conn2);
                cmd2.Parameters.AddWithValue("@teamName", teamName);
                MySqlDataReader rdr2 = cmd2.ExecuteReader();

                while (rdr2.Read())
                {
                    string readID = rdr2["TeamID"] as string;
                    while (!int.TryParse(readID, out teamID))
                    {
                        Console.WriteLine($"ID not recognized.\n" +
                            $"Please input the number seen here: {rdr2["TeamID"]}");
                        readID = Console.ReadLine();
                    }
                }
            }
            return teamID;
        }

        public static void TeamScreen(int teamNumberID)
        {
            using (MySqlConnection conn = new MySqlConnection(cs))
            {
                conn.Open();
                MySqlDataReader rdr = null;

                string stm = "SELECT Teams.TeamName AS TeamName, Pokemon.Name AS PokemonName, mv1.Name AS Move1Name, mv2.Name AS Move2Name, mv3.Name AS Move3Name, mv4.Name AS Move4Name, Item.Name AS ItemName, Ability.Name AS AbilityName, Nature.Name AS NatureName " +
                    "FROM Team " +
                    "LEFT JOIN Teams ON Teams.TeamID = Team.TeamID " +
                    "LEFT JOIN Pokemon ON Pokemon.PokemonID = Team.PokemonID " +
                    "LEFT JOIN Move AS mv1 ON mv1.MoveID = Team.Move1ID " +
                    "LEFT JOIN Move AS mv2 ON mv2.MoveID = Team.Move2ID " +
                    "LEFT JOIN Move AS mv3 ON mv3.MoveID = Team.Move3ID " +
                    "LEFT JOIN Move AS mv4 ON mv4.MoveID = Team.Move4ID " +
                    "LEFT JOIN Item ON Item.ItemID = Team.ItemID " +
                    "LEFT JOIN Ability ON Ability.AbilityID = Team.AbilityID " +
                    "LEFT JOIN Nature ON Nature.NatureID = Team.NatureID " +
                    "WHERE Team.TeamID = @teamNumberID";
                
                MySqlCommand cmd = new MySqlCommand(stm, conn);
                cmd.Parameters.AddWithValue("@teamNumberID", teamNumberID);
                rdr = cmd.ExecuteReader();
                bool firstItem = true;
                int number = 1;
                Console.Clear();
                while (rdr.Read())
                {
                    if (firstItem)
                    {
                        Console.WriteLine($"==={rdr["TeamName"]}===\n");
                        Console.WriteLine($"Pokemon {number}: {rdr["PokemonName"]}\n" +
                            $"Ability: {rdr["AbilityName"]}\n" +
                            $"Nature: {rdr["NatureName"]}\tItem: {rdr["ItemName"]}\n\n" +
                            $"Move 1: {rdr["Move1Name"]}\tMove 3: {rdr["Move3Name"]}\n" +
                            $"Move 2: {rdr["Move2Name"]}\tMove 4: {rdr["Move4Name"]}\n");
                    }
                    else
                    {
                        Console.WriteLine($"Pokemon: {rdr["PokemonName"]}\n" +
                            $"Ability: {rdr["AbilityName"]}\n" +
                            $"Nature: {rdr["NatureName"]}\tItem: {rdr["ItemName"]}\n\n" +
                            $"Move 1: {rdr["Move1Name"]}\tMove 3: {rdr["Move3Name"]}\n" +
                            $"Move 2: {rdr["Move2Name"]}\tMove 4: {rdr["Move4Name"]}\n");
                    }
                    number++;
                }
                Utility.KeyToProceed();
            }
        }

        public static void Moves()
        {
            using (MySqlConnection conn = new MySqlConnection(cs))
            {
                conn.Open();
                MySqlDataReader rdr = null;

                string stm = "SELECT Name, Power, Accuracy, PP, Type1, Type2, \"Phys-Spec-Stat\", Generation FROM Move";

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
                        Console.WriteLine($"PP: {rdr["PP"]}\t\tStatus Move");
                    }
                    if (!(rdr["Type2"] as string == "Null"))
                    {
                        Console.WriteLine($"Types: {rdr["Type1"]}\t{rdr["Type2"]}");
                    }
                    else
                    {
                        Console.WriteLine($"Type: {rdr["Type1"]}");
                    }
                    Console.WriteLine();
                    if (infoBreaker >= 3)
                    {
                        Utility.KeyToProceed();
                        infoBreaker = 0;
                        Console.Clear();
                    }
                }
                Utility.KeyToProceed();
            }
        }

        public static void Options()
        {
            string menu = "1. Items\n" +
                "2. Abilities\n" +
                "3. Natures\n" +
                "4. Show Favorites\n" +
                "5. Settings\n" +
                "6. Upgrade\n" +
                "7. Back";
            bool menuRunning = true;
            string inputLine;

            while (menuRunning)
            {
                Console.Clear();
                Console.WriteLine(menu);
                Console.Write("\nSelection: ");

                inputLine = Console.ReadLine().ToLower();
                switch (inputLine)
                {
                    case "1":
                    case "items":
                        Items();
                        break;
                    case "2":
                    case "abilities":
                        Abilities();
                        break;
                    case "3":
                    case "natures":
                        Natures();
                        break;
                    case "4":
                    case "show favorites":
                        ShowFavorites();
                        break;
                    case "5":
                    case "settings":
                        Settings();
                        break;
                    case "6":
                    case "upgrade":
                        Upgrade();
                        break;
                    case "7":
                    case "back":
                    case "exit":
                        menuRunning = false;
                        break;
                    default:
                        Console.WriteLine("Entry not recognized...");
                        Utility.KeyToProceed();
                        break;
                }
            }
        }

        public static void Items()
        {
            using (MySqlConnection conn = new MySqlConnection(cs))
            {
                conn.Open();
                MySqlDataReader rdr = null;

                string stm = "SELECT Name, Description FROM Item";

                MySqlCommand cmd = new MySqlCommand(stm, conn);
                rdr = cmd.ExecuteReader();
                Console.Clear();
                int infoBreaker = 0;
                while (rdr.Read())
                {
                    infoBreaker++;
                    Console.WriteLine($"Name: {rdr["Name"]}\n" +
                        $"Description: {rdr["Description"]}");
                    Console.WriteLine();
                    if (infoBreaker >= 4)
                    {
                        Utility.KeyToProceed();
                        infoBreaker = 0;
                        Console.Clear();
                    }
                }
                Utility.KeyToProceed();
            }
        }

        public static void Abilities()
        {
            using (MySqlConnection conn = new MySqlConnection(cs))
            {
                conn.Open();
                MySqlDataReader rdr = null;

                string stm = "SELECT Name, Description, Generation FROM Ability";

                MySqlCommand cmd = new MySqlCommand(stm, conn);
                rdr = cmd.ExecuteReader();
                Console.Clear();
                int infoBreaker = 0;
                while (rdr.Read())
                {
                    infoBreaker++;
                    Console.WriteLine($"Name: {rdr["Name"]}\tGeneration: {rdr["Generation"]}\n" +
                        $"Description: {rdr["Description"]}");
                    Console.WriteLine();
                    if (infoBreaker >= 4)
                    {
                        Utility.KeyToProceed();
                        infoBreaker = 0;
                        Console.Clear();
                    }
                }
                Utility.KeyToProceed();
            }
        }

        public static void Natures()
        {
            using (MySqlConnection conn = new MySqlConnection(cs))
            {
                conn.Open();
                MySqlDataReader rdr = null;

                string stm = "SELECT Name, StatIncreased, StatDecreased FROM Nature";

                MySqlCommand cmd = new MySqlCommand(stm, conn);
                rdr = cmd.ExecuteReader();
                Console.Clear();
                int infoBreaker = 0;
                while (rdr.Read())
                {
                    infoBreaker++;
                    Console.WriteLine($"Name: {rdr["Name"]}\n" +
                        $"Stat Increased: {rdr["StatIncreased"]}\tStat Decreased: {rdr["StatDecreased"]}");
                    Console.WriteLine();
                    if (infoBreaker >= 5)
                    {
                        Utility.KeyToProceed();
                        infoBreaker = 0;
                        Console.Clear();
                    }
                }
                Utility.KeyToProceed();
            }
        }

        public static void ShowFavorites()
        {
            Console.WriteLine("Show favorites function currently under construction");
            Utility.KeyToProceed();
        }

        public static void Settings()
        {
            Console.WriteLine("Settings function currently under construction");
            Utility.KeyToProceed();
        }

        public static void Upgrade()
        {
            Console.WriteLine("Upgrade function currently under construction");
            Utility.KeyToProceed();
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
