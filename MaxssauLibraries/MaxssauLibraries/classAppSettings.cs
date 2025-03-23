using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Xml;
using System.Xml.Serialization;
using image_designer;

namespace MaxssauLibraries
{

    public class SettingsManager
    {
        // Список для хранения настроек
        public List<Setting> Settings { get; set; } = new List<Setting>();

        // Путь к файлу
        private string filePath;

        private classLogger Logger=null;

        public OperationStatus Status;

        // Конструктор
        public SettingsManager(string filePath, ref classLogger logger)
        {
            Logger = logger;
            this.filePath = filePath;
            try
            {
                LoadSettings();
                Status = OperationStatus.STATUS_OK;
            }
            catch (Exception ex)
            {
                if (Logger != null)
                {
                    Logger.add_to_log("Settings: conversion", ex.Message);
                    if (ex.StackTrace != null)
                    {
                        Logger.add_to_log("Settings: conversion", ex.StackTrace);
                    }
                }
                Status = OperationStatus.STATUS_FAIL;
            }
        }

        // Метод для добавления новой настройки
        public void AddSetting(string key, string value)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                    throw new ArgumentException("Key cannot be null or empty.");

                // Проверяем, существует ли уже настройка с таким ключом
                if (Settings.Exists(s => s.Key == key))
                {
                    throw new InvalidOperationException($"Setting with key '{key}' already exists.");
                }

                Settings.Add(new Setting { Key = key, Value = value });
                Status = OperationStatus.STATUS_OK;
            }
            catch (Exception ex)
            {
                if (Logger != null)
                {
                    Logger.add_to_log("Settings: conversion", ex.Message);
                    if (ex.StackTrace != null)
                    {
                        Logger.add_to_log("Settings: conversion", ex.StackTrace);
                    }
                }
                Status = OperationStatus.STATUS_FAIL;
            }
        }

        // Метод для удаления настройки по ключу
        public void RemoveSetting(string key)
        {
            try
            {
                var setting = Settings.Find(s => s.Key == key);
                if (setting != null)
                {
                    Settings.Remove(setting);
                }
                else
                {
                    throw new KeyNotFoundException($"Setting with key '{key}' not found.");
                }
                Status = OperationStatus.STATUS_OK;
            }
            catch (Exception ex)
            {
                if (Logger != null)
                {
                    Logger.add_to_log("Settings: conversion", ex.Message);
                    if (ex.StackTrace != null)
                    {
                        Logger.add_to_log("Settings: conversion", ex.StackTrace);
                    }
                }
                Status = OperationStatus.STATUS_FAIL;
            }
        }

        // Метод для получения значения настройки по ключу
        public string GetSetting(string key)
        {
            try
            { 
                var setting = Settings.Find(s => s.Key == key);
                if (setting != null)
                {
                    Status = OperationStatus.STATUS_OK;
                    return setting.Value;
                }
                Status = OperationStatus.STATUS_FAIL;
                throw new KeyNotFoundException($"Setting with key '{key}' not found.");

            }
            catch (Exception ex)
            {
                if (Logger != null)
                {
                    Logger.add_to_log("Settings: conversion", ex.Message);
                    if (ex.StackTrace != null)
                    {
                        Logger.add_to_log("Settings: conversion", ex.StackTrace);
                    }
                }
                Status = OperationStatus.STATUS_FAIL;
                return "";
            }
        }

        // Метод для обновления значения настройки по ключу
        public void UpdateSetting(string key, string newValue, bool createIfNotExists = false)
        {
            try
            {
                var setting = Settings.Find(s => s.Key == key);
                if (setting != null)
                {
                    setting.Value = newValue; // Обновляем значение
                }
                else if (createIfNotExists)
                {
                    // Если ключ не найден и разрешено создание новой настройки
                    AddSetting(key, newValue);
                }
                else
                {
                    Status = OperationStatus.STATUS_FAIL;
                    throw new KeyNotFoundException($"Setting with key '{key}' not found and creation is not allowed.");
                }
                Status = OperationStatus.STATUS_OK;
            }
            catch (Exception ex)
            {
                if (Logger != null)
                {
                    Logger.add_to_log("Settings: conversion", ex.Message);
                    if (ex.StackTrace != null)
                    {
                        Logger.add_to_log("Settings: conversion", ex.StackTrace);
                    }
                }
                Status = OperationStatus.STATUS_FAIL;
            }
        }

        // Метод для сохранения настроек в XML-файл
        public void SaveSettings()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Setting>));
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    serializer.Serialize(writer, Settings);
                }
                Status = OperationStatus.STATUS_OK;
            }
            catch (Exception ex)
            {
                if (Logger != null)
                {
                    Logger.add_to_log("Settings: conversion", ex.Message);
                    if (ex.StackTrace != null)
                    {
                        Logger.add_to_log("Settings: conversion", ex.StackTrace);
                    }
                }
                Status = OperationStatus.STATUS_FAIL;
            }
        }

        // Метод для загрузки настроек из XML-файла
        public void LoadSettings()
        {
            try
            { 
                if (File.Exists(filePath))
                {
                    try
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<Setting>));
                        using (StreamReader reader = new StreamReader(filePath))
                        {
                            Settings = (List<Setting>)serializer.Deserialize(reader);
                        }
                    }
                    catch (Exception ex)
                    {
                        Status = OperationStatus.STATUS_FAIL;
                        throw new IOException("Failed to load settings.", ex);
                    }
                }
                else
                {
                    Settings = new List<Setting>(); // Если файл не существует, создаем пустой список
                }
                Status = OperationStatus.STATUS_OK;
            }
            catch (Exception ex)
            {
                if (Logger != null)
                {
                    Logger.add_to_log("Settings: conversion", ex.Message);
                    if (ex.StackTrace != null)
                    {
                        Logger.add_to_log("Settings: conversion", ex.StackTrace);
                    }
                }
                Status = OperationStatus.STATUS_FAIL;
            }
        }
    }

    // Класс для представления одной настройки
    [Serializable]
    public class Setting
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}