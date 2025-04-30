using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AgendaActivitati
{
    public class Agenda
    {
        public List<Activitate> Activitati { get; set; } = new List<Activitate>();
        public string caleFisier = @"C:\Users\danie\OneDrive\Desktop\UNIVER 24\AgendaActivitati\Activitati.txt";

        public void AdaugaActivitate (Activitate activitate)
        {
            Activitati.Add(activitate);
            ScrieFisier();
        }

        public void ScrieFisier()
        {
            List<string> linii = new List<string>();

            foreach (var activitate in Activitati)
            {
                string linie = $"{activitate.Titlu}|{activitate.Descriere}|{activitate.Data:dd/MM/yyyy}|{activitate.Ora}" +
                    $"|{(int)activitate.TipCategorie}|{(int)activitate.Optiuni}|{(int)activitate.TipReminder}";
                linii.Add(linie);
            }
            File.WriteAllLines(caleFisier, linii);
        }

    }
}
