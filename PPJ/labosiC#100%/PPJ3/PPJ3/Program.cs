using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PPJ3
{
    static class Globals
    {
        //global variables
        public static List<List<String[]>> definedVars = new List<List<String[]>>();
        public static List<String> ispis = new List<string>();
        //napravljene samo produkcije koje nemaju završni znak kao prvi
        public static string[] prerequisite_za_koristenje = { "OP_PLUS", "OP_MINUS", "OP_PUTA", "OP_DIJELI", "KR_DO", "KR_OD", "OP_PRIDRUZI", "L_ZAGRADA" };

    }
    
    
    class Program
    {
        static int provjeri_i_vrati_def_line(int dubina_petlje, ref List<String[]> ulaz, ref int currentRead)
        {
            for (int i = dubina_petlje; i >= 0; i--)
            {
                foreach (var idn_with_def_row in Globals.definedVars[i])
                {
                    if (idn_with_def_row[0] == ulaz[currentRead][2])
                    {
                        if (i == dubina_petlje && idn_with_def_row[1] == ulaz[currentRead][1])
                        {
                            return 0;
                        }
                        return int.Parse(idn_with_def_row[1]);
                    }
                }
            }
            return 0;
        }

        static void dodaj_u_ispis(String br_retka_koristenja, int br_retka_definicije, String leksicka_jedinka)
        {
            Globals.ispis.Add(br_retka_koristenja + " " + br_retka_definicije + " " + leksicka_jedinka);
        }
        
        static void call_error(String br_retka_koristenja, String leksicka_jedinka)
        {
            Globals.ispis.Add("err " + br_retka_koristenja + " " + leksicka_jedinka);
            foreach (var redak in Globals.ispis)
            {
                Console.WriteLine(redak);
            }
            Environment.Exit(0);
        }
        
        static void make_def_in_depth(int dubina_petlje, ref List<String[]> ulaz, ref int currentRead)
        {
            //PRVO PROVJERI POSTOJI LI VEĆ DEFINICIJA NAVEDENE VARIJABLE, AKO DA, NE MIJENJAJ NIŠTA, OSIM AKO SE
            //IDE U NOVU PETLJU, ONDA TAJ KR_ZA UVIJEK TREBA DEFINIRATI NOVI
            if (ulaz[currentRead - 1][0] == "KR_ZA")
            {
                Globals.definedVars[dubina_petlje].Add(new string[]
                {
                    ulaz[currentRead][2],
                    ulaz[currentRead][1]
                });
            }
            else if (provjeri_i_vrati_def_line(dubina_petlje, ref ulaz, ref currentRead) == 0)
            {
                Globals.definedVars[dubina_petlje].Add(new string[]
                {
                    ulaz[currentRead][2],
                    ulaz[currentRead][1]
                });
            }
        }
        
        static void delete_def_list_in_depth(int dubina_petlje)
        {
            Globals.definedVars.RemoveAt(dubina_petlje);
        }

        static void naredbe(int dubina_petlje, ref List<String[]> ulaz, ref int currentRead)
        {
            Globals.definedVars.Add(new List<string[]>());
            while (ulaz[currentRead][0] != "KR_AZ" && ulaz[currentRead][0] != "GOTOVO")
            {
                if (ulaz[currentRead][0] == "IDN")
                {
                    if (Globals.prerequisite_za_koristenje.Contains(ulaz[currentRead - 1][0]))
                    {
                        int def_line = provjeri_i_vrati_def_line(dubina_petlje, ref ulaz, ref currentRead);
                        if (def_line != 0)
                        {
                            dodaj_u_ispis(ulaz[currentRead][1], def_line, ulaz[currentRead][2]);
                        }
                        else
                        {
                            //NAPRAVI ERROR, ISPIŠI ŠTO IMA I IZAĐI IZ PROGRAMA
                            call_error(ulaz[currentRead][1], ulaz[currentRead][2]);
                        }
                    }
                    else
                    {
                        //ONDA JE DEFINICIJA, NAPRAVI NOVU DEFINICIJU U Globals varijabli
                        make_def_in_depth(dubina_petlje, ref ulaz, ref currentRead);
                    }
                }
                else if (ulaz[currentRead][0] == "KR_ZA")
                {
                    currentRead++;
                    naredbe(dubina_petlje+1, ref ulaz, ref currentRead);
                }

                currentRead++;
            }
            //POMAKNUTI currentRead I IZBRISATI LISTU U OVOJ DUBINI
            delete_def_list_in_depth(dubina_petlje);
        }
        
        static void Main(string[] args)
        {
            String line;
            List<String[]> lista = new List<String[]>();
            lista.Add(new string[] {
                "POCETAK",
                "POCETAK",
                "POCETAK",
            });
            while ((line = Console.ReadLine()) != null && line != "")
            {
                line = line.TrimStart(' ');
                if (!line.StartsWith('<') && !line.StartsWith('$'))
                {
                    String[] ulazArr = line.Split(" ");
                    lista.Add(ulazArr);
                }
        }
            lista.Add(new string[] {
                "GOTOVO",
                "GOTOVO",
                "GOTOVO",
            });
            int currentRead = 1;
            naredbe(0, ref lista, ref currentRead);
            foreach (var redak in Globals.ispis)
            {
                Console.WriteLine(redak);
            }
        }
    }
}