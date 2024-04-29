using System.Runtime.InteropServices;

namespace ESimConnect.Aircrafts;

public class PMDG_CDU_SDK
{
    // CDU Screen Cell Structure
    //
    // The Symbol is the ASCII code of the character to be drawn plus the following special symbols:
    // \xA1: left arrow
    // \xA2: right arrow
    // \xA3: up arrow
    // \xA4: down arrow
    //
    // See the note on enabling the CDU screen broadcast at the beginning of this file.

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
    public struct CDU_Cell
    {
        public byte Symbol;
        public byte Color;                    // any of CDU_COLOR_ defines
        public byte Flags;                    // a combination of CDU_FLAG_ bits
    };

    // CDU Screen Data Structure

    public const int nCDU_COLUMNS = 24;
    public const int nCDU_ROWS = 14;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
    public struct CDU_Screen
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = nCDU_COLUMNS)]
        public CDUROWS[] CDU_Columns;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public struct CDUROWS
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = nCDU_ROWS)]
            public CDU_Cell[] CDU_ROWS;
        }

        [MarshalAs(UnmanagedType.I1)] public bool Powered;                                      // true if CDU is powered
    };

    public interface ICDU_Screen
    {
        public CDU_Screen CDU_Screen { get; }
    }

    // CDU Screen Cell Colors
    public const int CDU_COLOR_WHITE = 0;
    public const int CDU_COLOR_CYAN = 1;
    public const int CDU_COLOR_GREEN = 2;
    public const int CDU_COLOR_MAGENTA = 3;
    public const int CDU_COLOR_AMBER = 4;
    public const int CDU_COLOR_RED = 5;

    // CDU Screen Cell flags
    public const int CDU_FLAG_SMALL_FONT = 0x01;		// small font, including that used for line headers 
    public const int CDU_FLAG_REVERSE = 0x02;           // character background is highlighted in reverse video
    public const int CDU_FLAG_UNUSED = 0x04;            // dimmed character color indicating inop/unused entries
}