using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaActivitati
{
    public enum Categorie
    {
        Studii,
        Hobby,
        Relax,
        Altele
    }

    [Flags]
    public enum OptiuniExtra
    {
        Niciuna = 0,
        Important = 1,
        Online = 2
    }


    public enum Reminder
    {
        Memento,
        Email
    }

    public class Activitate
    {
        public string Titlu { get; set; }
        public string Descriere { get; set; }
        public DateTime Data { get; set; }
        public string Ora { get; set; }
        public Categorie TipCategorie { get; set; }
        public OptiuniExtra Optiuni { get; set; }
        public Reminder TipReminder { get; set; }

        public Activitate(string titlu, string descriere, DateTime data, string ora, Categorie tipCategorie, OptiuniExtra optiuni, Reminder tipReminder)
        {
            Titlu = titlu;
            Descriere = descriere;
            Data = data;
            Ora = ora;
            TipCategorie = tipCategorie;
            Optiuni = optiuni;
            TipReminder = tipReminder;
        }
    }
}
