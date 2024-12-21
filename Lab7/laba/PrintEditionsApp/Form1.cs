using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PrintEditionsApp.Models;

namespace PrintEditionsApp
{
    public partial class Form1 : Form
    {
        private MenuStrip menuStrip;
        private ToolStripMenuItem fileMenuItem;
        private ToolStripMenuItem saveMenuItem;

        private TabControl mainTabControl;
        private TabPage tabJournals;
        private TabPage tabTextbooks;
        // Можно добавить дополнительные вкладки для других типов

        // Для журналов
        private SplitContainer splitContainerJournals;
        private DataGridView dgvJournals;
        private GroupBox grpJournalDetails;
        private TextBox txtJournalTitle;
        private TextBox txtJournalPublisher;
        private NumericUpDown numJournalYear;
        private NumericUpDown numJournalPages;
        private ComboBox cmbJournalPeriodicity;
        private TextBox txtJournalTopic;
        private NumericUpDown numJournalIssue;
        private TextBox txtJournalLanguage;
        private ToolStrip journalToolStrip;
        private ToolStripButton btnAddJournal;
        private ToolStripButton btnDeleteJournal;
        private ToolStripButton btnSaveJournals;

        // Для учебников
        private SplitContainer splitContainerTextbooks;
        private DataGridView dgvTextbooks;
        private GroupBox grpTextbookDetails;
        private TextBox txtTTitle;
        private TextBox txtTPublisher;
        private NumericUpDown numTYear;
        private NumericUpDown numTPages;
        private TextBox txtTAuthor;
        private TextBox txtTGenre;
        private TextBox txtTISBN;
        private NumericUpDown numTPrintRun;
        private TextBox txtTSubject;
        private NumericUpDown numTCourseNumber;
        private TextBox txtTInstitution;
        private TextBox txtTLanguage;
        private ToolStrip textbookToolStrip;
        private ToolStripButton btnAddTextbook;
        private ToolStripButton btnDeleteTextbook;
        private ToolStripButton btnSaveTextbooks;

        private List<Journal> journals = new List<Journal>();
        private List<Textbook> textbooks = new List<Textbook>();

        private BindingSource journalBindingSource = new BindingSource();
        private BindingSource textbookBindingSource = new BindingSource();
        private Dictionary<string, List<PrintEdition>> dataCollections;
        private Dictionary<string, BindingSource> bindingSources;

        public Form1()
        {
            InitializeComponent();
        }
    
        private void InitializeComponent()
        {
            this.Text = "Print Editions App";
            this.Width = 1200;
            this.Height = 700;

            // Меню
            menuStrip = new MenuStrip();
            fileMenuItem = new ToolStripMenuItem("File");
            saveMenuItem = new ToolStripMenuItem("Save All");
            saveMenuItem.Click += SaveAll_Click;
            fileMenuItem.DropDownItems.Add(saveMenuItem);
            menuStrip.Items.Add(fileMenuItem);
            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);

            // Основной TabControl
            mainTabControl = new TabControl();
            mainTabControl.Dock = DockStyle.Fill;

            tabJournals = new TabPage("Journals");
            tabTextbooks = new TabPage("Textbooks");

            mainTabControl.TabPages.Add(tabJournals);
            mainTabControl.TabPages.Add(tabTextbooks);

            this.Controls.Add(mainTabControl);

            // -------------------- Journals --------------------
            splitContainerJournals = new SplitContainer();
            splitContainerJournals.Dock = DockStyle.Fill;
            splitContainerJournals.Orientation = Orientation.Vertical;
            tabJournals.Controls.Add(splitContainerJournals);

            // Левый Panel для Journals (список)
            dgvJournals = new DataGridView();
            dgvJournals.Dock = DockStyle.Fill;
            dgvJournals.AutoGenerateColumns = true; 
            dgvJournals.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvJournals.MultiSelect = false;
            dgvJournals.ReadOnly = true;
            dgvJournals.DataSource = journalBindingSource;
            dgvJournals.SelectionChanged += DgvJournals_SelectionChanged;
            splitContainerJournals.Panel1.Controls.Add(dgvJournals);

