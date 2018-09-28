using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radioPlayerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            RadioStation rs1 = new RadioStation("Radio Classic", "test", new FmFrequencyBand(99, null));
            RadioStation rs2 = new RadioStation("Radio Classic", "test", new FmFrequencyBand(99.8, 100));
            RadioPlayerApp rpa = new RadioPlayerApp();
            rpa.Add(rs1);
            rpa.Add(rs2);
            
            rpa.LikeRadioStation(rs1.stationName);

            rpa.Play(rs1.stationName);
            rpa.Pause(rs1.stationName);
            Console.ReadLine();
        }
    }

    #region INTERFACE

    /// <summary>
    /// IStreamable interface. Contains methods that allow a media item that can be streamed (e.g. a radio station) to be played or paused.
    /// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/interfaces/
    /// An interface contains definitions for a group of related functionalities that a class or a struct can implement.
    /// The interface defines only the signature. In that way, an interface in C# is similar to an abstract class in which all the methods are abstract.
    /// However, a class or struct can implement multiple interfaces, but a class can inherit only a single class, abstract or not. 
    /// Therefore, by using interfaces, you can include behavior from multiple sources in a class.
    /// Interfaces can contain methods, properties, events, indexers, or any combination of those four member types
    /// An interface can't contain constants, fields, operators, instance constructors, finalizers, or types.
    /// Interface members are automatically public, and they can't include any access modifiers. Members also can't be static.
    /// To implement an interface member, the corresponding member of the implementing class must be public, non-static,
    /// and have the same name and signature as the interface member. 1
    /// When a class or struct implements an interface, the class or struct must provide an implementation for all of the members that the interface defines.
    /// The interface itself provides no functionality that a class or struct can inherit in the way that it can inherit base class functionality.
    /// However, if a base class implements an interface, any class that's derived from the base class inherits that implementation.
    /// </summary>
    /// <typeparam name="T">Type of Payload</typeparam>
    public interface IStreamable<T>
    {
        /// <summary>
        /// Play radio, returns nothing.
        /// </summary>
        void Play();

        /// <summary>
        /// Pauses radio, returns nothing.
        /// </summary>
        void Pause();
    }

    #endregion


    #region STRUCT

    /// <summary>
    /// FMFrequencyBand structure to store information about the frequency band that a radio station is broadcasted on e.g. 89 – 90 MHz, 104.4 MHz etc
    /// </summary>
    public struct FmFrequencyBand
    {

        /// <summary>
        /// Initiate Double FmFrequencyBand.
        /// </summary>
        /// <param name="lowFreq">low frequency</param>
        /// <param name="upperFreq">upper frequency</param>
        public FmFrequencyBand(double lowFreq, double? upperFreq)
            : this()
        {
            try
            {
                if (!BandIsWithinRange(lowFreq, upperFreq))
                {
                    isInRange = false;
                }
                else
                {
                    isInRange = true;
                }
            }
            catch (Exception ex)
            {
                //Log to event viewer or to log file
                throw new Exception("Frequency out of range", ex.InnerException);
            }

        }

        /// <summary>
        /// Test if stations band is within the lower and upper fm band range.
        /// </summary>
        /// <param name="low">Low band frequency</param>
        /// <param name="high">High band frequency</param>
        /// <returns></returns>
        private bool BandIsWithinRange(double low, double? high)
        {
            if (high != null && (low.CompareTo(fmBandLowerLimit) >= 0 && low.CompareTo(fmBandUpperLimit) <= 0 &&
                                 Convert.ToDouble(high).CompareTo(fmBandUpperLimit) <= 0 && Convert.ToDouble(high).CompareTo(fmBandLowerLimit) >= 0))
            {
                this.lowerBandLimit = low;
                this.upperBandLimit = high;
                return true;
            }

            if (high == null && (low.CompareTo(fmBandLowerLimit) >= 0 && low.CompareTo(fmBandUpperLimit) <= 0))
            {
                this.lowerBandLimit = low;
                this.upperBandLimit = null;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Lower limit for the band.
        /// </summary>
        public double lowerBandLimit;

        /// <summary>
        /// Upper limit for the band.
        /// </summary>
        public double? upperBandLimit;

        /// <summary>
        /// True if band is within range, false otherwise.
        /// </summary>
        public bool isInRange;

        /// <summary>
        /// Lower limit for the whole FM band.
        /// </summary>
        private const double fmBandLowerLimit = 87.5;

        /// <summary>
        /// Upper limit for the whole FM band.
        /// </summary>
        private const double fmBandUpperLimit = 108.0;
    }

    #endregion


    #region PUBLIC CLASSES

    /// <summary>
    /// RadioStation class which implements the IStreamable interface and provides auto-implemented properties for namd, genre, frequency.
    ///
    /// A class or struct declaration is like a blueprint that is used to create instances or objects at run time 
    /// Classes and structs are two of the basic constructs of the common type system in the .NET Framework.
    /// Each is essentially a data structure that encapsulates a set of data and behaviors that belong together as a logical unit.
    /// The data and behaviors are the members of the class or struct, and they include its methods, properties, and events
    /// 
    /// Inheritance enables you to create new classes that reuse, extend, and modify the behavior that is defined in other classes.
    /// The class whose members are inherited is called the base class, and the class that inherits those members is called the derived class.
    /// A derived class can have only one direct base class. However, inheritance is transitive. If ClassC is derived from ClassB, and ClassB is derived from ClassA,
    ///  ClassC inherits the members declared in ClassB and ClassA.
    /// 
    /// Conceptually, a derived class is a specialization of the base class.
    /// 
    /// When you define a class to derive from another class, the derived class implicitly gains all the members of the base class, except for its constructors and finalizers.
    /// The derived class can thereby reuse the code in the base class without having to re-implement it. 
    /// In the derived class, you can add more members. In this manner, the derived class extends the functionality of the base class.
    /// 
    /// In general, classes are used to model more complex behavior, or data that is intended to be modified after a class object is created.
    /// Structs are best suited for small data structures that contain primarily data that is not intended to be modified after the struct is created
    /// </summary>
    public class RadioStation : IStreamable<RadioStation>
    {
        /// <summary>
        /// Initialise radio station.
        /// </summary>
        public RadioStation()
        {
        }

        /// <summary>
        /// Initialise radio station overload.
        /// </summary>
        /// <param name="stationName">station name</param>
        /// <param name="stationGenre"><station genre/param>
        /// <param name="stationFrequency">station frequency</param>
        public RadioStation(string stationName, string stationGenre, FmFrequencyBand stationFrequency)
        {
            this.stationName = stationName;
            this.stationGenre = stationGenre;
            this.stationFrequency = stationFrequency;
        }

        /// <summary>
        /// Play radio station.
        /// </summary>
        public void Play()
        {
            Console.WriteLine("Playing station {0}", this.stationName);
        }

        /// <summary>
        /// Pause radio station.
        /// </summary>
        public void Pause()
        {
            Console.WriteLine("Pausing station {0}", this.stationName);
        }

        /// <summary>
        /// Station name.
        /// </summary>
        public string stationName = String.Empty;

        /// <summary>
        /// Station genre.
        /// </summary>
        public string stationGenre = String.Empty;

        /// <summary>
        /// Station frequency.
        /// </summary>
        public FmFrequencyBand stationFrequency;

        /// <summary>
        /// To string overload.
        /// </summary>
        /// <returns>Formatted string containing full information about the radio station</returns>
        public override string ToString()
        {
            if (String.IsNullOrEmpty(stationFrequency.upperBandLimit.ToString()))
            {
                return "Station Name: " + stationName + "-- Station Genre: " + stationGenre
                       + "-- Station frequency:  " + stationFrequency.lowerBandLimit + ".";
            }
            else
            {
                return "Station Name: " + stationName + "-- Station Genre: " + stationGenre
                   + "-- Station frequency:  " + stationFrequency.lowerBandLimit + " - " + stationFrequency.upperBandLimit + ".";
            }
        }
    }

    /// <summary>
    /// RadioPlayerApp class to represent a radio player app
    /// </summary>
    public class RadioPlayerApp
    {
        /// <summary>
        /// Radio App stations list.
        /// </summary>
        public List<RadioStation> RpaList { get; set; }

        /// <summary>
        /// Favourite user stations
        /// </summary>
        private readonly List<RadioStation> favouriteStationList = new List<RadioStation>();

        /// <summary>
        /// Favourite user stations
        /// </summary>
        public List<RadioStation> FavouriteStationList
        {
            get { return favouriteStationList; }
        }

        /// <summary>
        /// Radio App station name.
        /// </summary>
        public string radioAppStationName { get; set; }

        /// <summary>
        /// Radio App station genre.
        /// </summary>
        public string radioAppStationGenre { get; set; }

        /// <summary>
        /// Radio App frequency band.
        /// </summary>
        public FmFrequencyBand radioAppStationFrequencyBand { get; set; }

        /// <summary>
        /// Initialise.
        /// </summary>
        public RadioPlayerApp()
        {
            LoadDefaultStations();
            this.favouriteStationList = new List<RadioStation>();
        }

        /// <summary>
        /// Initialize radio player.
        /// </summary>
        /// <param name="stationName">station name</param>
        /// <param name="stationGenre">station genre</param>
        /// <param name="frequencyBand">frequency band</param>
        public RadioPlayerApp(string stationName, string stationGenre, FmFrequencyBand frequencyBand)
        {
            radioAppStationName = stationName;
            radioAppStationGenre = stationGenre;
            radioAppStationFrequencyBand = frequencyBand;
        }

        public void Play(string stationName)
        {
            RadioStation found = this.RpaList?.Where(x => x.stationName.Equals(stationName)).FirstOrDefault();
            found.Play();
        }

        public void Pause(string stationName)
        {
            RadioStation found = this.RpaList?.Where(x => x.stationName.Equals(stationName)).FirstOrDefault();
            found.Pause();
        }

        /// <summary>
        /// Load default stations
        /// </summary>
        private void LoadDefaultStations()
        {
            this.RpaList = new List<RadioStation>()
            {
                new RadioStation("RTE Radio 1", "General", new FmFrequencyBand(89, 90)),
                new RadioStation("RTE 2FM", "Music", new FmFrequencyBand(90, 92)),
                new RadioStation("Newstalk", "News", new FmFrequencyBand(106, 108)),
                new RadioStation("FM 104", "Music", new FmFrequencyBand(104.4, null)),
                new RadioStation("98 FM", "Music", new FmFrequencyBand(97.4, 98.1))
            };
        }

        /// <summary>
        /// Add station band
        /// </summary>
        /// <param name="rpApp">Radio player app object</param>
        public void Add(RadioStation rpApp)
        {
            bool stationNameDuplicated = false;
            
            if (!RpaList.Contains(rpApp) && rpApp.stationFrequency.isInRange)
            {
                foreach (RadioStation radioStation in RpaList)
                {
                    if (radioStation.stationName.Equals(rpApp.stationName))
                    {
                        stationNameDuplicated = true;
                        break;
                    }
                }

                if (!stationNameDuplicated)
                    this.RpaList.Add(new RadioStation(rpApp.stationName, rpApp.stationGenre,
                        rpApp.stationFrequency));
            }
        }

        /// <summary>
        /// Like a station from the list of stations and therefore add it to the end of their list of favourite stations
        /// </summary>
        /// <param name="stationName">Station name</param>
        public void LikeRadioStation(string stationName)
        {
            RadioStation found = this.RpaList.Where(x => x.stationName.Equals(stationName)).FirstOrDefault();
            if (found != null)
            {
                RadioStation foundinFav = this.favouriteStationList.Where(x => x.stationName.Equals(stationName)).FirstOrDefault();
                if(foundinFav == null)
                {
                    favouriteStationList.Add(found);
                }
                else
                {
                    favouriteStationList.Remove(found);
                    favouriteStationList.Add(found);
                }
            }
            else
            {
                Console.WriteLine("Radion station {0} not found.", stationName);
            }
        }

    }

    #endregion
}
