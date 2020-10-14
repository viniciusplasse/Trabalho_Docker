using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace T1_SMA
{
    class Program
    {
        public const int execucoes = 5; //capacidade da fila
        public const int K = 3; //capacidade da fila
        public static int num_servidores = 2; //número de servidores da fila        
        public const double chegada_inicial = 2.5; //valor do tempo de chegada inicial
        public static int fila = 0;

        public static int fila_2 = 0;
        public static int num_servidores_2 = 1; //número de servidores da fila 

        public static double[] estado_fila = new double[K + 1];
        public static double[] media_estado_fila = new double[K + 1];
        public static double[] probabilidade = new double[K + 1];
        public static double media_tempo_global = 0;
        public static List<Escalonador> lista_Escalonador = new List<Escalonador>();

        public static double[] estado_fila_2 = new double[K + 1];
        public static double[] media_estado_fila_2 = new double[K + 1];
        public static double[] probabilidade_2 = new double[K + 1];


        public const int a = 245746985; //Método Congruente Linear
        public const int c = 4358359; //Método Congruente Linear
        public const Int64 M = 4294967296; //Método Congruente Linear
        public static double[] x = new double[5] { 45127635, 954127, 6419829, 1597538426, 61423897 }; //semente inicial Método Congruente Linear
        public static int pos_seed = 0; //posição semente inicial
        public static int n = 0; // quantidade de números aleatórios

        public const int chegada1 = 2;
        public const int chegada2 = 3;
        public const int atendimento1 = 2;
        public const int atendimento2 = 5;
        public static int perda = 0;
        public static int perda_2 = 0;

        public const int atendimento1_f2 = 3;
        public const int atendimento2_f2 = 5;

        public static double media_perda = 0;
        public static double media_perda_2 = 0;

        public static double aleatorio = 0;
        public static double tempo_global = 0;
        public static double delta_t = 0;
        public static double delta_t_2 = 0;
        public static StringBuilder texto = new StringBuilder();
        

        public static void zerar_valores()
        {
            fila = 0;
            fila_2 = 0;
            aleatorio = 0;
            delta_t = 0;
            delta_t_2 = 0;
            tempo_global = 0;
            estado_fila = new double[K + 1] { 0, 0, 0, 0 };
            estado_fila_2 = new double[K + 1] { 0, 0, 0, 0 };
            lista_Escalonador = new List<Escalonador>();
            perda = 0;
            perda_2 = 0;
        }

        public static double converter_chegada(double uniforme_chegada)
        {
            double numero_entre_chegada;
            numero_entre_chegada = ((chegada2 - chegada1) * uniforme_chegada) + chegada1;

            return numero_entre_chegada;
        }

        public static double converter_saida(double uniforme_saida)
        {
            double numero_entre_saida;
            numero_entre_saida = ((atendimento2_f2 - atendimento1_f2) * uniforme_saida) + atendimento1_f2;

            return numero_entre_saida;
        }

        public static double converter_passagem(double uniforme_passagem) //redundância converter saida
        {
            double numero_entre_passagem;
            numero_entre_passagem = ((atendimento2_f2 - atendimento1_f2) * uniforme_passagem) + atendimento1_f2;

            return numero_entre_passagem;
        }

        public static double converter_atendimento(double uniforme_atendimento)
        {
            double numero_entre_atendimento;
            numero_entre_atendimento = ((atendimento2 - atendimento1) * uniforme_atendimento) + atendimento1;

            return numero_entre_atendimento;
        }

        public static double sorteio(double seed)
        {
            aleatorio = seed / M;
            seed = (a * seed + c) % M;
            x[pos_seed] = seed;
            
            //texto.Append(aleatorio);
            //texto.Append("\n");

            return aleatorio; //número uniforme entre 0 e 1
        }

        public static void CHEGADA(double tempo_chegada)
        {
            delta_t = tempo_chegada - tempo_global;
            delta_t_2 = tempo_chegada - tempo_global;
            tempo_global = tempo_chegada;

            estado_fila[fila] = estado_fila[fila] + delta_t;
            estado_fila_2[fila_2] = estado_fila_2[fila_2] + delta_t_2;

            if (fila < K)
            {
                fila++;


                if (fila <= num_servidores)
                {
                    Escalonador esc = new Escalonador();
                    esc.tempo = tempo_chegada + converter_atendimento(sorteio(x[pos_seed]));

                    esc.funcao = 2; //passagem
                    lista_Escalonador.Add(esc);
                    n++;
                }

            }
            else
            {
                perda++;
            }


            Escalonador esc2 = new Escalonador();
            esc2.funcao = 0; //chegada
            esc2.tempo = tempo_chegada + converter_chegada(sorteio(x[pos_seed]));
            lista_Escalonador.Add(esc2);
            n++;


        }

        public static void PASSAGEM(double tempo_passagem)
        {
            delta_t = tempo_passagem - tempo_global;
            delta_t_2 = tempo_passagem - tempo_global;
            tempo_global = tempo_passagem;

            estado_fila[fila] = estado_fila[fila] + delta_t;
            estado_fila_2[fila_2] = estado_fila_2[fila_2] + delta_t_2;

            fila--;

            if (fila >= num_servidores)
            {
                Escalonador esc = new Escalonador();
                esc.tempo = tempo_passagem + converter_atendimento(sorteio(x[pos_seed]));

                esc.funcao = 2; //passagem
                lista_Escalonador.Add(esc);
                n++;
            }

            if (fila_2 < K)
            {
                fila_2++;

                if (fila_2 <= num_servidores_2)
                {
                    Escalonador esc = new Escalonador();
                    esc.tempo = tempo_passagem + converter_saida(sorteio(x[pos_seed])); //converter_saida

                    esc.funcao = 1; //saida
                    lista_Escalonador.Add(esc);
                    n++;
                }                
            }
            else
            {
                perda_2++;
            }
        }

        public static void SAIDA(double tempo_saida)
        {
            delta_t = tempo_saida - tempo_global;
            delta_t_2 = tempo_saida - tempo_global;
            tempo_global = tempo_saida;

            estado_fila[fila] = estado_fila[fila] + delta_t;
            estado_fila_2[fila_2] = estado_fila_2[fila_2] + delta_t_2;

            fila_2--;

            if (fila_2 >= num_servidores_2)
            {
                Escalonador esc = new Escalonador();
                esc.tempo = tempo_saida + converter_saida(sorteio(x[pos_seed]));
                esc.funcao = 1; //saida
                lista_Escalonador.Add(esc);
                n++;
            }

        }

        static void Main(string[] args)
        {
            int controle_execucoes = 0;
            //Console.WriteLine("Digite o número de servidores: ");
            //num_servidores = Convert.ToInt32(Console.ReadLine());

            while (controle_execucoes < execucoes)
            {
                Escalonador esc = new Escalonador();

                esc.tempo = chegada_inicial;
                esc.funcao = 0; //chegada
                lista_Escalonador.Add(esc);
                n = 0;

                while (n < 10000)
                {
                    Escalonador evento = lista_Escalonador.OrderBy(x => x.tempo).First();
                    lista_Escalonador.Remove(evento);

                    if (evento.funcao == 0) //chegada
                    {
                        CHEGADA(evento.tempo);
                    }
                    else    if (evento.funcao == 1) //saida
                            {
                                SAIDA(evento.tempo);
                            }
                            else //passagem
                            {
                                PASSAGEM(evento.tempo);
                            }
                }

                media_perda = media_perda + perda; //soma das perdas da fila 1
                media_perda_2 = media_perda_2 + perda_2; //soma das perdas da fila 1

                for (int i = 0; i <= K; i++)
                {
                    media_estado_fila[i] = media_estado_fila[i] + estado_fila[i]; //soma dos tempos de estados das filas 1
                    media_estado_fila_2[i] = media_estado_fila_2[i] + estado_fila_2[i]; //soma dos tempos de estados das filas 2
                }

                media_tempo_global = media_tempo_global + tempo_global; //soma dos tempos globais

                controle_execucoes++;
                pos_seed++;
                zerar_valores();

                //System.IO.File.WriteAllText(@"C:\temp\aleatorio.txt", texto.ToString());
                //texto.Clear();
            }


            media_tempo_global = media_tempo_global / execucoes;

            for (int i = 0; i <= K; i++)
            {
                media_estado_fila[i] = media_estado_fila[i] / execucoes;
                media_estado_fila_2[i] = media_estado_fila_2[i] / execucoes;
            }

            //calculo prob
            for (int i = 0; i <= K; i++)
            {
                probabilidade[i] = (media_estado_fila[i] * 100) / media_tempo_global;
                probabilidade_2[i] = (media_estado_fila_2[i] * 100) / media_tempo_global;
            }
            Console.WriteLine("\nG/G/" + num_servidores + "/" + K);
            Console.WriteLine("\nChegada: " + chegada1 + " ... " + chegada2);
            Console.WriteLine("\nAtendimento: " + atendimento1 + " ... " + atendimento2);

            Console.WriteLine("\nG/G/" + num_servidores_2 + "/" + K);
            Console.WriteLine("\nChegada: direto fila 1");
            Console.WriteLine("\nAtendimento: " + atendimento1_f2 + " ... " + atendimento2_f2);
            Console.WriteLine("\n\n**********************************************************************************************");
            Console.WriteLine("\nFILA 1");
            for (int i = 0; i <= K; i++)
            {

                Console.WriteLine("Estado " + i + "         Tempo " + string.Format("{0:F4}", media_estado_fila[i]) + "          Probabilidade " + string.Format("{0:F2}", probabilidade[i]) + "%");

            }
            Console.WriteLine("\nPerdas fila 1: " + media_perda / 5);

            Console.WriteLine("\nFILA 2");
            for (int i = 0; i <= K; i++)
            {

                Console.WriteLine("Estado " + i + "         Tempo " + string.Format("{0:F4}", media_estado_fila_2[i]) + "          Probabilidade " + string.Format("{0:F2}", probabilidade_2[i]) + "%");

            }
            //Console.WriteLine("\nPerdas: " + perda);
            Console.WriteLine("\nPerdas fila 2: " + media_perda_2 / 5);
            Console.WriteLine("\nTempo total : " + string.Format("{0:F4}",media_tempo_global));

            //System.IO.File.WriteAllText(@"C:\temp\aleatorio.txt",texto.ToString());

            
            Console.ReadLine();

        }
    }

    public class Escalonador
    {
        public int funcao { get; set; } //0 - chegada / 1 - saida / 2 - passagem

        public double tempo { get; set; }

    }

}
