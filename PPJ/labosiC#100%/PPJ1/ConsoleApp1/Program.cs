using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    public class Prijelaz
    {
        public String trenutno_stanje;
        public String simbol_abecede;
        public String[] skup_iducih_stanja;
    }
    
    class Program
    {
        static bool eng_abeceda(char x)
        {
            if (x >= 65 && x <= 90 || x >= 97 && x <= 122)
            {
                return true;
            }

            return false;
        }
        
        static bool broj(char x)
        {
            if (x >= 48 && x <= 57)
            {
                return true;
            }

            return false;
        }
        
        static bool op_test(char x, int lineNumber)
        {
            if (x == '=')
            {
                Console.WriteLine("OP_PRIDRUZI " + lineNumber + " =");
                return true;
            }
            if (x == '+')
            {
                Console.WriteLine("OP_PLUS " + lineNumber + " +");
                return true;
            }
            if (x == '-')
            {
                Console.WriteLine("OP_MINUS " + lineNumber + " -");
                return true;
            }
            if (x == '*')
            {
                Console.WriteLine("OP_PUTA " + lineNumber + " *");
                return true;
            }
            if (x == '/')
            {
                Console.WriteLine("OP_DIJELI " + lineNumber + " /");
                return true;
            }
            if (x == '(')
            {
                Console.WriteLine("L_ZAGRADA " + lineNumber + " (");
                return true;
            }
            if (x == ')')
            {
                Console.WriteLine("D_ZAGRADA " + lineNumber + " )");
                return true;
            }

            return false;
        }

        static bool razmak(char x)
        {
            if (x == '\n' || x == '\t' || x == ' ')
            {
                return true;
            }

            return false;
        }
        
        static bool kljucna_rijec(String x, int lineNumber)
        {
            if (x == "za")
            {
                Console.WriteLine("KR_ZA " + lineNumber + " za");
                return true;
            }
            if (x == "od")
            {
                Console.WriteLine("KR_OD " + lineNumber + " od");
                return true;
            }
            if (x == "do")
            {
                Console.WriteLine("KR_DO " + lineNumber + " do");
                return true;
            }
            if (x == "az")
            {
                Console.WriteLine("KR_AZ " + lineNumber + " az");
                return true;
            }

            return false;
        }

        static void Main(string[] args)
        {
            //citanje svih ulaznih podataka
            String line;
            int lineNumber = 1;
            //&& line != ""
            while ((line = Console.ReadLine()) != null)
            {
                int start = 0, end = 0;
                while (end < line.Length)
                {
                    if (razmak(line[start]))
                    {
                        start++;
                        end++;
                        continue;
                    }

                    if (line[start] == '/')
                    {
                        if (start+1 < line.Length)
                        {
                            if (line[start + 1] == '/')
                            {
                                break;
                            }
                        }
                    }

                    if (op_test(line[start], lineNumber))
                    {
                        start++;
                        end++;
                        continue;
                    }

                    if (broj(line[start]))
                    {
                        end++;
                        while (end < line.Length)
                        {
                            if (!broj(line[end]))
                            {
                                break;
                            }
                            end++;
                        } //od start do end-1 ispis, end je next char, ali substring ide (start, end-start) jer je length
                        Console.WriteLine("BROJ " + lineNumber + " " + line.Substring(start,end-start));
                        start = end;
                        continue;
                    }
                    
                    if (eng_abeceda(line[start]))
                    {
                        end++;
                        while (end < line.Length)
                        {
                            if (!broj(line[end]) && !eng_abeceda(line[end]))
                            {
                                break;
                            }
                            end++;
                        } //od start do end-1 ispis, end je next char, ali substring ide (start, end) jer je length

                        if (!kljucna_rijec(line.Substring(start, end-start), lineNumber))
                        {
                            Console.WriteLine("IDN " + lineNumber + " " + line.Substring(start,end-start));
                        }
                        start = end;
                    }
                }
                lineNumber++;
            }
        }
    }
}