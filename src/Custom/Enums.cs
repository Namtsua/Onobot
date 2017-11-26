using System.ComponentModel;

 namespace ProgramEnum
{
    public enum Programs: int 
        {
            
            [Description("Arts")]
            Arts=1, 
            
            [Description("Architecture")]
            Architecture, 
            
            [Description("Biopsychology")]
            Biopsychology, 
            
            [Description("Biotechnology")]
            Biotechnology,

            [Description("Computer Science")]
            ComputerScience, 
            
            [Description("Engineering")]
            Engineering,
            
            [Description("Forestry")]
            Forestry, 
            
            [Description("Integrated Sciences")]
            IntegratedSciences, 
            
            [Description("Kinesiology")]
            Kinesiology, 
            
            [Description("LFS")]
            LFS, 
            
            [Description("Music")]
            Music, 
            
            [Description("Pharmacology")]
            Pharmacology, 
            
            [Description("Pharmacy")]
            Pharmacy,
            
            [Description("Physics & Astronomy")]
            PhysicsAstronomy, 
            
            [Description("Political Science")]
            PoliticalScience, 
            
            [Description("Sauder")]
            Sauder, 
            
            [Description("Science")]
            Science, 
            
            [Description("Statistics")]
            Statistics, 
            
            [Description("VISA")]
            VISA, 
            
            [Description("Langara Student")]
            Langara, 
            
            [Description("UVIC/SFU Spy")]
            UVICSFU, 
            
            [Description("High School Student")]
            HighSchool 
        };
}