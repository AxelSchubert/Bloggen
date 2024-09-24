using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FörstaInlämningsuppgiften
{
  class Program
  {
    static void Main(string[] args)
    {
      bool körProgram = true; //Bool som avgör om programmet körs
      List<string[]> bloggInlägg = new List<string[]>(); //Lista med strängvektorer för blogginlägg
      while (körProgram)
      {
        Console.Clear();
        Console.Write("\t##########\n\tHej och välkommen till bloggen!\n\tVälj ett av alternativen nedan.\n\t1.Skriv ett blogginlägg\n\t2.Sök bland tidigare inlägg\n\t3.Skriv ut alla blogginlägg\n\t4.Sortera blogginlägg\n\t5.Rensa blogginlägg\n\t6.Avsluta programmet.\n\tVälj: ");
        int.TryParse(Console.ReadLine(), out int menyVal); //Här matar användaren in sitt menyval.
        switch (menyVal)//En switch-sats då det lämpar sig bäst för menyer
        {
          case 1: //I det här caset får användaren göra ett inlägg
            Console.Clear();
            string[] nyttInlägg = new string[4]; //Skapar en ny strängvektor med 3 element för titel, innehåll och datum
            Console.Write("\tSkriv in rubriken på ditt inlägg: ");
            nyttInlägg[0] = Console.ReadLine(); //Användaren matar in titel
            Console.Write("\tSkriv in texten till ditt inlägg: ");
            nyttInlägg[1] = Console.ReadLine(); //Användaren matar in innehåll
            nyttInlägg[2] = DateTime.Now.ToString("g"); //Sparar tid och datum när inlägget skrevs. Läste på MSDN att om man skickar med argumentet "g" så exkluderas sekunderna från datetime-variabeln vilket jag tycker passar bättre.
            nyttInlägg[3] = DateTime.Now.ToString(); //Jag la till ett till osynligt element som sparar hela DateTimet med sekunder för att kunna göra den kronologiska sorteringen mer exakt.
            bloggInlägg.Add(nyttInlägg); //Lägger till det nya inlägget i listan med alla inlägg
            TillbakaTillMeny(); //Metod för menyavslutning
            break;
          case 2:
            Console.Clear();
            Console.Write("\tSkriv in titeln du vill söka på: ");
            string sökOrd = Console.ReadLine(); //Användaren matar in sökord
            SorteringsAlgoritmNamn(bloggInlägg); //Listan sorteras alfabetiskt då det krävs för binärsökningen 
            int sökResultat = BinärSökning(sökOrd, bloggInlägg);//Sökresultatet returneras från binärsöknings-metoden i form av en int som visar vilket index matchande inlägg har.
            if (sökResultat < 0) //Min binärsöknings-metod returnerar -1 ifall inga matchningar hittas så den här if-satsen kollar det.
            {
              Console.WriteLine("\tDet fanns inga inlägg som matchade din sökning.");
              TillbakaTillMeny();
            }
            else
            {
              //Ifall en matchning görs skrivs det inlägget ut och man får en meny med några alternativ för vad man kan göra med det.
              Console.WriteLine("\tEtt inlägg matchade din sökning:\n\t" + bloggInlägg[sökResultat][0] + "\n\n\t" + bloggInlägg[sökResultat][1] + "\n\t" + bloggInlägg[sökResultat][2] + "\n");
              Console.Write("\t1.Ändra titeln på inlägget\n\t2.Ändra texten på inlägget\n\t3.Radera inlägget\n\t4.Gör inget\n\tVälj: ");
              int.TryParse(Console.ReadLine(), out int inläggVal); //Användaren väljer ur den nya menyn
              switch (inläggVal) //Ännu en switch-sats för den här undermenyn
              {
                case 1:
                  Console.Write("\tVad vill du ändra titeln till? ");
                  bloggInlägg[sökResultat][0] = Console.ReadLine(); //Användaren får mata in en ny titel
                  TillbakaTillMeny();
                  break;
                case 2:
                  Console.Write("\tVad vill du ändra texten till? ");
                  bloggInlägg[sökResultat][1] = Console.ReadLine(); //Användaren får mata in en ny text
                  TillbakaTillMeny();
                  break;
                case 3:
                  bloggInlägg.RemoveAt(sökResultat); //Inlägget tas bort ur listan.
                  Console.WriteLine("\tInlägget är raderat!");
                  TillbakaTillMeny();
                  break;
              }
            }
            break;
          case 3:
            Console.Clear();
            if (bloggInlägg.Count != 0) //Kollar ifall det finns några blogginlägg.
              SkrivUtInlägg(bloggInlägg); //Anropar en metod som skriver ut alla sparade inlägg
            else
              Console.WriteLine("\tDet finns inga sparade inlägg!"); //Ifall inga inlägg har gjorts.
            TillbakaTillMeny();
            break;
          case 4:
            Console.Clear();
            Console.WriteLine("\tVill du\n\t1. sortera inläggen alfabetiskt\n\t2. sortera inläggen kronologiskt");
            //Här får användaren välja hur listan ska sorteras, jag valde att använda en if-sats i stället för switch då det bara finns två alternativ och det blev så rörigt med för många switch-satser i varandra.
            int.TryParse(Console.ReadLine(), out int svar);
            if (svar == 1)
            {
              SorteringsAlgoritmNamn(bloggInlägg);//Den alfabetiska sorteringsalgoritmen anropas.
              Console.WriteLine("\tNu är inläggen sorterade alfabetiskt");
              TillbakaTillMeny();
            }
            else if (svar == 2)
            {
              SorteringsAlgoritmDatum(bloggInlägg);//Den kronologiska sorteringsalgoritmen anropas.
              Console.WriteLine("\tNu är inläggen sorterade kronologiskt");
              TillbakaTillMeny();
            }
            else
            {
              Console.WriteLine("\tDu måste välja mellan 1 och 2.");
              TillbakaTillMeny();
            }
            break;
            case 5:
              Console.Clear();
              if (bloggInlägg.Count > 0) bloggInlägg.Clear(); //Om det finns några sparade inlägg tas dom bort.
              Console.WriteLine("\tAlla tidigare inlägg borttagna.");
              TillbakaTillMeny();
              break;
            case 6:
              körProgram = false; //Programmet avslutas.
              break;
            default:
              Console.WriteLine("Du måste skriva en ett giltigt menyval.");
              TillbakaTillMeny();
              break;
            }
        }
      }
      static void TillbakaTillMeny() // Tog inspiration av uppgiften Algoritmer där en sån här metod användes för att ta användarinput innan man går tillbaka menyn
      {
        Console.WriteLine("\n\tTryck ENTER för att återvända till menyn.");
        Console.ReadKey();
      }

      //Den här metoden skriver ut alla sparade inlägg med hjälp av en foreach-loop
      static void SkrivUtInlägg(List<string[]> inläggLista)
      {
        foreach (string[] inlägg in inläggLista)
        {
          Console.WriteLine("\t" + inlägg[0] + "\n");
          Console.WriteLine("\t" + inlägg[1] + "\n\t");
          Console.WriteLine("\t" + inlägg[2]);
          //Det tre första elementen ur varje strängvektor i listan skrivs ut. Jag skriver inte ut den fjärde då den bara är ett mer exakt datum som används för en sorterings algoritm
        }
      }
      /* Här är linjärsöknings-algoritmen jag använde innan jag implementerade en binärsökning.
      static int SökAlgoritm(string sökning, List<string[]> bloggInlägg)
      {
        for (int i = 0; i < bloggInlägg.Count; i++)
        {
          if (sökning.ToUpper() == bloggInlägg[i][0].ToUpper())
          {
            return i;
          }
        }
        return -1;
      }
      */
      //Här är bubblesort-algoritmen som sorterar inläggen alfabetiskt
      static void SorteringsAlgoritmNamn(List<string[]> bloggInlägg)
      {
        for (int i = 0; i < bloggInlägg.Count - 1; i++)
        {
          for (int j = 0; j < bloggInlägg.Count - i - 1; j++)
          {
            if (bloggInlägg[j][0].CompareTo(bloggInlägg[j + 1][0]) == 1)
            {
              string[] temp = bloggInlägg[j];
              bloggInlägg[j] = bloggInlägg[j + 1];
              bloggInlägg[j + 1] = temp;
            }
          }
        }
      }
      //Här är en likadan bubblesort-algoritm som istället sorterar efter den exaktare datetimen som finns på index 3
      static void SorteringsAlgoritmDatum(List<string[]> bloggInlägg)
      {
        for (int i = 0; i < bloggInlägg.Count - 1; i++)
        {
          for (int j = 0; j < bloggInlägg.Count - i - 1; j++)
          {
            if (Convert.ToDateTime(bloggInlägg[j][3]) > Convert.ToDateTime(bloggInlägg[j + 1][3]))
            {
              string[] temp = bloggInlägg[j];
              bloggInlägg[j] = bloggInlägg[j + 1];
              bloggInlägg[j + 1] = temp;
            }
          }
        }
      }
      //Här är en binärsökningsalgoritm som returnar indexet för matchningar. Ifall den inte gör någon matchning returneras -1.
      static int BinärSökning(string sökning, List<string[]> bloggInlägg)
      {
        int first = 0;
        int last = (bloggInlägg.Count - 1);
        while (first <= last)
        {
          int mid = (last + first) / 2;
          if (sökning.CompareTo(bloggInlägg[mid][0]) == 0)
          {
            return mid;
            break;
          }
          else if (sökning.CompareTo(bloggInlägg[mid][0]) == 1)
          {

            first = mid + 1;
          }
          else if (sökning.CompareTo(bloggInlägg[mid][0]) == -1)
          {
            last = mid - 1;
          }
        }
        return -1;
      }
    }
  }