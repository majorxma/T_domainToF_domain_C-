using System;
using System.IO;

/********************************************************************************************************
 * 共有三个类
 * Main类跑了一个200个数据时的小demo.
 * Complex类定义复数的操作
 * DFT类包含主DFT方法和归一化的方法。
 * ********************************************************************************************************/



namespace T_domain2F_domain
{
    class Program
    {
        static void Main(string[] args)
        {   //根据200个数据得到的输出demo.
            int Len;
            double[] prr;
            double max = 0;
            double[] input = new double[200] {490, 477, 467, 458, 450, 442, 433, 426, 419, 413, 411, 428, 445, 441, 434, 436, 446, 442, 427, 414, 402,
                                            391, 381, 372, 366, 363, 363, 364, 366, 372, 382, 397, 414, 430, 444, 460, 481, 502, 522, 539, 551, 561,
                                            567, 569, 568, 566, 570, 576, 578, 574, 565, 553, 541, 529, 519, 507, 496, 486, 494, 528, 551, 563, 576,
                                            596, 612, 624, 631, 636, 639, 640, 640, 638, 635, 633, 630, 625, 620, 615, 609, 603, 597, 590, 584, 578,
                                            571, 559, 541, 529, 524, 511, 486, 454, 422, 394, 372, 348, 340, 335, 334, 332, 332, 332, 332, 332, 333,
                                            336, 339, 341, 344, 349, 355, 360, 366, 372, 383, 396, 408, 419, 432, 448, 463, 473, 482, 493, 511, 530,
                                            551, 568, 580, 595, 597, 597, 595, 593, 598, 606, 619, 632, 642, 653, 659, 658, 653, 645, 640, 641, 643,
                                            650, 656, 659, 659, 655, 649, 640, 632, 626, 621, 614, 603, 590, 575, 564, 550, 530, 519, 507, 495, 484,
                                            472, 462, 452, 445, 437, 430, 423, 417, 423, 442, 445, 435, 423, 422, 431, 436, 428, 413, 401, 390, 381,
                                            373, 367, 363, 364, 365, 367, 371, 378, 396, 411, 428};
            prr = DFT.Dft(input);
            //prr = DFT.Dft(input);//prr为离散傅里叶变换得到的结果
            Len = prr.Length;
            //由于傅里叶变换得到的结果本应该有对称性，但第一个值一般有较大误差，所以取结果时请直接舍弃第一个值。
            double[] normalization_y;
            normalization_y = DFT.Normalization(prr);
            //得到归一化的双边振幅频谱。
            //若要输出双边振幅频谱，直接输出normalization_y[]1到Len的值，输出单边振幅谱则输出normalization_y[]1到Len/2的值。保存到文件中的为单边振幅谱。

            Len = normalization_y.Length;
            for (int i = 0; i < Len; i++)
            {
                Console.WriteLine(normalization_y[i]);
            }
            Console.WriteLine(Len);
            //输出归一化的双边振幅。
            string result1 = @"F:\result1.txt";
            FileStream fs = new FileStream(result1, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            for (int i = 0; i < Len; i++)
            {
                sw.Write(normalization_y[i]);
                sw.Flush();
                sw.Write("\n");
                sw.Flush();
            }
            sw.Close();
            fs.Close();
            //将归一化的半边振幅频谱保存到文件中
        }
    }

    //Complex类用来表示复数。
    public class Complex
    {
        public double Real = 0;
        public double Image = 0;
        public Complex(double re, double im)
        {
            Real = re;
            Image = im;
        }

        public Complex(double re)
        {
            Real = re;
            Image = 0;
        }

        public Complex()
        {
            Real = 0;
            Image = 0;
        }
        //得到复数的模
        public double getModulus()
        {
            return Math.Sqrt(Real * Real + Image * Image);
        }
        //操作符重载
        public static Complex operator +(Complex c1, Complex c2)
        {
            return new Complex(c1.Real + c2.Real, c1.Image + c2.Image);
        }

        public static Complex operator +(double d, Complex c)
        {
            return new Complex(d + c.Real, c.Image);
        }
        public static Complex operator +(Complex c, double d)
        {
            return new Complex(d + c.Real, c.Image);
        }
        public static Complex operator -(Complex c1, Complex c2)
        {
            return new Complex(c1.Real - c2.Real, c1.Image - c2.Image);
        }
        public static Complex operator -(double d, Complex c2)
        {
            return new Complex(d - c2.Real, 0 - c2.Image);
        }
        public static Complex operator -(Complex c1, double d)
        {
            return new Complex(c1.Real - d, c1.Image);
        }
        public static Complex operator *(Complex c1, Complex c2) 
        {
            return new Complex(c1.Real * c2.Real - c1.Image * c2.Image, c1.Real * c2.Image + c2.Real * c1.Image);
        }
        public static Complex operator *(Complex c, double d)
        {
            return new Complex(c.Real * d, c.Image * d);
        }
        public static Complex operator *(double d, Complex c)
        {
            return new Complex(c.Real * d, c.Image * d);
        }
        public static Complex operator /(Complex c, double d)
        {
            return new Complex(c.Real / d, c.Image / d);
        }
        public static Complex operator /(double d, Complex c)
        {
            double temp = d / (c.Real * c.Real + c.Image * c.Image);
            return new Complex(c.Real * temp, -c.Image * temp);
        }
        public static Complex operator /(Complex c1, Complex c2)
        {
            double Denominator = 1 / (c2.Real * c2.Real + c2.Image * c2.Image);
            return new Complex((c1.Real * c2.Real + c1.Image * c2.Image) * Denominator, (-c1.Real * c2.Image + c2.Real * c1.Image) * Denominator);
        }

