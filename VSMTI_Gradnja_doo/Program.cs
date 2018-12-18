using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using ConsoleTables;


namespace VSMTI_Gradnja_doo
{ 
    //klasu imamo da mozemo pozivati objekte
    class Program
    {
        //An enum is a convenient way to use names instead of numbers, in order to denote something. 
        //It makes your code far more readable and maintainable than using numbers.
        public enum Tip
        {
            PS = 1,
            IZD = 2,
            PRM = 3
        }

        public enum Key
        {
            DIGIT_1 = 49,
            DIGIT_2 = 50,
            DIGIT_3 = 51,
            DIGIT_4 = 52,
            DIGIT_5 = 53,
            C = 67,
            P = 80,
            X = 88,
            Y = 89
        }

        static void Main(string[] args)
        { 
            Pocetna();
        }
        
        //struktura koja definira user-a
        public struct Korisnik
        {
            public string email;
            public string lozinka;

            public Korisnik(string u, string p)
            {
                email = u;
                lozinka = p;
            }
        }

        //struktura koja definira slozen tip podatka artikl
        public struct Artikl
        {
            public string sifra;
            public string naziv;
            public string jmj; // jedinicna mjera
            public decimal cijena;

            public Artikl(string s, string n, string j, decimal c)
            {
                sifra = s;
                naziv = n;
                jmj = j;
                cijena = c;
            }
        }

        // struktura koja definira slozen tip podatka dokument iz stanja.xml
        public struct Dokument
        {
            public int tip;
            public string datum;
            public string sifra;
            public string naziv;
            public int kolUlaz;
            public decimal iznosUlaz;
            public int kolIzlaz;
            public decimal iznosIzlaz;

            //konstruktor - funkcija koja se poziva prilikom kreiranja objekta strukture Dokument
            public Dokument(int t, string d, string c, string n, int ku, decimal iu, int ki, decimal ii)
            {
                //"tip" je podatkovni clan , a "t" parametar u koji se poziva kad citamo atribute iz xml datoteke
                tip = t;
                datum = d;
                sifra = c;
                naziv = n;
                kolUlaz = ku;
                iznosUlaz = iu;
                kolIzlaz = ki;
                iznosIzlaz = ii;
            }
        }

        //kreiranje liste objekata strukture Korisnik
        static List<Korisnik> DohvatiKorisnike()
        {
            List<Korisnik> lUsers = new List<Korisnik>();
            string sXml = "";
            StreamReader oSr = new StreamReader(Path.Combine(Environment.CurrentDirectory + "/xml", "config.xml"));
            using (oSr)
            {
                sXml = oSr.ReadToEnd();
            }
            XmlDocument oXml = new XmlDocument(); //novi objekt koji predstavlja XML
            oXml.LoadXml(sXml);

            XmlNodeList oNodes = oXml.SelectNodes("//korisnici/korisnik");

            foreach (XmlNode oNode in oNodes)
            {
                string email = oNode.Attributes["email"].Value;
                string lozinka = oNode.Attributes["lozinka"].Value;
                lUsers.Add(new Korisnik(email, lozinka)); //konstruktor
            }
            oSr.Close();
            return lUsers;
        }

        //kreiranje liste objekata strukture Artikl
        static List<Artikl> DohvatiArtikle()
        {
            List<Artikl> lArticles = new List<Artikl>();
            string sXml = "";
            StreamReader oSr = new StreamReader(Path.Combine(Environment.CurrentDirectory + "/xml", "artikli.xml"));
            using (oSr)
            {
                sXml = oSr.ReadToEnd();
            }

            XmlDocument xArtikli = new XmlDocument();
            xArtikli.LoadXml(sXml);

            XmlNodeList oNodes = xArtikli.SelectNodes("//artikli/artikl");

            foreach (XmlNode oNode in oNodes)
            {
                string sifra = oNode.Attributes["sifra"].Value;
                string naziv = oNode.Attributes["naziv"].Value;
                string jmj = oNode.Attributes["jmj"].Value;
                decimal cijena = decimal.Parse(oNode.Attributes["cijena"].Value);

                lArticles.Add(new Artikl(sifra, naziv, jmj, cijena));
            }
            oSr.Close();
            return lArticles;
        }

