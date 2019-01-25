using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using ConsoleTables;

namespace VSMTI_Gradnja_doo
{
    class Program    //klasu imamo da mozemo pozivati objekte
    {
        public enum Tip         //An enum is a convenient way to use names instead of numbers, in order to denote something. 
        {                       //It makes your code far more readable and maintainable than using numbers.         
            PS = 1,
            IZD = 2,
            PRM = 3
        }

        static void Main(string[] args)
        {
            DohvatiPocetniIzbornik();
        }

        public struct Korisnik        //struktura koja definira user-a
        {
            public string email;
            public string lozinka;

            public Korisnik(string u, string p)
            {
                email = u;
                lozinka = p;
            }
        }

        public struct Artikl        //struktura koja definira slozen tip podatka artikl
        {
            public string sifra;
            public string naziv;
            public string jmj;      //jedinicna mjera
            public decimal cijena;

            public Artikl(string s, string n, string j, decimal c)
            {
                sifra = s;
                naziv = n;
                jmj = j;
                cijena = c;
            }
        }

        public struct Dokument        // struktura koja definira slozen tip podatka dokument iz stanja.xml
        {
            public int tip;
            public string datum;
            public string sifra;
            public string naziv;
            public int kolUlaz;
            public decimal iznosUlaz;
            public int kolIzlaz;
            public decimal iznosIzlaz;

            public Dokument(int t, string d, string c, string n, int ku, decimal iu, int ki, decimal ii)  //konstruktor - funkcija koja se poziva prilikom kreiranja objekta strukture Dokument
            {
                tip = t;                 //"tip" je podatkovni clan , a "t" argument konstruktora
                datum = d;
                sifra = c;
                naziv = n;
                kolUlaz = ku;
                iznosUlaz = iu;
                kolIzlaz = ki;
                iznosIzlaz = ii;
            }
        }

        static List<Korisnik> DohvatiKorisnike()        //kreiranje liste objekata strukture Korisnik
        {
            List<Korisnik> lUsers = new List<Korisnik>();
            string sXml = "";
            StreamReader oSr = new StreamReader("config.xml");
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
            return lUsers;
        }

        static List<Artikl> DohvatiArtikle()        //kreiranje liste objekata strukture Artikl
        {
            List<Artikl> lArticles = new List<Artikl>();
            string sXml = "";
            StreamReader oSr = new StreamReader("artikli.xml");
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
            return lArticles;
        }

        static List<Dokument> DohvatiDokumente()        //kreiranje liste objekata strukture Dokument
        {
            List<Dokument> lDokumenti = new List<Dokument>();
            string sXml = "";
            StreamReader oSr = new StreamReader("stanje.xml");
            using (oSr)
            {
                sXml = oSr.ReadToEnd();
            }
            XmlDocument xDokument = new XmlDocument();
            xDokument.LoadXml(sXml);
            XmlNodeList oNodes = xDokument.SelectNodes("//dokumenti/dokument");
            foreach (XmlNode oNode in oNodes)
            {
                int tip = Convert.ToInt32(oNode.Attributes["tip"].Value);
                string datum = oNode.Attributes["datum"].Value;
                string sifra = oNode.Attributes["sifra"].Value;
                string naziv = oNode.Attributes["naziv"].Value;
                int kolUlaz = Convert.ToInt32(oNode.Attributes["kol_ulaz"].Value);
                decimal iznosUlaz = decimal.Parse(oNode.Attributes["iznos_ulaz"].Value);
                int kolIzlaz = Convert.ToInt32(oNode.Attributes["kol_izlaz"].Value);
                decimal iznosIzlaz = decimal.Parse(oNode.Attributes["iznos_izlaz"].Value);
                lDokumenti.Add(new Dokument(tip, datum, sifra, naziv, kolUlaz, iznosUlaz, kolIzlaz, iznosIzlaz));
            }
            return lDokumenti;
        }

