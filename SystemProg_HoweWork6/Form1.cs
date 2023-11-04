using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemProg_HoweWork6;
using System.Collections;
using System.IO;
using System.Reflection;


namespace SystemProg_HoweWork6
{
    public partial class Form1 : Form
    {
        Random random = new Random();
        public Form1()
        {
           

            InitializeComponent();
        }

        private void ButtonStartPrgBar_Click(object sender, EventArgs e)
        {

            NewProgressBar tempBar;
            tableLayoutPanel1.Controls.Clear();
            for (int i = 0;i < numericPrgBar.Value; i++)
            {
                tempBar = new NewProgressBar();
                tempBar.RandomColor();
                tempBar.Maximum = random.Next(10,20);
                tempBar.Width = 500;
                tableLayoutPanel1.Controls.Add(tempBar, 0, i);
                tempBar.Dance();
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            panelRaceResults.Visible = false;
            panelRace.Visible = true;

            Task<int> time1 = Race(barHorse1);
            Task<int> time2 = Race(barHorse2);
            Task<int> time3 = Race(barHorse3);
            Task<int> time4 = Race(barHorse4);

            await RaceResults(time1,time2,time3,time4);

            panelRaceResults.Visible = true ;
            panelRace.Visible = false;
        }

        private async Task<int> Race(ProgressBar horse)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            horse.Value = 0;
            int horseBreed = random.Next(0, 50);
            while (horse.Value < horse.Maximum)
            {
                horse.Value++;
                await Task.Delay(horseBreed + random.Next(0, 10));
            }
            
            stopwatch.Stop();
            return (int)stopwatch.Elapsed.TotalMilliseconds;
        }

        private async Task RaceResults(Task<int> time1, Task<int> time2, Task<int> time3, Task<int> time4)
        {
            while (!time1.IsCompleted || !time2.IsCompleted || !time3.IsCompleted || !time4.IsCompleted)
                await Task.Delay(10);
            SortedList<int, Label> horses = new SortedList<int, Label>() { {time1.Result, labelHorse1 }, { time2.Result, labelHorse2 }, { time3.Result, labelHorse3 }, { time4.Result, labelHorse4 } };
            horses.OrderByDescending(k => k.Key);
            horses.Values[0].Text = $"First place!  Time = {horses.Keys[0]} milisec.";
            horses.Values[0].BackColor = Color.Gold;
            horses.Values[1].Text = $"Second place Time = {horses.Keys[1]} milisec.";
            horses.Values[1].BackColor = Color.Silver;
            horses.Values[2].Text = $"Third place   Time = {horses.Keys[2]} milisec.";
            horses.Values[2].BackColor = Color.SandyBrown;
            horses.Values[3].Text = $"Last place..  Time = {horses.Keys[3]} milisec.";
            horses.Values[3].BackColor = Color.LightCoral;
        }


        private async void fibonacciButton_Click(object sender, EventArgs e)
        {
            fibonacciTextBox.Text = "";
            if (!int.TryParse(textBox1.Text, out int limit))
            {
                limit = 100;
            }
            await FibonacciGenerator(limit);

        }

        private async Task FibonacciGenerator(int limit)
        {
            int stepBehind = 1;
            int twoStepsBehind = 1;
            fibonacciTextBox.Text += stepBehind.ToString() + " ";
            fibonacciTextBox.Text += twoStepsBehind.ToString() + " ";
            int newNumber = twoStepsBehind + stepBehind;
            while(newNumber <= limit)
            {
                fibonacciTextBox.Text += newNumber.ToString() + " ";
                twoStepsBehind = stepBehind;
                stepBehind = newNumber;
                newNumber = twoStepsBehind + stepBehind;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog1.ShowDialog() == DialogResult.OK) 
            {
                pathTextBox.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void StartSearchButton_Click(object sender, EventArgs e)
        {
            SearchDirectories(pathTextBox.Text, wordToSearch.Text);

        }

        private async Task SearchDirectories(string root, string word)
        {
            string[] folders = Directory.GetDirectories(root, "*", SearchOption.AllDirectories);
            foreach (string folder in folders) 
            {
                await SearchFiles(folder, word);
            }

        }

        private async Task SearchFiles(string dir, string word)
        {
            string name = "";
            StringComparison comparison = StringComparison.OrdinalIgnoreCase;
            string[] files = Directory.GetFiles(dir, "*.txt");
            foreach (string file in files)
            {
                name = Path.GetFileName(file);
                //name = file.Substring(file.LastIndexOf("\\" + 1));
                using (StreamReader sreader = new StreamReader(file))
                {
                    string text = sreader.ReadToEnd();
                    int repetitions = 0;
                    int index = text.IndexOf(word, comparison);
                    while (index != -1)
                    {
                        repetitions++;

                        // Resume the search just after the end of the previous occurrence:
                        index = text.IndexOf(word, index + word.Length, comparison);
                    }
                    if (repetitions != 0)
                    {

                        

                        ListViewItem item = new ListViewItem();
                        item.Text = name; item.SubItems.Add(file); item.SubItems.Add(repetitions.ToString());
                        listView1.Items.Add(item);
                    }
                    


                }
            }

        }

        private void assigmentButtons_Click(object sender, EventArgs e)
        {
            Button thisButton = sender as Button;
            foreach(Control control in Controls)
            {
                if(control as Panel != null)
                {
                    if(control.Tag != thisButton.Tag)
                    {
                        control.Visible = false;
                    }
                    else control.Visible = true;
                }
                if (control as Button != null)
                {
                    if (control.Tag != thisButton.Tag)
                    {
                        control.Enabled = true;
                    }
                    else control.Enabled = false;
                }
            }
        }
    }

    
}