        public override string ToString()
        {
            string Result;
            if (Math.Abs(Image) < 0.0001) Result = Real.ToString("f4");
            else if (Math.Abs(Real) < 0.0001)
            {
                if (Image > 0) Result = "j" + Image.ToString("f4");
                else Result = "- j" + (0 - Image).ToString("f4");
            }
            else
            {
                if (Image > 0) Result = Real.ToString("f4") + "+ j" + Image.ToString("f4");
                else Result = Real.ToString("f4") + "- j" + (0 - Image).ToString("f4");
            }
            Result += " ";
            return Result;
        }
    }


    //DFT用离散傅里叶变换得到结果。(得到的结果为双边频谱)
    public class DFT
    {
        //dft算法
        public static double[] Dft(double[] array)
        {
            int N = array.Length;
            Complex[] result = new Complex[N];
            for (int i = 0; i < N; i++)
            {
                Complex sum = new Complex();
                for (int j = 0; j < N; j++)
                {
                    sum += array[j] * (new Complex(Math.Cos(2 * Math.PI / N * j * i), -1 * Math.Sin(2 * Math.PI / N * i * j)));
                }
                result[i] = sum;
            }

            double[] result2 = new double[N];
            for (int i = 0; i < N; i++)
            {
                result2[i] = result[i].getModulus();
            }
            return result2;
        }


        //得到双边的归一化数据
        public static double[] Normalization(double[] array)
        {
            double[] normalization_y = new double[array.Length - 1];
            for (int i = 1; i < array.Length; i++)
            {
                normalization_y[i - 1] = array[i] / array.Length * 2;
            }
            return normalization_y;
        }
    }

    //fft算法
    public class FFT
    {
        public static double[] Fft(double[] In)
        {
            int stageNum = GetMin2n(In.Length);
            In = Get2nIn(In);
            int N = In.Length;
            Complex[] WN = new Complex[N];
            double[] result = new double[N];
            for (int i = 0; i < N / 2; i++)
            {
                WN[i] = new Complex(Math.Cos(2 * Math.PI / N * i), -1 * Math.Sin(2 * Math.PI / N * i));
            }
            int[] stage = new int[stageNum];
            stage[0] = 0;
            for (int i = 0; i <stageNum; i++)
            {
                stage[i] = Convert.ToInt32(Math.Round(Math.Pow(2, stageNum - 1 - i)));
            }

            //重排数据
            Complex[] Register = new Complex[N];
            for (int i = 0; i < N; i++)
            {
                int index = GetIndex(i, stageNum);
                Register[i] = new Complex(In[index], 0);
            }
            Complex[] p = new Complex[N];
            Complex[] q = new Complex[N];
            Complex[] w = new Complex[N];
            int group = N;
            for (int i = 0; i < stageNum; i++)
            {
                group = group >> 1;
                int subnum = N / group;
                
                for (int j = 0; j < group; j++)
                {
                    for (int k = 0; k < subnum / 2; k++)
                    {
                        p[j * subnum + k] = p[j * subnum + k + subnum / 2] = Register[j * subnum + k];
                        w[j * subnum + k] = WN[stage[i] * k];
                    }
                    for (int k = subnum / 2; k < subnum; k++)
                    {
                        q[j * subnum + k] = q[j * subnum + k - subnum / 2] = Register[j * subnum + k];
                        w[j * subnum + k] = -1 * w[k - subnum / 2];
                    }
                }

                for (int j = 0; j < N; j++)
                {
                    Register[j] = p[j] + w[j] * q[j]; 
                }
            }
            for (int i = 0; i < N; i++)
            {
                result[i] = Register[i].getModulus();
            }

            return result;
        }

        private static int GetIndex(int dat, int stageNum)
        {
            int result = 0;
            for (int i = 0; i < stageNum; i++)
            {
                if (0 != (dat & (0x01 << i))) result |= ((0x01 << (stageNum - 1)) >> i);
            }
            return result;
        }

        //因为fft需要保证数组长度为2的n次幂，因此要在结尾补0.
        public static double[] Get2nIn(double[] In)
        {
            int n = GetMin2n(In.Length);
            int N = 0x01 << n;
            double[] result = new double[N];
            int i = 0;
            for (i = 0; i < In.Length; i++)
            {
                result[i] = In[i];
            }
            for (; i < N; i++)
            {
                result[i] = 0;
            }
            return result;
        }
        //得到最小的2的n次幂的n值。
        public static int GetMin2n(int len)
        {
            int num = 1;
            int i = 0;
            while (num < len)
            {
                num *= 2;
                i++;
            }
            return i;
        }
    }
}
