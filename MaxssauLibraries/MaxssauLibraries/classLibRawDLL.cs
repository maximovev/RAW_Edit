using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Runtime.InteropServices;
using System.Text;
using static MaxssauLibraries.classLibRAW;

namespace MaxssauLibraries
{

    public static class LibRawImageSizesExtensions
    {
        public static Size GetRawSize(this libraw_image_sizes_t sizes)
        {
            return new Size(sizes.raw_width, sizes.raw_height);
        }

        public static Rectangle GetCropArea(this libraw_image_sizes_t sizes)
        {
            return new Rectangle(
                sizes.left_margin,
                sizes.top_margin,
                sizes.width,
                sizes.height);
        }

        public static bool IsRotated(this libraw_image_sizes_t sizes)
        {
            return sizes.flip == LibRawOrientation.ORIENTATION_90 ||
                   sizes.flip == LibRawOrientation.ORIENTATION_270;
        }

        public static float GetBlackLevel(this libraw_image_sizes_t sizes, int channel = 0)
        {
            return sizes.sensor_levels.black[channel];
        }
    }
    public class classLibRAW
    {
        private const string LibraryName = "libraw";

        using System;
using System.Runtime.InteropServices;

namespace LibRaw
    {
        public enum LibRaw_progress
        {
            // Progress stages would be defined here
        }

        public enum LibRaw_image_formats
        {
            // Image format types would be defined here
        }

        public enum LibRaw_thumbnail_formats
        {
            // Thumbnail format types would be defined here
        }

        public enum LibRaw_internal_thumbnail_formats
        {
            // Internal thumbnail format types would be defined here
        }

