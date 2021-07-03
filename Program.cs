using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AO_Lib;
using static AO_Lib.AO_Devices;

namespace AO_SimpleExample
{
    class Program
    {
        public static AO_Filter Filter;
        public static AO_Deflector Deflector;

        public static bool ConsoleSpam = true; //u can set this to "true" to see, what's going on

        static void Main(string[] args)
        {
            
            //One filter connection test 
            Filter = AO_Filter.Find_and_connect_AnyFilter();
            Filter.Read_dev_file("DemoDev_2.dev");
            Filter.PowerOn();
        
            //subscribe on events
            Filter.onSetHz += Filter_onSetHz;
            Filter.onSetWl += Filter_onSetWl;

            //tests start. Look inside the RightSetting function to see, how to interact with an AOF
            Test(Filter_ShowInfo, Filter);
            Test(Filter_RightSetting, Filter);
            Test(Filter_WrongSetting, Filter);
            Filter.PowerOff();
            Filter.Dispose();
            //Several filters connection test 

            var Filters = AO_Filter.Find_all_filters();
            if (Filters.Count > 1) Complex_test_of(Filters);
            else foreach (AO_Filter f in Filters) { f.PowerOff(); f.Dispose(); }
              
            Deflector = AO_Deflector.Find_and_connect_any_Deflector();
            Deflector.Read_dev_file("DemoDev_defl_2.dev");
            Deflector.PowerOn();

            Deflector.onSetHz += Deflector_onSetHz; ;

            Test(Deflector_ShowInfo, Deflector);
            Test(Deflector_RightSetting, Deflector);
            Test(Deflector_WrongSetting, Deflector);
            Deflector.PowerOff();
            Deflector.Dispose();

            //Several deflectors connection test 
            var Deflectors = AO_Deflector.Find_all_deflectors();
            if (Deflectors.Count > 1) Complex_test_of(Deflectors);
            else foreach (AO_Deflector d in Deflectors) { d.PowerOff(); d.Dispose(); }

            Console.ReadKey();
        }

        private static void Deflector_onSetHz(AO_Deflector sender, float Angle_now_1, float HZ_now_1, float Angle_now_2, float HZ_now_2)
        {

           if(ConsoleSpam) Console.WriteLine(string.Format("Current Angle_1 / RF_1: {0} / {1}", Angle_now_1, HZ_now_1));
            if (ConsoleSpam) Console.WriteLine(string.Format("Current Angle_2 / RF_2: {0} / {1}", Angle_now_2, HZ_now_2));
        }

        private static void Filter_ShowInfo(AO_Filter pFilter)
        {
            if (ConsoleSpam) Console.WriteLine("Detected filter type: {0}", pFilter.FilterType);
            if (ConsoleSpam) Console.WriteLine("Filter name (descriptor): {0}. Filter serial number: {1}", pFilter.FilterName, pFilter.FilterSerial);
            if (ConsoleSpam) Console.WriteLine("MAX wavelenght / MAX RF: {0} / {1}", pFilter.WL_Max, pFilter.HZ_Max);
            if (ConsoleSpam) Console.WriteLine("MIN wavelenght / MIN RF: {0} / {1}", pFilter.WL_Min, pFilter.HZ_Min);
        }

