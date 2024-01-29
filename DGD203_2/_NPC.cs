using DGD203_2;
using System.Numerics;


public class _NPC
{
    public Player Player { get; set; }
    public Game Game { get; set; }
    public Map Map { get; set; }
    public Location Location { get; set; }
    public Inventory Inventory { get; set; }

    public string Answer;
    public string OK;
    public Vector2 npcLocation;
    public string name;
    public bool CanTake = true;
    public int heatlh;


    public _NPC(string answer, string oK, Vector2 npclocation, bool cantake)
    {
        Answer = answer;
        OK = oK;
        npcLocation = npclocation;
        CanTake = cantake;
    }

    public void talk()
    {
        Player = new Player(name, null);
        Inventory = new Inventory();
        Game = new Game();
        Map = new Map(Game, 5, 5);
        heatlh = Player.Health;
        Console.WriteLine("Hello " + name + " my name is Sherlock Holmes, and I am smart\n\n" +
            "I hear that you need the Bomb to kill the Goblin, but only the worthy get to be called heros so let's see....");
        question();
        while (CanTake)
        {
            getInput();
            handleInput();
        }
    }
    public void exit()
    {
        Environment.Exit(0);
    }
    public void question()
    {
        Console.WriteLine("Inevitably, all paths converge upon me,\r\nA somber silhouette, draped in solemn decree.\r\nIn silence, I beckon, impartial and stern,\r\nSome proclaim my touch, the final discern.\r\n\r\n\n1.Death\n\n2.Grief\n\n3.Murder\n\n4.Suicide\n\n5.Depression");
    }
    public void getInput()
    {
        Answer = Console.ReadLine();
    }
    public void handleInput()
    {
        if (Answer != null)
        {
            if (heatlh > 0)
            {
                if (CanTake)
                {
                    switch (Answer)
                    {
                        case "1":
                            Console.WriteLine("correct,you win 'take' your bomb");
                            Player.TakeItem(Item.Bomb);
                            CanTake = false;
                            break;
                        case "2":
                            if (heatlh == 2)
                            {
                                Console.WriteLine("false you have two chances left");
                            }
                            else if (heatlh == 1)
                            {
                                Game.PlayerDied();
                                exit();
                            }
                            break;
                        case "3":
                            if (heatlh == 2)
                            {
                                Console.WriteLine("false you have one chance left");
                            }
                            else if (heatlh == 1)
                            {
                                Game.PlayerDied();
                                exit();
                            }
                            break;
                        case "4":
                            if (heatlh == 2)
                            {
                                Console.WriteLine("false you have one chance left");
                            }
                            else if (heatlh == 1)
                            {
                                Game.PlayerDied();
                                exit();
                            }
                            break;
                        case "5":
                            if (heatlh == 2)
                            {
                                Console.WriteLine("false you have one chance left");
                            }
                            else if (heatlh == 1)
                            {
                                Game.PlayerDied();
                                exit();
                            }
                            break;
                        default:
                            Console.WriteLine("wrong input!!");
                            break;
                    }
                    heatlh--;
                }
            }
            else
            {
                Game.PlayerDied();
                exit();
            }
        }
    }
}