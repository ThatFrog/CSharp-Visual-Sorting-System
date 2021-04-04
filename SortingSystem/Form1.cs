using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace SortingSystem
{
    public partial class MainForm : Form
    {
        //variables
        Bitmap sortingBitmap = new Bitmap(800, 800);
        int[] numberArray = new int[800];
        Task currentSortTask;
        bool firstRan = true;

        Stopwatch stopWatch = new Stopwatch();

        const int operationsPerRefresh = 100; //speed value
        int operationsRan = 0;

        public MainForm() //initial loading
        {
            InitializeComponent();
            pictureBox.Image = sortingBitmap;

            using (Graphics g = Graphics.FromImage(sortingBitmap))
            {
                SolidBrush backgroundBrush = new SolidBrush(Color.Black);
                g.FillRectangle(backgroundBrush, new Rectangle(0, 0, 800, 800));
            }
            
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            initialDisplay();
            switch (selectionBox.Text)
            {
                case "Bubble Sort":
                    currentSortTask = bubbleSort();
                    break;
                case "Insert Sort":
                    currentSortTask = insertSort();
                    break;
                case "Selection Sort":
                    currentSortTask = selectionSort();
                    break;
                case "Merge Sort":
                    currentSortTask = mergeSort(numberArray, 0, numberArray.Length-1);
                    break;
                case "Quick Sort":
                    //currentSortTask = quickSort();
                    break;
                case "Comb Sort":
                    //currentSortTask = combSort();
                    break;
                case "Radix Sort":
                    //currentSortTask = radixSort();
                    break;
                case "Heap Sort":
                    //currentSortTask = heapSort();
                    break;
                case "Cocktail Shaker Sort":
                    //currentSortTask = cocktailShakerSort();
                    break;
            }
            greenAfterEffect();
            if(false)
            {
                debugListDisplay();
                debugErrorCheck();
            }
            
        }

        private async void initialDisplay()
        {
            Random rnd = new Random();
            const int animationFrames = 200;

            //shrink numberArray
            if (!firstRan)
            {
                for (int i = 0; i < animationFrames; i++)
                {
                    for (int j = 0; j < numberArray.Length; j++)
                    {
                        //current value / frames left to do it in
                        numberArray[i] = numberArray[i] / (animationFrames-i);
                        await drawLine(j);
                    }
                    refreshScreen(true);
                }
            }
            else
                firstRan = true;

            //populate growArray
            int[] growArray = new int[numberArray.Length];
            for (int i = 0; i < growArray.Length; i++)
                growArray[i] = rnd.Next(1, 800);

            //grow numberArray
            for (int i = 0; i < animationFrames; i++)
            {
                for (int j = 0; j < 800; j++)
                {
                    //current value + (difference between maxheight and current) / (frames left to do it in)
                    numberArray[j] = numberArray[j] + ((growArray[j] - numberArray[j]) / (animationFrames - i));
                    await drawLine(j);
                }
                refreshScreen(true);
            }
        }

        private async Task bubbleSort()
        {
            await changeStatusText("BubbleSorting started");
            int timesRan = 0;
            int changespace = 800; //used, cause when a value reaches the end (sorted), it doesn't need to be re-checked)

            stopWatch.Start();

            bool changeMade = true; //used for checking if anything was changed during the sorting.
            while (changeMade == true)
            {
                changeMade = false;
                timesRan++;
                await changeStatusText("Bubblesort(" + timesRan + ")");

                changespace--;

                for (int i = 0; i < changespace; i++)
                {
                    //Console.WriteLine("iChecking: " + i + ", " + (i+1));
                    if (numberArray[i] > numberArray[i + 1])
                    {
                        //Console.WriteLine("swapping ^");
                        await swapNumberArrayPlaces(i, i + 1);
                        changeMade = true;
                    }
                    await refreshScreen(false);
                } 
            }

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
            await changeStatusText("Bubblesort Completed, finished in " + elapsedTime);
            currentSortTask = null;
        }

        private async Task insertSort()
        {
            await changeStatusText("InsertSorting started");
            stopWatch.Start();
            int timesRan = 0;
            int insertNumberPosition;
            bool inOrder;
            for(int i = 0; i < 800; i++)
            {
                timesRan++;
                await changeStatusText("InsertSort(" + timesRan + ")");

                inOrder = false;
                insertNumberPosition = i;
                while (inOrder == false)
                {
                    if(insertNumberPosition > 0)
                    {
                        if (numberArray[insertNumberPosition] < numberArray[insertNumberPosition - 1])
                        {
                            await swapNumberArrayPlaces(insertNumberPosition, insertNumberPosition - 1);
                            insertNumberPosition--;
                        }
                        else
                        {
                            inOrder = true;
                        }
                    }
                    else
                    {
                        inOrder = true;
                    }
                    await refreshScreen(false);
                }
            }

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
            await changeStatusText("Insertsort Completed, finished in " + elapsedTime);
            currentSortTask = null;
        }

        private async Task selectionSort()
        {
            await changeStatusText("SelectionSorting started");
            stopWatch.Start();
            int timesRan = 0;
            for (int i = 0; i < 800; i++)
            {
                timesRan++;
                await changeStatusText("InsertSort(" + timesRan + ")");
                int lowestValue = 1000;
                int lowestLocation = 0;

                for (int j = i; j < 800; j++)
                {
                    if (numberArray[j] < lowestValue)
                    {
                        lowestValue = numberArray[j];
                        lowestLocation = j;
                    }

                    using (Graphics g = Graphics.FromImage(sortingBitmap))
                    {
                        //Graphics change
                        //g.DrawLine(new Pen(Color.GreenYellow), j, 800 - numberArray[j], j, 800); //colour line green
                        g.DrawLine(new Pen(Color.Yellow), lowestLocation, 800 - numberArray[lowestLocation], lowestLocation, 800); //colour line green
                    }

                    await refreshScreen(false);

                    //reload line
                    //await drawLine(j);

                    //reload line
                    await drawLine(lowestLocation);
                }

                await swapNumberArrayPlaces(i, lowestLocation);
                await refreshScreen(false);
            }

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
            await changeStatusText("Selectionsort Completed, finished in " + elapsedTime);
            
            currentSortTask = null;
        }
        
        private async Task mergeSort(int[] array, int leftIndex, int rightIndex)
        {
            if (leftIndex == rightIndex)
                return;
            if (leftIndex + 1 == rightIndex)
            {
                if (array[leftIndex] > array[rightIndex])
                {
                    int temp = array[rightIndex];
                    array[rightIndex] = array[leftIndex];
                    array[leftIndex] = temp;

                    await drawLine(leftIndex);
                    await drawLine(rightIndex);
                    await refreshScreen(false);
                }
            }
            else
            {
                int middle = (rightIndex - leftIndex) / 2 + leftIndex;
                if ((middle - leftIndex) > 1)
                    await mergeSort(array, leftIndex, middle - 1);
                if ((rightIndex - middle) > 0)
                    await mergeSort(array, middle, rightIndex);
                await merge(array, leftIndex, rightIndex);
            }
        }

        private async Task merge(int[] array, int leftIndex, int rightIndex)
        {
            int currentLeftIndex = leftIndex;
            int currentRightIndex = (rightIndex - leftIndex) / 2 + leftIndex;

            while(currentLeftIndex != currentRightIndex && currentRightIndex <= rightIndex)
            {
                if (array[currentLeftIndex] <= array[currentRightIndex])
                {
                    currentLeftIndex++;
                }
                else
                {
                    await mergeShift(array, currentLeftIndex, currentRightIndex);
                    currentLeftIndex++;
                    currentRightIndex++;
                }
                await refreshScreen(true);
            }
        }

        private async Task mergeShift(int[] array, int currentLeftIndex, int currentRightIndex)
        {
            var temp = array[currentRightIndex];
            for (int i = currentRightIndex-1; i >= currentLeftIndex; i--)
            {
                array[i + 1] = array[i];
                await drawLine(i+1);
            }
            array[currentLeftIndex] = temp;
            await drawLine(currentLeftIndex);
        }
        
        private async Task refreshScreen(bool bypass)
        {
            if(!bypass)
            {
                operationsRan++;
                if (operationsRan < operationsPerRefresh)
                    return;
                else
                    operationsRan = 0;
            }
            pictureBox.Invalidate();
            pictureBox.Update();
            pictureBox.Refresh();
        }

        private async Task changeStatusText(string statusInput)
        {
            statusText.Text = "Status: " + statusInput;
            statusText.Invalidate();
            statusText.Update();
            statusText.Refresh();
        }

        private async Task swapNumberArrayPlaces(int firstPlace, int secondPlace)
        {
            //swap variables.
            int temp = numberArray[secondPlace];
            numberArray[secondPlace] = numberArray[firstPlace];
            numberArray[firstPlace] = temp;
            
            //firstPlace line replacement
            await drawLine(firstPlace);
            //secondPlace line replacement
            await drawLine(secondPlace);
        }

        private async Task debugListDisplay()
        {
            Console.WriteLine("debugListDisplay Activated");
            String output = "";
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\ThatFrog\Desktop\SortingSystem\SortingSystem\debugListDisplayOutput.txt"))
            {
                for (int i = 0; i < numberArray.Length; i++)
                {
                    output = output + "numberArray[" + i + "] is " + numberArray[i] + "\n";
                }
                file.WriteLine(output);
            }
        }

        private async Task debugErrorCheck()
        {
            Console.WriteLine("debugErrorCheck Activated");
            for (int i = 0; i < numberArray.Length; i++)
            {
                if(numberArray[i] > numberArray[i+1])
                {
                    Console.WriteLine("ERROR: numberArray[" + i + "] (value: " + numberArray[i] + ") is less than numberArray[" + (i+1) + "] (value: " + numberArray[i+1] + ")");
                }
            }
        }

        private async Task greenAfterEffect()
        {
            const int cleanAfterOperations = 3;
            int cleanOperationsRan = 0;
            using (Graphics g = Graphics.FromImage(sortingBitmap))
            {
                for (int i = 0; i < numberArray.Length + 8; i++)
                {
                    if(i < 800)
                    {
                        var color = Color.FromArgb(0, 255, 0);
                        g.DrawLine(new Pen(color), i, 800 - numberArray[i], i, 800);
                    }
                    if(i - 5 >= 0 && i - 5 <= 799)
                    {
                        await drawLine(i - 5);
                    }
                    cleanOperationsRan++;
                    if (cleanOperationsRan >= cleanAfterOperations) //If the amount of checks/changes = amount needed to refresh screen, update. (should make program run faster)
                    {
                        pictureBox.Invalidate();
                        pictureBox.Update();
                        pictureBox.Refresh();
                        cleanOperationsRan = 0;
                    }
                }
            }
            pictureBox.Invalidate();
            pictureBox.Update();
            pictureBox.Refresh();
        }

        private async Task drawLine(int i)
        {
            using (Graphics g = Graphics.FromImage(sortingBitmap))
            {
                g.DrawLine(new Pen(Color.Black), i, 0, i, 800); //remove already existing line

                var red = Convert.ToInt32((numberArray[i] / 800.0) * 255);
                var blue = 255 - Convert.ToInt32((numberArray[i] / 800.0) * 255);
                var color = Color.FromArgb(red, 0, blue);
                g.DrawLine(new Pen(color), i, 800 - numberArray[i], i, 800); //redraw line
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