        private static void Filter_RightSetting(AO_Filter pFilter)
        {
            //Попробуем установить правильную длину волны или частоту

            float WL_2set = 620.5f;
            if (ConsoleSpam) Console.WriteLine("WL of {0} setting attempt...", WL_2set);
            pFilter.Set_Wl(WL_2set);
            if (ConsoleSpam) Console.WriteLine();

            float HZ_2set = 65.341f;
            if (ConsoleSpam) Console.WriteLine("RF of {0} setting attempt...", HZ_2set);
            pFilter.Set_Hz(HZ_2set);

        }
        private static void Filter_WrongSetting(AO_Filter pFilter)
        {
            //Попробуем установить неправильную длину волны или частоту
            float WL_2set_wrong = 99999f;
            if (ConsoleSpam) Console.WriteLine("WL of {0} setting attempt...", WL_2set_wrong);
            try
            {
                pFilter.Set_Wl(WL_2set_wrong);
            }
            catch (Exception e)
            {
                if (ConsoleSpam) Console.WriteLine(e.Message);
            }
            if (ConsoleSpam) Console.WriteLine(string.Format("Current WL / RF: {0} / {1}", pFilter.WL_Current, pFilter.HZ_Current));
            if (ConsoleSpam) Console.WriteLine();

            float HZ_2set_wrong = 99999f;
            if (ConsoleSpam) Console.WriteLine("RF of {0} setting attempt.....", HZ_2set_wrong);
            try
            {
                pFilter.Set_Hz(HZ_2set_wrong);
            }
            catch (Exception e)
            {
                if (ConsoleSpam) Console.WriteLine(e.Message);
            }
            if (ConsoleSpam) Console.WriteLine(string.Format("Current WL / RF: {0} / {1}", pFilter.WL_Current, pFilter.HZ_Current));
        }
        private static void Filter_BoundSetting(AO_Filter pFilter)
        {
            //Попробуем установить неправильную длину волны или частоту
            float WL_2set_wrong = 845f;
            if (ConsoleSpam) Console.WriteLine("WL of {0} setting attempt...", WL_2set_wrong);
            try
            {
                pFilter.Set_Wl(WL_2set_wrong);
            }
            catch (Exception e)
            {
                if (ConsoleSpam) Console.WriteLine(e.Message);
            }
            if (ConsoleSpam) Console.WriteLine(string.Format("Current WL / RF: {0} / {1}", pFilter.WL_Current, pFilter.HZ_Current));
            if (ConsoleSpam) Console.WriteLine();

            float HZ_2set_wrong = 62f;
            if (ConsoleSpam) Console.WriteLine("RF of {0} setting attempt.....", HZ_2set_wrong);
            try
            {
                pFilter.Set_Hz(HZ_2set_wrong);
            }
            catch (Exception e)
            {
                if (ConsoleSpam) Console.WriteLine(e.Message);
            }
            if (ConsoleSpam) Console.WriteLine(string.Format("Current WL / RF: {0} / {1}", pFilter.WL_Current, pFilter.HZ_Current));
        }

        private static void Deflector_ShowInfo(AO_Deflector pDeflector)
        {
            if (ConsoleSpam) Console.WriteLine("Detected deflector type: {0}", pDeflector.DeflectorType);
            if (ConsoleSpam) Console.WriteLine("Deflector name (descriptor): {0}. Deflector serial number: {1}", pDeflector.DeflectorName, pDeflector.DeflectorSerial);
            if (ConsoleSpam) Console.WriteLine("MAX angle 1 / MAX RF 1: {0} / {1}", pDeflector.Angle_Max_1, pDeflector.HZ_Max_1);
            if (ConsoleSpam) Console.WriteLine("MIN angle 1 / MAX RF 1: {0} / {1}", pDeflector.Angle_Min_1, pDeflector.HZ_Min_1);
            if (ConsoleSpam) Console.WriteLine("MAX angle 2 / MAX RF 2: {0} / {1}", pDeflector.Angle_Max_2, pDeflector.HZ_Max_2);
            if (ConsoleSpam) Console.WriteLine("MIN angle 2 / MAX RF 2: {0} / {1}", pDeflector.Angle_Min_2, pDeflector.HZ_Min_2);
        }