        static bool ProvjeriUcitavanjeDatoteka()        //f za provjeru moze li ste ucitati datoteka
        {
            bool bUcitavanje = false;
            try            //provjera da li su ucitane vrsi se pokusajem ucitavanja, ako nisu ucitane ispisuje se blok catch
            {
                XmlDocument config = new XmlDocument();
                config.Load("config.xml");

                XmlDocument stanje = new XmlDocument();
                stanje.Load("stanje.xml");

                XmlDocument artikli = new XmlDocument();
                artikli.Load("artikli.xml");
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

        static void DohvatiPocetniIzbornik()        //static je f koja se moze pozvati bez da je objekt klase kojoj pripada ta funkcija instanciran.
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red; //prvi plan(text)
            string sXml = "";
            StreamReader oSr = new StreamReader("vsmti.txt");
            using (oSr)
            {
                sXml = oSr.ReadToEnd();
            }
            Console.WriteLine(sXml);

            Console.WriteLine("\nPritisnite [P] za  prijavu korisnika");
            Console.WriteLine("Pritisnite [X] za izlazak iz programa");
            Console.ResetColor();
            Console.Write("\nVaš odabir: ");
            int odabir = Convert.ToInt32(Console.ReadKey().Key); //.Key cita ascii kod tipke koju pritisnemo
            while (odabir != (int)ConsoleKey.P && odabir != (int)ConsoleKey.X)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nKrivi odabir, pokušajte ponovno\n");
                Console.ResetColor();
                Console.Write("\nVaš odabir: ");
                odabir = Convert.ToInt32(Console.ReadKey().Key);
            }
            switch (odabir)
            {
                case (int)ConsoleKey.P:
                    {
                        if (ProvjeriUcitavanjeDatoteka())
                        {
                            Prijava();
                        }
                        break;
                    }
                case (int)ConsoleKey.X:
                    {
                        Environment.Exit(0);
                        break;
                    }
            }
        }

        static void Prijava()        //f koja omogucuje unos podataka i provjerava jesu li ti podatci istovjetni sa onim iz LoginCheck()
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

