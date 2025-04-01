/*

maxssau

Maximov Evgeny 
9890175@mail.ru
Russia, Samara

2024/05/01 - first edit
2025/04/01 - refactoring

Logger

 */
using System;
using System.IO;
using System.Windows.Forms;

namespace MaxssauLibraries
{

    public class ClassAddToLog
    {
        public ClassLogger Logger;
        public void AddToLog(Exception ex, string ModuleName)
        {
            if (Logger != null && ex != null && ModuleName != null)
            {
                if (Logger.Status == ClassLogger.LogStatus.Open)
                {
                    Logger.AddToLog(ModuleName, ex.Message);
                    if (ex.StackTrace != null)
                    {
                        Logger.AddToLog(ModuleName, ex.StackTrace);
                    }
                }
            }
        }
    }


    /// <summary>
    /// Логгер для записи событий приложения в файл
    /// </summary>
    public class ClassLogger : IDisposable
    {
        private StreamWriter _fileWriter;
        private bool _disposed = false;

        public string FileName { get; private set; }
        public string AppPath { get; private set; }
        public LogStatus Status { get; private set; } = LogStatus.Close;

        /// <summary>
        /// Статус работы логгера
        /// </summary>
        public enum LogStatus
        {
            Close = 0,
            Open = 1,
            Fail = 2,
            ReadOnly = 3
        }

        /// <summary>
        /// Режим работы с файлом лога
        /// </summary>
        public enum FileMode
        {
            New = 0,
            Append = 1
        }

        /// <summary>
        /// Инициализирует новый экземпляр логгера
        /// </summary>
        /// <param name="appPath">Путь к приложению</param>
        /// <param name="fileName">Имя файла лога</param>
        public ClassLogger(string appPath, string fileName)
        {
            if (string.IsNullOrWhiteSpace(appPath))
                throw new ArgumentException("Application path cannot be empty", nameof(appPath));

            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("File name cannot be empty", nameof(fileName));

            AppPath = appPath;
            FileName = fileName;
        }

        /// <summary>
        /// Открывает файл лога в указанном режиме
        /// </summary>
        /// <param name="mode">Режим работы с файлом</param>
        /// <returns>Статус операции</returns>
        public LogStatus OpenLog(FileMode mode)
        {
            if (Status == LogStatus.Open)
                return Status;

            string logPath = Path.Combine(AppPath, "logs");
            string fullFilePath = string.Empty;

            try
            {
                // Создаем директорию для логов если ее нет
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }

                fullFilePath = Path.Combine(logPath, FileName);

                // Проверяем доступность файла для записи
                if (File.Exists(fullFilePath))
                {
                    var fileInfo = new FileInfo(fullFilePath);
                    if (fileInfo.IsReadOnly)
                    {
                        Status = LogStatus.ReadOnly;
                        return Status;
                    }
                }

                // Открываем файл в нужном режиме
                _fileWriter = new StreamWriter(fullFilePath, mode == FileMode.Append)
                {
                    AutoFlush = true // Автоматически сбрасываем буфер после каждой записи
                };

                Status = LogStatus.Open;
                return Status;
            }
            catch (UnauthorizedAccessException ex)
            {
                Status = LogStatus.ReadOnly;
                ShowError("Access denied to log file", ex);
                return Status;
            }
            catch (Exception ex)
            {
                Status = LogStatus.Fail;
                ShowError("Failed to open log file", ex);
                return Status;
            }
        }

        /// <summary>
        /// Добавляет запись в лог
        /// </summary>
        /// <param name="module">Модуль-источник сообщения</param>
        /// <param name="value">Текст сообщения</param>
        /// <returns>true если запись успешна, false в случае ошибки</returns>
        public bool AddToLog(string module, string value)
        {
            if (Status != LogStatus.Open || _fileWriter == null)
                return false;

            if (string.IsNullOrWhiteSpace(module))
                module = "UNKNOWN";

            try
            {
                string logEntry = $"{GetTimeStamp()}[{module}] {value}";
                _fileWriter.WriteLine(logEntry);
                return true;
            }
            catch (Exception ex)
            {
                Status = LogStatus.Fail;
                ShowError("Failed to write to log", ex);
                return false;
            }
        }

        /// <summary>
        /// Закрывает файл лога
        /// </summary>
        public void CloseLog()
        {
            if (_fileWriter != null)
            {
                try
                {
                    _fileWriter.Close();
                }
                catch (Exception ex)
                {
                    ShowError("Failed to close log file", ex);
                }
                finally
                {
                    _fileWriter = null;
                    Status = LogStatus.Close;
                }
            }
        }

        /// <summary>
        /// Освобождает ресурсы логгера
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    CloseLog();
                }
                _disposed = true;
            }
        }

        ~ClassLogger()
        {
            Dispose(false);
        }

        private string GetTimeStamp()
        {
            DateTime now = DateTime.Now;
            return $"[{now:yyyy-MM-dd HH:mm:ss.fff}]";
        }

        private void ShowError(string message, Exception ex)
        {
            MessageBox.Show($"{message}: {ex.Message}{Environment.NewLine}{ex.StackTrace}",
                            "Log Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
        }
    }
}