﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Alturos.Yolo;
using Alturos.Yolo.Model;

namespace NNHelper
{
    public class NeuralNet
    {
        public string[] TrainingNames;
        private YoloWrapper yoloWrapper;

        private NeuralNet()
        {
        }

        public bool TrainingMode { get; set; } = false;

        public static YoloWrapper GetYolo(string Game)
        {
            if (File.Exists($"trainfiles/{Game}.cfg") && File.Exists($"trainfiles/{Game}.weights") &&
                File.Exists($"trainfiles/{Game}.names"))
            {
                var yoloWrapper = new YoloWrapper($"trainfiles/{Game}.cfg", $"trainfiles/{Game}.weights",
                    $"trainfiles/{Game}.names");
                Console.Clear();
                if (yoloWrapper.EnvironmentReport.CudaExists == false)
                {
                    Console.WriteLine("Install CUDA 10");
                    Process.GetCurrentProcess().Kill();
                }

                if (yoloWrapper.EnvironmentReport.CudnnExists == false)
                {
                    Console.WriteLine("Cudnn doesn't exist");
                    Process.GetCurrentProcess().Kill();
                }

                if (yoloWrapper.EnvironmentReport.MicrosoftVisualCPlusPlus2017RedistributableExists == false)
                {
                    Console.WriteLine("Install Microsoft Visual C++ 2017 Redistributable");
                    Process.GetCurrentProcess().Kill();
                }

                if (yoloWrapper.DetectionSystem.ToString() != "GPU")
                {
                    MessageBox.Show("No GPU card detected. Exiting...");
                    Process.GetCurrentProcess().Kill();
                }

                return yoloWrapper;
            }

            return null;
        }

        public static NeuralNet Create(string Game)
        {
            var nn = new NeuralNet();
            nn.TrainingNames = null;
            nn.yoloWrapper = GetYolo(Game);

            if (nn.yoloWrapper == null)
                return null;

            nn.TrainingNames = File.ReadAllLines($"trainfiles/{Game}.names");
            File.Copy("defaultfiles/default_trainmore.cmd", $"darknet/{Game}_trainmore.cmd", true);
            if (File.Exists($"trainfiles/{Game}.cfg"))
                File.Copy($"trainfiles/{Game}.cfg", $"darknet/{Game}.cfg", true);
            else
                File.Copy("defaultfiles/default.cfg", $"darknet/{Game}.cfg", true);

            File.Copy("defaultfiles/default.conv.15", $"darknet/{Game}.conv.15", true);
            File.Copy("defaultfiles/default.data", $"darknet/data/{Game}.data", true);

            if (File.Exists($"trainfiles/{Game}.names"))
                File.Copy($"trainfiles/{Game}.names", $"darknet/{Game}.names", true);
            else
                File.Copy("defaultfiles/default.names", $"darknet/data/{Game}.names", true);

            File.Copy("defaultfiles/default.txt", $"darknet/data/{Game}.txt", true);
            File.Copy("defaultfiles/default.cmd", $"darknet/{Game}.cmd", true);

            return nn;
        }


        public IEnumerable<YoloItem> getItems(Image img, double confidence = 0.4)
        {
            using (var ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Png);

                return yoloWrapper.Detect(ms.ToArray())
                    .Where(x => x.Confidence > confidence && TrainingNames.Contains(x.Type));
            }
        }
    }
}