using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using radioPlayerApp;

namespace radioAppUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestFrequenciesOutOfRange()
        {
            //Test adding radio station with frequency is within fm band limit
            RadioPlayerApp rap = new RadioPlayerApp();
            RadioStation rs1 = new RadioStation("L out of range", "Music", new FmFrequencyBand(109, 98));
            RadioStation rs2 = new RadioStation("H out of range", "Music", new FmFrequencyBand(45, 110));
            RadioStation rs3 = new RadioStation("both in range", "Music", new FmFrequencyBand(98, 99));
            RadioStation rs4 = new RadioStation("both out of upper range", "Music", new FmFrequencyBand(109, 110));
            RadioStation rs5 = new RadioStation("both out of lower range", "Music", new FmFrequencyBand(56, 87));
            RadioStation rs6 = new RadioStation("L out and H null", "Music", new FmFrequencyBand(56, null));
            RadioStation rs7 = new RadioStation("H out and H null", "Music", new FmFrequencyBand(111, null));

            rap.Add(rs1);
            rap.Add(rs2);
            rap.Add(rs3);
            rap.Add(rs4);
            rap.Add(rs5);
            rap.Add(rs6);
            rap.Add(rs7);

            Assert.IsTrue(rap.RpaList.Count == 6 && rap.RpaList[5].stationName.Equals("both in range"));
        }
        [TestMethod]
        public void TestMethod2()
        {
            //Test adding duplicate radio station with frequency is within fm band limit
            RadioStation rs1 = new RadioStation("98 FM", "Music", new FmFrequencyBand(104.4, null));
            RadioStation rs2 = new RadioStation("98 FM", "Music", new FmFrequencyBand(104.4, null));

            RadioPlayerApp rap = new RadioPlayerApp();
            rap.Add(rs1);
            //duplicate entry
            rap.Add(rs2);

            RadioPlayerApp rap2 = new RadioPlayerApp();
            rap2.Add(rs2);

            //Test number of records in both lists, should be equal
            Assert.IsTrue(rap.RpaList.Count == rap2.RpaList.Count);

        }
        [TestMethod]
        public void TestPlayPauseFunctionOutput()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                RadioStation rs1 = new RadioStation("98 FM", "Music", new FmFrequencyBand(104.4, null));
                RadioPlayerApp rpa = new RadioPlayerApp();
                rpa.Add(rs1);
                rpa.Play(rs1.stationName);

                string expectedPlay = string.Format("Playing station {0}", rs1.stationName);
                Assert.AreEqual<string>(expectedPlay, sw.ToString().Remove(sw.ToString().Length - 2));
                sw.Close();
            }

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                RadioStation rs1 = new RadioStation("98 FM", "Music", new FmFrequencyBand(104.4, null));
                RadioPlayerApp rpa = new RadioPlayerApp();
                rpa.Add(rs1);
                rpa.Pause(rs1.stationName);
                string expectedPause = string.Format("Pausing station {0}", rs1.stationName);
                Assert.AreEqual<string>(expectedPause, sw.ToString().Remove(sw.ToString().Length - 2));
                sw.Close();
            }
        }

        [TestMethod]
        public void TestLikeFunction()
        {
            RadioStation rs1 = new RadioStation("98 FM", "Music", new FmFrequencyBand(97.4, 98.1));
            RadioStation rs2 = new RadioStation("FM 104", "Music", new FmFrequencyBand(104.4, null));
            RadioStation rs3 = new RadioStation("Newstalk", "News", new FmFrequencyBand(106, 108));

            RadioPlayerApp rap = new RadioPlayerApp();
            rap.Add(rs1);
            rap.Add(rs2);
            rap.Add(rs3);

            rap.LikeRadioStation(rs1.stationName);

            Assert.IsTrue(rap.FavouriteStationList[rap.FavouriteStationList.Count - 1].stationName.Equals(rs1.stationName));
        }
    }
}
