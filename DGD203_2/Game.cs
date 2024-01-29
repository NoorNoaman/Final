using System.Numerics;

namespace DGD203_2
{
    public class Game
    {
        #region VARIABLES


        #region Game Constants

        private const int _defaultMapWidth = 5;
        private const int _defaultMapHeight = 5;

        #endregion

        #region Game Variables

        #region Player Variables

        public Player Player { get; set; }
        public Inventory Inventory { get; set; }
        public _NPC nPC { get; set; }

        private string _playerName;
        private List<Item> _loadedItems;
        public bool _haswon = false;

        #endregion

        #region World Variables

        private Location[] _locations;
        public Vector2 BombLocation = new Vector2(-2, -1);

        #endregion

        public bool _gameRunning;
        public Map _gameMap;
        private string? _playerInput;
        public string _Answer;
        public bool canTalk = false;
        public bool canTake = true;

        #endregion

        #endregion

        #region METHODS

        #region Initialization


        public void StartGame(Game gameInstanceReference)
        {
            // Generate game environment
            CreateNewMap();

            // Load game
            LoadGame();

            // Deal with player generation
            CreatePlayer();

            nPC = new _NPC(_Answer, null, new Vector2(-2, -2), canTake);

            InitializeGameConditions();

            _gameRunning = true;

            StartGameLoop();

        }

        private void CreateNewMap()
        {
            _gameMap = new Map(this, _defaultMapWidth, _defaultMapHeight);
        }

        private void CreatePlayer()
        {
            if (_playerName == null)
            {
                GetPlayerName();
            }

            // _playerName may be null. It would be a good idea to put a check here.
            Player = new Player(_playerName, _loadedItems);
            Console.WriteLine("You have " + Player.Health.ToString() +" lives\n");
        }

        private void GetPlayerName()
        {
            Console.WriteLine("Welcome to the Sherlock's Realm!");
            Console.WriteLine("What do you go by?");
            _playerName = Console.ReadLine();

            Console.WriteLine($"Hi {_playerName}, In This Adventure Your Goal is to kill the Goblin\n\nHowever, you need to get the item that helps you achieve that.\n\nIf you decide to visit the goblin for fun you WILL die. Have Fun <3");
        }

        private void InitializeGameConditions()
        {
            _gameMap.CheckForLocation(_gameMap.GetCoordinates());
        }

        #endregion

        #region Game Loop

        private void StartGameLoop()
        {
            while (_gameRunning)
            {
                GetInput();
                ProcessInput();
                npcstart();


                if (Player.Health == 0)
                {
                    PlayerDied();
                }

            }

        }
        public void PlayerDied()
        {

            Console.WriteLine("You got murdered...");
            exiting();
        }

        private void GetInput()
        {
            _playerInput = Console.ReadLine();
        }

        public void npcstart()
        {
            if (_gameMap.GetCoordinates() == new Vector2(-2, -2))
            {
                canTalk = true;
            }
            else
            {
                canTalk = false;
            }
        }

        private void ProcessInput()
        {
            if (_playerInput == "" || _playerInput == null)
            {
                Console.WriteLine("Give me a command!");
                return;
            }

            switch (_playerInput)
            {
                case "w":
                    _gameMap.MovePlayer(0, 1);
                    break;
                case "s":
                    _gameMap.MovePlayer(0, -1);
                    break;
                case "d":
                    _gameMap.MovePlayer(1, 0);
                    break;
                case "a":
                    _gameMap.MovePlayer(-1, 0);
                    break;
                case "exit":
                    exiting();
                    break;
                case "save":
                    SaveGame();
                    Console.WriteLine("Game saved");
                    break;
                case "load":
                    LoadGame();
                    Console.WriteLine("Game loaded");
                    break;
                case "help":
                    Console.WriteLine(HelpMessage());
                    break;
                case "where":
                    _gameMap.CheckForLocation(_gameMap.GetCoordinates());
                    break;
                case "clear":
                    Console.Clear();
                    break;
                case "who":
                    Console.WriteLine($"Who are you? Not Batnman, just {Player.Name}, Bummer.");
                    break;
                case "take":
                    if (_gameMap.GetCoordinates() == nPC.npcLocation)
                    {
                        if (!nPC.CanTake)
                        {
                            _gameMap.TakeItem(Player, _gameMap.GetCoordinates());
                        }
                        else
                        {
                            Console.WriteLine("Cant take the bomb");
                        }
                    }
                    else
                        _gameMap.TakeItem(Player, _gameMap.GetCoordinates());
                    break;
                case "talk":
                    if (_gameMap.GetCoordinates() == nPC.npcLocation)
                    {
                        nPC.talk();
                    }
                    break;
                case "bomb":
                    if (_gameMap.GetCoordinates() == new Vector2(-2, 2))
                    {
                        if (Player.HasBomb(Item.Bomb))
                        {
                            Player.DropItem(Item.Bomb);
                            Console.WriteLine($"Finally, you killed Gollum, serves him right, thanks { _playerName}" );
                            exiting();
                        }
                        else
                        {
                            PlayerDied();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Command not recognized. Please type 'help' for a list of available commands");
                    }

                    break;
                case "inventory":
                    Player.CheckInventory();
                    break;
                default:
                    Console.WriteLine("Command not recognized. Please type 'help' for a list of available commands");
                    break;

            }
        }

        public void exiting()
        {
            Console.WriteLine("Thanks for trying out our game, till next time <3!");
            _gameRunning = false;
        }

        #endregion

        #region Save Management

        private void LoadGame()
        {
            string path = SaveFilePath();

            if (!File.Exists(path)) return;

            // Reading the file contents
            string[] saveContent = File.ReadAllLines(path);

            // Set the player name
            _playerName = saveContent[0];

            // Set player coordinates
            List<int> coords = saveContent[1].Split(',').Select(int.Parse).ToList();
            Vector2 coordArray = new Vector2(coords[0], coords[1]);

            // Set player inventory
            _loadedItems = new List<Item>();

            List<string> itemStrings = saveContent[2].Split(',').ToList();

            for (int i = 0; i < itemStrings.Count; i++)
            {
                if (Enum.TryParse(itemStrings[i], out Item result))
                {
                    Item item = result;
                    _loadedItems.Add(item);
                    _gameMap.RemoveItemFromLocation(item);
                }
            }

            _gameMap.SetCoordinates(coordArray);

        }

        private void SaveGame()
        {
            // Player Coordinates
            string xCoord = _gameMap.GetCoordinates()[0].ToString();
            string yCoord = _gameMap.GetCoordinates()[1].ToString();
            string playerCoords = $"{xCoord},{yCoord}";

            // Player inventory
            List<Item> items = Player.Inventory.Items;
            string playerItems = "";
            for (int i = 0; i < items.Count; i++)
            {
                playerItems += items[i].ToString();

                if (i != items.Count - 1)
                {
                    playerItems += ",";
                }
            }

            string saveContent = $"{_playerName}{Environment.NewLine}{playerCoords}{Environment.NewLine}{playerItems}";

            string path = SaveFilePath();

            File.WriteAllText(path, saveContent);
        }

        private string SaveFilePath()
        {
            // Get the save file path
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            string path = projectDirectory + @"\save.txt";

            return path;
        }

        #endregion

        #region Miscellaneous

        private string HelpMessage()
        {
            return @"Here are the current commands:
w: Go up
s: Go down
a: Go left
d: Go right
Where: Your current coordinates
Who: Your name
Clear: Clear the screen
Take: Take item
Inventory: Open Inventory
Load: Load saved game
Save: Save current game
Exit: Exit the game";

        }

        #endregion

        #endregion
    }
}