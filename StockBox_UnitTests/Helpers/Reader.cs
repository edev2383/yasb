using System;
using System.IO;
using StockBox.Associations;

namespace StockBox_UnitTests.Helpers
{
    public class Reader : ICallContextProvider
    {

        private string _root = "/Users/jefferyedick/Projects/StockBox/StockBox_UnitTests/Files/";

        public string Text { get { return _text; } }
        private string _text;

        public Reader()
        {

        }

        public MemoryStream GetDaily()
        {
            return GetFileStream(EFile.eAmdDaily);
        }

        public MemoryStream GetWeekly()
        {
            return GetFileStream(EFile.eAmdWeekly);
        }

        public MemoryStream GetMontly()
        {
            return GetFileStream(EFile.eAmdMonthly);
        }

        internal object GetFileStream(object eAmdDaily)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return the target file contents as a string
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public string GetFileContents(EFile target)
        {
            string source = GetFileSource(target);
            string path = $@"{source}";
            _text = System.IO.File.ReadAllText(path);
            return Text;
        }

        /// <summary>
        /// Returns the target file contents as a MemoryStream object to
        /// interface with Deedle library
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public MemoryStream GetFileStream(EFile target)
        {
            string fileSource = new Reader().GetFileSource(target);
            byte[] content = File.ReadAllBytes(fileSource);
            return new MemoryStream(content);
        }

        /// <summary>
        /// Return system path of selected file
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public string GetFileSource(EFile target)
        {
            string ret = string.Empty;
            EExt ext = EExt.eTxt;
            switch (target)
            {
                case EFile.eBasicString:
                    ret = "BasicString";
                    break;
                case EFile.eBasicInt:
                    ret = "BasicInt";
                    break;
                case EFile.eBasicDouble:
                    ret = "BasicDouble";
                    break;
                case EFile.eAmdDaily:
                    ret = "AMD_DAILY";
                    ext = EExt.eCsv;
                    break;
                case EFile.eAmdWeekly:
                    ret = "AMD_WEEKLY";
                    ext = EExt.eCsv;
                    break;
                case EFile.eAmdMonthly:
                    ret = "AMD_MONTHLY";
                    ext = EExt.eCsv;
                    break;
                case EFile.eAmdDailySmallDataset:
                    ret = "AMD_DAILY_SMALL_DATASET";
                    ext = EExt.eCsv;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return $"{_root}{ret}.{GetFIleExtension(ext)}";
        }

        private string GetFIleExtension(EExt ext)
        {
            switch (ext)
            {
                case EExt.eTxt:
                    return "txt";
                case EExt.eCsv:
                    return "csv";
                default:
                    return string.Empty;
            }
        }

    }
}
