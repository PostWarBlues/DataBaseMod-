﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Drawing;
using Ionic.Zip;


namespace NCE.DataBase
{
    public class DataBaseService
    {
        #region Поля

        /// <summary> Путь к файлу БД </summary>
        private static string filePathDB = string.Format(Application.StartupPath + "\\Data\\" + Const.FileNameDB);        
        /// <summary> Путь к архивам БД </summary>
        private static string archiveDir = string.Format(Application.StartupPath + "\\Archive\\");
        /// <summary> Путь директории БД </summary>
        private static string dataBaseDir = filePathDB.Substring(0, filePathDB.LastIndexOf("\\"));
        /// <summary> Подключение БД </summary>
        private static SQLiteConnection connectionDB = new SQLiteConnection(string.Format("Data Source={0};Version=3;", filePathDB));
        
        private TableControl tableControl = new TableControl();
        private ProgressBar progressBar = new ProgressBar();

        #endregion

        public DataBaseService()
        {
            
        }

        public DataBaseService(ProgressBar progressBar)
        {
            this.progressBar = progressBar;
        }

        /// <summary>
        /// Создаёт новую БД
        /// </summary>        
        public bool CreateNewDB()
        {
            try
            {
                if (!Directory.Exists(dataBaseDir))
                    Directory.CreateDirectory(dataBaseDir);

                if (!File.Exists(filePathDB))
                {
                    SQLiteConnection.CreateFile(filePathDB);
                    OpenConnection();
                    CreateTableInfo();
                    CreateTableControl();
                }
                                
                return true;
            }

            catch (Exception ex)
            {
                string error = string.Format(ex.Message + "\nCan't create data base directory '{0}'!", dataBaseDir);
                MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// Созданёт директорию для Архива
        /// </summary>   
        private bool CreateArchiveDir()
        {
            try
            {
                if (!Directory.Exists(archiveDir))
                    Directory.CreateDirectory(archiveDir);               
                return true;
            }

            catch (Exception)
            {                
                return false;
            }
        }

        /// <summary>
        /// Открывает соединение с БД
        /// </summary>        
        private bool OpenConnection()
        {
            try
            {
                connectionDB.Open();
                return true;
            }
            catch (Exception ex)
            {
                string error = string.Format(ex.Message + "\nCan't connect to data base!");
                MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
                throw;
            }
        }
        
        /// <summary>
        /// Закрывает соединение с БД
        /// </summary>  
        private bool CloseConnection()
        {
            try
            {
                connectionDB.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        /// <summary>
        /// Создает таблицу Info
        /// </summary> 
        private bool CreateTableInfo()
        {
            using (SQLiteCommand commandDB = new SQLiteCommand(connectionDB))
            {
                commandDB.CommandText =
                "CREATE TABLE IF NOT EXISTS "
                + "info "
                + "("
                + "id INTEGER PRIMARY KEY AUTOINCREMENT, "
                + "fio TEXT NOT NULL, "
                + "tabelNumb TEXT NOT NULL, "
                + "shift TEXT NOT NULL, "
                + "normDoc TEXT NOT NULL, "
                + "specification TEXT NOT NULL, "
                + "drawing TEXT NOT NULL, "
                + "steelType TEXT NOT NULL, "
                + "sopNumb TEXT NOT NULL, "
                + "lot TEXT NOT NULL, "
                + "lotNumb TEXT NOT NULL, "
                + "heatNumb TEXT NOT NULL, "
                + "bundleNumb TEXT NOT NULL, "
                + "serialNumb TEXT NOT NULL, "
                + "addNumb1 TEXT NOT NULL, "
                + "addNumb2 TEXT NOT NULL, "
                + "addNumb3 TEXT NOT NULL, "
                + "addNumb4 TEXT NOT NULL, "
                + "addNumb5 TEXT NOT NULL, "
                + "objectLength REAL NOT NULL, "
                + "objectWidth REAL NOT NULL, "
                + "objectHeight REAL NOT NULL, "
                + "objectSpeed REAL NOT NULL, "
                + "objectOuterDiameter REAL NOT NULL, "
                + "objectInnerDiameter REAL NOT NULL, "
                + "objectWallThickness REAL NOT NULL, "
                + "objectA REAL NOT NULL, "
                + "objectB REAL NOT NULL, "
                + "objectC REAL NOT NULL, "
                + "objectD REAL NOT NULL, "
                + "objectE REAL NOT NULL, "
                + "objectF REAL NOT NULL, "
                + "objectG REAL NOT NULL, "
                + "objectH REAL NOT NULL, "
                + "objectI REAL NOT NULL, "
                + "objectJ REAL NOT NULL, "
                + "objectK REAL NOT NULL, "
                + "objectL REAL NOT NULL, "
                + "objectM REAL NOT NULL, "
                + "objectN REAL NOT NULL, "
                + "objectO REAL NOT NULL, "
                + "objectP REAL NOT NULL, "
                + "objectQ REAL NOT NULL, "
                + "objectR REAL NOT NULL, "
                + "objectS REAL NOT NULL, "
                + "objectT REAL NOT NULL, "
                + "objectU REAL NOT NULL, "
                + "objectV REAL NOT NULL, "
                + "objectW REAL NOT NULL, "
                + "objectX REAL NOT NULL, "
                + "objectY REAL NOT NULL, "
                + "objectZ REAL NOT NULL, "
                + "usFilesPath TEXT NOT NULL, "
                + "controlResult INT NOT NULL, "
                + "dateTime TEXT NOT NULL"                
                + ")";
                try
                {                    
                    commandDB.CommandType = CommandType.Text;
                    commandDB.ExecuteNonQuery();                    
                    return true;
                }
                catch (Exception ex)
                {                    
                    return false;
                    throw new Exception(ex.Message);
                }
            }
        }

        /// <summary>
        /// Создает таблицу Control
        /// </summary> 
        private bool CreateTableControl()
        {
            using (SQLiteCommand commandDB = new SQLiteCommand(connectionDB))
             {
                    commandDB.CommandText =
                       "CREATE TABLE IF NOT EXISTS "
                       + "control "
                       + "("
                       + "id INTEGER PRIMARY KEY AUTOINCREMENT, "                       
                       + "dataFile BLOB NOT NULL, "
                       + "tableDefects BLOB NOT NULL, "
                       + "controlConfig BLOB NOT NULL, "
                       + "usFiles BLOB NOT NULL"
                       //+ "id_control INT NOT NULL"
                       //+ "FOREIGN KEY (id_control) REFERENCES info(id)"
                       + ")";
                    try
                    {                        
                        commandDB.CommandType = CommandType.Text;
                        commandDB.ExecuteNonQuery();                        
                        return true;
                    }
                    catch (Exception)
                    {                        
                        return false;
                        throw;
                    }
            }
        }

        /// <summary>
        /// DELETE из таблицы Info
        /// </summary>
        /// <param name="id"></param>
        private void DeleteRowFromInfo(long id)
        {
            using (SQLiteCommand commandDB = new SQLiteCommand(connectionDB))
            {
                commandDB.CommandText = ($"DELETE FROM info WHERE ID={id};");

                try
                {
                    commandDB.CommandType = CommandType.Text;
                    commandDB.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// DELETE из таблицы Control
        /// </summary>
        /// <param name="id"></param>
        private void DeleteRowFromControl(long id)
        {
            using (SQLiteCommand commandDB = new SQLiteCommand(connectionDB))
            {
                commandDB.CommandText = ($"DELETE FROM control WHERE ID={id};");

                try
                {
                    commandDB.CommandType = CommandType.Text;
                    commandDB.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    throw;
                }
            }            
        }
        
        /// <summary>
        /// INSERT в таблицу Info
        /// </summary>
        /// <param name="info">Экземпляр TableInfo</param>
        private bool InsertIntoTableInfo(TableInfo info)
        {
            using (SQLiteCommand commandDB = new SQLiteCommand(connectionDB))
            {
                commandDB.CommandText =
                    "INSERT INTO info ("
                    + "fio, "
                    + "tabelNumb, "
                    + "shift, "
                    + "normDoc, "
                    + "specification, "
                    + "steelType, "
                    + "drawing, "
                    + "sopNumb, "
                    + "lot, "
                    + "lotNumb, "
                    + "heatNumb, "
                    + "bundleNumb, "
                    + "serialNumb, "
                    + "addNumb1, "
                    + "addNumb2, "
                    + "addNumb3, "
                    + "addNumb4, "
                    + "addNumb5, "
                    + "objectLength, "
                    + "objectWidth, "
                    + "objectHeight, "
                    + "objectSpeed, "
                    + "objectOuterDiameter, "
                    + "objectInnerDiameter, "
                    + "objectWallThickness, "
                    + "objectA, "
                    + "objectB, "
                    + "objectC, "
                    + "objectD, "
                    + "objectE, "
                    + "objectF, "
                    + "objectG, "
                    + "objectH, "
                    + "objectI, "
                    + "objectJ, "
                    + "objectK, "
                    + "objectL, "
                    + "objectM, "
                    + "objectN, "
                    + "objectO, "
                    + "objectP, "
                    + "objectQ, "
                    + "objectR, "
                    + "objectS, "
                    + "objectT, "
                    + "objectU, "
                    + "objectV, "
                    + "objectW, "
                    + "objectX, "
                    + "objectY, "
                    + "objectZ, "
                    + "usFilesPath, "
                    + "controlResult, "
                    + "dateTime"
                    + ")"
                + " VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                SQLiteParameter fioParam = new SQLiteParameter
                {
                    Value = info.Fio,
                    DbType = DbType.String
                };
                commandDB.Parameters.Add(fioParam);

                SQLiteParameter tabelNumbParam = new SQLiteParameter
                {
                    Value = info.TabelNumb,
                    DbType = DbType.String
                };
                commandDB.Parameters.Add(tabelNumbParam);

                SQLiteParameter shiftParam = new SQLiteParameter
                {
                    Value = info.Shift,
                    DbType = DbType.String
                };
                commandDB.Parameters.Add(shiftParam);

                SQLiteParameter normDocParam = new SQLiteParameter
                {
                    Value = info.NormDoc,
                    DbType = DbType.String
                };
                commandDB.Parameters.Add(normDocParam);

                SQLiteParameter specificationParam = new SQLiteParameter
                {
                    Value = info.Specification,
                    DbType = DbType.String
                };
                commandDB.Parameters.Add(specificationParam);

                SQLiteParameter drawingParam = new SQLiteParameter
                {
                    Value = info.Drawing,
                    DbType = DbType.String
                };
                commandDB.Parameters.Add(drawingParam);

                SQLiteParameter steelTypeParam = new SQLiteParameter
                {
                    Value = info.SteelType,
                    DbType = DbType.String
                };
                commandDB.Parameters.Add(steelTypeParam);

                SQLiteParameter sopNumbParam = new SQLiteParameter
                {
                    Value = info.SopNumb,
                    DbType = DbType.String
                };
                commandDB.Parameters.Add(sopNumbParam);

                SQLiteParameter lotParam = new SQLiteParameter
                {
                    Value = info.Lot,
                    DbType = DbType.String
                };
                commandDB.Parameters.Add(lotParam);

                SQLiteParameter lotNumbParam = new SQLiteParameter
                {
                    Value = info.LotNumb,
                    DbType = DbType.String
                };
                commandDB.Parameters.Add(lotNumbParam);

                SQLiteParameter heatNumbParam = new SQLiteParameter
                {
                    Value = info.HeatNumb,
                    DbType = DbType.String
                };
                commandDB.Parameters.Add(heatNumbParam);

                SQLiteParameter bundleNumbParam = new SQLiteParameter
                {
                    Value = info.BundleNumb,
                    DbType = DbType.String
                };
                commandDB.Parameters.Add(bundleNumbParam);

                SQLiteParameter serialNumbParam = new SQLiteParameter
                {
                    Value = info.SerialNumb,
                    DbType = DbType.String
                };
                commandDB.Parameters.Add(serialNumbParam);
                
                SQLiteParameter addNumb1Param = new SQLiteParameter
                {
                    Value = info.AddNumb1,
                    DbType = DbType.String
                };
                commandDB.Parameters.Add(addNumb1Param);

                SQLiteParameter addNumb2Param = new SQLiteParameter
                {
                    Value = info.AddNumb1,
                    DbType = DbType.String
                };
                commandDB.Parameters.Add(addNumb2Param);

                SQLiteParameter addNumb3Param = new SQLiteParameter
                {
                    Value = info.AddNumb1,
                    DbType = DbType.String
                };
                commandDB.Parameters.Add(addNumb3Param);

                SQLiteParameter addNumb4Param = new SQLiteParameter
                {
                    Value = info.AddNumb1,
                    DbType = DbType.String
                };
                commandDB.Parameters.Add(addNumb4Param);

                SQLiteParameter addNumb5Param = new SQLiteParameter
                {
                    Value = info.AddNumb1,
                    DbType = DbType.String
                };
                commandDB.Parameters.Add(addNumb5Param);
                
                SQLiteParameter objectLengthParam = new SQLiteParameter
                {
                    Value = info.ObjectLength,
                    DbType = DbType.Double
                };
                commandDB.Parameters.Add(objectLengthParam);

                SQLiteParameter objectWidthParam = new SQLiteParameter
                {
                    Value = info.ObjectWidth,
                    DbType = DbType.Double
                };
                commandDB.Parameters.Add(objectWidthParam);

                SQLiteParameter objectHeightParam = new SQLiteParameter
                {
                    Value = info.ObjectHeight,
                    DbType = DbType.Double
                };
                commandDB.Parameters.Add(objectHeightParam);

                SQLiteParameter objectSpeedParam = new SQLiteParameter
                {
                    Value = info.ObjectSpeed,
                    DbType = DbType.Double
                };
                commandDB.Parameters.Add(objectSpeedParam);
                
                SQLiteParameter objectOuterDiameterParam = new SQLiteParameter
                {
                    Value = info.ObjectOuterDiameter,
                    DbType = DbType.Double
                };
                commandDB.Parameters.Add(objectOuterDiameterParam);

                SQLiteParameter objectInnerDiameterParam = new SQLiteParameter
                {
                    Value = info.ObjectInnerDiameter,
                    DbType = DbType.Double
                };
                commandDB.Parameters.Add(objectInnerDiameterParam);

                SQLiteParameter objectWallThicknessParam = new SQLiteParameter
                {
                    Value = info.ObjectWallThickness,
                    DbType = DbType.Double
                };
                commandDB.Parameters.Add(objectWallThicknessParam);
                
                SQLiteParameter objectAParam = new SQLiteParameter
                {
                    Value = info.ObjectA,
                    DbType = DbType.Double
                };
                commandDB.Parameters.Add(objectAParam);

                SQLiteParameter objectBParam = new SQLiteParameter
                {
                    Value = info.ObjectB,
                    DbType = DbType.Double
                };
                commandDB.Parameters.Add(objectBParam);

                SQLiteParameter objectCParam = new SQLiteParameter
                {
                    Value = info.ObjectC,
                    DbType = DbType.Double
                };
                commandDB.Parameters.Add(objectCParam);

                SQLiteParameter objectDParam = new SQLiteParameter
                {
                    Value = info.ObjectD,
                    DbType = DbType.Double
                };
                commandDB.Parameters.Add(objectDParam);

                SQLiteParameter objectEParam = new SQLiteParameter
                {
                    Value = info.ObjectE,
                    DbType = DbType.Double
                };
                commandDB.Parameters.Add(objectEParam);

                SQLiteParameter objectFParam = new SQLiteParameter
                {
                    Value = info.ObjectF,
                    DbType = DbType.Double
                };
                commandDB.Parameters.Add(objectFParam);

                SQLiteParameter objectGParam = new SQLiteParameter
                {
                    Value = info.ObjectG,
                    DbType = DbType.Double
                };
                commandDB.Parameters.Add(objectGParam);

                SQLiteParameter objectHParam = new SQLiteParameter
                {
                    Value = info.ObjectH,
                    DbType = DbType.Double
                };
                commandDB.Parameters.Add(objectHParam);

                SQLiteParameter objectIParam = new SQLiteParameter
                {
                    Value = info.ObjectI,
                    DbType = DbType.Double
                };
                commandDB.Parameters.Add(objectIParam);

                SQLiteParameter objectJParam = new SQLiteParameter
                {
                    Value = info.ObjectJ,
                    DbType = DbType.Double
                };
                commandDB.Parameters.Add(objectJParam);

                SQLiteParameter objectKParam = new SQLiteParameter
                {
                    Value = info.ObjectK,
                    DbType = DbType.Double
                };
                commandDB.Parameters.Add(objectKParam);

                SQLiteParameter objectLParam = new SQLiteParameter
                {
                    Value = info.ObjectL,
                    DbType = DbType.Double
                };
                commandDB.Parameters.Add(objectLParam);

                SQLiteParameter objectMParam = new SQLiteParameter
                {
                    Value = info.ObjectM,
                    DbType = DbType.Double
                };
                commandDB.Parameters.Add(objectMParam);

                SQLiteParameter objectNParam = new SQLiteParameter
                {
                    Value = info.ObjectN,
                    DbType = DbType.Double
                };
                commandDB.Parameters.Add(objectNParam);

                SQLiteParameter objectOParam = new SQLiteParameter
                {
                    Value = info.ObjectO,
                    DbType = DbType.Double
                };
                commandDB.Parameters.Add(objectOParam);

                SQLiteParameter objectPParam = new SQLiteParameter
                {
                    Value = info.ObjectP,
                    DbType = DbType.Double
                };
                commandDB.Parameters.Add(objectPParam);

                SQLiteParameter objectQParam = new SQLiteParameter
                {
                    Value = info.ObjectQ,
                    DbType = DbType.Double
                };
                commandDB.Parameters.Add(objectQParam);

                SQLiteParameter objectRParam = new SQLiteParameter
                {
                    Value = info.ObjectR,
                    DbType = DbType.Double
                };
                commandDB.Parameters.Add(objectRParam);

                SQLiteParameter objectSParam = new SQLiteParameter
                {
                    Value = info.ObjectS,
                    DbType = DbType.Double
                };
                commandDB.Parameters.Add(objectSParam);

                SQLiteParameter objectTParam = new SQLiteParameter
                {
                    Value = info.ObjectT,
                    DbType = DbType.Double
                };
                commandDB.Parameters.Add(objectTParam);

                SQLiteParameter objectUParam = new SQLiteParameter
                {
                    Value = info.ObjectU,
                    DbType = DbType.Double
                };
                commandDB.Parameters.Add(objectUParam);

                SQLiteParameter objectVParam = new SQLiteParameter
                {
                    Value = info.ObjectV,
                    DbType = DbType.Double
                };
                commandDB.Parameters.Add(objectVParam);

                SQLiteParameter objectWParam = new SQLiteParameter
                {
                    Value = info.ObjectW,
                    DbType = DbType.Double
                };
                commandDB.Parameters.Add(objectWParam);

                SQLiteParameter objectXParam = new SQLiteParameter
                {
                    Value = info.ObjectX,
                    DbType = DbType.Double
                };
                commandDB.Parameters.Add(objectXParam);

                SQLiteParameter objectYParam = new SQLiteParameter
                {
                    Value = info.ObjectY,
                    DbType = DbType.Double
                };
                commandDB.Parameters.Add(objectYParam);

                SQLiteParameter objectZParam = new SQLiteParameter
                {
                    Value = info.ObjectZ,
                    DbType = DbType.Double
                };
                commandDB.Parameters.Add(objectZParam);
                
                SQLiteParameter usFilesPathParam = new SQLiteParameter
                {
                    Value = info.UsFilesPath,
                    DbType = DbType.String
                };
                commandDB.Parameters.Add(usFilesPathParam);

                SQLiteParameter controlResultParam = new SQLiteParameter
                {
                    Value = info.ControlResult,
                    DbType = DbType.Byte
                };
                commandDB.Parameters.Add(controlResultParam);

                SQLiteParameter dataTimeParam = new SQLiteParameter
                {
                    Value = info.DateTimeStump,
                    DbType = DbType.DateTime
                };
                commandDB.Parameters.Add(dataTimeParam);

                try
                {                    
                    commandDB.CommandType = CommandType.Text;
                    commandDB.ExecuteNonQuery();                    
                    return true;
                }
                catch (Exception ex)
                {                    
                    return false;
                    throw new Exception(ex.Message);
                }
            }
        }

        /// <summary>
        /// INSERT таблицу Control
        /// </summary>
        /// <param name="control">Экземпляр TableControl</param>
        private bool InsertIntoTableControl(TableControl control)
        {
            using (SQLiteCommand commandDB = new SQLiteCommand(connectionDB))
            {
                commandDB.CommandText =
                    "INSERT INTO control ("
                    + "dataFile, "
                    + "tableDefects, "
                    + "controlConfig, "
                    + "usFiles"
                    + ")"
                + " VALUES(?,?,?,?)";

                SQLiteParameter dataFileParam = new SQLiteParameter
                {
                    Value = control.DataFile,
                    DbType = DbType.Binary
                };
                commandDB.Parameters.Add(dataFileParam);

                SQLiteParameter defectsTableParam = new SQLiteParameter
                {
                    Value = control.TableDefects,
                    DbType = DbType.String
                };
                commandDB.Parameters.Add(defectsTableParam);

                SQLiteParameter controlConfigParam = new SQLiteParameter
                {
                    Value = control.ControlConfig,
                    DbType = DbType.Binary
                };
                commandDB.Parameters.Add(controlConfigParam);

                SQLiteParameter usFilesParam = new SQLiteParameter
                {
                    Value = control.UsFiles,
                    DbType = DbType.Binary
                };
                commandDB.Parameters.Add(usFilesParam);

                try
                {                    
                    commandDB.CommandType = CommandType.Text;
                    commandDB.ExecuteNonQuery();                    
                    return true;
                }
                catch (Exception ex)
                {                    
                    return false;
                    throw new Exception(ex.Message);
                }
            }
        }
        
        /// <summary>
        /// INSERT-транзакция для двух таблиц
        /// </summary>
        /// <param name="info">Экземпляр TableInfo</param>
        /// <param name="control">Экземпляр TableControl</param>
        public bool InsertTransaction(TableInfo info, TableControl control)
        {
            using (SQLiteCommand commandDB = new SQLiteCommand(connectionDB))
            {                
                SQLiteTransaction transaction;

                OpenConnection();

                transaction = connectionDB.BeginTransaction();                
                commandDB.Connection = connectionDB;
                commandDB.Transaction = transaction;

                try
                {                    
                    InsertIntoTableInfo(info);
                    InsertIntoTableControl(control);                    
                    transaction.Commit();                    
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();                    
                    return false;
                    throw new Exception(ex.Message);
                }
                finally
                {
                    CloseConnection();
                }
            }
        }

        /// <summary>
        /// Удаляет файл БД
        /// </summary>
        /// <returns></returns>
        public bool DeleteDateBaseFile()
        {
            try
            {
                File.Delete(filePathDB);
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// Удаляет строку в БД
        /// </summary>
        /// <param name="id">ID строки</param>
        public bool DeleteTransaction(long id)
        {
            using (SQLiteCommand commandDB = new SQLiteCommand(connectionDB))
            {
                SQLiteTransaction transaction;

                OpenConnection();

                transaction = connectionDB.BeginTransaction();
                commandDB.Connection = connectionDB;
                commandDB.Transaction = transaction;

                try
                {
                    DeleteRowFromInfo(id);
                    DeleteRowFromControl(id);
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                    throw new Exception(ex.Message);
                }
                finally
                {
                    CloseConnection();
                }
            }
        }

        /// <summary>
        /// Возвращает заполненный экземпляр TableControl
        /// </summary>
        /// <param name="pathTableDefects">Путь к файлу таблицы дефектов</param>
        /// <param name="dataFile">Путь к файлу данных контроля</param>
        /// <param name="configFile">Путь к файлу конфигурации контроля</param>
        public TableControl FillTableControl(string pathTableDefects, string dataFile, string configFile)
        {            
            byte[] dataArr;
            byte[] configArr;
            byte[] usFilesArr = { 000, 00, 000 };

            using (FileStream streamConfigFile = File.OpenRead(configFile))
            {
                configArr = new byte[streamConfigFile.Length];
                streamConfigFile.Read(configArr, 0, configArr.Length);
            }

            using (FileStream streamDataFile = new FileStream(dataFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                dataArr = new byte[streamDataFile.Length];
                streamDataFile.Read(dataArr, 0, dataArr.Length);
            }
                        
            tableControl.DataFile = dataArr;
            tableControl.ControlConfig = configArr;
            tableControl.TableDefects = pathTableDefects;
            tableControl.UsFiles = usFilesArr;

            return tableControl;
        }    

        /// <summary>
        /// Выгрузка данных из таблицы Info для DataGridView
        /// </summary>
        /// <returns>Список строк в БД</returns>
        public List<TableInfo> SelectAllFromTableInfo()
        {
            List<TableInfo> list = new List<TableInfo>();
            try
            {
                OpenConnection();
                using (SQLiteConnection sqlConn = new SQLiteConnection(connectionDB))
                using (SQLiteCommand cmd = new SQLiteCommand(sqlConn))
                {
                    cmd.CommandText = "SELECT * FROM info";
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                            while (reader.Read())
                            {
                                TableInfo item = new TableInfo();
                                item.ID = reader.GetInt64(0);
                                item.Fio = reader.GetString(1);
                                item.TabelNumb = reader.GetString(2);
                                item.Shift = reader.GetString(3);
                                item.NormDoc = reader.GetString(4);
                                item.Specification = reader.GetString(5);
                                item.Drawing = reader.GetString(6);
                                item.SteelType = reader.GetString(7);
                                item.SopNumb = reader.GetString(8);
                                item.Lot = reader.GetString(9);
                                item.LotNumb = reader.GetString(10);
                                item.HeatNumb = reader.GetString(11);
                                item.BundleNumb = reader.GetString(12);
                                item.SerialNumb = reader.GetString(13);
                                item.AddNumb1 = reader.GetString(14);
                                item.AddNumb2 = reader.GetString(15);
                                item.AddNumb3 = reader.GetString(16);
                                item.AddNumb4 = reader.GetString(17);
                                item.AddNumb5 = reader.GetString(18);
                                item.ObjectLength = reader.GetDouble(19);
                                item.ObjectWidth = reader.GetDouble(20);
                                item.ObjectHeight = reader.GetDouble(21);
                                item.ObjectSpeed = reader.GetDouble(22);
                                item.ObjectOuterDiameter = reader.GetDouble(23);
                                item.ObjectInnerDiameter = reader.GetDouble(24);
                                item.ObjectWallThickness = reader.GetDouble(25);
                                item.ObjectA = reader.GetDouble(26);
                                item.ObjectB = reader.GetDouble(27);
                                item.ObjectC = reader.GetDouble(28);
                                item.ObjectD = reader.GetDouble(29);
                                item.ObjectE = reader.GetDouble(30);
                                item.ObjectF = reader.GetDouble(31);
                                item.ObjectG = reader.GetDouble(32);
                                item.ObjectH = reader.GetDouble(33);
                                item.ObjectI = reader.GetDouble(34);
                                item.ObjectJ = reader.GetDouble(35);
                                item.ObjectK = reader.GetDouble(36);
                                item.ObjectL = reader.GetDouble(37);
                                item.ObjectM = reader.GetDouble(38);
                                item.ObjectN = reader.GetDouble(39);
                                item.ObjectO = reader.GetDouble(40);
                                item.ObjectP = reader.GetDouble(41);
                                item.ObjectQ = reader.GetDouble(42);
                                item.ObjectR = reader.GetDouble(43);
                                item.ObjectS = reader.GetDouble(44);
                                item.ObjectT = reader.GetDouble(45);
                                item.ObjectU = reader.GetDouble(46);
                                item.ObjectV = reader.GetDouble(47);
                                item.ObjectW = reader.GetDouble(48);
                                item.ObjectX = reader.GetDouble(49);
                                item.ObjectY = reader.GetDouble(50);
                                item.ObjectZ = reader.GetDouble(51);
                                item.UsFilesPath = reader.GetString(52);
                                item.ControlResult = reader.GetByte(53);
                                item.DateTimeStump = reader.GetDateTime(54);
                                list.Add(item);
                            };
                    }
                }
                return list;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// Выгружает файлы контроля и конфига по заданому ID
        /// </summary>
        /// <param name="id">ID записи в БД</param>
        /// <param name="streamDataControl">MemoryStream Контроля</param>
        /// <param name="streamDataConfig">MemoryStream Конфигурации</param>
        public void SelectSingleId(long id, out MemoryStream streamDataControl, out MemoryStream streamDataConfig)
        {
            byte[] _dataControlArray;
            byte[] _dataConfigArray;
            string _tableDefects;
            try
            {
                OpenConnection();
                using (SQLiteConnection sqlConn = new SQLiteConnection(connectionDB))
                using (SQLiteCommand cmd = new SQLiteCommand(sqlConn))
                {
                    cmd.CommandText = ("SELECT * FROM control WHERE id=" + id);
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            _dataControlArray = (byte[])reader[1];
                            _tableDefects = reader.GetString(2);
                            _dataConfigArray = (byte[])reader[3];
                            streamDataControl = new MemoryStream(_dataControlArray);
                            streamDataConfig = new MemoryStream(_dataConfigArray);
                        } 
                    }                    
                }
            }
            
            catch (Exception)
            {
                throw;
            }

            finally
            {
                CloseConnection();
            }
        }        

        /// <summary>
        /// Возвращает временной штамп первой и последней записи
        /// </summary>
        /// <param name="firstRowDT">DateTime первой записи</param>
        /// <param name="lastRowDT">DateTime последней записи</param>
        public void SelectDatePeriod(out DateTime firstRowDT, out DateTime lastRowDT)
        {            
            try
            {
                OpenConnection();
                using (SQLiteConnection sqlConn = new SQLiteConnection(connectionDB))
                using (SQLiteCommand cmd = new SQLiteCommand(sqlConn))
                {
                    cmd.CommandText = ("SELECT MIN(dateTime) AS First, " +
                                              "MAX(dateTime) AS Last FROM info;");
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            firstRowDT = reader.GetDateTime(0);
                            lastRowDT = reader.GetDateTime(1);
                        }
                    }
                }
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// Возвращает общее количество записей в таблице Info
        /// </summary>
        public int SelectCountFromTable()
        {
            try
            {
                OpenConnection();
                using (SQLiteConnection sqlConn = new SQLiteConnection(connectionDB))
                using (SQLiteCommand cmd = new SQLiteCommand(sqlConn))
                {
                    cmd.CommandText = ("SELECT COUNT(*) FROM info");
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            return reader.GetInt32(0);
                        }
                    }
                }
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                CloseConnection();
            }            
        }

        /// <summary>
        /// Проверяет налиичие записей в таблице Info
        /// </summary>
        /// <returns>Вернет true если таблица пустая</returns>
        public bool CheckIsTableEmpty()
        {
            try
            {
                OpenConnection();
                using (SQLiteConnection sqlConn = new SQLiteConnection(connectionDB))
                using (SQLiteCommand cmd = new SQLiteCommand(sqlConn))
                {
                    cmd.CommandText = ("SELECT COUNT(*) FROM info");
                    int result = int.Parse(cmd.ExecuteScalar().ToString());

                    if (result == 0)
                        return true;
                    else
                        return false;
                }
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// Проверяет размер файла БД
        /// </summary>
        /// <param name="critSize">Допустимый размер БД в ГБ</param>
        /// <param name="fileSize">Фактический размер БД, в строковом значении</param>
        /// <param name="sizeColor">Цвет по превышению допустимого размера становится красным</param>
        public void СheckDataBaseSize(double critSize, out string fileSize, out Color sizeColor)
        {
            long size = GetDataBaseFileSize();
            string[] suffix = { "B", "KB", "MB", "GB", "TB" };

            int i;
            double dBytes = size;
            for (i = 0; i < suffix.Length && size >= 1024; i++, size /= 1024)
            {
                dBytes = size / 1024.0;
            }

            fileSize = String.Format("{0:0.#} {1}", dBytes, suffix[i]);

            if (dBytes >= critSize && suffix[i] == "GB") sizeColor = Color.DarkRed;
            else sizeColor = Color.Black;
        }

        /// <summary>
        /// Проверяет размер файла БД
        /// </summary>
        /// <param name="critSize">Допустимый размер БД в ГБ</param>
        /// <returns>Лимит достигнут true</returns>
        public bool СheckDataBaseSize(double critSize)
        {
            long size = GetDataBaseFileSize();
            double gb = (double)size / (1024 * 1024 * 1024);
            if (gb >= critSize) return true;
            else return false;
        }

        /// <summary>
        /// Возвращает дату создания и модификации файла БД
        /// </summary>
        /// <param name="creation">Дата создания</param>
        /// <param name="modification">Дата модификации</param>
        public void GetDataBaseFileInfo(out DateTime creation, out DateTime modification)
        {
            creation = File.GetCreationTime(filePathDB);
            modification = File.GetLastWriteTime(filePathDB);            
        }                

        /// <summary>
        /// Архивация файла БД
        /// </summary>
        /// <returns>Успех операции</returns>
        public bool ZipDataBase()
        {
            CreateArchiveDir();
            SelectDatePeriod(out DateTime firstRow, out DateTime secondRow);
            string created = String.Format("{0:dd.MM.yyyy}", firstRow);
            string modified = String.Format("{0:dd.MM.yyyy}", secondRow);
            string archiveFileName = archiveDir + "UTDB" + " " + created + " - " + modified + ".zip";
            try
            {
                using (ZipFile zip = new ZipFile())
                {                    
                    zip.AddFile(filePathDB, "");
                    zip.SaveProgress += ZipProgress;
                    zip.Save(archiveFileName);
                    return true;
                }                          
            }
            catch (Exception)
            {
                return false;                
            }            
        }

        private void ZipProgress(object sender, SaveProgressEventArgs e)
        {
            if(e.EventType == ZipProgressEventType.Saving_EntryBytesRead)
            {
                progressBar.Invoke(new MethodInvoker(delegate
                {
                    progressBar.Maximum = 100;                    
                    progressBar.Value = (int)((e.BytesTransferred * 100) / (e.TotalBytesToTransfer));                    
                }
                ));
            }
        }

        private long GetDataBaseFileSize()
        {
            long size;
            return size = new FileInfo(filePathDB).Length;
        }

    }
}