        //kreiranje liste objekata strukture Dokument
        static List<Dokument> DohvatiDokumente()
        {
            List<Dokument> lDokumenti = new List<Dokument>();
            string sXml = "";
            StreamReader oSr = new StreamReader(Path.Combine(Environment.CurrentDirectory + "/xml", "stanje.xml"));
            using (oSr)
            {
                sXml = oSr.ReadToEnd();
            }

            XmlDocument xDokument = new XmlDocument();
            xDokument.LoadXml(sXml);

            XmlNodeList oNodes = xDokument.SelectNodes("//dokumenti/dokument");
            foreach (XmlNode oNode in oNodes)
            {
                int Tip = Convert.ToInt32(oNode.Attributes["tip"].Value);
                string Datum = oNode.Attributes["datum"].Value;
                string Sifra = oNode.Attributes["sifra"].Value;
                string Naziv = oNode.Attributes["naziv"].Value;
                int KolUlaz = Convert.ToInt32(oNode.Attributes["kol_ulaz"].Value);
                decimal IznosUlaz = decimal.Parse(oNode.Attributes["iznos_ulaz"].Value);
                int KolIzlaz = Convert.ToInt32(oNode.Attributes["kol_izlaz"].Value);
                decimal IznosIzlaz = decimal.Parse(oNode.Attributes["iznos_izlaz"].Value);

                lDokumenti.Add(new Dokument(Tip, Datum, Sifra, Naziv, KolUlaz, IznosUlaz, KolIzlaz, IznosIzlaz));
            }
            oSr.Close();
            return lDokumenti;
        }

        //f za provjeru moze li ste ucitati datoteka
        static bool ProvjeraUcitavanje()
        {
            bool bUcitavanje = false;
            //provjera da li su ucitane vrsi se pokusajem ucitavanja, ako nisu ucitane ispisuje se blok catch
            try
            {
                XmlDocument config = new XmlDocument();
                config.Load(Path.Combine(Environment.CurrentDirectory + "/xml", "config.xml"));

                XmlDocument stanje = new XmlDocument();
                stanje.Load(Path.Combine(Environment.CurrentDirectory + "/xml", "stanje.xml"));

                XmlDocument artikli = new XmlDocument();
                artikli.Load(Path.Combine(Environment.CurrentDirectory + "/xml", "artikli.xml"));
                bUcitavanje = true;
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n!!Datoteke nije moguce ucitati, molimo izadjite iz programa pritiskom na bilo koju tipku i pokusajte ponovno.");
                Console.ResetColor();
                Console.ReadKey();
                Environment.Exit(0);
            }
            return bUcitavanje;
        }

        //static je f koja se moze pozvati bez da je objekt klase kojoj pripada ta funkcija instanciran.
        static void Pocetna()
        {
            Console.Clear();// brisanje sadrzaja koji je vidljiv na konzoli
            Console.ForegroundColor = ConsoleColor.Red; //prvi plan(text)
            string sXml = "";
            StreamReader oSr = new StreamReader(Path.Combine(Environment.CurrentDirectory + "/xml", "vsmti.txt"));
            using (oSr)
            {
                sXml = oSr.ReadToEnd();
            }
            Console.WriteLine(sXml);

            Console.WriteLine("\nPritisnite [P] za  prijavu korisnika");
            Console.WriteLine("Pritisnite [X] za izlazak iz programa");
            Console.ResetColor();
            Console.Write("\nVaš odabir: ");
            int odabir = Convert.ToInt32(Console.ReadKey().Key); //.Key cita kod tipke koju pritisnemo
            while (odabir != (int)Key.P && odabir != (int)Key.X)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nKrivi odabir, pokušajte ponovno\n");
                Console.ResetColor();
                Console.Write("\nVaš odabir: ");
                odabir = Convert.ToInt32(Console.ReadKey().Key);
            }
            switch (odabir)
            {
                case (int)Key.P:
                    {
                        if (ProvjeraUcitavanje())
                        {
                            Zapis("APP-START");
                            Prijava();
                        }
                        break;
                    }    
                case (int)Key.X:
                    {
                        Zapis("APP-EXIT");
                        Environment.Exit(0);
                        break;
                    }
            }
        }
    
