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

        [Flags]
        public enum LibRaw_init_flags : uint
        {
            LIBRAW_OPTIONS_NONE = 0,
            LIBRAW_OPIONS_NO_MEMERR_CALLBACK = 1,
            LIBRAW_OPIONS_NO_DATAERR_CALLBACK = 1 << 1
        }

        [Flags]
        public enum LibRaw_errors : int
        {
            LIBRAW_SUCCESS = 0,
            LIBRAW_UNSPECIFIED_ERROR = -1,
            LIBRAW_FILE_UNSUPPORTED = -2,
            LIBRAW_REQUEST_FOR_NONEXISTENT_IMAGE = -3,
            LIBRAW_OUT_OF_ORDER_CALL = -4,
            LIBRAW_NO_THUMBNAIL = -5,
            LIBRAW_UNSUPPORTED_THUMBNAIL = -6,
            LIBRAW_INPUT_CLOSED = -7,
            LIBRAW_NOT_IMPLEMENTED = -8,
            LIBRAW_UNSUFFICIENT_MEMORY = -100007,
            LIBRAW_DATA_ERROR = -100008,
            LIBRAW_IO_ERROR = -100009,
            LIBRAW_CANCELLED_BY_CALLBACK = -100010,
            LIBRAW_BAD_CROP = -100011,
            LIBRAW_TOO_BIG = -100012,
            LIBRAW_MEMPOOL_OVERFLOW = -100013
        }

        [Flags]
        public enum LibRaw_progress : int
        {
            LIBRAW_PROGRESS_START = 0,
            LIBRAW_PROGRESS_OPEN = 1,
            LIBRAW_PROGRESS_IDENTIFY = 1 << 1,
            LIBRAW_PROGRESS_SIZE_ADJUST = 1 << 2,
            LIBRAW_PROGRESS_LOAD_RAW = 1 << 3,
            LIBRAW_PROGRESS_RAW2_IMAGE = 1 << 4,
            LIBRAW_PROGRESS_REMOVE_ZEROES = 1 << 5,
            LIBRAW_PROGRESS_BAD_PIXELS = 1 << 6,
            LIBRAW_PROGRESS_DARK_FRAME = 1 << 7,
            LIBRAW_PROGRESS_FOVEON_INTERPOLATE = 1 << 8,
            LIBRAW_PROGRESS_SCALE_COLORS = 1 << 9,
            LIBRAW_PROGRESS_PRE_INTERPOLATE = 1 << 10,
            LIBRAW_PROGRESS_INTERPOLATE = 1 << 11,
            LIBRAW_PROGRESS_MIX_GREEN = 1 << 12,
            LIBRAW_PROGRESS_MEDIAN_FILTER = 1 << 13,
            LIBRAW_PROGRESS_HIGHLIGHTS = 1 << 14,
            LIBRAW_PROGRESS_FUJI_ROTATE = 1 << 15,
            LIBRAW_PROGRESS_FLIP = 1 << 16,
            LIBRAW_PROGRESS_APPLY_PROFILE = 1 << 17,
            LIBRAW_PROGRESS_CONVERT_RGB = 1 << 18,
            LIBRAW_PROGRESS_STRETCH = 1 << 19,
            /* reserved */
            LIBRAW_PROGRESS_STAGE20 = 1 << 20,
            LIBRAW_PROGRESS_STAGE21 = 1 << 21,
            LIBRAW_PROGRESS_STAGE22 = 1 << 22,
            LIBRAW_PROGRESS_STAGE23 = 1 << 23,
            LIBRAW_PROGRESS_STAGE24 = 1 << 24,
            LIBRAW_PROGRESS_STAGE25 = 1 << 25,
            LIBRAW_PROGRESS_STAGE26 = 1 << 26,
            LIBRAW_PROGRESS_STAGE27 = 1 << 27,
            LIBRAW_PROGRESS_THUMB_LOAD = 1 << 28,
            LIBRAW_PROGRESS_TRESERVED1 = 1 << 29,
            LIBRAW_PROGRESS_TRESERVED2 = 1 << 30
        }

        [Flags]
        public enum LibRaw_image_formats : int
        {
            LIBRAW_IMAGE_JPEG = 1,
            LIBRAW_IMAGE_BITMAP = 2
        };

        [Flags]
        public enum LibRaw_output_formats : int
        {
            PPM = 0,
            TIFF = 1
        }

        [Flags]
        public enum LibRaw_output_bps : int
        {
            BPS8 = 8,
            BPS16 = 16
        }

        [Flags]
        public enum LibRaw_output_color : int
        {
            RAW = 0,
            SRGB = 1,
            ADOBE = 2,
            WIDE = 3,
            PROPHOTO = 4,
            XYZ = 5,
            ACES = 6
        }

        [Flags]
        public enum LibRaw_decoder_flags : uint
        {
            LIBRAW_DECODER_HASCURVE = 1 << 4,
            LIBRAW_DECODER_SONYARW2 = 1 << 5,
            LIBRAW_DECODER_TRYRAWSPEED = 1 << 6,
            LIBRAW_DECODER_OWNALLOC = 1 << 7,
            LIBRAW_DECODER_FIXEDMAXC = 1 << 8,
            LIBRAW_DECODER_ADOBECOPYPIXEL = 1 << 9,
            LIBRAW_DECODER_LEGACY_WITH_MARGINS = 1 << 10,
            LIBRAW_DECODER_3CHANNEL = 1 << 11,
            LIBRAW_DECODER_SINAR4SHOT = 1 << 11,
            LIBRAW_DECODER_FLATDATA = 1 << 12,
            LIBRAW_DECODER_FLAT_BG2_SWAPPED = 1 << 13,
            LIBRAW_DECODER_NOTSET = 1 << 15
        }

        [Flags]
        public enum LibRaw_runtime_capabilities : uint
        {
            LIBRAW_CAPS_UNDEFINED = 0,
            LIBRAW_CAPS_RAWSPEED = 1,
            LIBRAW_CAPS_DNGSDK = 2,
            LIBRAW_CAPS_GPRSDK = 4,
            LIBRAW_CAPS_UNICODEPATHS = 8,
            LIBRAW_CAPS_X3FTOOLS = 16,
            LIBRAW_CAPS_RPI6BY9 = 32
        }

        [Flags]
        public enum LibRaw_interpolation_quality : int
        {
            LINEAR = 0,
            VNG = 1,
            PPG = 2,
            AHD = 3,
            DCB = 4,
            DHT = 11,
            MODIFIED_AHD = 12
        }

        [Flags]
        public enum LibRaw_highlight_mode : int
        {
            CLIP = 0,
            UNCLIP = 1,
            BLEND = 2,
            REBUILD = 3,
            REBUILD4 = 4,
            REBUILD5 = 5,
            REBUILD6 = 6,
            REBUILD7 = 7,
            REBUILD8 = 8,
            REBUILD9 = 9
        }

        [Flags]
        public enum LibRaw_FBDD_noise_reduction : int
        {
            NO_FBDD = 0,
            LIGHT_FBDD = 1,
            FULL_FBDD = 2
        }

        [Flags]
        public enum LibRaw_cameramaker_index : uint
        {
            LIBRAW_CAMERAMAKER_Unknown = 0,
            LIBRAW_CAMERAMAKER_Agfa,
            LIBRAW_CAMERAMAKER_Alcatel,
            LIBRAW_CAMERAMAKER_Apple,
            LIBRAW_CAMERAMAKER_Aptina,
            LIBRAW_CAMERAMAKER_AVT,
            LIBRAW_CAMERAMAKER_Baumer,
            LIBRAW_CAMERAMAKER_Broadcom,
            LIBRAW_CAMERAMAKER_Canon,
            LIBRAW_CAMERAMAKER_Casio,
            LIBRAW_CAMERAMAKER_CINE,
            LIBRAW_CAMERAMAKER_Clauss,
            LIBRAW_CAMERAMAKER_Contax,
            LIBRAW_CAMERAMAKER_Creative,
            LIBRAW_CAMERAMAKER_DJI,
            LIBRAW_CAMERAMAKER_DXO,
            LIBRAW_CAMERAMAKER_Epson,
            LIBRAW_CAMERAMAKER_Foculus,
            LIBRAW_CAMERAMAKER_Fujifilm,
            LIBRAW_CAMERAMAKER_Generic,
            LIBRAW_CAMERAMAKER_Gione,
            LIBRAW_CAMERAMAKER_GITUP,
            LIBRAW_CAMERAMAKER_Google,
            LIBRAW_CAMERAMAKER_GoPro,
            LIBRAW_CAMERAMAKER_Hasselblad,
            LIBRAW_CAMERAMAKER_HTC,
            LIBRAW_CAMERAMAKER_I_Mobile,
            LIBRAW_CAMERAMAKER_Imacon,
            LIBRAW_CAMERAMAKER_JK_Imaging,
            LIBRAW_CAMERAMAKER_Kodak,
            LIBRAW_CAMERAMAKER_Konica,
            LIBRAW_CAMERAMAKER_Leaf,
            LIBRAW_CAMERAMAKER_Leica,
            LIBRAW_CAMERAMAKER_Lenovo,
            LIBRAW_CAMERAMAKER_LG,
            LIBRAW_CAMERAMAKER_Logitech,
            LIBRAW_CAMERAMAKER_Mamiya,
            LIBRAW_CAMERAMAKER_Matrix,
            LIBRAW_CAMERAMAKER_Meizu,
            LIBRAW_CAMERAMAKER_Micron,
            LIBRAW_CAMERAMAKER_Minolta,
            LIBRAW_CAMERAMAKER_Motorola,
            LIBRAW_CAMERAMAKER_NGM,
            LIBRAW_CAMERAMAKER_Nikon,
            LIBRAW_CAMERAMAKER_Nokia,
            LIBRAW_CAMERAMAKER_Olympus,
            LIBRAW_CAMERAMAKER_OmniVison,
            LIBRAW_CAMERAMAKER_Panasonic,
            LIBRAW_CAMERAMAKER_Parrot,
            LIBRAW_CAMERAMAKER_Pentax,
            LIBRAW_CAMERAMAKER_PhaseOne,
            LIBRAW_CAMERAMAKER_PhotoControl,
            LIBRAW_CAMERAMAKER_Photron,
            LIBRAW_CAMERAMAKER_Pixelink,
            LIBRAW_CAMERAMAKER_Polaroid,
            LIBRAW_CAMERAMAKER_RED,
            LIBRAW_CAMERAMAKER_Ricoh,
            LIBRAW_CAMERAMAKER_Rollei,
            LIBRAW_CAMERAMAKER_RoverShot,
            LIBRAW_CAMERAMAKER_Samsung,
            LIBRAW_CAMERAMAKER_Sigma,
            LIBRAW_CAMERAMAKER_Sinar,
            LIBRAW_CAMERAMAKER_SMaL,
            LIBRAW_CAMERAMAKER_Sony,
            LIBRAW_CAMERAMAKER_ST_Micro,
            LIBRAW_CAMERAMAKER_THL,
            LIBRAW_CAMERAMAKER_VLUU,
            LIBRAW_CAMERAMAKER_Xiaomi,
            LIBRAW_CAMERAMAKER_XIAOYI,
            LIBRAW_CAMERAMAKER_YI,
            LIBRAW_CAMERAMAKER_Yuneec,
            LIBRAW_CAMERAMAKER_Zeiss,
            // Insert additional indexes here
            LIBRAW_CAMERAMAKER_TheLastOne
        }

        public static class LibRawDefines
        {
            public const int CBLACK_SIZE = 4102; // LIBRAW_CBLACK_SIZE (обычно 4102)
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct libraw_colordata_t
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10000)]
            public ushort[] curve; // Кривая тонов (65,536 элементов)

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LibRawDefines.CBLACK_SIZE)]
            public uint[] cblack; // Массив чёрных уровней

            public uint black;        // Общий чёрный уровень
            public uint data_maximum; // Максимальное значение данных
            public uint maximum;      // Максимальное значение после коррекции

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] linear_max; // Максимальные линейные уровни (для Canon, Kodak и др.)

            public float fmaximum; // Максимальное значение (float)
            public float fnorm;    // Нормализация

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8 * 8)]
            public ushort[] white; // Белые уровни (8x8 матрица)

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] cam_mul; // Множители камеры (баланс белого)

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] pre_mul; // Предварительные множители

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3 * 4)]
            public float[] cmatrix; // Color matrix (3x4)

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3 * 4)]
            public float[] ccm;     // Color Correction Matrix (3x4)

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3 * 4)]
            public float[] rgb_cam; // RGB Camera matrix (3x4)

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4 * 3)]
            public float[] cam_xyz; // Camera XYZ matrix (4x3)

            public ph1_t phase_one_data; // Данные Phase One (отдельная структура)

            public float flash_used;      // Использование вспышки
            public float canon_ev;        // Поправка экспозиции Canon

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string model2; // Дополнительное название модели

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string UniqueCameraModel; // Уникальное имя камеры

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string LocalizedCameraModel; // Локализованное имя модели

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string ImageUniqueID; // Уникальный ID изображения

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
            public string RawDataUniqueID; // Уникальный ID RAW-данных

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string OriginalRawFileName; // Исходное имя RAW-файла

            public IntPtr profile;           // Указатель на ICC-профиль
            public uint profile_length;      // Размер профиля

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] black_stat; // Статистика чёрного

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public libraw_dng_color_t[] dng_color; // DNG color info

            public libraw_dng_levels_t dng_levels; // DNG уровни

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256 * 4)]
            public int[] WB_Coeffs; // Коэффициенты баланса белого (R, G1, B, G2)

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64 * 5)]
            public float[] WBCT_Coeffs; // Коэффициенты WB по цветовой температуре (CCT, R, G1, B, G2)

            public int as_shot_wb_applied; // Был ли применён баланс белого "как в камере"

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public libraw_P1_color_t[] P1_color; // Phase One color data

            public uint raw_bps; // Битовая глубина RAW (или формат Phase One)
            public int ExifColorSpace; // Цветовое пространство из EXIF
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct libraw_image_sizes_t
        {
            // Основные размеры изображения
            public ushort raw_height;
            public ushort raw_width;
            public ushort height;
            public ushort width;

            // Поля и отступы
            public ushort top_margin;
            public ushort left_margin;

            // Размеры выходного изображения
            public ushort iheight;
            public ushort iwidth;

            // RAW параметры
            public ushort raw_pitch;
            public double pixel_aspect;

            // Ориентация и маски
            [MarshalAs(UnmanagedType.I4)]
            public LibRawOrientation flip; // Используем enum для лучшей читаемости

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public int[] mask;

            // Области кропа
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public ushort[] raw_crop;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public int[] raw_inset_crops;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public int[] raw_inset_crops_size;

            // Информация о производителе
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string maker_index;

            // Уровни сенсора
            public SensorLevels sensor_levels;

            // Вложенная структура для уровней сенсора
            [StructLayout(LayoutKind.Sequential)]
            public struct SensorLevels
            {
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
                public float[] black;

                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
                public float[] white;

                public float cfablack;
                public float cfawhite;
            }
        }

        // Enum для ориентации
        public enum LibRawOrientation
        {
            ORIENTATION_0 = 0,
            ORIENTATION_90 = 3,
            ORIENTATION_180 = 2,
            ORIENTATION_270 = 1,
            ORIENTATION_HFLIP = 4,
            ORIENTATION_VFLIP = 5,
            ORIENTATION_ROTATE_90 = 6,
            ORIENTATION_ROTATE_270 = 7
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct libraw_makernotes_t
        {
            public libraw_canon_makernotes_t canon;
            public libraw_nikon_makernotes_t nikon;
            public libraw_hasselblad_makernotes_t hasselblad;
            public libraw_fuji_info_t fuji;
            public libraw_olympus_makernotes_t olympus;
            public libraw_sony_info_t sony;
            public libraw_kodak_makernotes_t kodak;
            public libraw_panasonic_makernotes_t panasonic;
            public libraw_pentax_makernotes_t pentax;
            public libraw_p1_makernotes_t phaseone;
            public libraw_ricoh_makernotes_t ricoh;
            public libraw_samsung_makernotes_t samsung;
            public libraw_metadata_common_t common;
        }

        // Пример реализации одной из вложенных структур (Canon)
        [StructLayout(LayoutKind.Sequential)]
        public struct libraw_canon_makernotes_t
        {
            public int CameraType;
            public int CameraSerial;
            public int CanonModelID;
            public int Firmware;
            public int DriveMode;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public short[] ImageStabilization;

            public float MeasuredEV;
            public int CameraTemperature;
            public int FocusDistance;
            public int AFPointsInFocus;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] AFInfo;
        }

        // Аналогично для других производителей...

        [StructLayout(LayoutKind.Sequential)]
        public struct libraw_metadata_common_t
        {
            public int FlashEC;
            public int FlashGN;
            public float CameraTemperature;
            public float SensorTemperature;
            public float LensTemperature;
            public float AmbientTemperature;
            public float BatteryTemperature;
            public float ExposureCompensation;
            public float ShutterSpeed;
            public float Aperture;
            public float FocalLength;
            public int ISO;
            public int FlashFired;
            public int FlashMode;
            public int FlashReturn;
            public int FlashType;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct libraw_shootinginfo_t
        {
            public short DriveMode;
            public short FocusMode;
            public short MeteringMode;
            public short AFPoint;
            public short ExposureMode;
            public short ExposureProgram;
            public short ImageStabilization;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string BodySerial;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string InternalBodySerial;

            // Методы расширения для удобства работы
            public string GetFormattedDriveMode()
            {
                return DriveMode switch
                {
                    0 => "Single Shot",
                    1 => "Continuous",
                    2 => "Self Timer",
                    3 => "Bracketing",
                    _ => $"Unknown ({DriveMode})"
                };
            }

            public bool HasImageStabilization()
            {
                return ImageStabilization != 0;
            }
        }

        // Enum для режимов (опционально)
        public enum CameraDriveMode
        {
            Single = 0,
            Continuous = 1,
            SelfTimer = 2,
            Bracketing = 3
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct libraw_data_t
        {
            public IntPtr image;  // Указатель на массив ushort[4]

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

        [StructLayout(LayoutKind.Sequential)]
        public struct libraw_thumbnail_t
        {
            public LibRaw_thumbnail_formats tformat; // Формат превью
            public ushort twidth;                    // Ширина превью
            public ushort theight;                   // Высота превью
            public ushort tcolors;                   // Количество цветов
            public int tlength;                      // Размер данных превью

            public IntPtr thumb;                     // Указатель на данные превью

            // Размеры встроенного JPEG превью
            public ushort tflip;
            public float tshutter;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string tdesc;                     // Описание превью

            // Методы для удобной работы
            public byte[] GetThumbnailBytes()
            {
                if (thumb == IntPtr.Zero || tlength <= 0)
                    return null;

                byte[] bytes = new byte[tlength];
                Marshal.Copy(thumb, bytes, 0, tlength);
                return bytes;
            }

            public bool IsJPEG => tformat == LibRaw_thumbnail_formats.JPEG;
            public bool IsLAYER => tformat == LibRaw_thumbnail_formats.LAYER;
            public bool IsBITMAP => tformat == LibRaw_thumbnail_formats.BITMAP;

            public Size Dimensions => new Size(twidth, theight);
        }

        // Форматы превью
        public enum LibRaw_thumbnail_formats
        {
            NO_THUMBNAIL = 0,    // Превью отсутствует
            JPEG = 1,            // JPEG превью
            BITMAP = 2,          // 8-битное RGB превью
            LAYER = 3,           // Слоистое превью
            ROLLEI = 4           // Rollei превью
        }

        // Методы расширения
        public static class ThumbnailExtensions
        {
            public static Image ToImage(this libraw_thumbnail_t thumbnail)
            {
                if (!thumbnail.IsJPEG || thumbnail.thumb == IntPtr.Zero)
                    return null;

                using (var ms = new MemoryStream(thumbnail.GetThumbnailBytes()))
                {
                    return Image.FromStream(ms);
                }
            }

            public static void SaveToFile(this libraw_thumbnail_t thumbnail, string path)
            {
                var bytes = thumbnail.GetThumbnailBytes();
                if (bytes != null)
                    File.WriteAllBytes(path, bytes);
            }
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct libraw_raw_unpack_params_t
        {
            public int use_rawspeed;       // Использование RawSpeed
            public int use_dngsdk;         // Использование DNG SDK
            public uint options;           // Битовые флаги опций
            public uint shot_select;       // -s выбор кадра
            public uint specials;          // Специальные флаги
            public uint max_raw_memory_mb; // Лимит памяти (в МБ)

            public int sony_arw2_posterization_thr; // Порог потери качества Sony ARW2

            public float coolscan_nef_gamma;        // Гамма-коррекция Nikon Coolscan

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public byte[] p4shot_order;             // Порядок кадров 4-shot

            public IntPtr custom_camera_strings;    // Массив строк кастомных камер

            // Свойства для удобного доступа
            public bool UseRawSpeed => use_rawspeed != 0;
            public bool UseDngSdk => use_dngsdk != 0;

            public string P4ShotOrderString =>
                p4shot_order != null ? Encoding.ASCII.GetString(p4shot_order).TrimEnd('\0') : null;

            // Метод для получения кастомных строк камер
            public List<string> GetCustomCameraStrings()
            {
                var result = new List<string>();
                if (custom_camera_strings == IntPtr.Zero)
                    return result;

                IntPtr current = custom_camera_strings;
                while (true)
                {
                    IntPtr stringPtr = Marshal.ReadIntPtr(current);
                    if (stringPtr == IntPtr.Zero)
                        break;

                    result.Add(Marshal.PtrToStringAnsi(stringPtr));
                    current += IntPtr.Size;
                }

                return result;
            }
        }

        // Вспомогательные enum для флагов
        [Flags]
        public enum RawUnpackOptions : uint
        {
            None = 0,
            DisableAutoScale = 1 << 0,
            DisableInterpolation = 1 << 1,
            // Добавьте другие флаги по мере необходимости
        }

        [Flags]
        public enum RawUnpackSpecials : uint
        {
            None = 0,
            HalfSize = 1 << 0,
            ForceFourColors = 1 << 1,
            // Добавьте другие специальные флаги
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct libraw_output_params_t
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] greybox;       // -A x1 y1 x2 y2

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] cropbox;       // -B x1 y1 x2 y2

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public double[] aber;        // -C

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public double[] gamm;        // -g

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] user_mul;     // -r mul0 mul1 mul2 mul3

            public float bright;         // -b
            public float threshold;      // -n
            public int half_size;        // -h
            public int four_color_rgb;   // -f
            public int highlight;        // -H
            public int use_auto_wb;      // -a
            public int use_camera_wb;    // -w
            public int use_camera_matrix; // +M/-M
            public int output_color;     // -o

            public IntPtr output_profile; // -o
            public IntPtr camera_profile; // -p
            public IntPtr bad_pixels;    // -P
            public IntPtr dark_frame;    // -K

            public int output_bps;       // -4
            public int output_tiff;      // -T
            public int output_flags;
            public int user_flip;        // -t
            public int user_qual;        // -q
            public int user_black;       // -k

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public int[] user_cblack;

            public int user_sat;         // -S
            public int med_passes;       // -m
            public float auto_bright_thr;
            public float adjust_maximum_thr;
            public int no_auto_bright;   // -W
            public int use_fuji_rotate;  // -j
            public int use_p1_correction;
            public int green_matching;

            // DCB parameters
            public int dcb_iterations;
            public int dcb_enhance_fl;
            public int fbdd_noiserd;

            // Exposure correction
            public int exp_correc;
            public float exp_shift;
            public float exp_preser;

            public int no_auto_scale;
            public int no_interpolation;

            // Методы для работы со строками
            public string OutputProfile => Marshal.PtrToStringAnsi(output_profile);
            public string CameraProfile => Marshal.PtrToStringAnsi(camera_profile);
            public string BadPixelsFile => Marshal.PtrToStringAnsi(bad_pixels);
            public string DarkFrameFile => Marshal.PtrToStringAnsi(dark_frame);

            // Методы расширения
            public bool UseAutoWhiteBalance => use_auto_wb != 0;
            public bool UseCameraWhiteBalance => use_camera_wb != 0;
            public bool ShouldApplyFujiRotate => use_fuji_rotate != 0;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct libraw_P1_color_t
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            public float[] romm_cam; // ROMM (ProPhoto RGB) цветовая матрица 3x3
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct libraw_dng_levels_t
        {
            public uint parsedfields; // Битовые флаги обработанных полей

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LibRawConstants.CBLACK_SIZE)]
            public uint[] dng_cblack; // Чёрные уровни DNG

            public uint dng_black; // Общий чёрный уровень DNG

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LibRawConstants.CBLACK_SIZE)]
            public float[] dng_fcblack; // Чёрные уровни DNG (float)

            public float dng_fblack; // Общий чёрный уровень DNG (float)

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] dng_whitelevel; // Уровни белого для каждого канала

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public ushort[] default_crop; // Область кропа [left, top, right, bottom]

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] user_crop; // Пользовательский кроп [top, left, bottom, right]

            public uint preview_colorspace; // Цветовое пространство превью

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] analogbalance; // Баланс аналоговых усилений

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] asshotneutral; // Нейтральные точки "как снято"

            public float baseline_exposure; // Базовый экспо-коррекция
            public float LinearResponseLimit; // Лимит линейного отклика
        }

        public static class LibRawConstants
        {
            public const int CBLACK_SIZE = 4102; // Стандартный размер для LIBRAW_CBLACK_SIZE
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct libraw_dng_color_t
        {
            public uint parsedfields;  // Битовые флаги обработанных полей

            public ushort illuminant; // Идентификатор источника освещения

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4 * 4)]
            public float[] calibration; // Матрица калибровки 4x4

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4 * 3)]
            public float[] colormatrix; // Цветовая матрица 4x3

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3 * 4)]
            public float[] forwardmatrix; // Прямая матрица 3x4
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ph1_t
        {
            public int format;    // Формат RAW Phase One
            public int key_off;   // Смещение ключа (часто используется в Phase One)
            public int tag_21a;   // Неизвестный тег (зависит от модели камеры)

            public int t_black;   // Чёрный уровень (верхний)
            public int split_col; // Разделение по столбцам
            public int black_col; // Чёрный уровень столбцов
            public int split_row; // Разделение по строкам
            public int black_row; // Чёрный уровень строк

            public float tag_210; // Дополнительный параметр (обычно float)
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi /*, Pack = 1 */)]
        public struct libraw_iparams_t
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)] public string guard;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)] public string make;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)] public string model;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)] public string software;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)] public string normalized_make;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)] public string normalized_model;
            public LibRaw_cameramaker_index maker_index;
            public uint raw_count;
            public uint dng_version;
            public uint is_foveon;
            public int colors;
            public uint filters;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 36)] public byte[] xtrans;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 36)] public byte[] xtrans_abs;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)] public string cdesc;
            public uint xmplen;
            public IntPtr xmpdata;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi /*, Pack = 1 */)]
        public struct libraw_nikonlens_t
        {
            public float EffectiveMaxAp;
            public byte LensIDNumber;
            public byte LensFStops;
            public byte MCUVersion;
            public byte LensType;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi /*, Pack = 1 */)]
        public struct libraw_dnglens_t
        {
            public float MinFocal;
            public float MaxFocal;
            public float MaxAp4MinFocal;
            public float MaxAp4MaxFocal;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi /*, Pack = 1 */)]
        public struct libraw_makernotes_lens_t
        {
            public long LensID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] public string Lens;
            public ushort LensFormat; /* to characterize the image circle the lens covers */
            public ushort LensMount;  /* 'male', lens itself */
            public long CamID;
            public ushort CameraFormat; /* some of the sensor formats */
            public ushort CameraMount;  /* 'female', body throat */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)] public string body;
            public short FocalType; /* -1/0 is unknown; 1 is fixed focal; 2 is zoom */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)] public string LensFeatures_pre;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)] public string LensFeatures_suf;
            public float MinFocal;
            public float MaxFocal;
            public float MaxAp4MinFocal;
            public float MaxAp4MaxFocal;
            public float MinAp4MinFocal;
            public float MinAp4MaxFocal;
            public float MaxAp;
            public float MinAp;
            public float CurFocal;
            public float CurAp;
            public float MaxAp4CurFocal;
            public float MinAp4CurFocal;
            public float MinFocusDistance;
            public float FocusRangeIndex;
            public float LensFStops;
            public long TeleconverterID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] public string Teleconverter;
            public long AdapterID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] public string Adapter;
            public long AttachmentID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] public string Attachment;
            public ushort FocalUnits;
            public float FocalLengthIn35mmFormat;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi /*, Pack = 1 */)]
        public struct libraw_lensinfo_t
        {
            public float MinFocal;
            public float MaxFocal;
            public float MaxAp4MinFocal;
            public float MaxAp4MaxFocal;
            public float EXIF_MaxAp;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] public string LensMake;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] public string Lens;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] public string LensSerial;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] public string InternalLensSerial;
            public ushort FocalLengthIn35mmFormat;
            [MarshalAs(UnmanagedType.Struct)] public libraw_nikonlens_t nikon;
            [MarshalAs(UnmanagedType.Struct)] public libraw_dnglens_t dng;
            [MarshalAs(UnmanagedType.Struct)] public libraw_makernotes_lens_t makernotes;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi /*, Pack = 1 */)]
        public struct libraw_gps_info_t
        {
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)] public float[] latitude;     /* Deg,min,sec */
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)] public float[] longitude;     /* Deg,min,sec */
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)] public float[] gpstimestamp;     /* Deg,min,sec */
            public float altitude;
            public byte altref;
            public byte latref;
            public byte longref;
            public byte gpsstatus;
            public byte gpsparsed;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi /*, Pack = 1 */)]
        public struct libraw_imgother_t
        {
            public float iso_speed;
            public float shutter;
            public float aperture;
            public float focal_len;
            public long timestamp; // time_t
            public uint shot_order;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U4, SizeConst = 32)] public uint[] gpsdata;
            [MarshalAs(UnmanagedType.Struct)] public libraw_gps_info_t parsed_gps;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)] public string desc;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)] public string artist;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 4)] public float[] analogbalance;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi /*, Pack = 1 */)]
        public struct libraw_decoder_info_t
        {
            [MarshalAs(UnmanagedType.LPStr)] public string decoder_name;
            public LibRaw_decoder_flags decoder_flags;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi /*, Pack = 1 */)]
        public struct libraw_processed_image_t
        {
            public LibRaw_image_formats type;
            public ushort height;
            public ushort width;
            public ushort colors;
            public ushort bits;
            public uint data_size;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 1)] public byte[] data;
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