                if (ProvjeriPrijavu(email, lozinka))
                {
                    string dobrodosli = "";
                    StreamReader oSr = new StreamReader("welcome.txt");
                    using (oSr) // expl.down
                    {
                        dobrodosli = oSr.ReadToEnd();
                    }
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\n" + dobrodosli);
                    Console.ResetColor();
                    System.Threading.Thread.Sleep(1500);//delay
                    DohvatiGlavniIzbornik();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nE-mail i/ili lozinka nisu valjani.Pokusajte ponovno\n");
                    Console.ResetColor();
                }
            } while (ProvjeriPrijavu(email, lozinka) == false);
        }

        static bool ProvjeriPrijavu(string email, string lozinka)        //f pomaze pri provjeri da li postoji korisnik sa unesenom sifrnom i imenom
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
            var Korisnik = lKorisnici.Where(d => d.email == email && d.lozinka == lozinka).FirstOrDefault();
            return bUser;
        }

        static string DohvatiLozinku()        //f koja omogucuje upis sifre, imaginarno pretvara svako slovo u *  i vraca sifru
        {
            string lozinka = "";
            Console.Write("Sifra: ");
            do
            {
                ConsoleKeyInfo tipka = Console.ReadKey(true); //opisuje konzolnu tipku koja je pritisnuta                 // Backspace Should Not Work
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

        static string DohvatiBroj()        //f koja omogucuje unos iskljucivo brojcanih vrijednosti te vraca istu
        {
            string broj = "";
            int noNeed = 0;
            ConsoleKeyInfo chr;
            do
            {
                chr = Console.ReadKey(true);  // true - don't display pressed key
                if (chr.Key != ConsoleKey.Backspace)
                {
                    bool kontrola = int.TryParse(chr.KeyChar.ToString(), out noNeed);
                    if (kontrola)
                    {
                        broj += chr.KeyChar;
                        Console.Write(chr.KeyChar);
                    }
                }
                else
                {
                    if (chr.Key == ConsoleKey.Backspace && broj.Length > 0)
                    {
                        broj = broj.Substring(0, (broj.Length - 1));
                        Console.Write("\b \b");
                    }
                }
            } while (chr.Key != ConsoleKey.Enter);

            while (int.Parse(broj) == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\nNemoguc unos! Unesite broj veci od nule: ");
                Console.ResetColor();
                broj = DohvatiBroj();
            }
            return broj;
        }

        static void DohvatiTipku()        //f koja nakon svake operacije u izborniku omogucuje povrat na izbornik
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nPritisnite [C] za povratak u glavni izbornik");
            Console.ResetColor();
            Console.Write("\nVas odabir: ");
            int odabir = Convert.ToInt32(Console.ReadKey().Key);
            while (odabir != (int)ConsoleKey.C)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nKrivi odabir, pokušajte ponovno\n");
                Console.ResetColor();
                Console.Write("\nVas odabir: ");
                odabir = Convert.ToInt32(Console.ReadKey().Key);
            }
            if (odabir == (int)ConsoleKey.C)
            {
                DohvatiGlavniIzbornik();
            }
        }

        static void DohvatiGlavniIzbornik()        //f za izlist izbornika i odabir
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
            int[] dozvoljeniUnosi = { (int)ConsoleKey.D1, (int)ConsoleKey.D2, (int)ConsoleKey.D3, (int)ConsoleKey.D4, (int)ConsoleKey.D5 }; //polje ascii enum key
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
                case (int)ConsoleKey.D1:
                    {
                        DohvatiStanje();
                        DohvatiTipku();
                        break;
                    }
                case (int)ConsoleKey.D2:
                    {
                        DohvatiIzvjestaj();
                        DohvatiTipku();
                        break;
                    }
                case (int)ConsoleKey.D3:
                    {
                        NovaIzdatnica();
                        DohvatiTipku();
                        break;
                    }
                case (int)ConsoleKey.D4:
                    {
                        NovaPrimka();
                        DohvatiTipku();
                        break;
                    }
                case (int)ConsoleKey.D5:
                    {
                        DohvatiPocetniIzbornik();
                        break;
                    }
            }
        }

        static string DohvatiTablicuArtikala()
        {
            List<Artikl> lArtikli = DohvatiArtikle();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n~~~~~~~~~PONUDENI ARTIKLI~~~~~~~~~");
            Console.ResetColor();

            var tablicaArtikli = new ConsoleTable("Sifra", "Naziv artikla"); //An implicitly typed local variable -var- is strongly typed just as if you had declared the type yourself, but the compiler determines the type
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
            return zeljeniArtikl;
        }

        static void DohvatiStanje() //f koja ispisuje pregled stanja za svaki pojedini artikl na skladistu
        {
            Console.Clear();
            List<Artikl> lArtikli = DohvatiArtikle();
            List<Dokument> lDokumenti = DohvatiDokumente();

            var tablicaStanja = new ConsoleTable("Rb.", "Naziv artikla ", "Cijena", "PS Kolicina", "PS Iznos", "Kolicina ulaza", "Iznos ulaz", "Kolicina izlaza", "Iznos izlaz", "TS Kolicina", "TS Iznos");
            for (int i = 0; i < lArtikli.Count(); i++)                //linq querry (LINQYOU) operations to manipulatre with lists
            {
                var pocetnoKol = lDokumenti.Where(d => d.sifra == lArtikli[i].sifra && d.tip == (int)Tip.PS).Select(d => d.kolUlaz).FirstOrDefault();
                var pocetnoIznos = pocetnoKol * lArtikli[i].cijena;
                var kolUlaz = lDokumenti.Where(d => d.sifra == lArtikli[i].sifra && d.tip == (int)Tip.PRM).Sum(d => d.kolUlaz);
                var iznosUlaz = kolUlaz * lArtikli[i].cijena;
                var kolIzlaz = lDokumenti.Where(d => d.sifra == lArtikli[i].sifra && d.tip == (int)Tip.IZD).Sum(d => d.kolIzlaz);
                var iznosIzlaz = kolIzlaz * lArtikli[i].cijena;
                var trenutnoKol = pocetnoKol + kolUlaz - kolIzlaz;
                var jmj = lArtikli.Select(d => d.jmj).ToList();
                var trenutnoIznos = trenutnoKol * lArtikli[i].cijena;
                string valuta = " kn";

                tablicaStanja.AddRow(
                        i + 1 + ".",
                        lArtikli[i].naziv,
                        lArtikli[i].cijena + valuta,
                        pocetnoKol + " " + jmj[i],
                        pocetnoIznos + valuta,
                        kolUlaz + " " + jmj[i],
                        iznosUlaz + valuta,
                        kolIzlaz + " " + jmj[i],
                        iznosIzlaz + valuta,
                        trenutnoKol + " " + jmj[i],
                        trenutnoIznos + valuta
                        );
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n*****************************STANJE SKLADISTA*****************************");
            Console.ResetColor();
            tablicaStanja.Write();
        }

        static void DohvatiIzvjestaj()          //f koja omogucuje unos sifre/naziva te provjerava postoji li isti na skladistu te ispisuje stanje tog artikla
        {
            Console.Clear();
            List<Artikl> lArtikli = DohvatiArtikle();
            List<Dokument> lDokumenti = DohvatiDokumente();
            string zeljeniArtikl = DohvatiTablicuArtikala();
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n*****************************IZVJESTAJ ARTIKLA*****************************");
            Console.ResetColor();

            var pocetnoKol = lDokumenti.Where(d => (d.sifra == zeljeniArtikl || d.naziv == zeljeniArtikl) && d.tip == (int)Tip.PS).Select(d => d.kolUlaz).FirstOrDefault();
            var kolUlaz = lDokumenti.Where(d => (d.sifra == zeljeniArtikl || d.naziv == zeljeniArtikl) && d.tip == (int)Tip.PRM).Sum(d => d.kolUlaz);
            var kolIzlaz = lDokumenti.Where(d => (d.sifra == zeljeniArtikl || d.naziv == zeljeniArtikl) && d.tip == (int)Tip.IZD).Sum(d => d.kolIzlaz);
            var trenutnoKol = pocetnoKol + kolUlaz - kolIzlaz;
            var trenutnoIznos = trenutnoKol * lArtikli.Where(d => (d.sifra == zeljeniArtikl || d.naziv == zeljeniArtikl)).Select(d => d.cijena).FirstOrDefault();
            var jmj = lArtikli.Where(d => d.sifra == zeljeniArtikl || d.naziv == zeljeniArtikl).Select(d => d.jmj).FirstOrDefault();
            string valuta = " kn";
            string tip = "";

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Trenutna kolicina artikla je: ");
            Console.ResetColor();
            Console.Write(trenutnoKol + " " + jmj + "\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Trenutan ukupan iznos artikla je: ");
            Console.ResetColor();
            Console.Write(trenutnoIznos + valuta + "\n");

            var tablicaIzvjesaj = new ConsoleTable("Rb.", "Naziv artikla ", "Tip dokumenta", "Datum", "Sifra", "Cijena", "Kolicina ulaz", "Iznos ulaz", "Kolicina izlaz", "Iznos izlaz");
            var dokumentiArtikla = lDokumenti.Where(d => d.sifra == zeljeniArtikl || d.naziv == zeljeniArtikl).OrderBy(d => DateTime.Today).ToList();
            var cijena = lArtikli.Where(d => d.sifra == zeljeniArtikl || d.naziv == zeljeniArtikl).Select(d => d.cijena).FirstOrDefault();
            for (int i = 0; i < dokumentiArtikla.Count; i++)
            {
                switch (dokumentiArtikla[i].tip)
                {
                    case 1:
                        tip = "PS";
                        break;
                    case 2:
                        tip = "IZD";
                        break;
                    case 3:
                        tip = "PRM";
                        break;
                }
                tablicaIzvjesaj.AddRow(
                       i + 1 + ".",
                      dokumentiArtikla[i].naziv,
                      tip,
                      dokumentiArtikla[i].datum,
                      dokumentiArtikla[i].sifra,
                      cijena + valuta,
                      dokumentiArtikla[i].kolUlaz + " " + jmj,
                      dokumentiArtikla[i].kolUlaz * cijena + valuta,
                      dokumentiArtikla[i].kolIzlaz + " " + jmj,
                      dokumentiArtikla[i].kolIzlaz * cijena + valuta
                      );
            }
            tablicaIzvjesaj.Write();
        }

        static void NovaIzdatnica()
        {
            Console.Clear();
            List<Artikl> lArtikli = DohvatiArtikle();
            List<Dokument> lDokumenti = DohvatiDokumente();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n*****************************NOVA IZDATNICA*****************************");
            Console.ResetColor();
            string zeljeniArtikl = DohvatiTablicuArtikala();

            var pocetnoKol = lDokumenti.Where(d => (d.sifra == zeljeniArtikl || d.naziv == zeljeniArtikl) && d.tip == (int)Tip.PS).Select(d => d.kolUlaz).FirstOrDefault();
            var kolUlaz = lDokumenti.Where(d => (d.sifra == zeljeniArtikl || d.naziv == zeljeniArtikl) && d.tip == (int)Tip.PRM).Sum(d => d.kolUlaz);
            var kolIzlaz = lDokumenti.Where(d => (d.sifra == zeljeniArtikl || d.naziv == zeljeniArtikl) && d.tip == (int)Tip.IZD).Sum(d => d.kolIzlaz);
            var trenutnoKol = pocetnoKol + kolUlaz - kolIzlaz;
            var jmj = lArtikli.Where(d => d.sifra == zeljeniArtikl || d.naziv == zeljeniArtikl).Select(d => d.jmj).FirstOrDefault();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Trenutna kolicina artikla je: ");
            Console.ResetColor();
            Console.Write(trenutnoKol + " " + jmj);

            Console.Write("\nUnesite kolicinu izlaza: ");
            int kolicinaIzlaza = Convert.ToInt32(DohvatiBroj());

            if (trenutnoKol == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nNemoguca izdatnica! Artikla nema na skladištu.\n");
                Console.ResetColor();
                DohvatiTipku();
            }
            if (trenutnoKol < kolicinaIzlaza)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nNemoguca izdatnica! Kolicina izlaza veca od trenutnog stanja, pokusajte ponovno.\n");
                Console.ResetColor();
                Console.Write("Unesite kolicinu izlaza: ");
                kolicinaIzlaza = Convert.ToInt32(DohvatiBroj());
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
            xStanje.Load("stanje.xml");
            XmlElement eNode = xStanje.CreateElement("dokument");  // element > atributi
            string nazivDokumenta = "izdatnicu";
            SpremiCvor(xStanje, eNode, izdatnica, nazivDokumenta);
        }

        static void NovaPrimka()
        {
            Console.Clear();
            List<Artikl> lArtikli = DohvatiArtikle();
            List<Dokument> lDokumenti = DohvatiDokumente();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n*****************************NOVA PRIMKA***************************");
            Console.ResetColor();

            string zeljeniArtikl = DohvatiTablicuArtikala();
            Console.Write("Unesite kolicinu ulaza: ");
            int kolicinaUlaza = Convert.ToInt32(DohvatiBroj());
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
            xStanje.Load("stanje.xml");
            XmlElement eNode = xStanje.CreateElement("dokument");  // element > atributi
            string nazivDokumenta = "primku";
            SpremiCvor(xStanje, eNode, primka, nazivDokumenta);
        }

        static void SpremiCvor(XmlDocument xStanje, XmlElement eNode, Dokument dokument, string nazivDokumenta)
        {
            eNode.SetAttribute("tip", dokument.tip.ToString());
            eNode.SetAttribute("datum", dokument.datum);
            eNode.SetAttribute("sifra", dokument.sifra);
            eNode.SetAttribute("naziv", dokument.naziv.ToString());
            eNode.SetAttribute("kol_ulaz", dokument.kolUlaz.ToString());
            eNode.SetAttribute("kol_izlaz", dokument.kolIzlaz.ToString());
            eNode.SetAttribute("iznos_ulaz", dokument.iznosUlaz.ToString());
            eNode.SetAttribute("iznos_izlaz", dokument.iznosIzlaz.ToString());

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n\nOdaberite[Y] -> Spremi " + nazivDokumenta + " !");
            Console.WriteLine("Odaberite bilo koju drugu tipku kako biste odustali.");
            Console.ResetColor();
            Console.Write("\nOdabir: ");
            int odabir = Convert.ToInt32(Console.ReadKey(true).Key);
            if (odabir != (int)ConsoleKey.Y)
            {
                DohvatiGlavniIzbornik();
            }
            else
            {
                xStanje.DocumentElement.AppendChild(eNode);
                xStanje.Save("stanje.xml");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n*********************PRIMKA USPJESNO SPREMLJENA**********************\n");
                Console.ResetColor();
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
   
    ******HOW TO SORT ELEMENT OF ARRAY LIST IN c#
    * https://stackoverflow.com/questions/852439/how-to-sort-elements-of-array-list-in-c-sharp
    * 
     
     ******SORTING LIST BY DATE
     * https://stackoverflow.com/questions/22437031/how-do-i-sort-a-list-of-structures-with-a-specific-struct-element
     
    *****ADDING NODE TO XML
    * https://stackoverflow.com/questions/31421891/how-a-add-a-xml-node-attributes-using-c
    * 
     **PASSWORD MASKING
     * https://stackoverflow.com/questions/3404421/password-masking-console-application

    ** .TryParse() out--
    * https://stackoverflow.com/questions/19592084/why-do-all-tryparse-overloads-have-an-out-parameter
     
    ** DELAY
     System.Threading.Thread.Sleep(5000);*/

/*XmlAttribute atributTip = xStanje.CreateAttribute("tip");
       XmlAttribute atributDatum = xStanje.CreateAttribute("datum");
       XmlAttribute atributSifra = xStanje.CreateAttribute("sifra");
       XmlAttribute atributNaziv = xStanje.CreateAttribute("naziv");
       XmlAttribute atributKolUlaz = xStanje.CreateAttribute("kol_ulaz");
       XmlAttribute atributKolIzlaz = xStanje.CreateAttribute("kol_izlaz");
       XmlAttribute atributIznosUlaz = xStanje.CreateAttribute("iznos_ulaz");
       XmlAttribute atributIznosIzlaz = xStanje.CreateAttribute("iznos_izlaz");

       atributTip.Value = papir.tip.ToString();
       atributDatum.Value = papir.datum;
       atributSifra.Value = papir.sifra;
       atributNaziv.Value = papir.naziv;
       atributKolUlaz.Value = papir.kolUlaz.ToString();
       atributKolIzlaz.Value = papir.kolIzlaz.ToString();
       atributIznosUlaz.Value = papir.iznosUlaz.ToString();
       atributIznosIzlaz.Value = papir.iznosIzlaz.ToString();

       eNode.Attributes.Append(atributTip);
       eNode.Attributes.Append(atributDatum);
       eNode.Attributes.Append(atributSifra);
       eNode.Attributes.Append(atributNaziv);
       eNode.Attributes.Append(atributKolUlaz);
       eNode.Attributes.Append(atributKolIzlaz);
       eNode.Attributes.Append(atributIznosUlaz);
       eNode.Attributes.Append(atributIznosIzlaz);
       
        static void Zapis(string sArg)        //f za logove
        {
            string trenutno = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ssTZD");
            StreamWriter oSw = new StreamWriter("zapisi.log", true);
            using (oSw)
            {
                oSw.WriteLine("\n");
                oSw.WriteLine(sArg);
                oSw.WriteLine(trenutno);
            }
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
*/
