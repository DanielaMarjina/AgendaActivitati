using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgendaActivitati
{
    public partial class FormAgenda : Form
    {
        private Agenda agenda;
        private ListBox listboxActivitati;
        private Button btnAdauga;
        private Button btnCauta;
        private Button btnEditeaza;
        private Button btnSterge;
        private Label lbTitlu;
        private TextBox txtCautare;
        private Button btnReset;
        private Button btnSorteaza;


        public FormAgenda()
        {
            InitializeComponent();
            agenda = new Agenda();
            IncarcaInterfata();
            IncarcaActivitatiFisier();
            VerificaReminders();
        }

        private void IncarcaInterfata()
        {
            this.Text = "Agenda";
            this.Size = new Size(500, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FloralWhite;

            lbTitlu = new Label()
            {
                Text = "Activitati",
                Font = new Font("Arial", 16, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };
            this.Controls.Add(lbTitlu);

            listboxActivitati = new ListBox()
            {
                Size = new Size(450, 200),
                Location = new Point(20, 50)
            };
            this.Controls.Add(listboxActivitati);

            btnAdauga = new Button()
            {
                Text = "Adauga activitate:",
                Size = new Size(150, 60),
                Location = new Point(20, 320)
            };
            btnAdauga.Click += BtnAdauga_Click;
            this.Controls.Add(btnAdauga);

            btnSorteaza = new Button()
            {
                Text = "Sorteaza",
                Location = new Point(20, 290),
                Size = new Size(60, 25)
            };
            btnSorteaza.Click += BtnSorteaza_Click;
            this.Controls.Add(btnSorteaza);

            Label lblCautare = new Label()
            {
                Text = "Cauta activitate:",
                Location = new Point(20, 260),
                AutoSize = true
            };
            this.Controls.Add(lblCautare);

            txtCautare = new TextBox()
            {
                Location = new Point(120, 260),
                Width = 200
            };
            this.Controls.Add(txtCautare);

            btnCauta = new Button()
            {
                Text = "Cauta",
                Location = new Point(350, 260),
                Size = new Size(60, 25)
            };
            btnCauta.Click += BtnCauta_Click;
            this.Controls.Add(btnCauta);

            btnReset = new Button()
            {
                Text = "Reset",
                Location = new Point(420, 260),
                Size = new Size(60, 25)
            };
            btnReset.Click += BtnReset_Click;
            this.Controls.Add(btnReset);

            btnEditeaza = new Button()
            {
                Text = "Editeaza",
                Location = new Point(350, 290),
                Size = new Size(60, 25)
            };
            btnEditeaza.Click += BtnEditeaza_Click;
            this.Controls.Add(btnEditeaza);

            btnSterge = new Button()
            {
                Text = "Sterge",
                Location = new Point(420, 290),
                Size = new Size(60, 25)
            };
            btnSterge.Click += BtnSterge_Click;
            this.Controls.Add(btnSterge);

        }

        private void BtnAdauga_Click(object sender, EventArgs e)
        {
            FormAdaugaActivitate formAdauga = new FormAdaugaActivitate(agenda, listboxActivitati);
            formAdauga.ShowDialog();
        }

        private void BtnCauta_Click(object sender, EventArgs e)
        {
            string textCautare = txtCautare.Text.Trim();
            listboxActivitati.Items.Clear();

            var rezultate = agenda.Activitati
    .Where(a => a.Titlu.IndexOf(textCautare, StringComparison.OrdinalIgnoreCase) >= 0
             || a.Descriere.IndexOf(textCautare, StringComparison.OrdinalIgnoreCase) >= 0)
    .ToList();

            if (rezultate.Count == 0)
            {
                listboxActivitati.Items.Add("Nicio activitate găsită.");
            }
            else
            {
                foreach (var activitate in rezultate)
                {
                    listboxActivitati.Items.Add($"{activitate.Titlu} : {activitate.Descriere} - Data: {activitate.Data:dd/MM/yyyy} Ora: {activitate.Ora}" +
                        $" ({activitate.TipCategorie} + {activitate.Optiuni}) ✓{activitate.TipReminder}");
                }
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            txtCautare.Clear();
            listboxActivitati.Items.Clear();

            foreach (var activitate in agenda.Activitati)
            {
                listboxActivitati.Items.Add($"{activitate.Titlu} : {activitate.Descriere} - Data: {activitate.Data:dd/MM/yyyy} Ora: {activitate.Ora}" +
                    $" ({activitate.TipCategorie} + {activitate.Optiuni}) ✓{activitate.TipReminder}");
            }
        }

        private void BtnEditeaza_Click(object sender, EventArgs e)
        {
            if (listboxActivitati.SelectedIndex >= 0)
            {
                Activitate activitateSelectata = agenda.Activitati[listboxActivitati.SelectedIndex];
                FormAdaugaActivitate formEditare = new FormAdaugaActivitate(agenda, listboxActivitati, activitateSelectata, listboxActivitati.SelectedIndex);
                formEditare.ShowDialog();
            }
            else
            {
                MessageBox.Show("Selectează o activitate pentru a o edita.");
            }
        }

        private void BtnSorteaza_Click(object sender, EventArgs e)
        {
            listboxActivitati.Items.Clear();

            var activitatiSortate = agenda.Activitati
                .OrderBy(a => a.Data)
                .ThenBy(a => a.Ora)
                .ToList();

            agenda.Activitati = activitatiSortate;

            foreach (var activitate in activitatiSortate)
            {
                listboxActivitati.Items.Add($"{activitate.Titlu} : {activitate.Descriere} - Data: {activitate.Data:dd/MM/yyyy} Ora: {activitate.Ora}" +
                    $" ({activitate.TipCategorie} + {activitate.Optiuni}) ✓{activitate.TipReminder}");
            }
            agenda.ScrieFisier();

        }

        private void BtnSterge_Click(object sender, EventArgs e)
        {
            if (listboxActivitati.SelectedIndex >= 0)
            {
                DialogResult result = MessageBox.Show("Ești sigur că vrei să ștergi activitatea?", "Confirmare ștergere", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    agenda.Activitati.RemoveAt(listboxActivitati.SelectedIndex);
                    listboxActivitati.Items.RemoveAt(listboxActivitati.SelectedIndex);

                    agenda.ScrieFisier();
                }
            }
            else
            {
                MessageBox.Show("Selectează o activitate pentru a o șterge.");
            }
        }



        private void IncarcaActivitatiFisier()
        {
            string caleFisier = @"C:\Users\danie\OneDrive\Desktop\UNIVER 24\AgendaActivitati\Activitati.txt";
            if (File.Exists(caleFisier))
            {
                string[] linii = File.ReadAllLines(caleFisier);

                foreach (string linie in linii)
                {
                    string[] parti = linie.Split('|');
                    if (parti.Length >= 7)
                    {
                        string titlu = parti[0];
                        string descriere = parti[1];

                        if (!DateTime.TryParse(parti[2], out DateTime data))
                            continue;

                        string ora = parti[3];

                        if (!int.TryParse(parti[4], out int categorieIndex) || !Enum.IsDefined(typeof(Categorie), categorieIndex))
                            continue;
                        Categorie categorie = (Categorie)categorieIndex;

                        if (!int.TryParse(parti[5], out int optiuniIndex))
                            continue;
                        OptiuniExtra optiuni = (OptiuniExtra)optiuniIndex;

                        if (!int.TryParse(parti[6], out int remindersIndex))
                            continue;
                        Reminder reminder = (Reminder)remindersIndex;

                        Activitate activitate = new Activitate(titlu, descriere, data, ora, categorie, optiuni, reminder);
                        agenda.AdaugaActivitate(activitate);
                        listboxActivitati.Items.Add($"{activitate.Titlu} : {activitate.Descriere} - Data: {activitate.Data:dd/MM/yyyy} Ora: {activitate.Ora}" +
                            $" ({activitate.TipCategorie} + {activitate.Optiuni}) ✓{activitate.TipReminder}");
                    }
                }
            }
        }

        private void VerificaReminders()
        {
            var activitatiAzi = agenda.Activitati
                .Where(a => a.Data.Date == DateTime.Today)
                .ToList();

            foreach(var activitate in activitatiAzi)
            {
                MessageBox.Show($"Reminder: {activitate.Titlu} {activitate.Descriere} AZI la ora {activitate.Ora}", "REMINDER ACTIVITATE", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }

    public class FormAdaugaActivitate : Form
    {
        private const int MaxTitluLength = 50;
        private const int MaxDescriereLength = 50;

        private TextBox txtTitlu, txtDescriere, txtOra;
        private Label lbTitlu, lbDescriere, lbEroareTitlu, lbEroareDescriere, lbData, lbOra, lbCategorie, lbExtra, lbReminder;
        private Button btnSalveaza;
        private DateTimePicker dtpData;
        private ComboBox cmbCategorie;


        private CheckBox chkImportant, chkOnline, chkNiciuna;
        private RadioButton rdbMemento, rdbEmail;

        private Activitate activitateDeEditat;
        private int indexDeEditat = -1;

        private Agenda agenda;
        private ListBox listBoxActivitati;

        public FormAdaugaActivitate(Agenda agenda, ListBox listBox, Activitate activitate = null, int index = -1)
        {
            this.agenda = agenda;
            this.listBoxActivitati = listBox;
            this.activitateDeEditat = activitate;
            this.indexDeEditat = index;
            IncarcaFormular();

            if (activitateDeEditat != null)
            {
                txtTitlu.Text = activitateDeEditat.Titlu;
                txtDescriere.Text = activitateDeEditat.Descriere;
                dtpData.Value = activitateDeEditat.Data;
                txtOra.Text = activitateDeEditat.Ora;
                cmbCategorie.SelectedItem = activitateDeEditat.TipCategorie;
                if (activitateDeEditat.Optiuni.HasFlag(OptiuniExtra.Important)) chkImportant.Checked = true;
                if (activitateDeEditat.Optiuni.HasFlag(OptiuniExtra.Online)) chkOnline.Checked = true;
                if (activitateDeEditat.Optiuni == OptiuniExtra.Niciuna) chkNiciuna.Checked = true;
                if (activitateDeEditat.TipReminder == Reminder.Memento) rdbMemento.Checked = true;
                if (activitateDeEditat.TipReminder == Reminder.Email) rdbEmail.Checked = true;
            }
        }

        private void IncarcaFormular()
        {
            this.Text = "Adauga Activitate";
            this.Size = new Size(500, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FloralWhite;

            lbTitlu = new Label() { Text = "Titlu:", Location = new Point(20, 20) };
            txtTitlu = new TextBox() { Location = new Point(120, 20), Width = 250 };
            lbEroareTitlu = new Label() { ForeColor = Color.Red, Location = new Point(120, 45), AutoSize = true };

            lbDescriere = new Label() { Text = "Descriere:", Location = new Point(20, 60) };
            txtDescriere = new TextBox() { Location = new Point(120, 60), Width = 250 };
            lbEroareDescriere = new Label() { ForeColor = Color.Red, Location = new Point(120, 105), AutoSize = true };

            lbData = new Label() { Text = "Data:", Location = new Point(20, 100) };
            dtpData = new DateTimePicker() { Location = new Point(120, 100), Format = DateTimePickerFormat.Short };

            Label lbOra = new Label() { Text = "Ora:", Location = new Point(20, 140) };
            txtOra = new TextBox() { Location = new Point(120, 140), Width = 250, Text = "00:00" };

            lbCategorie = new Label() { Text = "Categorie:", Location = new Point(20, 180) };
            cmbCategorie = new ComboBox()
            {
                Location = new Point(120, 180),
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList,
                DataSource = Enum.GetValues(typeof(Categorie))
            };

            Label lbExtra = new Label() { Text = "Optiuni Extra:", Location = new Point(20, 220) };
            chkImportant = new CheckBox() { Text = "Important", Location = new Point(120, 220), AutoSize = true };
            chkOnline = new CheckBox() { Text = "Online", Location = new Point(220, 220), AutoSize = true };
            chkNiciuna = new CheckBox() { Text = "Niciuna", Location = new Point(320, 220), AutoSize = true };


            Label lbReminder = new Label()
            {
                Text = "Reminders:",
                Location = new Point(20, 260),
                AutoSize = true
            };
            this.Controls.Add(lbReminder);

            rdbMemento = new RadioButton()
            {
                Text = "Memento",
                Location = new Point(120, 260),
                AutoSize = true
            };
            this.Controls.Add(rdbMemento);

            rdbEmail = new RadioButton()
            {
                Text = "Trimite email",
                Location = new Point(120, 300),
                AutoSize = true
            };
            this.Controls.Add(rdbEmail);

            btnSalveaza = new Button() { Text = "SALVEAZA", Location = new Point(150, 350), Width = 120 };
            btnSalveaza.Click += BtnSalveaza_Click;

            this.Controls.Add(lbTitlu);
            this.Controls.Add(txtTitlu);
            this.Controls.Add(lbEroareTitlu);
            this.Controls.Add(lbDescriere);
            this.Controls.Add(txtDescriere);
            this.Controls.Add(lbEroareDescriere);
            this.Controls.Add(lbData);
            this.Controls.Add(dtpData);
            this.Controls.Add(lbOra);
            this.Controls.Add(txtOra);
            this.Controls.Add(lbCategorie);
            this.Controls.Add(cmbCategorie);
            this.Controls.Add(lbExtra);
            this.Controls.Add(chkImportant);
            this.Controls.Add(chkOnline);
            this.Controls.Add(chkNiciuna);
            this.Controls.Add(lbReminder);
            this.Controls.Add(rdbMemento);
            this.Controls.Add(rdbEmail);
            this.Controls.Add(btnSalveaza);

        }

        private void BtnSalveaza_Click(object sender, EventArgs e)
        {
            lbEroareTitlu.Text = "";
            lbEroareDescriere.Text = "";

            bool valid = true;

            if (string.IsNullOrWhiteSpace(txtTitlu.Text) || txtTitlu.Text.Length > MaxTitluLength)
            {
                lbEroareTitlu.Text = "Titlul e obligatoriu (max 50 caractere)";
                valid = false;
            }

            if (string.IsNullOrWhiteSpace(txtDescriere.Text) || txtDescriere.Text.Length > MaxDescriereLength)
            {
                lbEroareDescriere.Text = "Descrierea e obligatorie (max 50 caractere)";
                valid = false;
            }

            if (dtpData.Value.Date < DateTime.Today)
            {
                MessageBox.Show("Data nu poate fi in trecut!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!valid) return;

            OptiuniExtra optiuniSelectate = OptiuniExtra.Niciuna;
            if (chkNiciuna.Checked)
            {
                optiuniSelectate = OptiuniExtra.Niciuna;
            }
            else
            {
                if (chkImportant.Checked)
                    optiuniSelectate |= OptiuniExtra.Important;
                if (chkOnline.Checked)
                    optiuniSelectate |= OptiuniExtra.Online;
            }

            Reminder reminder;
            if (rdbMemento.Checked)
            {
                reminder = Reminder.Memento;
            }
            else if (rdbEmail.Checked)
            {
                reminder = Reminder.Email;
            }
            else
            {
                MessageBox.Show("Selecteaza un tip de reminder (Memento sau Email).", "Reminder lipsa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Activitate activitate = new Activitate(
                txtTitlu.Text,
                txtDescriere.Text,
                dtpData.Value,
                txtOra.Text,
                (Categorie)cmbCategorie.SelectedItem,
                optiuniSelectate,
                reminder
            );

            if (indexDeEditat >= 0)
            {
                agenda.Activitati[indexDeEditat] = activitate;
                listBoxActivitati.Items[indexDeEditat] = $"{activitate.Titlu} : {activitate.Descriere} - Data: {activitate.Data:dd/MM/yyyy} Ora: {activitate.Ora} " +
                    $"({activitate.TipCategorie} + {activitate.Optiuni}) ✓{activitate.TipReminder}";
            }
            else
            {
                agenda.AdaugaActivitate(activitate);
                listBoxActivitati.Items.Add($"{activitate.Titlu} : {activitate.Descriere} - Data: {activitate.Data:dd/MM/yyyy} Ora: {activitate.Ora} " +
                    $"({activitate.TipCategorie} + {activitate.Optiuni}) ✓{activitate.TipReminder}");
            }
            agenda.ScrieFisier();

            this.Close();
        }
    }
}