        private static void Deflector_RightSetting(AO_Deflector pDeflector)
        {
            //Попробуем установить правильные угол или частоту

            float Angle1_2set = -1.00f;
            if (ConsoleSpam) Console.WriteLine("Angle_1 of {0} setting attempt...", Angle1_2set);
            pDeflector.Set_Angle_1(Angle1_2set);
            if (ConsoleSpam) Console.WriteLine();

            float HZ1_2set = 65.341f;
            if (ConsoleSpam) Console.WriteLine("RF_1 of {0} setting attempt...", HZ1_2set);
            pDeflector.Set_Hz_1(HZ1_2set);
            if (ConsoleSpam) Console.WriteLine();

            float Angle2_2set = 1.00f;
            if (ConsoleSpam) Console.WriteLine("Angle_2 of {0} setting attempt...", Angle2_2set);
            pDeflector.Set_Angle_2(Angle2_2set);
            if (ConsoleSpam) Console.WriteLine();

            float HZ2_2set = 70.341f;
            if (ConsoleSpam) Console.WriteLine("RF_2 of {0} setting attempt...", HZ2_2set);
            pDeflector.Set_Hz_2(HZ2_2set);
            if (ConsoleSpam) Console.WriteLine();

        }
        private static void Deflector_WrongSetting(AO_Deflector pDeflector)
        {
            //Попробуем установить неправильные угол или частоту

            float Angle1_2set = -5.00f;
            if (ConsoleSpam) Console.WriteLine("Angle_1 of {0} setting attempt...", Angle1_2set);
            try
            {
                pDeflector.Set_Angle_1(Angle1_2set);
            }
            catch (Exception e)
            {
                if (ConsoleSpam) Console.WriteLine(e.Message);
            }
            if (ConsoleSpam) Console.WriteLine();

            float HZ1_2set = 165.341f;
            if (ConsoleSpam) Console.WriteLine("RF_1 of {0} setting attempt...", HZ1_2set); 
            try
            {
                pDeflector.Set_Hz_1(HZ1_2set);
            }
            catch (Exception e)
            {
                if (ConsoleSpam) Console.WriteLine(e.Message);
            }
            if (ConsoleSpam) Console.WriteLine();

            float Angle2_2set = 5.00f;
            if (ConsoleSpam) Console.WriteLine("Angle_2 of {0} setting attempt...", Angle2_2set);
            try
            {
                pDeflector.Set_Angle_2(Angle2_2set);
            }
            catch (Exception e)
            {
                if (ConsoleSpam) Console.WriteLine(e.Message);
            }
            if (ConsoleSpam) Console.WriteLine();

            float HZ2_2set = 170.341f;
            if (ConsoleSpam) Console.WriteLine("RF_2 of {0} setting attempt...", HZ2_2set);
            try
            {
                pDeflector.Set_Hz_2(HZ2_2set);
            }
            catch (Exception e)
            {
                if (ConsoleSpam) Console.WriteLine(e.Message);
            }
            if (ConsoleSpam) Console.WriteLine();

        }
        private static void Deflector_BoundSetting(AO_Deflector pDeflector)
        {
            //Попробуем установить неправильные угол или частоту

            float Angle1_2set = -1.50f;
            if (ConsoleSpam) Console.WriteLine("Angle_1 of {0} setting attempt...", Angle1_2set);
            try
            {
                pDeflector.Set_Angle_1(Angle1_2set);
            }
            catch (Exception e)
            {
                if (ConsoleSpam) Console.WriteLine(e.Message);
            }
            if (ConsoleSpam) Console.WriteLine();

            float HZ1_2set = 137.341f;
            if (ConsoleSpam) Console.WriteLine("RF_1 of {0} setting attempt...", HZ1_2set);
            try
            {
                pDeflector.Set_Hz_1(HZ1_2set);
            }
            catch (Exception e)
            {
                if (ConsoleSpam) Console.WriteLine(e.Message);
            }
            if (ConsoleSpam) Console.WriteLine();

            float Angle2_2set = 1.50f;
            if (ConsoleSpam) Console.WriteLine("Angle_2 of {0} setting attempt...", Angle2_2set);
            try
            {
                pDeflector.Set_Angle_2(Angle2_2set);
            }
            catch (Exception e)
            {
                if (ConsoleSpam) Console.WriteLine(e.Message);
            }
            if (ConsoleSpam) Console.WriteLine();

            float HZ2_2set = 62.341f;
            if (ConsoleSpam) Console.WriteLine("RF_2 of {0} setting attempt...", HZ2_2set);
            try
            {
                pDeflector.Set_Hz_2(HZ2_2set);
            }
            catch (Exception e)
            {
                if (ConsoleSpam) Console.WriteLine(e.Message);
            }
            if (ConsoleSpam) Console.WriteLine();

        }
        

        private static void Filter_onSetWl(AO_Filter sender, float WL_now, float HZ_now)
        {
            if (ConsoleSpam) Console.WriteLine(string.Format("Current WL / RF: {0} / {1}", WL_now, HZ_now));
        }

        private static void Filter_onSetHz(AO_Filter sender, float WL_now, float HZ_now)
        {
            if (ConsoleSpam) Console.WriteLine(string.Format("Current WL / RF: {0} / {1}", WL_now, HZ_now));
        }


