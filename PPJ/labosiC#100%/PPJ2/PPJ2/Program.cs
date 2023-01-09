using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PPJ2
{
    static class Globals
    {
        //global variables
        public static List<String> ispis = new List<string>();
        public static bool error = false;
        //napravljene samo produkcije koje nemaju završni znak kao prvi
        public static string[] program_primjeni = { "IDN", "KR_ZA", "GOTOVO" };
        public static string[] lista_naredbi_primjeni = { "IDN", "KR_ZA"};
        public static string[] lista_naredbi_epsilon_primjeni = { "KR_AZ", "GOTOVO"};
        public static string[] naredba1_primjeni = { "IDN"};
        public static string[] naredba2_primjeni = { "KR_ZA"};
        
        public static string[] e_primjeni = { "IDN", "BROJ", "OP_PLUS", "OP_MINUS", "L_ZAGRADA" };
        public static string[] e_lista_epsilon_primjeni = { "IDN", "KR_ZA", "KR_DO", "KR_AZ", "D_ZAGRADA", "GOTOVO" };
        public static string[] t_primjeni = { "IDN", "BROJ", "OP_PLUS", "OP_MINUS", "L_ZAGRADA" };
        public static string[] t_lista_epsilon_primjeni = { "IDN", "KR_ZA", "KR_DO", "KR_AZ", "OP_PLUS", "OP_MINUS", "D_ZAGRADA", "GOTOVO"};

    }
    
    class Program
    {

        static bool provjera_err_i_ispis_add(int dubina, String kajIspisat)
        {
            if (Globals.error)
            {
                return true;
            }
            String ispis = "";
            ispis = ispis.PadLeft(dubina) + kajIspisat;
            Globals.ispis.Add(ispis);
            return false;
        }
        
        static void ispis_add(int dubina, String kajIspisat)
        {
            String ispis = "";
            ispis = ispis.PadLeft(dubina) + kajIspisat;
            Globals.ispis.Add(ispis);
        }
        static void writeError(ref List<String> ulaz, ref int currentRead)
        {
            if (ulaz.Count == currentRead+1)
            {
                Console.WriteLine("err kraj");
            }
            else
            {
                Console.WriteLine("err " + ulaz[currentRead]);
            }

            Globals.error = true;
        }
        static bool program(int dubina, ref List<String> ulaz, ref int currentRead)
        {
            if (provjera_err_i_ispis_add(dubina, "<program>"))
            {
                return false;
            }

            String ul_znak = ulaz[currentRead].Split(" ")[0];
            if (Globals.program_primjeni.Contains(ul_znak))
            {
                lista_naredbi(dubina + 1, ref ulaz, ref currentRead);
            }
            else
            {
                writeError(ref ulaz, ref currentRead);
                return false;
            }

            return true;
        }
        
        static bool lista_naredbi(int dubina, ref List<String> ulaz, ref int currentRead)
        {
            if (provjera_err_i_ispis_add(dubina, "<lista_naredbi>"))
            {
                return false;
            }
            
            String ul_znak = ulaz[currentRead].Split(" ")[0];
            if (Globals.lista_naredbi_primjeni.Contains(ul_znak))
            {
                naredba(dubina + 1, ref ulaz, ref currentRead);
                lista_naredbi(dubina + 1, ref ulaz, ref currentRead);
            }
            else if (Globals.lista_naredbi_epsilon_primjeni.Contains(ul_znak))
            {
                epsilon(dubina+1);
            }
            else
            {
                writeError(ref ulaz, ref currentRead);
                return false;
            }

            return true;
        }
        
        static bool naredba(int dubina, ref List<String> ulaz, ref int currentRead)
        {
            if (provjera_err_i_ispis_add(dubina, "<naredba>"))
            {
                return false;
            }

            String ul_znak = ulaz[currentRead].Split(" ")[0];

            if (Globals.naredba1_primjeni.Contains(ul_znak))
            {
                naredba_pridruzivanja(dubina + 1, ref ulaz, ref currentRead);
            }
            else if (Globals.naredba2_primjeni.Contains(ul_znak))
            {
                za_petlja(dubina + 1, ref ulaz, ref currentRead);
            }
            else
            {
                writeError(ref ulaz, ref currentRead);
                return false;
            }

            return true;
        }

        static bool naredba_pridruzivanja(int dubina, ref List<String> ulaz, ref int currentRead)
        {
            if (provjera_err_i_ispis_add(dubina, "<naredba_pridruzivanja>"))
            {
                return false;
            }  
            String ul_znak = ulaz[currentRead].Split(" ")[0];
            if (ul_znak == "IDN")
            {
                ispis_add(dubina+1, ulaz[currentRead]);
                currentRead++;
                ul_znak = ulaz[currentRead].Split(" ")[0];
                
                if (ul_znak == "OP_PRIDRUZI")
                {
                    ispis_add(dubina+1, ulaz[currentRead]);
                    currentRead++;
                }
                else
                {
                    writeError(ref ulaz, ref currentRead);
                    return false;
                }
            }
            else
            {
                writeError(ref ulaz, ref currentRead);
                return false;
            }
            e(dubina + 1, ref ulaz, ref currentRead);

            return true;
        }
        
        static bool za_petlja(int dubina, ref List<String> ulaz, ref int currentRead)
        {
            if (provjera_err_i_ispis_add(dubina, "<za_petlja>"))
            {
                return false;
            }  
            String ul_znak = ulaz[currentRead].Split(" ")[0];
            
            if (ul_znak == "KR_ZA")
            {
                ispis_add(dubina+1, ulaz[currentRead]);
                currentRead++;
                ul_znak = ulaz[currentRead].Split(" ")[0];
                if (ul_znak == "IDN")
                {
                    ispis_add(dubina+1, ulaz[currentRead]);
                    currentRead++;
                    ul_znak = ulaz[currentRead].Split(" ")[0];
                }
                else
                {
                    writeError(ref ulaz, ref currentRead);
                    return false;
                }
                if (ul_znak == "KR_OD")
                {
                    ispis_add(dubina+1, ulaz[currentRead]);
                    currentRead++;
                }
                else
                {
                    writeError(ref ulaz, ref currentRead);
                    return false;
                }
                e(dubina + 1, ref ulaz, ref currentRead);
                ul_znak = ulaz[currentRead].Split(" ")[0];
                if (ul_znak == "KR_DO" && !Globals.error)
                {
                    ispis_add(dubina+1, ulaz[currentRead]);
                    currentRead++;
                }
                else if (Globals.error)
                {
                    return false;
                }
                else
                {
                    writeError(ref ulaz, ref currentRead);
                    return false;
                }
                e(dubina + 1, ref ulaz, ref currentRead);
                lista_naredbi(dubina + 1, ref ulaz, ref currentRead);
                ul_znak = ulaz[currentRead].Split(" ")[0];
                if (ul_znak == "KR_AZ" && !Globals.error)
                {
                    ispis_add(dubina+1, ulaz[currentRead]);
                    currentRead++;
                }
                else if (Globals.error)
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("err kraj");
                    Globals.error = true;
                    return false;
                }
            }
            else
            {
                writeError(ref ulaz, ref currentRead);
                return false;
            }

            return true;
        }
        
        static bool e(int dubina, ref List<String> ulaz, ref int currentRead)
        {
            if (provjera_err_i_ispis_add(dubina, "<E>"))
            {
                return false;
            }
            
            String ul_znak = ulaz[currentRead].Split(" ")[0];
            if (Globals.e_primjeni.Contains(ul_znak))
            {
                t(dubina + 1, ref ulaz, ref currentRead);
                e_lista(dubina + 1, ref ulaz, ref currentRead);
            }
            else
            {
                writeError(ref ulaz, ref currentRead);
                return false;
            }

            return true;
        }
        
        static bool e_lista(int dubina, ref List<String> ulaz, ref int currentRead)
        {
            if (provjera_err_i_ispis_add(dubina, "<E_lista>"))
            {
                return false;
            }

            String ul_znak = ulaz[currentRead].Split(" ")[0];

            if (ul_znak == "OP_PLUS")
            {
                ispis_add(dubina+1, ulaz[currentRead]);
                currentRead++;
                e(dubina + 1, ref ulaz, ref currentRead);
            }
            else if (ul_znak == "OP_MINUS")
            {
                ispis_add(dubina+1, ulaz[currentRead]);
                currentRead++;
                e(dubina + 1, ref ulaz, ref currentRead);
            }
            else if (Globals.e_lista_epsilon_primjeni.Contains(ul_znak))
            {
                epsilon(dubina+1);
            }
            else
            {
                writeError(ref ulaz, ref currentRead);
                return false;
            }

            return true;
        }
        
        static bool t(int dubina, ref List<String> ulaz, ref int currentRead)
        {
            if (provjera_err_i_ispis_add(dubina, "<T>"))
            {
                return false;
            }
            
            String ul_znak = ulaz[currentRead].Split(" ")[0];
            if (Globals.t_primjeni.Contains(ul_znak))
            {
                p(dubina + 1, ref ulaz, ref currentRead);
                t_lista(dubina + 1, ref ulaz, ref currentRead);
            }
            else
            {
                writeError(ref ulaz, ref currentRead);
                return false;
            }

            return true;
        }
        
        static bool t_lista(int dubina, ref List<String> ulaz, ref int currentRead)
        {
            if (provjera_err_i_ispis_add(dubina, "<T_lista>"))
            {
                return false;
            }

            String ul_znak = ulaz[currentRead].Split(" ")[0];

            if (ul_znak == "OP_PUTA")
            {
                ispis_add(dubina+1, ulaz[currentRead]);
                currentRead++;
                t(dubina + 1, ref ulaz, ref currentRead);
            }
            else if (ul_znak == "OP_DIJELI")
            {
                ispis_add(dubina+1, ulaz[currentRead]);
                currentRead++;
                t(dubina + 1, ref ulaz, ref currentRead);
            }
            else if (Globals.t_lista_epsilon_primjeni.Contains(ul_znak))
            {
                epsilon(dubina+1);
            }
            else
            {
                writeError(ref ulaz, ref currentRead);
                return false;
            }

            return true;
        }
        
        static bool p(int dubina, ref List<String> ulaz, ref int currentRead)
        {
            if (provjera_err_i_ispis_add(dubina, "<P>"))
            {
                return false;
            }

            String ul_znak = ulaz[currentRead].Split(" ")[0];

            if (ul_znak == "OP_PLUS")
            {
                ispis_add(dubina+1, ulaz[currentRead]);
                currentRead++;
                p(dubina + 1, ref ulaz, ref currentRead);
            }
            else if (ul_znak == "OP_MINUS")
            {
                ispis_add(dubina+1, ulaz[currentRead]);
                currentRead++;
                p(dubina + 1, ref ulaz, ref currentRead);
            }
            else if (ul_znak == "L_ZAGRADA")
            {
                ispis_add(dubina+1, ulaz[currentRead]);
                currentRead++;
                e(dubina + 1, ref ulaz, ref currentRead);
                ul_znak = ulaz[currentRead].Split(" ")[0];
                if (ul_znak == "D_ZAGRADA" && !Globals.error)
                {
                    ispis_add(dubina+1, ulaz[currentRead]);
                    currentRead++;
                }
                else if (Globals.error)
                {
                    return false;
                }
                else
                {
                    writeError(ref ulaz, ref currentRead);
                    return false;
                }
            }
            else if (ul_znak == "IDN")
            {
                ispis_add(dubina+1, ulaz[currentRead]);
                currentRead++;
            }
            else if (ul_znak == "BROJ")
            {
                ispis_add(dubina+1, ulaz[currentRead]);
                currentRead++;
            }
            else
            {
                writeError(ref ulaz, ref currentRead);
                return false;
            }

            return true;
        }

        static void epsilon(int dubina)
        {
            String ispis = "";
            ispis = ispis.PadLeft(dubina) + "$";
            Globals.ispis.Add(ispis);
        }

        static void Main(string[] args)
        {
            String line;
            List<String> lista = new List<String>();
            while ((line = Console.ReadLine()) != null && line != "")
            {
                lista.Add(line);
            }
            lista.Add("GOTOVO GOTOVO GOTOVO");
            int currentRead = 0;
            program(0, ref lista, ref currentRead);
            //Console.WriteLine("ok+ " + lista[lista.Count-1]);
            if (!Globals.error)
            {
                for (int i = 0; i < Globals.ispis.Count; i++)
                {
                    Console.WriteLine(Globals.ispis[i]);
                }
            }

        }
    }
}