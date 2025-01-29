namespace GpsLogEdit
{
    /// <summary>
    /// 編集データクラス
    /// </summary>
    internal class EditData : IComparable<EditData>
    {
        public int dataNumber;
        public EditType editType;

        public EditData(int index, EditType type)
        {
            dataNumber = index;
            editType = type;
        }

        /// <summary>
        /// DivisionDataのデフォルトcomparer
        /// </summary>
        /// <param name="data">比較するデータ</param>
        /// <returns>比較結果</returns>
        public int CompareTo(EditData? data)
        {
            if (data == null)
            {
                return 1;
            }
            return dataNumber - data.dataNumber;
        }
    }

    /// <summary>
    /// GPXファイル編集マネージャクラス
    /// </summary>
    internal class EditManager
    {
        List<EditData> editList;

        /// <summary>
        /// 編集マネージャのコンストラクタ
        /// </summary>
        public EditManager()
        {
            editList = new List<EditData>();
        }

        /// <summary>
        /// 編集の状態をトグル的に入れ替える
        /// </summary>
        /// <param name="index">編集位置のデータ番号</param>
        /// <param name="type">編集のタイプ</param>
        /// <returns>編集した結果セットされているEditType</returns>
        public EditType toggleEditState(int index, EditType type)
        {
            EditType result = EditType.None;
            bool removed = false;
            int found = editList.FindIndex(x => x.dataNumber == index);
            if (found != -1)
            {
                EditType now = editList[found].editType;
                if ((now & type) != 0)
                {
                    // 指定されたタイプのデータがある
                    removed = true;
                    if ((now & ~type) != 0)
                    {
                        // 指定された以外のタイプも持つ -> 指定のタイプのみクリア
                        now ^= type;
                        editList[found].editType = now;
                        result = now;
                    }
                    else
                    {
                        // 指定されたタイプのみのデータ -> データそのものを削除
                        editList.RemoveAt(found);
                        result = EditType.None;
                    }
                }
            }
            if (!removed)
            {
                if (found != -1)
                {
                    // 指定タイプを持たないデータがある -> 指定タイプをセット
                    editList[found].editType |= type;
                    result = editList[found].editType;
                }
                else
                {
                    // データがない -> データを新規追加
                    EditData data = new EditData(index, type);
                    editList.Add(data);
                    editList.Sort();
                    result = type;
                }
            }
            return result;
        }

        public int GetEditPositionCount(EditType type)
        {
            if (type == EditType.All)
            {
                return editList.Count;
            }
            return editList.Count(x => (x.editType & type) != 0);
        }





        /// <summary>
        /// 編集n番目のデータ番号を返す
        /// </summary>
        /// <param name="index">n番目</param>
        /// <param name="type">編集のタイプ</param>
        /// <returns>開始データ番号／-1=n番目が範囲外</returns>
        /// <remarks>編集位置は昇順にソートされている</remarks>
        public int GetEditedDataNumber(int index, EditType type)
        {
            if (type == EditType.All)
            {
                if ((index < 0) || (index >= editList.Count))
                {
                    return -1;
                }
                return editList[index].dataNumber;
            }
            List<EditData> list = editList.Where(x => (x.editType & type) != 0).ToList();
            if ((index < 0) || (index >=  list.Count))
            {
                return -1;
            }
            return list[index].dataNumber;
        }

        /// <summary>
        /// 編集情報を全てクリアする
        /// </summary>
        public void Clear()
        {
            editList.Clear();
        }

        /// <summary>
        /// 指定データ番号から次の位置にある編集点のデータ番号を返す
        /// </summary>
        /// <param name="index">基準となるデータ番号</param>
        /// <returns>データ番号 / -1=次の編集点はない</returns>
        public int GetNextEditedDataNumber(int index)
        {
            int nextNumber = -1;
            int candidate = -1;
            bool goForward = true;
            while (goForward)
            {
                goForward = false;
                bool nowDeleted = IsEditedLine(index, EditType.Delete) && !IsEditedLine(index, EditType.Divide);
                foreach (EditData data in editList)
                {
                    if ((data.dataNumber > index) && ((nextNumber == -1) || (data.dataNumber < nextNumber)))
                    {
                        nextNumber = data.dataNumber;
                    }
                }
                if (nowDeleted && (nextNumber == index+1) && IsEditedLine(nextNumber, EditType.Delete) && !IsEditedLine(nextNumber, EditType.Divide))
                {
                    candidate = nextNumber;
                    index = nextNumber;
                    nextNumber = -1;
                    goForward = true;
                }
            }
            if (candidate != -1)
            {
                nextNumber = candidate;
            }
            return nextNumber;
        }

        /// <summary>
        /// 指定データ番号から前の位置にある編集点のデータ番号を返す
        /// </summary>
        /// <param name="index">基準となるデータ番号</param>
        /// <returns>データ番号 / -1=前の編集点はない</returns>
        public int GetPrevEditedDataNumber(int index)
        {
            int nextNumber = -1;
            int candidate = -1;
            bool goForward = true;
            while (goForward)
            {
                goForward = false;
                bool nowDeleted = IsEditedLine(index, EditType.Delete) && !IsEditedLine(index, EditType.Divide);
                foreach (EditData data in editList)
                {
                    if ((data.dataNumber < index) && (data.dataNumber > nextNumber))
                    {
                        nextNumber = data.dataNumber;
                    }
                }
                if (nowDeleted && (nextNumber == index - 1) && IsEditedLine(nextNumber, EditType.Delete) && !IsEditedLine(nextNumber, EditType.Divide))
                {
                    candidate = nextNumber;
                    index = nextNumber;
                    nextNumber = -1;
                    goForward = true;
                }
            }
            if (candidate != -1)
            {
                nextNumber = candidate;
            }
            return nextNumber;
        }


        public bool IsEditedLine(int index, EditType type)
        {
            bool status = (editList.Count(x => (x.dataNumber == index) && ((x.editType & type) != 0)) != 0);
            return status;
        }

    }
}