            journalToolStrip = new ToolStrip();
            btnAddJournal = new ToolStripButton("Add");
            btnAddJournal.Click += BtnAddJournal_Click;
            btnDeleteJournal = new ToolStripButton("Delete");
            btnDeleteJournal.Click += BtnDeleteJournal_Click;
            btnSaveJournals = new ToolStripButton("Save");
            btnSaveJournals.Click += BtnSaveJournals_Click;

            journalToolStrip.Items.Add(btnAddJournal);
            journalToolStrip.Items.Add(btnDeleteJournal);
            journalToolStrip.Items.Add(new ToolStripSeparator());
            journalToolStrip.Items.Add(btnSaveJournals);

            splitContainerJournals.Panel1.Controls.Add(journalToolStrip);
            journalToolStrip.Dock = DockStyle.Top;

            // Правый Panel для Journals (детали)
            grpJournalDetails = new GroupBox();
            grpJournalDetails.Text = "Journal Details";
            grpJournalDetails.Dock = DockStyle.Fill;
            splitContainerJournals.Panel2.Controls.Add(grpJournalDetails);

            // Расположение полей журнала
            var journalLayout = new TableLayoutPanel();
            journalLayout.Dock = DockStyle.Fill;
            journalLayout.ColumnCount = 2;
            journalLayout.RowCount = 8;
            journalLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            journalLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            for (int i = 0; i < 8; i++)
                journalLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            grpJournalDetails.Controls.Add(journalLayout);

            journalLayout.Controls.Add(new Label() { Text = "Title:", AutoSize = true }, 0, 0);
            txtJournalTitle = new TextBox() { Dock = DockStyle.Fill };
            journalLayout.Controls.Add(txtJournalTitle, 1, 0);

            journalLayout.Controls.Add(new Label() { Text = "Publisher:", AutoSize = true }, 0, 1);
            txtJournalPublisher = new TextBox() { Dock = DockStyle.Fill };
            journalLayout.Controls.Add(txtJournalPublisher, 1, 1);

            journalLayout.Controls.Add(new Label() { Text = "Year:", AutoSize = true }, 0, 2);
            numJournalYear = new NumericUpDown() { Minimum = 1900, Maximum = 2100, Dock = DockStyle.Fill };
            journalLayout.Controls.Add(numJournalYear, 1, 2);

            journalLayout.Controls.Add(new Label() { Text = "Pages:", AutoSize = true }, 0, 3);
            numJournalPages = new NumericUpDown() { Minimum = 1, Maximum = 10000, Dock = DockStyle.Fill };
            journalLayout.Controls.Add(numJournalPages, 1, 3);

            journalLayout.Controls.Add(new Label() { Text = "Periodicity:", AutoSize = true }, 0, 4);
            cmbJournalPeriodicity = new ComboBox() { Dock = DockStyle.Fill };
            cmbJournalPeriodicity.Items.AddRange(new object[] { "Monthly", "Weekly", "Yearly" });
            journalLayout.Controls.Add(cmbJournalPeriodicity, 1, 4);

            journalLayout.Controls.Add(new Label() { Text = "Topic:", AutoSize = true }, 0, 5);
            txtJournalTopic = new TextBox() { Dock = DockStyle.Fill };
            journalLayout.Controls.Add(txtJournalTopic, 1, 5);

            journalLayout.Controls.Add(new Label() { Text = "Issue #:", AutoSize = true }, 0, 6);
            numJournalIssue = new NumericUpDown() { Minimum = 1, Maximum = 1000, Dock = DockStyle.Fill };
            journalLayout.Controls.Add(numJournalIssue, 1, 6);

            journalLayout.Controls.Add(new Label() { Text = "Language:", AutoSize = true }, 0, 7);
            txtJournalLanguage = new TextBox() { Dock = DockStyle.Fill };
            journalLayout.Controls.Add(txtJournalLanguage, 1, 7);

            // -------------------- Textbooks --------------------
            splitContainerTextbooks = new SplitContainer();
            splitContainerTextbooks.Dock = DockStyle.Fill;
            tabTextbooks.Controls.Add(splitContainerTextbooks);

            // Левый Panel для Textbooks (список)
            dgvTextbooks = new DataGridView();
            dgvTextbooks.Dock = DockStyle.Fill;
            dgvTextbooks.AutoGenerateColumns = true;
            dgvTextbooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTextbooks.MultiSelect = false;
            dgvTextbooks.ReadOnly = true;
            dgvTextbooks.DataSource = textbookBindingSource;
            dgvTextbooks.SelectionChanged += DgvTextbooks_SelectionChanged;

