using System.Text;

namespace GpsLogEdit
{

    /// <summary>
    /// GPXファイルリーダークラス
    /// </summary>
    internal class GpsFileReader
    {
        private string fileName;
        private int fileNumber;
        private Form ownerForm;

        public GpsFileReader(string fileName, int fileNum, Form owner)
        {
            this.fileNumber = fileNum;
            this.fileName = fileName.Replace("\"", "");  // ファイル名のダブルクオートを取り除く
            ownerForm = owner;
        }

        /// <summary>
        /// GPXファイル読み込み
        /// </summary>
        /// <returns>true=読み込み成功</returns>
        public bool Read(PositionList positionList)
        {
            bool success = false;
            try
            {
                Encoding enc = Encoding.GetEncoding("UTF-8");
                using StreamReader reader = new StreamReader(fileName, enc);
                string gpxRaw = reader.ReadToEnd();
                reader.Close();
                string[] gpxLines = gpxRaw.Replace("\r\n", "\n").Split('\n');

                FileType type = FileType.Gpx;
                if (fileName.EndsWith(".nmea", true, null))
                {
                    type = FileType.Nmea;
                }
                success = positionList.Analize(type, fileNumber, gpxLines);
            }
            catch (Exception e)
            {
                CustomMessageBox.Show(ownerForm, e.Message, "読み込みエラー", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            if (!success)
            {
                string msg = String.Format("以下のファイルではGPS位置情報が見つかりません\r\n  {0}", fileName);
                CustomMessageBox.Show(ownerForm, msg, "読み込みエラー", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            return success;
        }


    }
}
