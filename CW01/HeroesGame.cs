using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CW01
{
    public static class HeroesGame
    {
        public static Hero hero;
        public static DialogParser parser;
        public static List<Location> LocationList;

        public static void Init() //initialization dialogs and NPCs 
        {
            parser = new DialogParser(hero);

            NpcDialogPart n1 = new NpcDialogPart("Witaj, czy możesz mi pomóc dostać się do innego miasta?");

            HeroDialogPart h1_1 = new HeroDialogPart("Tak, chętnie pomogę.");
            HeroDialogPart h1_2 = new HeroDialogPart("Nie, nie pomogę, żegnaj.");
            n1.answers.Add(h1_1);
            n1.answers.Add(h1_2);

            NpcDialogPart n2 = new NpcDialogPart("Dziękuję! W nagrodę otrzymasz ode mnie 100 sztuk złota.");
            h1_1.answers.Add(n2);

            HeroDialogPart h2_1 = new HeroDialogPart("Dam znać jak będę gotowy.");
            HeroDialogPart h2_2 = new HeroDialogPart("100 sztuk złota to za mało!");
            n2.answers.Add(h2_1);
            n2.answers.Add(h2_2);

            NpcDialogPart n3 = new NpcDialogPart("Niestety nie mam więcej. Jestem bardzo biedny.");
            h2_2.answers.Add(n3);

            HeroDialogPart h3_1 = new HeroDialogPart("OK, może być 100 sztuk złota.");
            HeroDialogPart h3_2 = new HeroDialogPart("W takim razie radź sobie sam.");
            n3.answers.Add(h3_1);
            n3.answers.Add(h3_2);

            NpcDialogPart n4 = new NpcDialogPart("Dziękuję.");
            h3_1.answers.Add(n4);


            NpcDialogPart n5 = new NpcDialogPart("Witaj, mam do sprzedarzy różne rzeczy. Czego szukasz?");

            HeroDialogPart h5_1 = new HeroDialogPart("Potrzebuję zbroję.");
            HeroDialogPart h5_2 = new HeroDialogPart("Nic nie potrzebuję.");
            n5.answers.Add(h5_1);
            n5.answers.Add(h5_2);

            NpcDialogPart n6 = new NpcDialogPart("Mam skórzaną.");
            h5_1.answers.Add(n6);

            HeroDialogPart h6_1 = new HeroDialogPart("Nie, chcę lamelkową.");
            HeroDialogPart h6_2 = new HeroDialogPart("Ile kosztuje?");
            n6.answers.Add(h6_1);
            n6.answers.Add(h6_2);

            NpcDialogPart n7 = new NpcDialogPart("300 sztuk złota. A razem z kapturem za 350.");
            h6_2.answers.Add(n7);

            HeroDialogPart h7_1 = new HeroDialogPart("Dobrze, kupuję wszystko.");
            HeroDialogPart h7_2 = new HeroDialogPart("Biorę bez kaptura.");
            n7.answers.Add(h7_1);
            n7.answers.Add(h7_2);

            NpcDialogPart n8 = new NpcDialogPart("Fajny wybór.");
            h7_2.answers.Add(n8);

            NpcDialogPart n9 = new NpcDialogPart("Hej czy to Ty jesteś tym słynnym #HERONAME# – pogromcą smoków?”");

            HeroDialogPart h9_1 = new HeroDialogPart("Tak, jestem #HERONAME#.");
            HeroDialogPart h9_2 = new HeroDialogPart("Nie.");
            n9.answers.Add(h9_1);
            n9.answers.Add(h9_2);

            foreach (var location in LocationList)
            {
                location.Add_npc(new NonPlayerCharacter("Cain", n1));
                location.Add_npc(new NonPlayerCharacter("Warriv", n5));
                location.Add_npc(new NonPlayerCharacter("Deckard", n9));
            }

            Console.ReadLine();
        }

        public static void TalkTo(NonPlayerCharacter npc, DialogParser parser) // dialog process
        {
            Console.Clear();
            NpcDialogPart npc_part;
            HeroDialogPart hero_part;
            npc_part = npc.StartTalking();

            string choice;
            int part_index;

            while (true)
            {
                Console.WriteLine("{0}: {1}", npc.name, parser.ParseDialog(npc_part));

                if (npc_part.answers.Count == 0)
                {
                    Console.WriteLine("KONIEC");
                    Console.WriteLine();
                    Console.WriteLine("Wciśnij Enter...");
                    Console.ReadLine();
                    return;
                }
                for (int i = 0; i < npc_part.answers.Count; i++)
                {
                    Console.WriteLine("[{0}] {1}", i + 1, parser.ParseDialog(npc_part.answers[i]));
                }

                while (true)
                {
                    choice = Console.ReadLine();

                    if (!Int32.TryParse(choice, out part_index) || part_index > npc_part.answers.Count)
                    {
                        Console.WriteLine("Niepoprawna opcja. Sprobuj jeszcze raz.");
                    }
                    else
                    {
                        hero_part = npc_part.answers[part_index - 1];

                        if (hero_part.answers.Count == 0)
                        {
                            Console.WriteLine("KONIEC");
                            Console.WriteLine();
                            Console.WriteLine("Wciśnij Enter...");
                            Console.ReadLine();
                            return;
                        }
                        npc_part = hero_part.answers[0];
                        break;
                    }
                }
            }

        }
        public static void ShowLocation(Location location) // location menu 
        {
            Console.Clear();
            Console.WriteLine("Znajdujesz się w: {0}. Co chcesz zrobić?", location.name);

            for (int i = 0; i < location.npc_list.Count; i++)
            {
                Console.WriteLine("[{0}] Porozmawiaj z {1}", i + 1, location.npc_list[i].name);
            }
            Console.WriteLine("[T] Podróżuj");
            Console.WriteLine("[X] Zamknij program");

            string choice;
            int npc_index;

            while (true)
            {
                choice = Console.ReadLine();
                if (choice == "X")
                    return;
                else if (choice == "T")
                {
                    Travel(location);
                }
                else if (!Int32.TryParse(choice, out npc_index) || npc_index > location.npc_list.Count)
                {
                    Console.WriteLine("Niepoprawna opcja. Sprobuj jeszcze raz.");
                }
                else
                {
                    TalkTo(location.npc_list[npc_index - 1], parser);
                    ShowLocation(location);
                    break;
                }
            }
        }

        public static  void Travel(Location location)
        {
            Console.Clear();
            Console.WriteLine($"Znajdujesz się w : {location.name}. Gdzie chcesz wyruszyć?");

            var AccessibleList = LocationList.Where(loc => loc.IsUnlocked && !loc.Equals(location)).OrderBy(loc => loc.name);

            for (int i = 0; i < AccessibleList.Count(); i++)
            {
                Console.WriteLine("[{0}] {1}", i + 1, AccessibleList.ElementAt(i).name);
            }
            Console.WriteLine("[X] Powrót");

            string choice;
            int loc_index;

            while (true)
            {
                choice = Console.ReadLine();
                if (choice == "X")
                {
                    ShowLocation(location);
                    break;
                }
                else if (!Int32.TryParse(choice, out loc_index) || loc_index > AccessibleList.Count())
                {
                    Console.WriteLine("Niepoprawna opcja. Sprobuj jeszcze raz.");
                }
                else
                {
                    ShowLocation(AccessibleList.ElementAt(loc_index - 1));
                    break;
                }
            }
        }

        public static EHeroClass Choose_class(string name) // class choice menu
        {
            Console.Clear();
            Console.WriteLine($"Witaj {name}, wybierz klasę bohatera: ");

            Console.WriteLine("[1] barbarzyńca");
            Console.WriteLine("[2] paladyn");
            Console.WriteLine("[3] amazonka");

            while (true)
            {
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        return EHeroClass.barbarian;
                    case "2":
                        return EHeroClass.paladin;
                    case "3":
                        return EHeroClass.amazon;
                    default:
                        Console.WriteLine("Niepoprawna opcja. Sprobuj jeszcze raz.");
                        break;
                }
            }
        }

        public static void Name_menu() // name choice menu
        {
            Regex regex = new Regex(@"^[a-zA-Z\s]{2,}$");
            string name = "";
            string name_ = "";

            Console.WriteLine("Proszę podać imię bohatera:");
            while (true)
            {
                name_ = "";
                name = Console.ReadLine();
                if (regex.IsMatch(name))
                {
                    int space_counter = 0;
                    int letter_counter = 0;
                    for (int i = 0; i < name.Length; i++)
                    {
                        if (name[i] != ' ')
                        {
                            name_ += name[i];
                            space_counter = 0;
                            letter_counter++;
                        }
                        else if (name[i] == ' ' && space_counter == 0 && name_ != "")
                        {
                            name_ += name[i];
                            space_counter++;
                        }
                    }

                 

                    if (name_[name_.Length - 1] == ' ')
                    {
                        name_ = name_.Remove(name_.Length - 1);
                    }

                    if (letter_counter >= 3)
                    {
                        hero = new Hero(name_, Choose_class(name_));
                        return;
                    }

                    Console.WriteLine("Niepoprawne imię. Sprobuj jeszcze raz.");

                }
                else
                {
                    Console.WriteLine("Niepoprawne imię. Sprobuj jeszcze raz.");
                }

            }
        }

        public static void Start_menu() // start menu 
        {
            Console.Clear();
            Console.WriteLine("Witaj w grze < Hero Wars >");
            Console.WriteLine("[1] Zacznij nową grę");
            Console.WriteLine("[X] Zamknij program");

            string choice;
            
            while (true)
            {
                choice = Console.ReadLine();
                if (choice == "1")
                {
                    Console.Clear();
                    Name_menu();
                    break;
                }
                else if (choice == "X")
                    return;
                else
                {
                    Console.WriteLine("Niepoprawna opcja. Sprobuj jeszcze raz.");
                }
                
            }

            Console.Clear();

            string hero_class = "Barbarzyńca";
            if (hero.hero_class == EHeroClass.paladin)
                hero_class = "Paladyn";
            else if (hero.hero_class == EHeroClass.amazon)
                hero_class = "Amazonka";

            Console.WriteLine("{0} {1} wyrusza na przygodę", hero_class, hero.name);
            Console.WriteLine();
            Console.WriteLine("Wciśnij Enter...");

            LocationList = new List<Location>();
            LocationList.Add(new Location("Calimport", true));
            LocationList.Add(new Location("Brenigem", true));
            LocationList.Add(new Location("Osterfield", true));
            LocationList.Add(new Location("Milestone", true));
            LocationList.Add(new Location("Gimberland", false));
            LocationList.Add(new Location("Stanindeff", false));
           
            Init();
            ShowLocation(LocationList[0]);
        }
    }
}