            textbookToolStrip = new ToolStrip();
            btnAddTextbook = new ToolStripButton("Add");
            btnAddTextbook.Click += BtnAddTextbook_Click;
            btnDeleteTextbook = new ToolStripButton("Delete");
            btnDeleteTextbook.Click += BtnDeleteTextbook_Click;
            btnSaveTextbooks = new ToolStripButton("Save");
            btnSaveTextbooks.Click += BtnSaveTextbooks_Click;

            textbookToolStrip.Items.Add(btnAddTextbook);
            textbookToolStrip.Items.Add(btnDeleteTextbook);
            textbookToolStrip.Items.Add(new ToolStripSeparator());
            textbookToolStrip.Items.Add(btnSaveTextbooks);

            splitContainerTextbooks.Panel1.Controls.Add(dgvTextbooks);
            splitContainerTextbooks.Panel1.Controls.Add(textbookToolStrip);
            textbookToolStrip.Dock = DockStyle.Top;

            // Правый Panel для Textbooks (детали)
            grpTextbookDetails = new GroupBox();
            grpTextbookDetails.Text = "Textbook Details";
            grpTextbookDetails.Dock = DockStyle.Fill;
            splitContainerTextbooks.Panel2.Controls.Add(grpTextbookDetails);

            var textbookLayout = new TableLayoutPanel();
            textbookLayout.Dock = DockStyle.Fill;
            textbookLayout.ColumnCount = 2;
            textbookLayout.RowCount = 12;
            for (int i = 0; i < 12; i++)
                textbookLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            textbookLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            textbookLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            grpTextbookDetails.Controls.Add(textbookLayout);

            textbookLayout.Controls.Add(new Label() { Text = "Title:", AutoSize = true }, 0, 0);
            txtTTitle = new TextBox() { Dock = DockStyle.Fill };
            textbookLayout.Controls.Add(txtTTitle, 1, 0);

            textbookLayout.Controls.Add(new Label() { Text = "Publisher:", AutoSize = true }, 0, 1);
            txtTPublisher = new TextBox() { Dock = DockStyle.Fill };
            textbookLayout.Controls.Add(txtTPublisher, 1, 1);

            textbookLayout.Controls.Add(new Label() { Text = "Year:", AutoSize = true }, 0, 2);
            numTYear = new NumericUpDown() { Minimum = 1900, Maximum = 2100, Dock = DockStyle.Fill };
            textbookLayout.Controls.Add(numTYear, 1, 2);

            textbookLayout.Controls.Add(new Label() { Text = "Pages:", AutoSize = true }, 0, 3);
            numTPages = new NumericUpDown() { Minimum = 1, Maximum = 10000, Dock = DockStyle.Fill };
            textbookLayout.Controls.Add(numTPages, 1, 3);

            textbookLayout.Controls.Add(new Label() { Text = "Author:", AutoSize = true }, 0, 4);
            txtTAuthor = new TextBox() { Dock = DockStyle.Fill };
            textbookLayout.Controls.Add(txtTAuthor, 1, 4);

            textbookLayout.Controls.Add(new Label() { Text = "Genre:", AutoSize = true }, 0, 5);
            txtTGenre = new TextBox() { Dock = DockStyle.Fill };
            textbookLayout.Controls.Add(txtTGenre, 1, 5);

            textbookLayout.Controls.Add(new Label() { Text = "ISBN:", AutoSize = true }, 0, 6);
            txtTISBN = new TextBox() { Dock = DockStyle.Fill };
            textbookLayout.Controls.Add(txtTISBN, 1, 6);

            textbookLayout.Controls.Add(new Label() { Text = "PrintRun:", AutoSize = true }, 0, 7);
            numTPrintRun = new NumericUpDown() { Minimum = 1, Maximum = 1000000, Dock = DockStyle.Fill };
            textbookLayout.Controls.Add(numTPrintRun, 1, 7);

            textbookLayout.Controls.Add(new Label() { Text = "Subject:", AutoSize = true }, 0, 8);
            txtTSubject = new TextBox() { Dock = DockStyle.Fill };
            textbookLayout.Controls.Add(txtTSubject, 1, 8);

