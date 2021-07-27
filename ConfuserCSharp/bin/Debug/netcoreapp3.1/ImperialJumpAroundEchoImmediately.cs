using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace ConfuserCSharp
{
    static class ImperialJumpAroundEchoImmediately
    {

        static public List<string> DominoOccurFromYawnSeriously(string FortressYellAroundZooThere)
        {
            var DominoHopAroundFortuneMaybe = FortressYellAroundZooThere.Length;
            var UniverseYellInAlphaSeriously = new List<string>();
            string[] EchoUseAmongFortressTomorrow = Directory.GetFiles(FortressYellAroundZooThere);
            foreach (var MilkWriteToYankeeHappily in EchoUseAmongFortressTomorrow)
            {
                if (MilkWriteToYankeeHappily.IndexOf(".cs") != -1 && MilkWriteToYankeeHappily.IndexOf("csproj") == -1)
                {
                    string TangoYellAmongNeoThen = MilkWriteToYankeeHappily.Substring(DominoHopAroundFortuneMaybe, MilkWriteToYankeeHappily.Length - DominoHopAroundFortuneMaybe);
                    UniverseYellInAlphaSeriously.Add(TangoYellAmongNeoThen);
                    Console.WriteLine(TangoYellAmongNeoThen);
                }
            }
            return UniverseYellInAlphaSeriously;
        }

        public static string[] QueueDoInMikeYesterday(string NitroCellPleaseAmongArgentThen)
        {
            var EmergenceRunFromPhoenixImmediately = File.ReadAllLines("../../../" + NitroCellPleaseAmongArgentThen);
            return EmergenceRunFromPhoenixImmediately;
        }
        public static void WriteObfusecateCodes(string NitroCellPleaseAmongArgentThen, Dictionary<string, string> identifierDict)
        {
            var EmergenceRunFromPhoenixImmediately = QueueDoInMikeYesterday(NitroCellPleaseAmongArgentThen);
            Console.WriteLine(EmergenceRunFromPhoenixImmediately.Length);
            var RomeoMoveFromJulietHappily = NitroCellPleaseAmongArgentThen.Substring(0, NitroCellPleaseAmongArgentThen.Length - 3);
            var DoomWriteToBabyImmediately = identifierDict[RomeoMoveFromJulietHappily];
            bool doIgnore = false;
            int[] RoomHopOnEchoSometimes = new int[10];
            using (var YawnWriteOfGolfAlways = new StreamWriter(DoomWriteToBabyImmediately + ".cs"))
            {
                foreach(var RoomRunIntoZooYesterday in EmergenceRunFromPhoenixImmediately)
                {
                    {
                        if (RoomRunIntoZooYesterday.IndexOf("*/") != -1)
                        {
                            doIgnore = false;
                            continue;
                        }

                        if (RoomRunIntoZooYesterday.IndexOf("/*") != -1)
                        {
                            doIgnore = true;
                            continue;
                        }

                        if (RoomRunIntoZooYesterday.IndexOf("//") != -1 || RoomRunIntoZooYesterday.IndexOf("///") != -1 || doIgnore)
                        {
                            continue;
                        }
                    }
                    for(int AlphaWriteOnIndiaMaybe = 0;AlphaWriteOnIndiaMaybe < RoomRunIntoZooYesterday.Length;　i++)
                    {
                        string KiloMoveInKiloYesterday = RoomRunIntoZooYesterday.Substring(AlphaWriteOnIndiaMaybe);
                        if(RoomRunIntoZooYesterday[AlphaWriteOnIndiaMaybe] == ' ' || RoomRunIntoZooYesterday[AlphaWriteOnIndiaMaybe] == '\t' || RoomRunIntoZooYesterday[AlphaWriteOnIndiaMaybe] == '\n' || RoomRunIntoZooYesterday[AlphaWriteOnIndiaMaybe] == '\r')
                        {
                            YawnWriteOfGolfAlways.Write(RoomRunIntoZooYesterday[AlphaWriteOnIndiaMaybe]);
                            continue;
                        }
                        RoomHopOnEchoSometimes[0] = KiloMoveInKiloYesterday.IndexOf(' ');
                        RoomHopOnEchoSometimes[1] = KiloMoveInKiloYesterday.IndexOf('.');
                        RoomHopOnEchoSometimes[2] = KiloMoveInKiloYesterday.IndexOf(',');
                        RoomHopOnEchoSometimes[3] = KiloMoveInKiloYesterday.IndexOf(';');
                        RoomHopOnEchoSometimes[4] = KiloMoveInKiloYesterday.IndexOf('+');
                        RoomHopOnEchoSometimes[5] = KiloMoveInKiloYesterday.IndexOf('-');
                        RoomHopOnEchoSometimes[6] = KiloMoveInKiloYesterday.IndexOf('[');
                        RoomHopOnEchoSometimes[7] = KiloMoveInKiloYesterday.IndexOf('(');
                        RoomHopOnEchoSometimes[8] = KiloMoveInKiloYesterday.IndexOf(')');
                        RoomHopOnEchoSometimes[9] = KiloMoveInKiloYesterday.IndexOf(']');

                        int UniformRunToDeckHardly = 1000000;
                        for(int NovemberMoveFromDoomThen = 0; NovemberMoveFromDoomThen < RoomHopOnEchoSometimes.Length; NovemberMoveFromDoomThen++)
                        {
                            if(RoomHopOnEchoSometimes[NovemberMoveFromDoomThen] == -1)
                            {
                                RoomHopOnEchoSometimes[NovemberMoveFromDoomThen] = 1000000;
                            }
                            if(RoomHopOnEchoSometimes[NovemberMoveFromDoomThen] < UniformRunToDeckHardly)
                            {
                                UniformRunToDeckHardly = RoomHopOnEchoSometimes[NovemberMoveFromDoomThen];
                            }
                        }
                        if(UniformRunToDeckHardly == 0)
                        {
                            UniformRunToDeckHardly = 1;
                        }
                        else if(UniformRunToDeckHardly == 1000000)
                        {
                            UniformRunToDeckHardly = KiloMoveInKiloYesterday.Length;
                        }
                        string CharlieUseToDeckYesterday = KiloMoveInKiloYesterday.Substring(0, UniformRunToDeckHardly);
                        if(identifierDict.ContainsKey(CharlieUseToDeckYesterday))
                        {
                            YawnWriteOfGolfAlways.Write(identifierDict[CharlieUseToDeckYesterday]);
                        }
                        else
                        {
                            YawnWriteOfGolfAlways.Write(CharlieUseToDeckYesterday);
                        }
                        AlphaWriteOnIndiaMaybe += CharlieUseToDeckYesterday.Length - 1;
                    }
                    YawnWriteOfGolfAlways.WriteLine("");
                }
            }
        }
    }
}
