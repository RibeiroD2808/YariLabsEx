using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace YariLabsEx
{
    public class Commands
    {
        //propriedades
        char[,] image;
        private string[] splitCommand; //argumentos divididos

        //construtor
        public Commands(){
            image = new char[1, 1];
            splitCommand = new string[1];
        }

        //lê o comando da consola e divide os argumentos
        public void writeCommand()
        {
            string command = "";
            command = Console.ReadLine();

            if (command.Length > 1)
                splitCommand = command.Split(' ');
            readCommand(command);
        }

        public void readCommand(string command)
        {
            
            switch (command[0])
            {
                //1 cria uma imagem MxN e preenche todos os elementos com O
                case 'I':
                    image = new char[int.Parse(splitCommand[1]), int.Parse(splitCommand[2])] ;
                    //               X                           Y

                    for (int m = 0; m < image.GetLength(0); m++) {
                        for (int n = 0; n < image.GetLength(1); n++) { 
                           image[m, n] = 'O';
                        }
                    }

                    writeCommand();
                    break;

                // preenche todos os elementos da imagem com O
                case 'C':

                    for (int m = 0; m < image.GetLength(0); m++) {
                        for (int n = 0; n < image.GetLength(1); n++) {
                            image[m, n] = 'O';
                        }
                    }

                    writeCommand();
                    break;

                // muda a cor na posiçao X,Y
                case 'L':
                    //       X                          Y                           Cor
                    image[int.Parse(splitCommand[1]), int.Parse(splitCommand[2])] = splitCommand[3][0];

                    writeCommand();
                    break;

                // desenha uma linha vertical entre duas posiçoes 
                case 'V':
                    //                     Y inicial            Y final 
                    for (int i = int.Parse(splitCommand[2]); i <= int.Parse(splitCommand[3]); i++)
                    {
                        image[i, int.Parse(splitCommand[1])] = splitCommand[4][0];
                        //          X                           Cor
                    }

                    writeCommand();
                    break;

                // desenha uma linha horizontal entre duas posiçoes 
                case 'H':
                    //              X inicial                       X final
                    for (int i = int.Parse(splitCommand[1]); i <= int.Parse(splitCommand[2]); i++)
                    {
                        image[int.Parse(splitCommand[3]), i] = splitCommand[4][0];
                        //      Y                               Cor
                    }

                    writeCommand();
                    break;

                //preenche um espaço 
                case 'F':
                    F();

                    writeCommand();
                    break;

                //denha na consola todos os pixeis
                case 'S':
                    for (int m = 0; m < image.GetLength(0); m++){
                        for (int n = 0; n < image.GetLength(1); n++){
                            Console.Write(image[m, n]);
                        }
                        Console.Write("\n"); 
                    }

                    writeCommand();
                    break;

                //sai do programa
                case 'X':
                    return;

                default:
                    break;
            }
        }

        //da posiçao inicial preeche todos os pixeis ao redor que tenham a mesma cor 
        private void F()
        {
            //posiçao inicial
            int X = int.Parse(splitCommand[1]); //posiçao X
            int Y = int.Parse(splitCommand[2]); //posiçao Y

            char oldC = image[X, Y]; //cor a ser identificada e susbtituida
            char C = splitCommand[3][0]; //nova cor

            Tuple<int, int> t = new Tuple<int, int>(-1,-1);
            Tuple<int, int> tUp = new Tuple<int, int>(-1,-1);
            Tuple<int, int> tDown = new Tuple<int, int>(-1,-1);
            Tuple<int, int> tLeft = new Tuple<int, int>(-1,-1);
            Tuple<int, int> tRight = new Tuple<int, int>(-1,-1);

            Queue<Tuple<int, int>> queue = new Queue<Tuple<int, int>>();
            List<Tuple<int, int>> R = new List<Tuple<int, int>>();

            queue.Enqueue(new Tuple<int, int>(X, Y));
            R.Add(new Tuple<int, int>(X, Y));

            image[X, Y] = C;

            //Parte do algoritmo Depth-First Search
            while (queue.Count > 0)
            {
                t = queue.Dequeue();
                tUp = Tuple.Create(t.Item1 - 1, t.Item2);
                tDown = Tuple.Create(t.Item1 + 1, t.Item2);
                tLeft = Tuple.Create(t.Item1, t.Item2 - 1);
                tRight = Tuple.Create(t.Item1, t.Item2 + 1);

                //pixel de cima
                if (!R.Contains(tUp) && 
                    tUp.Item1 >= 0 &&
                    image[tUp.Item1, tUp.Item2] == oldC)
                {
                    queue.Enqueue(tUp);
                    R.Add(tUp);
                    image[tUp.Item1, tUp.Item2] = C;
                }

                //pixel de baixo
                if (
                    !R.Contains(tDown) &&
                    tDown.Item1 < image.GetLength(0)  &&
                    image[tDown.Item1, tDown.Item2] == oldC)
                {
                    
                    queue.Enqueue(tDown);
                    R.Add(tDown);
                    image[tDown.Item1, tDown.Item2] = C;
                }

                //pixel da esquerda
                if (!R.Contains(tLeft) &&
                    tLeft.Item2 >= 0 &&
                    image[tLeft.Item1, tLeft.Item2] == oldC)
                {
                    queue.Enqueue(tLeft);
                    R.Add(tLeft);
                    image[tLeft.Item1, tLeft.Item2] = C;
                }

                //pixel da direita
                if (!R.Contains(tRight) &&
                    tRight.Item2 < image.GetLength(1) &&
                    image[tRight.Item1, tRight.Item2] == oldC)
                {
                    queue.Enqueue(tRight);
                    R.Add(tRight);
                    image[tRight.Item1, tRight.Item2] = C;
                }
            }
        }
    }
}
