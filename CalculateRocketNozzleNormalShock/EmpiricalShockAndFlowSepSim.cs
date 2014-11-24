﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CalculateRocketNozzleNormalShock
{
    class EmpiricalShockAndFlowSepSim : NozzleSim
    {
        public EmpiricalShockAndFlowSepSim(double exitAreaRatio, double gamma) : base(exitAreaRatio, gamma) { }
        private enum EmpiricalModel
        {
            schilling,
            schmucker,
            summerfield,
            stark,
            kaltbadal
        }

        private string[] EmpiricalModelStrings = 
        {
            "schilling",
            "schmucker",
            "summerfield",
            "stark",
            "kalt & badal"
        };

        private EmpiricalModel currentModel = EmpiricalModel.schilling;
        private Func<double, double, double, double> pSepFunc;

        public void SetCurrentModel()
        {
            currentModel = (EmpiricalModel)Enum.Parse(typeof(EmpiricalModel), SelectModel());

            if (currentModel == EmpiricalModel.schilling)
            {
                pSepFunc = new Func<double, double, double, double>(Schilling);
            }
            else if (currentModel == EmpiricalModel.summerfield)
            {
                pSepFunc = new Func<double, double, double, double>(Summerfield);
            }
            else if (currentModel == EmpiricalModel.kaltbadal)
            {
                pSepFunc = new Func<double, double, double, double>(KaltBadal);
            }
            else if (currentModel == EmpiricalModel.stark)
            {
                pSepFunc = new Func<double, double, double, double>(Stark);
            }
            else if (currentModel == EmpiricalModel.schmucker)
            {
                pSepFunc = new Func<double, double, double, double>(Schmucker);
            }
        }

        private string SelectModel()
        {
            string input = "";
            do
            {
                System.Console.WriteLine("");
                System.Console.WriteLine("Select Model:");
                System.Console.WriteLine("");
                for (int i = 0; i < EmpiricalModelStrings.Length; i++)
                    System.Console.WriteLine(EmpiricalModelStrings[i]);

                System.Console.WriteLine("");
                input = System.Console.ReadLine();
                input.ToLowerInvariant();

                System.Console.WriteLine("");
            } while (!CheckAgainstOptions(input, EmpiricalModelStrings));

            input = Regex.Replace(input, @"[ &]", "");

            return input;
        }

        private bool CheckAgainstOptions(string input, string[] options)
        {
            for (int i = 0; i < options.Length; i++)
            {
                if (input == options[i])
                    return true;
            }

            return false;
        }

        public bool TryCalculateEmpiricalModel(out double areaRatio, double chamberPressure, double backPressure)
        {
            this.chamberPressure = chamberPressure;
            this.backPressure = backPressure;

            areaRatio = 0;

            areaRatio = FindSolution();
            return true;
        }

        private double FindSolution()
        {
            double testRatio, lowerRatio, upperRatio;
            lowerRatio = 1;
            upperRatio = exitAreaRatio;
            testRatio = (upperRatio + lowerRatio) * 0.5;

            int counter = 0;

            int result;
            do
            {
                System.Console.Write("Iter: " + counter + " AreaRatio: " + testRatio);
                counter++;
                result = TestSepPressure(testRatio, pSepFunc, PRES_TOLERANCE);
                if (result > 0)
                {
                    lowerRatio = testRatio;
                }
                else if (result < 0)
                {
                    upperRatio = testRatio;
                }

                testRatio = (upperRatio + lowerRatio) * 0.5;

            } while (result != 0 && counter <= MAX_ITERATIONS);

            System.Console.WriteLine("");
            System.Console.WriteLine("Sim completed; iterations: " + counter);
            System.Console.WriteLine("Area Ratio at Sep: " + testRatio);
            System.Console.WriteLine("");

            return testRatio;
        }

        private int TestSepPressure(double areaRatio, Func<double, double, double, double> pSepFunc, double pressureTolerance)
        {
            double testM = machAreaRelation.EvaluateMachNumberSupersonic(areaRatio);
            double testP = chamberPressure / shockAndCompressibility.StagnationPressureRatio(testM);

            double pSep = pSepFunc(backPressure, chamberPressure, testM);

            double diffTestSepPres = testP - pSep;
            System.Console.WriteLine(" Pressure Diff: " + diffTestSepPres);

            if (Math.Abs(diffTestSepPres) < pressureTolerance)
                return 0;
            else if (diffTestSepPres < 0)
            {
                return -1;
            }
            else
                return 1;
        }

        private double KaltBadal(double backPressure, double chamberPressure, double machNumber)
        {
            double pSep = Math.Pow(chamberPressure / backPressure, -0.2) * 2 / 3 * backPressure;
            return pSep;
        }

        private double Schilling(double backPressure, double chamberPressure, double machNumber)
        {
            double pSep = Math.Pow(chamberPressure / backPressure, -0.195) * 0.583 * backPressure;
            return pSep;
        }

        private double Summerfield(double backPressure, double chamberPressure, double machNumber)
        {
            return 0.375;
        }

        private double Stark(double backPressure, double chamberPressure, double machNumber)
        {
            double pSep = Math.PI * backPressure / (3 * machNumber);
            return pSep;
        }

        private double Schmucker(double backPressure, double chamberPressure, double machNumber)
        {
            double pSep = Math.Pow(machNumber * 1.88 - 1, -0.64) * backPressure;

            return pSep;
        }
    }
}
