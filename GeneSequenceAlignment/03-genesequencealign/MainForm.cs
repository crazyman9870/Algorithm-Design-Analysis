using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;


namespace GeneticsLab
{
    public partial class MainForm : Form
    {
        ResultTable m_resultTable;
        GeneSequence[] m_sequences;
        PairWiseAlign processor;
        const int NUMBER_OF_SEQUENCES = 10;
        const string GENOME_FILE = "genomes.txt";

        public MainForm()
        {
            InitializeComponent();

            statusMessage.Text = "Loading Database...";

            // load database here

            try
            {
                m_sequences = loadFile("../../" + GENOME_FILE);
            }
            catch (FileNotFoundException e)
            {
                try // Failed, try one level down...
                {
                    m_sequences = loadFile("../" + GENOME_FILE);
                }
                catch (FileNotFoundException e2)
                {
                    // Failed, try same folder
                    m_sequences = loadFile(GENOME_FILE);
                }
            }

            m_resultTable = new ResultTable(this.dataGridViewResults, NUMBER_OF_SEQUENCES);

            statusMessage.Text = "Loaded Database.";

        }

        private GeneSequence[] loadFile(string fileName)
        {
            StreamReader reader = new StreamReader(fileName);
            string input = "";

            try
            {
                input = reader.ReadToEnd();
            }
            catch
            {
                Console.WriteLine("Error Parsing File...");
                return null;
            }
            finally
            {
                reader.Close();
            }

            GeneSequence[] temp = new GeneSequence[NUMBER_OF_SEQUENCES];
            string[] inputLines = input.Split('\r');

            for (int i = 0; i < NUMBER_OF_SEQUENCES; i++)
            {
                string[] line = inputLines[i].Replace("\n","").Split('#');
                temp[i] = new GeneSequence(line[0], line[1]);
            }
            return temp;
        }

        private void fillMatrix()
        {
            processor = new PairWiseAlign();
            for (int x = 0; x < NUMBER_OF_SEQUENCES; ++x)
            {
                for (int y = 0; y < NUMBER_OF_SEQUENCES; ++y)
                {
                    m_resultTable.SetCell(x, y, processor.Align(m_sequences[x], m_sequences[y],m_resultTable,x,y));
                }
            }
        }

        private void processButton_Click(object sender, EventArgs e)
        {
            statusMessage.Text = "Processing...";
            Stopwatch timer = new Stopwatch();
            timer.Start();
            fillMatrix();
            timer.Stop();
            statusMessage.Text = "Done.  Time taken: " + timer.Elapsed;

        }

        private void dataGridViewResults_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GeneSequence sequenceA = this.m_sequences[e.ColumnIndex];
            GeneSequence sequenceB = this.m_sequences[e.RowIndex];

            String[] results = processor.extractSequences(sequenceA, sequenceB);

            String outputText = "Output Console:";
            outputText += "\r\nCell (";
            outputText += (e.RowIndex + 1) + ", ";
            outputText += (e.ColumnIndex + 1) + ")";
            outputText += "\r\nSequence A: " + processor.stringLimit(results[0], 100);
            outputText += "\r\nSequence B: " + processor.stringLimit(results[1], 100);

            outputConsole.Text = outputText;
            
        }
    }
}