        public static class LibRawConstants
        {
            public const int LIBRAW_CBLACK_SIZE = 0x4000;
            public const int LIBRAW_AFDATA_MAXCOUNT = 256;
            public const int LIBRAW_THUMBNAIL_MAXCOUNT = 8;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct libraw_decoder_info_t
        {
            public string decoder_name;
            public uint decoder_flags;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct libraw_internal_output_params_t
        {
            public uint mix_green;
            public uint raw_color;
            public uint zero_is_bad;
            public ushort shrink;
            public ushort fuji_width;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct libraw_raw_inset_crop_t
        {
            public ushort cleft;
            public ushort ctop;
            public ushort cwidth;
            public ushort cheight;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct libraw_area_t
        {
            public short t; // top
            public short l; // left
            public short b; // bottom
            public short r; // right
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct libraw_sensor_highspeed_crop_t
        {
            public ushort cleft;
            public ushort ctop;
            public ushort cwidth;
            public ushort cheight;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct libraw_image_sizes_t
        {
            public ushort raw_height;
            public ushort raw_width;
            public ushort height;
            public ushort width;
            public ushort top_margin;
            public ushort left_margin;
            public ushort iheight;
            public ushort iwidth;
            public uint raw_pitch;
            public double pixel_aspect;
            public int flip;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.ByValArray, SizeConst = 4)]
            public int[][] mask;
            public ushort raw_aspect;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public libraw_raw_inset_crop_t[] raw_inset_crops;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct libraw_iparams_t
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] guard;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string make;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string model;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string software;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string normalized_make;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string normalized_model;
            public uint maker_index;
            public uint raw_count;
            public uint dng_version;
            public uint is_foveon;
            public int colors;
            public uint filters;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.ByValArray, SizeConst = 6)]
            public byte[][] xtrans;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.ByValArray, SizeConst = 6)]
            public byte[][] xtrans_abs;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string cdesc;
            public uint xmplen;
            public IntPtr xmpdata;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct libraw_dng_color_t
        {
            public uint parsedfields;
            public ushort illuminant;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.ByValArray, SizeConst = 4)]
            public float[][] calibration;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.ByValArray, SizeConst = 3)]
            public float[][] colormatrix;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.ByValArray, SizeConst = 4)]
            public float[][] forwardmatrix;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct libraw_dng_levels_t
        {
            public uint parsedfields;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LibRawConstants.LIBRAW_CBLACK_SIZE)]
            public uint[] dng_cblack;
            public uint dng_black;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LibRawConstants.LIBRAW_CBLACK_SIZE)]
            public float[] dng_fcblack;
            public float dng_fblack;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] dng_whitelevel;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public ushort[] default_crop;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] user_crop;
            public uint preview_colorspace;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] analogbalance;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] asshotneutral;
            public float baseline_exposure;
            public float LinearResponseLimit;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct libraw_P1_color_t
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            public float[] romm_cam;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct libraw_colordata_t
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10000)]
            public ushort[] curve;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LibRawConstants.LIBRAW_CBLACK_SIZE)]
            public uint[] cblack;
            public uint black;
            public uint data_maximum;
            public uint maximum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] linear_max;
            public float fmaximum;
            public float fnorm;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.ByValArray, SizeConst = 8)]
            public ushort[][] white;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] cam_mul;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] pre_mul;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.ByValArray, SizeConst = 4)]
            public float[][] cmatrix;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.ByValArray, SizeConst = 4)]
            public float[][] ccm;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.ByValArray, SizeConst = 4)]
            public float[][] rgb_cam;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.ByValArray, SizeConst = 3)]
            public float[][] cam_xyz;
            public ph1_t phase_one_data;
            public float flash_used;
            public float canon_ev;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string model2;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string UniqueCameraModel;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string LocalizedCameraModel;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string ImageUniqueID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
            public string RawDataUniqueID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string OriginalRawFileName;
            public IntPtr profile;
            public uint profile_length;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] black_stat;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public libraw_dng_color_t[] dng_color;
            public libraw_dng_levels_t dng_levels;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.ByValArray, SizeConst = 4)]
            public int[][] WB_Coeffs;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.ByValArray, SizeConst = 5)]
            public float[][] WBCT_Coeffs;
            public int as_shot_wb_applied;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public libraw_P1_color_t[] P1_color;
            public uint raw_bps;
            public int ExifColorSpace;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ph1_t
        {
            public int format;
            public int key_off;
            public int tag_21a;
            public int t_black;
            public int split_col;
            public int black_col;
            public int split_row;
            public int black_row;
            public float tag_210;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct libraw_thumbnail_t
        {
            public LibRaw_thumbnail_formats tformat;
            public ushort twidth;
            public ushort theight;
            public uint tlength;
            public int tcolors;
            public IntPtr thumb;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct libraw_thumbnail_item_t
        {
            public LibRaw_internal_thumbnail_formats tformat;
            public ushort twidth;
            public ushort theight;
            public ushort tflip;
            public uint tlength;
            public uint tmisc;
            public long toffset;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct libraw_thumbnail_list_t
        {
            public int thumbcount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LibRawConstants.LIBRAW_THUMBNAIL_MAXCOUNT)]
            public libraw_thumbnail_item_t[] thumblist;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct libraw_gps_info_t
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public float[] latitude;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public float[] longitude;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public float[] gpstimestamp;
            public float altitude;
            public byte altref;
            public byte latref;
            public byte longref;
            public byte gpsstatus;
            public byte gpsparsed;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct libraw_imgother_t
        {
            public float iso_speed;
            public float shutter;
            public float aperture;
            public float focal_len;
            public long timestamp;
            public uint shot_order;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public uint[] gpsdata;
            public libraw_gps_info_t parsed_gps;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string desc;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string artist;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] analogbalance;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct libraw_afinfo_item_t
        {
            public uint AFInfoData_tag;
            public short AFInfoData_order;
            public uint AFInfoData_version;
            public uint AFInfoData_length;
            public IntPtr AFInfoData;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct libraw_metadata_common_t
        {
            public float FlashEC;
            public float FlashGN;
            public float CameraTemperature;
            public float SensorTemperature;
            public float SensorTemperature2;
            public float LensTemperature;
            public float AmbientTemperature;
            public float BatteryTemperature;
            public float exifAmbientTemperature;
            public float exifHumidity;
            public float exifPressure;
            public float exifWaterDepth;
            public float exifAcceleration;
            public float exifCameraElevationAngle;
            public float real_ISO;
            public float exifExposureIndex;
            public ushort ColorSpace;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string firmware;
            public float ExposureCalibrationShift;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LibRawConstants.LIBRAW_AFDATA_MAXCOUNT)]
            public libraw_afinfo_item_t[] afdata;
            public int afcount;
        }

        // Additional structures would be defined here following the same pattern...

        [StructLayout(LayoutKind.Sequential)]
        public struct libraw_data_t
        {
            public IntPtr image; // ushort (*image)[4]
            public libraw_image_sizes_t sizes;
            public libraw_iparams_t idata;
            public libraw_lensinfo_t lens;
            public libraw_makernotes_t makernotes;
            public libraw_shootinginfo_t shootinginfo;
            public libraw_output_params_t params;
        public libraw_raw_unpack_params_t rawparams;
            public uint progress_flags;
            public uint process_warnings;
            public libraw_colordata_t color;
            public libraw_imgother_t other;
            public libraw_thumbnail_t thumbnail;
            public libraw_thumbnail_list_t thumbs_list;
            public libraw_rawdata_t rawdata;
            public IntPtr parent_class;
        }

        // The remaining structures would be defined similarly...
    }

    // callback functions
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate int ProgressCallback(IntPtr unused_data, LibRaw_progress state, int iter, int expected);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate void DataCallback(IntPtr data, string file, int offset);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate void MemoryCallback(IntPtr data, string file, string where);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate void EXIFParserCallback(IntPtr context, int tag, int type, int len, uint ord, IntPtr ifp, long _base);

        // Initialization and denitialization
        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern IntPtr libraw_init(LibRaw_init_flags flags);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern void libraw_close(IntPtr handler);

        // Data Loading from a File/Buffer
        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern LibRaw_errors libraw_open_file(IntPtr handler, string filename);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern LibRaw_errors libraw_open_file_ex(IntPtr handler, string filename, long max_buff_sz);

        [DllImport(LibraryName, CharSet = CharSet.Unicode)]
        public static extern LibRaw_errors libraw_open_wfile(IntPtr handler, string filename);

        [DllImport(LibraryName, CharSet = CharSet.Unicode)]
        public static extern LibRaw_errors libraw_open_wfile_ex(IntPtr handler, string filename, long max_buff_sz);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern LibRaw_errors libraw_open_buffer(IntPtr handler, byte[] buffer, long size);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern LibRaw_errors libraw_unpack(IntPtr handler);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern LibRaw_errors libraw_unpack_thumb(IntPtr handler);

        // Parameters setters/getters
        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern int libraw_get_raw_height(IntPtr handler);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern int libraw_get_raw_width(IntPtr handler);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern int libraw_get_iheight(IntPtr handler);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern int libraw_get_iwidth(IntPtr handler);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern float libraw_get_cam_mul(IntPtr handler, int index);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern float libraw_get_pre_mul(IntPtr handler, int index);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern float libraw_get_rgb_cam(IntPtr handler, int index1, int index2);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern IntPtr libraw_get_iparams(IntPtr handler);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern IntPtr libraw_get_lensinfo(IntPtr handler);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern IntPtr libraw_get_imgother(IntPtr handler);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern int libraw_get_color_maximum(IntPtr handler);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern void libraw_set_user_mul(IntPtr handler, int index, float val);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern void libraw_set_demosaic(IntPtr handler, LibRaw_interpolation_quality value);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern void libraw_set_output_color(IntPtr handler, LibRaw_output_color value);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern void libraw_set_output_bps(IntPtr handler, LibRaw_output_bps value);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern void libraw_set_gamma(IntPtr handler, int index, float value);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern void libraw_set_no_auto_bright(IntPtr handler, int value);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern void libraw_set_bright(IntPtr handler, float value);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern void libraw_set_highlight(IntPtr handler, LibRaw_highlight_mode value);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern void libraw_set_fbdd_noiserd(IntPtr handler, LibRaw_FBDD_noise_reduction value);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern void libraw_set_output_tif(IntPtr handler, LibRaw_output_formats value);

        // Auxiliary Functions
        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern IntPtr libraw_version();

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern int libraw_versionNumber();

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern LibRaw_runtime_capabilities libraw_capabilities();

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern int libraw_cameraCount();

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern IntPtr libraw_cameraList();

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern LibRaw_errors libraw_get_decoder_info(IntPtr handler, IntPtr decoder);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern IntPtr libraw_unpack_function_name(IntPtr handler);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern int libraw_COLOR(IntPtr handler, int row, int col);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern void libraw_subtract_black(IntPtr handler);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern void libraw_recycle_datastream(IntPtr handler);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern void libraw_recycle(IntPtr handler);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern IntPtr libraw_strerror(LibRaw_errors errorcode);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern IntPtr libraw_strprogress(LibRaw_progress progress);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern void libraw_set_memerror_handler(IntPtr handler, MemoryCallback cb, IntPtr datap);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern void libraw_set_exifparser_handler(IntPtr handler, EXIFParserCallback cb, IntPtr datap);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern void libraw_set_dataerror_handler(IntPtr handler, DataCallback func, IntPtr datap);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern void libraw_set_progress_handler(IntPtr handler, ProgressCallback callback, IntPtr datap);

        // Data Postprocessing, Emulation of dcraw Behavior
        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern LibRaw_errors libraw_dcraw_process(IntPtr handler);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern LibRaw_errors libraw_raw2image(IntPtr handler);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern void libraw_free_image(IntPtr handler);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern LibRaw_errors libraw_adjust_sizes_info_only(IntPtr handler);

        // Writing to Output Files
        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern LibRaw_errors libraw_dcraw_ppm_tiff_writer(IntPtr handler, string filename);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern LibRaw_errors libraw_dcraw_thumb_writer(IntPtr handler, string filename);

        // Writing processing results to memory buffer
        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern IntPtr libraw_dcraw_make_mem_image(IntPtr handler, ref int errc);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern IntPtr libraw_dcraw_make_mem_thumb(IntPtr handler, ref int errc);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern void libraw_dcraw_clear_mem(IntPtr img);

        // Microsoft Visual C runtime functions
        [DllImport("msvcrt", CharSet = CharSet.Ansi)]
        public static extern IntPtr strerror(int errc);
    }
}