        //f koja omogucuje unos podataka i provjerava jesu li ti podatci istovjetni sa onim iz LoginCheck()
        static void Prijava()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n********PRIJAVA********\n");
            Console.ResetColor();
            string email;
            string lozinka;
            do
            {
                Console.Write("Email: ");
                email = Console.ReadLine();
                lozinka = DohvatiLozinku();

                if (ProvjeraPrijave(email, lozinka))
                {
                    string dobrodosli = "";
                    StreamReader oSr = new StreamReader(Path.Combine(Environment.CurrentDirectory + "/xml", "welcome.txt"));
                    using (oSr) // expl.down
                    {
                        dobrodosli = oSr.ReadToEnd();
                    }
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\n" + dobrodosli);
                    Console.ResetColor();
                    System.Threading.Thread.Sleep(1500);//delay
                    Zapis("Login: " + email);
                    Izbornik();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nE-mail i/ili lozinka nisu valjani.Pokusajte ponovno\n");
                    Console.ResetColor();
                }
            } while (ProvjeraPrijave(email, lozinka) == false);
        }

        //f pomaze pri provjeri da li postoji korisnik sa unesenom sifrnom i imenom
        static bool ProvjeraPrijave(string email, string lozinka)
        {
            List<Korisnik> lKorisnici = DohvatiKorisnike();

            bool bUser = false;
            for (int i = 0; i < lKorisnici.Count(); i++)
            {
                if (email == lKorisnici[i].email && lozinka == lKorisnici[i].lozinka)
                {
                    bUser = true;
                } 
            }
            return bUser;
        }

        //f koja omogucuje upis sifre, imaginarno pretvara svako slovo u *  i vraca sifru
        static string DohvatiLozinku()
        {
            string lozinka = "";
            Console.Write("Sifra: ");
            do
            {
                ConsoleKeyInfo tipka = Console.ReadKey(true); //opisuje konzolnu tipku koja je pritisnuta
                // Backspace Should Not Work
                if (tipka.Key != ConsoleKey.Backspace && tipka.Key != ConsoleKey.Enter) //provjera pritisnute tipke
                {
                    lozinka += tipka.KeyChar; //.KeyChar dobavljanje pritisnute tipke
                    Console.Write("*");
                }
                else
                {
                    if (tipka.Key == ConsoleKey.Backspace && lozinka.Length > 0)
                    {
                        lozinka = lozinka.Substring(0, (lozinka.Length - 1)); //smanjujemo string za 1 slovo
                        Console.Write("\b \b"); //konzola krece jednu tipku unazad, maskira je sa prazninom i tu prazninu brise
                    }
                    else if (tipka.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            } while (true);
            return lozinka;
        }

        //f koja omogucuje unos iskljucivo brojcanih vrijednosti te vraca istu
        static string UnosBroja()
        {
            string kolicina = "";
            int vrijednost = 0;
            ConsoleKeyInfo chr;
            do
            {
                chr = Console.ReadKey(true);//true - don't display the pressed key
                if (chr.Key != ConsoleKey.Backspace)
                {
                    bool kontrola = int.TryParse(chr.KeyChar.ToString(), out vrijednost);
                    if (kontrola)
                    {
                        kolicina += chr.KeyChar;
                        Console.Write(chr.KeyChar);
                    }
                }
                else
                {
                    if (chr.Key == ConsoleKey.Backspace && kolicina.Length > 0)
                    {
                        kolicina = kolicina.Substring(0, (kolicina.Length - 1));
                        Console.Write("\b \b");
                    }
                }
            } while (chr.Key != ConsoleKey.Enter);

            if (int.Parse(kolicina) == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\nNemoguc unos! Provjerite te unesite ponovno: ");
                Console.ResetColor();
                UnosBroja();
            }
            return kolicina;
        }

        //f za logove
        static void Zapis(string sArg)
        {
            string trenutno = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ssTZD");
            StreamWriter oSw = new StreamWriter(Path.Combine(Environment.CurrentDirectory + "/xml", "zapisi.log"), true);
            using (oSw)
            {
                oSw.WriteLine("\n");
                oSw.WriteLine(sArg);
                oSw.WriteLine(trenutno);
            }
        }

        //f koja nakon svake operacije u izborniku omogucuje povrat na izbornik
        static void DohvatiTipku()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nPritisnite [C] za povratak u glavni izbornik");
            Console.ResetColor();
            Console.Write("\nVas odabir: ");
            int odabir = Convert.ToInt32(Console.ReadKey().Key);
            while (odabir != (int)Key.C)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nKrivi odabir, pokušajte ponovno\n");
                Console.ResetColor();
                Console.Write("\nVas odabir: ");
                odabir = Convert.ToInt32(Console.ReadKey().Key);
            }
            switch (odabir)//moze i if
            {
                case (int)Key.C:
                    Izbornik();
                    break;
            }
        }

        //f za izlist izbornika i odabir
        static void Izbornik()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n*****GLAVNI IZBORNIK***** \n");
            Console.ResetColor();
            Console.WriteLine("Iz izbornika odaberite zeljenu operaciju: \n");
            Console.WriteLine("[1] -> Stanje skladista");
            Console.WriteLine("[2] -> Izvjesce artikla");
            Console.WriteLine("[3] -> Nova izdatnica");
            Console.WriteLine("[4] -> Nova primka");
            Console.WriteLine("[5] -> Odjava");
            Console.Write("\nVas odabir: ");

            int odabir = Convert.ToInt32(Console.ReadKey().Key);
            int[] dozvoljeniUnosi = { (int)Key.DIGIT_1, (int)Key.DIGIT_2, (int)Key.DIGIT_3, (int)Key.DIGIT_4, (int)Key.DIGIT_5}; //polje ascii
            while (!dozvoljeniUnosi.Contains(odabir))   //Contains - f koja provjera da li u polju postoji zadana varijabla
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nKrivi odabir, pokusajte ponovno");
                Console.ResetColor();

                Console.Write("\nVas odabir: ");
                odabir = Convert.ToInt32(Console.ReadKey().Key);
            }

            switch (odabir)
            {
                case (int)Key.DIGIT_1:
                    {
                        DohvatiStanje();
                        DohvatiTipku();
                        break;
                    }
                case (int)Key.DIGIT_2:
                    {
                        DohvatiIzvjestaj();
                        DohvatiTipku();
                        break;
                    }
                case (int)Key.DIGIT_3:
                    {
                        NovaIzdatnica();
                        DohvatiTipku();
                        break;
                    }
                case (int)Key.DIGIT_4:
                    {
                        NovaPrimka();
                        DohvatiTipku();
                        break;
                    }
                case (int)Key.DIGIT_5:
                    {
                        Pocetna();
                        break;
                    }
            }
        }

        //f koja ispisuje pregled stanja za svaki pojedini artikl na skladistu
        static void DohvatiStanje()
        {
            Console.Clear();
            List<Artikl> lArtikli = DohvatiArtikle();
            List<Dokument> lDokumenti = DohvatiDokumente();

            var tablicaStanja = new ConsoleTable("Rb.", "Naziv artikla ", "Cijena", "PS Kolicina", "PS Iznos", "Kolicina ulaza", "Iznos ulaz", "Kolicina izlaza", "Iznos izlaz", "TS Kolicina", "TS Iznos");
            for (int i = 0; i < lArtikli.Count(); i++)
            {
                //linq querry (LINQYOU) operations to manipulatre with lists
                var pocetno_kol = lDokumenti.Where(d => d.sifra == lArtikli[i].sifra && d.tip == (int)Tip.PS).Select(d => d.kolUlaz).FirstOrDefault();
                var pocetno_iznos = pocetno_kol * lArtikli[i].cijena;
                var kol_ulaz = lDokumenti.Where(d => d.sifra == lArtikli[i].sifra && d.tip == (int)Tip.PRM).Sum(d => d.kolUlaz);
                var iznos_ulaz = kol_ulaz * lArtikli[i].cijena;
                var kol_izlaz = lDokumenti.Where(d => d.sifra == lArtikli[i].sifra && d.tip == (int)Tip.IZD).Sum(d => d.kolIzlaz);
                var iznos_izlaz = kol_izlaz * lArtikli[i].cijena;
                var trenutno_kol = pocetno_kol + kol_ulaz - kol_izlaz;
                var jmj = lArtikli.Select(d => d.jmj).ToList();
                var trenutno_iznos = trenutno_kol * lArtikli[i].cijena;
                string valuta = " kn";

                tablicaStanja.AddRow(
                        i + 1 + ".",
                        lArtikli[i].naziv,
                        lArtikli[i].cijena + valuta,
                        pocetno_kol + " " + jmj[i],
                        pocetno_iznos + valuta,
                        kol_ulaz + " " + jmj[i],
                        iznos_ulaz + valuta,
                        kol_izlaz + " " + jmj[i],
                        iznos_izlaz + valuta,
                        trenutno_kol + " " + jmj[i],
                        trenutno_iznos + valuta
                        );
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n*****************************STANJE SKLADISTA*****************************");
            Console.ResetColor();
            tablicaStanja.Write();
        }

        //f koja omogucuje unos sifre/naziva te provjerava postoji li isti na skladistu te ispisuje stanje tog artikla
        static void DohvatiIzvjestaj()
        {
            List<Artikl> lArtikli = DohvatiArtikle();
            List<Dokument> lDokumenti = DohvatiDokumente();
            lDokumenti = lDokumenti.OrderBy(x => x.datum).ToList();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n\n~~~~~~~~~PONUDENI ARTIKLI~~~~~~~~~");
            Console.ResetColor();
            var tablicaArtikli = new ConsoleTable("Sifra","Naziv artikla");
            for (int i = 0; i < lArtikli.Count(); i++)
            {
                tablicaArtikli.AddRow(
                                lArtikli[i].sifra,
                                lArtikli[i].naziv
                                );
            }
            tablicaArtikli.Write();

            Console.Write("\nUnesite sifru/naziv zeljenog artikla: ");
            string zeljeniArtikl = Console.ReadLine();

            var sifreArtikla = lArtikli.Select(a => a.sifra).ToList(); //name or pw Artikla
            var naziviArtikla = lArtikli.Select(a => a.naziv).ToList();
            while (!sifreArtikla.Contains(zeljeniArtikl) && !naziviArtikla.Contains(zeljeniArtikl))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ne postoji artikl sa navedenom sifrom/nazivom, pokušajte ponovno.");
                Console.ResetColor();
                Console.Write("\nUnesite sifru/naziv artikla: ");
                zeljeniArtikl = Console.ReadLine();
            }
            Console.Clear();

            Zapis("Izvjestaj: " + zeljeniArtikl);
            var tablicaIzvjesaj = new ConsoleTable("Rb.", "Naziv artikla ", "Tip dokumenta", "Datum", "Sifra","Cijena", "Kolicina ulaz", "Iznos ulaz", "Kolicina izlaz", "Iznos izlaz");
            var dokumentiArtikla = lDokumenti.Where(d => d.sifra == zeljeniArtikl || d.naziv == zeljeniArtikl).ToList();
            var cijena = lArtikli.Where(d => d.sifra == zeljeniArtikl || d.naziv == zeljeniArtikl).Select(d => d.cijena).FirstOrDefault();
            var jmj = lArtikli.Where(d => d.sifra == zeljeniArtikl || d.naziv == zeljeniArtikl).Select(d => d.jmj).FirstOrDefault();
            string valuta = " kn";
            
            for (int i = 0; i < dokumentiArtikla.Count; i++)
            {
                string tip = "";
                if (dokumentiArtikla[i].tip == 1)
                {
                    tip = "PS";
                }
                else if (dokumentiArtikla[i].tip == 2)
                {
                    tip = "IZD";
                }
                else if (dokumentiArtikla[i].tip == 3)
                {
                    tip = "PRM";
                }

                tablicaIzvjesaj.AddRow(
                       i + 1 + ".",
                      dokumentiArtikla[i].naziv,
                      tip,
                      dokumentiArtikla[i].datum,
                      dokumentiArtikla[i].sifra,
                      cijena,
                      dokumentiArtikla[i].kolUlaz +" "+ jmj,
                      dokumentiArtikla[i].kolUlaz * cijena + valuta,
                      dokumentiArtikla[i].kolIzlaz +" "+ jmj,
                      dokumentiArtikla[i].kolIzlaz * cijena + valuta
                      );
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n*****************************IZVJESTAJ ARTIKLA*****************************");
            Console.ResetColor();
            tablicaIzvjesaj.Write();
        }

        //f treceg odabira koja stvara atribute koje pridruzuje cvoru u stanja.xml
        static void NovaIzdatnica()
        {
            Console.Clear();
            List<Artikl> lArtikli = DohvatiArtikle();
            List<Dokument> lDokumenti = DohvatiDokumente();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n*****************************NOVA IZDATNICA*****************************");
            Console.WriteLine("~~~~~~~~~PONUDJENI ARTIKLI~~~~~~~~~");
            Console.ResetColor();

            var tablicaArtikli = new ConsoleTable("Sifra", "Naziv artikla");
            for (int i = 0; i < lArtikli.Count(); i++)
            {
                tablicaArtikli.AddRow(
                        lArtikli[i].sifra,
                        lArtikli[i].naziv
                       );
            }
            tablicaArtikli.Write();

            Console.Write("\nUnesite sifru/naziv zeljenog artikla: ");
            string zeljeniArtikl = Console.ReadLine();

            var sifreArtikla = lArtikli.Select(a => a.sifra).ToList(); //name or pw Artikla
            var naziviArtikla = lArtikli.Select(a => a.naziv).ToList();
            while (!sifreArtikla.Contains(zeljeniArtikl) && !naziviArtikla.Contains(zeljeniArtikl))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ne postoji artikl sa navedenom sifrom/nazivom, pokušajte ponovno.");
                Console.ResetColor();
                Console.Write("\nUnesite sifru/naziv artikla: ");
                zeljeniArtikl = Console.ReadLine();
            }

            Console.Write("Unesite kolicinu izlaza: ");
            int kolicinaIzlaza = Convert.ToInt32(UnosBroja());
            for (int i = 0; i < lDokumenti.Count(); i++)
            {
                var pocetno_kol = lDokumenti.Where(d => (d.sifra == zeljeniArtikl || d.naziv == zeljeniArtikl)  && d.tip == (int)Tip.PS).Select(d => d.kolUlaz).FirstOrDefault();
                var kol_ulaz = lDokumenti.Where(d => (d.sifra == zeljeniArtikl || d.naziv == zeljeniArtikl) && d.tip == (int)Tip.PRM).Sum(d => d.kolUlaz);
                var kol_izlaz = lDokumenti.Where(d => (d.sifra == zeljeniArtikl || d.naziv == zeljeniArtikl) && d.tip == (int)Tip.IZD).Sum(d => d.kolIzlaz);
                var trenutno_kol = pocetno_kol + kol_ulaz - kol_izlaz;

                if (kolicinaIzlaza > trenutno_kol)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nNemoguca izdatnica! Kolicina izlaza veca od trenutnog stanja, pokusajte ponovno.\n");
                    Console.ResetColor();
                    Console.Write("Unesite kolicinu izlaza: ");
                    kolicinaIzlaza = Convert.ToInt32(UnosBroja());
                }
                else
                {
                    break;
                }
            }

            var izdatnica = new Dokument //kreiranje objekta strukture Dokument kojem zadajemo vrijednosti
            {
                tip = (int)Tip.IZD,
                datum = DateTime.Today.ToString("dd/MM/yyyy"),
                sifra = lArtikli.Where(x => x.sifra == zeljeniArtikl || x.naziv == zeljeniArtikl).Select(x => x.sifra).FirstOrDefault(),
                naziv = lArtikli.Where(x => x.sifra == zeljeniArtikl || x.naziv == zeljeniArtikl).Select(x => x.naziv).FirstOrDefault(),
                kolUlaz = 0,
                iznosUlaz = 0,
                kolIzlaz = kolicinaIzlaza,
                iznosIzlaz = kolicinaIzlaza * lArtikli.Where(x => x.sifra == zeljeniArtikl || x.naziv == zeljeniArtikl).Select(x => x.cijena).FirstOrDefault()
            };

            lDokumenti.Add(izdatnica);
            
            XmlDocument xStanje = new XmlDocument(); 
            xStanje.Load(Path.Combine(Environment.CurrentDirectory + "/xml", "stanje.xml"));

            XmlElement eNode = xStanje.CreateElement("dokument");  // element > atributi
            XmlAttribute atributTip = xStanje.CreateAttribute("tip");
            XmlAttribute atributDatum = xStanje.CreateAttribute("datum");
            XmlAttribute atributSifra = xStanje.CreateAttribute("sifra");
            XmlAttribute atributNaziv = xStanje.CreateAttribute("naziv");
            XmlAttribute atributKolUlaz = xStanje.CreateAttribute("kol_ulaz");
            XmlAttribute atributKolIzlaz = xStanje.CreateAttribute("kol_izlaz");
            XmlAttribute atributIznosUlaz = xStanje.CreateAttribute("iznos_ulaz");
            XmlAttribute atributIznosIzlaz = xStanje.CreateAttribute("iznos_izlaz");

            atributTip.Value = izdatnica.tip.ToString(); // povezivanje objekta i atributa
            atributDatum.Value = izdatnica.datum;
            atributSifra.Value = izdatnica.sifra;
            atributNaziv.Value = izdatnica.naziv;
            atributKolUlaz.Value = izdatnica.kolUlaz.ToString();
            atributKolIzlaz.Value = izdatnica.kolIzlaz.ToString();
            atributIznosUlaz.Value = izdatnica.iznosUlaz.ToString();
            atributIznosIzlaz.Value = izdatnica.iznosIzlaz.ToString();

            eNode.Attributes.Append(atributTip);//povezivanje atributa i elementa
            eNode.Attributes.Append(atributDatum);
            eNode.Attributes.Append(atributSifra);
            eNode.Attributes.Append(atributNaziv);
            eNode.Attributes.Append(atributKolUlaz);
            eNode.Attributes.Append(atributKolIzlaz);
            eNode.Attributes.Append(atributIznosUlaz);
            eNode.Attributes.Append(atributIznosIzlaz);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n\nOdaberite[Y] -> Spremi izdatnicu!");
            Console.WriteLine("Odaberite bilo koju drugu tipku kako biste odustali.");
            Console.ResetColor();
            Console.Write("\nOdabir: ");
            int odabir = Convert.ToInt32(Console.ReadKey().Key);
            if (odabir != (int)Key.Y)
            {
                Izbornik();
            }
            else
            {
                xStanje.DocumentElement.AppendChild(eNode); // podiznje elementa iznad -> root elementa
                xStanje.Save(Path.Combine(Environment.CurrentDirectory + "/xml", "stanje.xml"));
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n*****************IZDATNICA USPJESNO SPREMLJENA*********************\n");
                Console.ResetColor();
                Zapis("Izdatnica: " + zeljeniArtikl + "Kolicina izlaza:" + kolicinaIzlaza);
            }
        }

        //f cetvrtog odabira koja stvara atribute koje pridruzuje cvoru u stanja.xml
        static void NovaPrimka()
        {
            Console.Clear();
            List<Artikl> lArtikli = DohvatiArtikle();
            List<Dokument> lDokumenti = DohvatiDokumente();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n*****************************NOVA PRIMKA***************************");
            Console.WriteLine("~~~~~~~~~PONUDENI ARTIKLI~~~~~~~~~");
            Console.ResetColor();

            //An implicitly typed local variable -var- is strongly typed just as if you had declared the type yourself, but the compiler determines the type
            var tablicaArtikli = new ConsoleTable("Sifra", "Naziv artikla");
            for (int i = 0; i < lArtikli.Count(); i++)
            {
                tablicaArtikli.AddRow(
                        lArtikli[i].sifra,
                        lArtikli[i].naziv
                       );
            }
            tablicaArtikli.Write();

            Console.Write("\nUnesite sifru/naziv zeljenog artikla: ");
            string zeljeniArtikl = Console.ReadLine();

            var sifreArtikla = lArtikli.Select(a => a.sifra).ToList(); //name or pw Artikla
            var naziviArtikla = lArtikli.Select(a => a.naziv).ToList();
            while (!sifreArtikla.Contains(zeljeniArtikl) && !naziviArtikla.Contains(zeljeniArtikl))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ne postoji artikl sa navedenom sifrom/nazivom, pokušajte ponovno.");
                Console.ResetColor();
                Console.Write("\nUnesite sifru/naziv artikla: ");
                zeljeniArtikl = Console.ReadLine();
            }

            Console.Write("Unesite kolicinu ulaza: ");
            int kolicinaUlaza = Convert.ToInt32(UnosBroja());

            var primka = new Dokument
            {
                tip = (int)Tip.PRM,
                datum = DateTime.Today.ToString("dd/MM/yyyy"),
                sifra = lArtikli.Where(x => x.sifra == zeljeniArtikl || x.naziv == zeljeniArtikl).Select(x => x.sifra).FirstOrDefault(),
                naziv = lArtikli.Where(x => x.sifra == zeljeniArtikl || x.naziv == zeljeniArtikl).Select(x => x.naziv).FirstOrDefault(),
                kolUlaz = kolicinaUlaza,
                iznosUlaz = kolicinaUlaza * lArtikli.Where(x => x.sifra == zeljeniArtikl || x.naziv == zeljeniArtikl).Select(x => x.cijena).FirstOrDefault(),
                kolIzlaz = 0,
                iznosIzlaz = 0,
            };
            lDokumenti.Add(primka);

            XmlDocument xStanje = new XmlDocument();
            xStanje.Load(Path.Combine(Environment.CurrentDirectory + "/xml", "stanje.xml"));

            XmlElement eNode = xStanje.CreateElement("dokument");  // element > atributi
            XmlAttribute atributTip = xStanje.CreateAttribute("tip");
            XmlAttribute atributDatum = xStanje.CreateAttribute("datum");
            XmlAttribute atributSifra = xStanje.CreateAttribute("sifra");
            XmlAttribute atributNaziv = xStanje.CreateAttribute("naziv");
            XmlAttribute atributKolUlaz = xStanje.CreateAttribute("kol_ulaz");
            XmlAttribute atributKolIzlaz = xStanje.CreateAttribute("kol_izlaz");
            XmlAttribute atributIznosUlaz = xStanje.CreateAttribute("iznos_ulaz");
            XmlAttribute atributIznosIzlaz = xStanje.CreateAttribute("iznos_izlaz");

            atributTip.Value = primka.tip.ToString();
            atributDatum.Value = primka.datum;
            atributSifra.Value = primka.sifra;
            atributNaziv.Value = primka.naziv;
            atributKolUlaz.Value = primka.kolUlaz.ToString();
            atributKolIzlaz.Value = primka.kolIzlaz.ToString();
            atributIznosUlaz.Value = primka.iznosUlaz.ToString();
            atributIznosIzlaz.Value = primka.iznosIzlaz.ToString();

            eNode.Attributes.Append(atributTip);
            eNode.Attributes.Append(atributDatum);
            eNode.Attributes.Append(atributSifra);
            eNode.Attributes.Append(atributNaziv);
            eNode.Attributes.Append(atributKolUlaz);
            eNode.Attributes.Append(atributKolIzlaz);
            eNode.Attributes.Append(atributIznosUlaz);
            eNode.Attributes.Append(atributIznosIzlaz);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n\nOdaberite[Y] -> Spremi primku!");
            Console.WriteLine("Odaberite bilo koju drugu tipku kako biste odustali.");
            Console.ResetColor();
            Console.Write("\nOdabir: ");
            int odabir = Convert.ToInt32(Console.ReadKey().Key);
            if (odabir != (int)Key.Y)
            {
                Izbornik();
            }
            else
            {
                xStanje.DocumentElement.AppendChild(eNode);
                xStanje.Save(Path.Combine(Environment.CurrentDirectory + "/xml", "stanje.xml"));
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n*********************PRIMKA USPJESNO SPREMLJENA**********************\n");
                Console.ResetColor();
                Zapis("Primka: " + zeljeniArtikl + "Kolicina ulaza:" + kolicinaUlaza);
            }

        }
    }
}
/*        
     *********  using(oSr) 
     
     When the lifetime of an IDisposable object is limited to a single method,   
    you should declare and instantiate it in the using statement. 
    The using statement calls the Dispose method on the object in the correct way,   
    and (when you use it as shown earlier) it also causes the object itself to go out of 
    scope as soon as Dispose is called. Within the using block, the object is read-only 
    and cannot be modified or reassigned.
    
     ********* new
     Used to create objects and invoke constructors.
     
     ********* static 
     * 
     * C# includes static keyword just like other programming languages such as C++, 
     * Java, etc. The static keyword can be applied on classes, variables,
     * methods, properties, operators, events and constructors. However, it cannot be 
     * used with indexers, destructors or types other than classes.
     * 
     * The static modifier makes an item non-instantiable, 
     * it means the static item cannot be instantiated. If the static 
     * modifier is applied to a class then that class cannot be instant
     * iated using the new keyword. If the static modifier is applied to a va
     * riable, method or property of class then they can be accessed without creating 
     * an object of the class, just use className.propertyName, className.methodName.
     * 
     * 
     * ********* public 
     * 
     * Public access is the most permissive access level. 
     * There are no restrictions on accessing public members, as in this example:
     //https://www.mycity.rs/NET/4-C-Metode-Nizovi.html


    **********
    * daj danasnji datum
    string today = DateTime.Today.ToString("dd/MM/yyyy");

    ** adding new node
    * https://stackoverflow.com/questions/14798854/c-xml-adding-new-nodes
   
    ** potrebno napraviti strukturu za artikle
    * slicno kao za usere :)
    * 
    
    ******HOW TO SORT ELEMENT OF ARRAY LIST IN c#
    * https://stackoverflow.com/questions/852439/how-to-sort-elements-of-array-list-in-c-sharp
     
     ******SORTING LIST BY DATE
     * https://stackoverflow.com/questions/22437031/how-do-i-sort-a-list-of-structures-with-a-specific-struct-element
     
     
    *****ADDING NODE TO XML
    * https://stackoverflow.com/questions/31421891/how-a-add-a-xml-node-attributes-using-c
    * 
    * 
     
     **PASSWORD MASKING
     * https://stackoverflow.com/questions/3404421/password-masking-console-application

    ** .TryParse() out--
    * https://stackoverflow.com/questions/19592084/why-do-all-tryparse-overloads-have-an-out-parameter
     
    ** DELAY
     System.Threading.Thread.Sleep(5000);*/