        private static void Test(Action test_function)
        {
            try
            {
                WriteYellow("Testing of the function {0}...", test_function.Method.Name);
                test_function();
                WriteGreen("Test passed!");
                Console.WriteLine();
            }
            catch
            {
                WriteRed("Test failed!");
                Console.WriteLine();
            }
        }
        private static void Test(Action<AO_Filter> test_function, AO_Filter pFilter)
        {
            try
            {
                WriteYellow("Testing of the function {0}...", test_function.Method.Name);
                test_function(pFilter);
                WriteGreen("Test passed!");
                Console.WriteLine();
            }
            catch
            {
                WriteRed("Test failed!");
                Console.WriteLine();
            }
        }
        private static void Test(Action<AO_Deflector> test_function, AO_Deflector pDeflector)
        {
            try
            {
                WriteYellow("Testing of the function {0}...", test_function.Method.Name);
                test_function(pDeflector);
                WriteGreen("Test passed!");
                Console.WriteLine();
            }
            catch
            {
                WriteRed("Test failed!");
                Console.WriteLine();
            }
        }
        private static void Complex_test_of(List<AO_Filter> AO_Filters)
        {
            AO_Filters[0].Read_dev_file("DemoDev_1.dev");
            AO_Filters[1].Read_dev_file("DemoDev_2.dev");
            AO_Filters[0].PowerOn();
            AO_Filters[1].PowerOn();
            Filter.onSetHz += Filter_onSetHz;
            Filter.onSetWl += Filter_onSetWl;

            Test(Filter_ShowInfo, AO_Filters[0]);
            Test(Filter_ShowInfo, AO_Filters[1]);
            Test(Filter_RightSetting, AO_Filters[0]);
            Test(Filter_RightSetting, AO_Filters[1]);
            Test(Filter_WrongSetting, AO_Filters[0]);
            Test(Filter_WrongSetting, AO_Filters[1]);
            Test(Filter_BoundSetting, AO_Filters[0]); //right setting for this filter
            Test(Filter_BoundSetting, AO_Filters[1]); //...and wrong for this
            AO_Filters[0].PowerOff();
            AO_Filters[0].Dispose();
            AO_Filters[1].PowerOff();
            AO_Filters[1].Dispose();
        }
        private static void Complex_test_of(List<AO_Deflector> AO_Deflectors)
        {
            AO_Deflectors[0].Read_dev_file("DemoDev_defl_1.dev");
            AO_Deflectors[1].Read_dev_file("DemoDev_defl_2.dev");
            AO_Deflectors[0].PowerOn();
            AO_Deflectors[1].PowerOn();
            AO_Deflectors[0].onSetHz += Deflector_onSetHz;
            AO_Deflectors[1].onSetHz += Deflector_onSetHz;

            Test(Deflector_ShowInfo, AO_Deflectors[0]);
            Test(Deflector_ShowInfo, AO_Deflectors[1]);
            Test(Deflector_RightSetting, AO_Deflectors[0]);
            Test(Deflector_RightSetting, AO_Deflectors[1]);
            Test(Deflector_WrongSetting, AO_Deflectors[0]);
            Test(Deflector_WrongSetting, AO_Deflectors[1]);
            Test(Deflector_BoundSetting, AO_Deflectors[0]); //right setting for this filter
            Test(Deflector_BoundSetting, AO_Deflectors[1]); //...and wrong for this
            AO_Deflectors[0].PowerOff();
            AO_Deflectors[0].Dispose();
            AO_Deflectors[1].PowerOff();
            AO_Deflectors[1].Dispose();
        }
        private static void WriteGreen(string format, params object[] parameters) => WriteColored(ConsoleColor.Green, format, parameters);       
        private static void WriteRed(string format, params object[] parameters) => WriteColored(ConsoleColor.Red, format, parameters);       
        private static void WriteYellow(string format, params object[] parameters) => WriteColored(ConsoleColor.Yellow, format, parameters);       
    
        private static void WriteColored(ConsoleColor color, string format, params object[] parameters)
        {
            var CC_was = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(format, parameters);
            Console.ForegroundColor = CC_was;
        }
    }
}
