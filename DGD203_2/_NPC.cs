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
        Console.WriteLine("Hi " + name + " I am Raven, I was sent by my team to help you out...poor Dobby\n\n" +
            "I hear that Dobby has gone rogue and needs to be sent back to his land by a wise human. So let's see how wise you can get....");
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
        Console.WriteLine("In the heart of justice, I find my place,\r\nA virtue rare, a compassionate grace.\r\nNot earned nor bought, freely I bestow,\r\nSoftening the blow, in both joy and woe..\r\n\r\n\n1.Emotions\n\n2.Truth\n\n3.Logic\n\n4.Mercy\n\n5.Grace");
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
                            if (heatlh == 2)
                            {
                                Console.WriteLine("False, get it wrong one more time and I will have to kill you human");
                            }
                            else if (heatlh == 1)
                            {
                                Game.PlayerDied();
                                exit();
                            }
                            break;
                        case "2":
                            if (heatlh == 2)
                            {
                                Console.WriteLine("False, get it wrong one more time and I will have to kill you human");
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
                                Console.WriteLine("False, get it wrong one more time and I will have to kill you human");
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
                                Console.WriteLine("correct, you can take the bomb.\n\nType 'take'");
                                Player.TakeItem(Item.Bomb);
                                CanTake = false;
                                break;
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
                                Console.WriteLine("False, get it wrong one more time and I will have to kill you human");
                            }
                            else if (heatlh == 1)
                            {
                                Game.PlayerDied();
                                exit();
                            }
                            break;
                        default:
                            Console.WriteLine("wrong input.");
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
