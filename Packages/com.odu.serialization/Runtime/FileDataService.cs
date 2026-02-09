using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace OduLib.Canivete.Serialization {
    /// <summary>
    /// Serviço genérico para serialização e deserialização de dados em arquivos de um projeto Unity.
    /// Oferece funcionalidades de salvar, carregar e deletar dados com suporte a criptografia e backup automático.
    /// </summary>
    /// <typeparam name="Data">O tipo de dados a ser serializado. Deve ser uma classe.</typeparam>
    public class FileDataService<Data> where Data : class
    {
        private string _dataDirPath = "";
        private string _dataFileName = "";
        
        private string _dataFileExtension = "";
        private readonly string _encryptionCodeWord = "P,A*M+0U@,G5U-m-=h/nS}+Y@3Ln$}{JP.z1dcg:JPCu3#GxMM";
        
        private readonly string _backupExtension = ".bak";
        
        /// <summary>
        /// Inicializa uma nova instância do serviço de dados de arquivo.
        /// </summary>
        /// <param name="dataDirPath">O caminho do diretório onde os dados serão armazenados.</param>
        /// <param name="dataFileName">O nome do arquivo de dados (sem extensão).</param>
        /// <param name="dataFileExtension">A extensão do arquivo de dados (ex: .json).</param>
        /// <param name="backupExtension">A extensão usada para arquivos de backup.</param>
        /// <param name="encryptionCodeWord">A chave de criptografia a ser usada. Se for null, será usada a chave padrão.</param>
        public FileDataService(string dataDirPath, string dataFileName, string dataFileExtension, string backupExtension = ".bak", string encryptionCodeWord = null){
            _dataDirPath = dataDirPath;
            _dataFileName = dataFileName;
            _dataFileExtension = dataFileExtension;
            _backupExtension = backupExtension;
            if(encryptionCodeWord != null){
                _encryptionCodeWord = encryptionCodeWord;
            }
        }

        /// <summary>
        /// Obtém o caminho completo do arquivo de dados para um perfil específico.
        /// </summary>
        /// <param name="profileID">O identificador do perfil.</param>
        /// <param name="fileName">O nome do arquivo.</param>
        /// <returns>O caminho completo do arquivo.</returns>
        public string GetPathToFile(string profileID, string fileName){
            return Path.Combine(_dataDirPath, profileID, string.Concat(fileName,_dataFileExtension));
        }

        /// <summary>
        /// Carrega dados do arquivo para um perfil específico.
        /// </summary>
        /// <param name="profileID">O identificador do perfil.</param>
        /// <param name="useEncryption">Se verdadeiro, os dados serão descriptografados ao carregar.</param>
        /// <param name="allowRestoreFromBackup">Se verdadeiro, tentará restaurar do arquivo de backup em caso de erro.</param>
        /// <returns>Os dados carregados ou null se o perfil não existir.</returns>
        public Data Load(string profileID, bool useEncryption, bool allowRestoreFromBackup = true){
            if(profileID == null){
                return null;
            }

            string fullPath = GetPathToFile(profileID, _dataFileName);

            Data loadedData = null;

            if(File.Exists(fullPath)){
                try 
                {
                    // load the serialized data from the file
                    string dataToLoad = ReadFromFile(fullPath);

                    if(useEncryption){
                        dataToLoad = EncryptDecrypt(dataToLoad);
                    }

                    // deserialize the data from Json back into the C# object
                    loadedData = JsonUtility.FromJson<Data>(dataToLoad);
                }
                catch (Exception e) 
                {
                    if(allowRestoreFromBackup){
                        Debug.LogWarning("Carregamento de dados falhou. Tentando retornar para um arquivo de backup. \n" + e);
                        bool rollbackSuccess = AttemptRollBack(fullPath);
                        if(rollbackSuccess){
                            loadedData = Load(profileID, useEncryption, false);
                        }
                        
                    } else {
                        Debug.LogError("Erro ocorreu ao tentar carregar dados do arquivo: " + fullPath + " e backup não funcionou.\n" + e);
                    }
                    
                }
            }
            return loadedData;
        }

        /// <summary>
        /// Lê o conteúdo de um arquivo como string.
        /// </summary>
        /// <param name="fullPath">O caminho completo do arquivo a ser lido.</param>
        /// <returns>O conteúdo do arquivo como string.</returns>
        string ReadFromFile(string fullPath){
            string dataToLoad = "";
            using (FileStream stream = new FileStream(fullPath, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    dataToLoad = reader.ReadToEnd();
                }
            }
            return dataToLoad;
        }

        /// <summary>
        /// Salva os dados em arquivo para um perfil específico com verificação e backup automático.
        /// </summary>
        /// <param name="profileID">O identificador do perfil.</param>
        /// <param name="data">Os dados a serem salvos.</param>
        /// <param name="useEncryption">Se verdadeiro, os dados serão criptografados antes de salvar.</param>
        /// <returns>Verdadeiro se o salvamento foi bem-sucedido, falso caso contrário.</returns>
        public bool Save(string profileID, Data data, bool useEncryption){
            if(profileID == null){
                return false;
            }

            string fullPath = GetPathToFile(profileID, _dataFileName);
            string backupFilePath = string.Concat(fullPath,_backupExtension);

            try 
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                string dataToStore = JsonUtility.ToJson(data, true);

                if(useEncryption){
                    dataToStore = EncryptDecrypt(dataToStore);
                }


                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream)) 
                    {
                        writer.Write(dataToStore);
                    }
                }

                //Verificar se save é válido
                Data verifiedData = Load(profileID, useEncryption);
                if(verifiedData != null){
                    File.Copy(fullPath, backupFilePath, true);
                } else {
                    throw new FileLoadException("Arquivo de dados não pôde ser verificado e backup não pôde ser criado.");
                }

                return true;
            }
            catch (Exception e) 
            {
                Debug.LogError("Erro ocorreu ao tentar salvar dados no arquivo: " + fullPath + "\n" + e);
                return false;
            }
        }

        /// <summary>
        /// Deleta todos os dados associados a um perfil específico.
        /// </summary>
        /// <param name="profileID">O identificador do perfil a ser deletado.</param>
        public void Delete(string profileID){
            if (profileID == null){
                return;
            }

            string fullPath = GetPathToFile(profileID, _dataFileName);
            try 
            {
                if (File.Exists(fullPath)) 
                {
                    Directory.Delete(Path.GetDirectoryName(fullPath), true);
                }
                else 
                {
                    Debug.LogWarning("Tentou deletar dados, mas nada foi encontrado no diretório: " + fullPath);
                }
            }
            catch (Exception e) 
            {
                Debug.LogError("Falhou em deletar dados do perfil para ProfileID: " 
                    + profileID + " no diretório: " + fullPath + "\n" + e);
            }
        }

        /// <summary>
        /// Carrega todos os perfis disponíveis no diretório de dados.
        /// </summary>
        /// <param name="useEncryption">Se verdadeiro, os dados serão descriptografados ao carregar.</param>
        /// <returns>Um dicionário contendo os IDs dos perfis e seus dados correspondentes.</returns>
        public Dictionary<string, Data> GetProfiles(bool useEncryption){
            Dictionary<string, Data> profileDictionary = new Dictionary<string, Data>();

            if (!Directory.Exists(_dataDirPath))
            {
                Debug.LogWarning($"Diretório {_dataDirPath} ainda não existe. Impossível carregar perfis.");
                return profileDictionary;
            }
            
            IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(_dataDirPath).EnumerateDirectories();
            foreach (DirectoryInfo dirInfo in dirInfos) 
            {
                string profileID = dirInfo.Name;

                string fullPath = GetPathToFile(profileID, _dataFileName);
                if (!File.Exists(fullPath))
                {
                    Debug.LogWarning("Ignorando diretório ao carregar todos os perfis pois este não contém dados: "
                        + profileID);
                    continue;
                }

                Data profileData = Load(profileID, useEncryption);
                if (profileData != null) 
                {
                    profileDictionary.Add(profileID, profileData);
                }
                else 
                {
                    Debug.LogError("Tentou carregar o perfil mas algo deu errado. ProfileId: " + profileID);
                }
            }

            return profileDictionary;
        }

        /// <summary>
        /// Criptografa ou descriptografa uma string usando a chave de criptografia padrão.
        /// </summary>
        /// <param name="data">Os dados a serem criptografados ou descriptografados.</param>
        /// <returns>Os dados após a operação de criptografia/descriptografia.</returns>
        private string EncryptDecrypt(string data){
            string modifiedData = "";
            for(int i = 0; i < data.Length; i++){
                modifiedData += (char) (data[i] ^ _encryptionCodeWord[i % _encryptionCodeWord.Length]);
            }
            return modifiedData;
        }

        /// <summary>
        /// Tenta restaurar o arquivo de dados a partir do arquivo de backup.
        /// </summary>
        /// <param name="fullPath">O caminho completo do arquivo de dados a ser restaurado.</param>
        /// <returns>Verdadeiro se a restauração foi bem-sucedida, falso caso contrário.</returns>
        private bool AttemptRollBack(string fullPath){
            bool success = false;
            string backupFilePath = string.Concat(fullPath,_backupExtension);

            try {
                if(File.Exists(backupFilePath)){
                    File.Copy(backupFilePath, fullPath, true);
                    success = true;
                    Debug.LogWarning("Foi necessário retornar para um arquivo de backup em: " + backupFilePath);
                }
                else{
                    throw new Exception("Não existe arquivo de backup para retornar.");
                }
            } 
            catch (Exception e) {
                Debug.LogError("Ocorreu um erro ao tentar retornar ao arquivo de backup em: " + backupFilePath + "\n" + e);
            }

            return success;
        }
    }
}