            textbookLayout.Controls.Add(new Label() { Text = "Course #:", AutoSize = true }, 0, 9);
            numTCourseNumber = new NumericUpDown() { Minimum = 1, Maximum = 10, Dock = DockStyle.Fill };
            textbookLayout.Controls.Add(numTCourseNumber, 1, 9);

            textbookLayout.Controls.Add(new Label() { Text = "Institution:", AutoSize = true }, 0, 10);
            txtTInstitution = new TextBox() { Dock = DockStyle.Fill };
            textbookLayout.Controls.Add(txtTInstitution, 1, 10);

            textbookLayout.Controls.Add(new Label() { Text = "Language:", AutoSize = true }, 0, 11);
            txtTLanguage = new TextBox() { Dock = DockStyle.Fill };
            textbookLayout.Controls.Add(txtTLanguage, 1, 11);

            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Пример добавления Journal
            journals.Add(new Journal("Some Journal Info")
            {
                Title = "Nature",
                Publisher = "Springer",
                Year = 2021,
                Pages = 100,
                Periodicity = "Monthly",
                Topic = "Science",
                IssueNumber = 10,
                Language = "English"
            });

            // Пример добавления Textbook
            textbooks.Add(new Textbook("Textbook Info")
            {
                Title = "C# Programming",
                Publisher = "MS Press",
                Year = 2020,
                Pages = 500,
                Author = "John Doe",
                Genre = "Educational",
                ISBN = "123-456",
                PrintRun = 1000,
                Subject = "Programming",
                CourseNumber = 1,
                Institution = "Some University",
                Language = "English"
            });

            journalBindingSource.DataSource = journals.ToList();
            textbookBindingSource.DataSource = textbooks.ToList();
        }

        private void DgvJournals_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvJournals.CurrentRow == null) return;
            var j = dgvJournals.CurrentRow.DataBoundItem as Journal;
            if (j == null) return;
            LoadJournalToFields(j);
        }

        private void LoadJournalToFields(Journal j)
        {
            txtJournalTitle.Text = j.Title;
            txtJournalPublisher.Text = j.Publisher;
            if (j.Year >= numJournalYear.Minimum && j.Year <= numJournalYear.Maximum)
                {
                    numJournalYear.Value = j.Year;
                }
                else
                {
                    numJournalYear.Value = numJournalYear.Minimum; // or some default value within the range
                }

            if (j.Pages >= numJournalPages.Minimum && j.Pages <= numJournalPages.Maximum)
                {
                    numJournalPages.Value = j.Pages;
                }
                else
                {
                    numJournalPages.Value = numJournalPages.Minimum; // or some default value within the range
                }

            cmbJournalPeriodicity.SelectedItem = j.Periodicity;
            txtJournalTopic.Text = j.Topic;
            if (j.IssueNumber >= numJournalIssue.Minimum && j.IssueNumber <= numJournalIssue.Maximum)
                {
                    numJournalIssue.Value = j.IssueNumber;
                }
                else
                {
                    numJournalIssue.Value = numJournalIssue.Minimum; // Or use a fallback value within range
                }

            txtJournalLanguage.Text = j.Language;
        }

        private void SaveJournalFromFields(Journal j)
        {
            j.Title = txtJournalTitle.Text;
            j.Publisher = txtJournalPublisher.Text;
            j.Year = (int)numJournalYear.Value;
            j.Pages = (int)numJournalPages.Value;
            j.Periodicity = cmbJournalPeriodicity.SelectedItem?.ToString() ?? "Monthly";
            j.Topic = txtJournalTopic.Text;
            j.IssueNumber = (int)numJournalIssue.Value;
            j.Language = txtJournalLanguage.Text;
        }

        private void BtnAddJournal_Click(object sender, EventArgs e)
        {
            Journal j = new Journal("New Journal Info");
            journals.Add(j);
            journalBindingSource.DataSource = journals.ToList();
        }

        private void BtnDeleteJournal_Click(object sender, EventArgs e)
        {
            if (dgvJournals.CurrentRow == null) return;
            var j = dgvJournals.CurrentRow.DataBoundItem as Journal;
            if (j != null)
            {
                journals.Remove(j);
                journalBindingSource.DataSource = journals.ToList();
            }
        }

        private void BtnSaveJournals_Click(object sender, EventArgs e)
        {
            // Сохраняем измененный журнал
            if (dgvJournals.CurrentRow != null)
            {
                var j = dgvJournals.CurrentRow.DataBoundItem as Journal;
                if (j != null)
                {
                    SaveJournalFromFields(j);
                }
            }

            // Сохранение в файл
            string filePath = "journals.txt";
            File.WriteAllText(filePath, "");
            foreach (var j in journals)
            {
                j.SaveToFile(filePath);
            }

            MessageBox.Show("Journals saved to " + filePath);
        }

        private void DgvTextbooks_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTextbooks.CurrentRow == null) return;
            var t = dgvTextbooks.CurrentRow.DataBoundItem as Textbook;
            if (t == null) return;
            LoadTextbookToFields(t);
        }

        private void LoadTextbookToFields(Textbook t)
        {
            txtTTitle.Text = t.Title;
            txtTPublisher.Text = t.Publisher;
            numTYear.Value = t.Year;
            numTPages.Value = t.Pages;
            txtTAuthor.Text = t.Author;
            txtTGenre.Text = t.Genre;
            txtTISBN.Text = t.ISBN;
            numTPrintRun.Value = t.PrintRun;
            txtTSubject.Text = t.Subject;
            numTCourseNumber.Value = t.CourseNumber;
            txtTInstitution.Text = t.Institution;
            txtTLanguage.Text = t.Language;
        }

        private void SaveTextbookFromFields(Textbook t)
        {
            t.Title = txtTTitle.Text;
            t.Publisher = txtTPublisher.Text;
            t.Year = (int)numTYear.Value;
            t.Pages = (int)numTPages.Value;
            t.Author = txtTAuthor.Text;
            t.Genre = txtTGenre.Text;
            t.ISBN = txtTISBN.Text;
            t.PrintRun = (int)numTPrintRun.Value;
            t.Subject = txtTSubject.Text;
            t.CourseNumber = (int)numTCourseNumber.Value;
            t.Institution = txtTInstitution.Text;
            t.Language = txtTLanguage.Text;
        }

        private void BtnAddTextbook_Click(object sender, EventArgs e)
        {
            Textbook tb = new Textbook("New Textbook Info");
            textbooks.Add(tb);
            textbookBindingSource.DataSource = textbooks.ToList();
        }

        private void BtnDeleteTextbook_Click(object sender, EventArgs e)
        {
            if (dgvTextbooks.CurrentRow == null) return;
            var t = dgvTextbooks.CurrentRow.DataBoundItem as Textbook;
            if (t != null)
            {
                textbooks.Remove(t);
                textbookBindingSource.DataSource = textbooks.ToList();
            }
        }

        private void BtnSaveTextbooks_Click(object sender, EventArgs e)
        {
            if (dgvTextbooks.CurrentRow != null)
            {
                var t = dgvTextbooks.CurrentRow.DataBoundItem as Textbook;
                if (t != null)
                    SaveTextbookFromFields(t);
            }

            // Сохранение в файл (аналогично журнала)
            string filePath = "textbooks.txt";
            File.WriteAllText(filePath, "");
            foreach (var t in textbooks)
            {
                t.SaveToFile(filePath);
            }

            MessageBox.Show("Textbooks saved to " + filePath);
        }

        private void SaveAll_Click(object sender, EventArgs e)
        {
            // Сохранение текущего выбранного журнала
            if (dgvJournals.CurrentRow != null)
            {
                var j = dgvJournals.CurrentRow.DataBoundItem as Journal;
                if (j != null) SaveJournalFromFields(j);
            }

            // Сохранение всех журналов
            string journalsFile = "journals.txt";
            File.WriteAllText(journalsFile, "");
            foreach (var j in journals)
            {
                j.SaveToFile(journalsFile);
            }

            // Сохранение текущего выбранного учебника
            if (dgvTextbooks.CurrentRow != null)
            {
                var t = dgvTextbooks.CurrentRow.DataBoundItem as Textbook;
                if (t != null) SaveTextbookFromFields(t);
            }

            // Сохранение всех учебников
            string textbooksFile = "textbooks.txt";
            File.WriteAllText(textbooksFile, "");
            foreach (var t in textbooks)
            {
                t.SaveToFile(textbooksFile);
            }

            MessageBox.Show("All data saved!");
        }
    }
}
