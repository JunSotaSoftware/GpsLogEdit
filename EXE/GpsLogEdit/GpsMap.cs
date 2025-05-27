//
// 地図の表示
//
// MIT License
// Copyright(c) 2024-2025 Sota. 

using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Nts;
using Mapsui.Nts.Extensions;
using Mapsui.Projections;
using Mapsui.Styles;
using Mapsui.Tiling;
using Mapsui.UI.WindowsForms;
using Mapsui.Utilities;
using NetTopologySuite.Geometries;

namespace GpsLogEdit
{
    /// <summary>
    /// 地図上に表示するマークの位置情報
    /// </summary>
    internal class PositionMark
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    /// <summary>
    /// X,Yの最小値と最大値を記録するクラス
    /// </summary>
    internal class DoubleMinMaxRect
    {
        public double minX;
        public double maxX;
        public double minY;
        public double maxY;

        /// <summary>
        /// X,Yの最小値と最大値を記録するクラスのコンストラクタ
        /// </summary>
        public DoubleMinMaxRect()
        {
            minX = double.MaxValue;
            maxX = double.MinValue;
            minY = double.MaxValue;
            maxY = double.MinValue;
        }

        /// <summary>
        /// 最小値と最大値を記録する
        /// </summary>
        /// <param name="x">Xの値</param>
        /// <param name="y">Yの値</param>
        public void SetPoint(double x, double y)
        {
            minX = double.Min(minX, x);
            maxX = double.Max(maxX, x);
            minY = double.Min(minY, y);
            maxY = double.Max(maxY, y);
        }
    }

    /// <summary>
    /// 地図表示クラス
    /// </summary>
    internal class GpsMap
    {
        private MapControl mapControl;      // 地図を表示するフォーム上のコントロール
        private ILayer? lineStringLayer;    // 経路を示す線を表示するレイヤー
        private ILayer? currentPointLayer;  // 現在位置を示すマークを表示するレイヤー
        private ILayer? cutPointLayer;      // 分割位置を示すマークを表示するレイヤー
        private EditManager? editManager;   // 編集マネージャ
        private Action<int>? formCallback;  // マップをクリックしたときにコールバックするメソッド
        private PositionList? positionList; // 位置情報リスト

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="control">地図を表示するフォーム上のコントロール</param>
        public GpsMap(MapControl control)
        {
            mapControl = control;

            // 以下のマップの作成は
            // Samples\Mapsui.Samples.WindowsForms\SampleWindow.cs : await sample.SetupAsync(_mapControl);
            //   Samples\Mapsui.Samples.Common\Extensions\SampleExtensions.cs : mapControl.Map = await asyncSample.CreateMapAsync();
            // を参考にしている

            // マップを作成する
            Catch.Exceptions(async () =>
            {
                await CreateMapAsync();
            });

            lineStringLayer = null;
            currentPointLayer = null;
            cutPointLayer = null;
            editManager = null;
            formCallback = null;
            positionList = null;
        }

        /// <summary>
        /// 地図をクリックしたときにコールバックするメソッドを指定する
        /// </summary>
        /// <param name="callback">メソッド</param>
        public void SetCallback(Action<int> callback)
        {
            formCallback = callback;
        }

        /// <summary>
        /// 読み込んだGPSの位置情報リストを指定する
        /// </summary>
        /// <param name="list">位置情報リスト</param>
        public void SetPositionList(PositionList list)
        {
            positionList = list;
        }

        /// <summary>
        /// 編集マネージャを指定する
        /// </summary>
        /// <param name="manager">編集マネージャ</param>
        public void SetEditManager(EditManager manager)
        {
            editManager = manager;
        }

        // 以下のマップ表示部分は
        // Samples\Mapsui.Samples.Common\Maps\Geometries\PointsSample.cs
        // を参考にしている

        /// <summary>
        /// マップを作成して表示する
        /// </summary>
        /// <returns>タスク</returns>
        private Task<Map> CreateMapAsync()
        {
            // マップを作成して表示
            mapControl.Map.Layers.Add(OpenStreetMap.CreateTileLayer("user-agent-of-GpsLogEdit.Windows.Application"));   // 文字列はユーザーエージェント

            // 以下のマップのクリックイベントの処理部分は
            // Samples\Mapsui.Samples.Common\Maps\Demo\AddPinsSample.cs (旧 WriteToLayerSample.cs)
            // を参考にしている

            // マップがクリックされると以下へ来る
            mapControl.Map.Tapped += (m, e) =>
            {
                if ((positionList != null) && (positionList.GetPositionCount() > 0) && (formCallback != null))
                {
                    // クリックされた場所に一番近いGPSの位置データを探す
                    double minDist = Double.MaxValue;
                    int nearestIndex = -1;
                    for (int i = 0; i < positionList.GetPositionCount(); i++)
                    {
                        PositionData data = positionList.GetPositionData(i);
                        MPoint pos = new MPoint(SphericalMercator.FromLonLat(data.longitude, data.latitude));
                        double dist = Algorithms.Distance(pos, e.WorldPosition);
                        if (dist < minDist)
                        {
                            nearestIndex = i;
                            minDist = dist;
                        }
                    }
                    // リストビューの項目を選択するメソッドをコールバック
                    formCallback(nearestIndex);
                }
                e.Handled = true;
            };

            return Task.FromResult(mapControl.Map);
        }

        // 以下のトラック表示の部分は
        // Samples\Mapsui.Samples.Common\Maps\Geometries\LineStringSample.cs
        // を参考にしている

        /// <summary>
        /// GPSのトラックを表示する
        /// </summary>
        public void ShowGpsTrack()
        {
            if ((positionList != null) && (positionList.GetPositionCount() > 0))
            {
                // 既にトラックが表示中なら消去
                if (lineStringLayer != null)
                {
                    mapControl.Map.Layers.Remove(lineStringLayer);
                    lineStringLayer = null;
                }
                // トラックを表示
                lineStringLayer = CreateLineStringLayer(out DoubleMinMaxRect minMaxRect);
                mapControl.Map.Layers.Add(lineStringLayer);

                // ズーム比率を計算
                System.Drawing.Size size = mapControl.Size;
                double res = ZoomHelper.CalculateResolutionForWorldSize(minMaxRect.maxX - minMaxRect.minX, minMaxRect.maxY - minMaxRect.minY, size.Width, size.Height, MBoxFit.Fit);
                res *= 1.1; // 計算で得られるズーム比率だと、少しはみ出る気がする
                mapControl.Map.Navigator.CenterOnAndZoomTo(lineStringLayer.Extent!.Centroid, res);
            }
        }

        /// <summary>
        /// トラックのストリングレイヤを作成する
        /// </summary>
        /// <param name="minMaxRect">out トラック全体が表示できる矩形</param>
        /// <returns>レイヤー</returns>
        private ILayer CreateLineStringLayer(out DoubleMinMaxRect minMaxRect)
        {
            minMaxRect = new DoubleMinMaxRect();

            List<Coordinate> coordinates = new List<Coordinate>();
#pragma warning disable CS8602
            for (int i = 0; i < positionList.GetPositionCount(); i++)
            {
                PositionData data = positionList.GetPositionData(i);
                Coordinate coordinate = new Coordinate(SphericalMercator.FromLonLat(data.longitude, data.latitude).ToCoordinate());
                coordinates.Add(coordinate);

                minMaxRect.SetPoint(coordinate.X, coordinate.Y);
            }
            LineString lineString = new LineString(coordinates.ToArray());

            IStyle? style = new VectorStyle
            {
                Fill = null,
                Outline = null,
#pragma warning disable CS8670 // Object or collection initializer implicitly dereferences possibly null member.
                Line = { Color = Mapsui.Styles.Color.Blue, Width = 4 }
            };

            return new MemoryLayer
            {
                Features = new[] { new GeometryFeature { Geometry = lineString } },
                Name = "LineStringLayer",
                Style = style
            };
        }

        // 以下の位置表示の部分は
        // Samples\Mapsui.Samples.Common\Maps\Geometries\PointsSample.cs
        // を参考にしている

        /// <summary>
        /// マップ上に現在の位置を表示する
        /// </summary>
        /// <param name="index">現在位置を示すデータ番号</param>
        public void ShowGpsCurrentPoint(int index)
        {
            if ((positionList != null) && (positionList.GetPositionCount() > 0))
            {
                // 既に現在位置が表示されていたら消去
                if (currentPointLayer != null)
                {
                    mapControl.Map.Layers.Remove(currentPointLayer);
                    currentPointLayer = null;
                }

                if (index >= 0)
                {
                    PositionData data = positionList.GetPositionData(index);
                    List<PositionMark> markList = new List<PositionMark>();
                    PositionMark mark = new PositionMark();
                    mark.Latitude = data.latitude;
                    mark.Longitude = data.longitude;
                    markList.Add(mark);

                    currentPointLayer = CreatePointLayer(markList, "Point1", "embedded://GpsLogEdit.cross.png");    // 埋め込みリソースにある画像
                    mapControl.Map.Layers.Add(currentPointLayer);

                    MPoint mPoint = SphericalMercator.FromLonLat(data.longitude, data.latitude).ToMPoint();
                    mapControl.Map.Navigator.CenterOn(mPoint);
                }
            }
        }

        /// <summary>
        /// マップ上に分割位置を表示する
        /// </summary>
        public void ShowGpsDividePoint()
        {
            if ((positionList != null) && (positionList.GetPositionCount() > 0) && (editManager != null))
            {
                // 既に分割情報を表示中なら消去
                if (cutPointLayer != null)
                {
                    mapControl.Map.Layers.Remove(cutPointLayer);
                    cutPointLayer = null;
                }

                List<PositionMark> markList = new List<PositionMark>();
                for (int i = 0; i < editManager.GetEditPositionCount(EditType.Divide); i++)
                {
                    int pos = editManager.GetEditedDataNumber(i, EditType.Divide);
                    PositionData data = positionList.GetPositionData(pos);
                    PositionMark mark = new PositionMark();
                    mark.Latitude = data.latitude;
                    mark.Longitude = data.longitude;
                    markList.Add(mark);
                }
                if (markList.Count > 0)
                {
                    cutPointLayer = CreatePointLayer(markList, "Point2", "embedded://GpsLogEdit.scissors.png");     // 埋め込みリソースにある画像
                    mapControl.Map.Layers.Add(cutPointLayer);
                }
            }
        }

        /// <summary>
        /// マークを表示するレイヤを作成する
        /// </summary>
        /// <param name="positionList">マークの位置リスト</param>
        /// <param name="name">レイヤの名前</param>
        /// <param name="imageSource">マークの画像ソース</param>
        /// <returns>レイヤ</returns>
        private static MemoryLayer CreatePointLayer(List<PositionMark> positionList, string name, string imageSource)
        {
            MemoryLayer layer = new MemoryLayer();
            layer.Name = name;
            //layer.IsMapInfoLayer = true;  // Mapsui 5.0.0-beta7で削除された
            layer.Features = GetPointMarkList(positionList);
            layer.Style = CreatePointMarkStyle(imageSource);
            return layer;
        }

        /// <summary>
        /// 位置リストを作成
        /// </summary>
        /// <param name="positionList">マークの位置リスト</param>
        /// <returns>位置リスト</returns>
        private static IEnumerable<IFeature> GetPointMarkList(List<PositionMark> positionList)
        {
            return positionList.Select(c =>
            {
                var feature = new PointFeature(SphericalMercator.FromLonLat(c.Longitude, c.Latitude).ToMPoint());
                return feature;
            }).ToArray();
        }

        /// <summary>
        /// 表示するマークを作成
        /// </summary>
        /// <param name="imageSource">画像のソース</param>
        /// <returns>マーク</returns>
        private static ImageStyle CreatePointMarkStyle(string imageSource)
        {
            return new ImageStyle
            {
                Image = imageSource,
                SymbolScale = 1,
                Offset = new Offset(0, 0)
            };
        }

        /// <summary>
        /// 地図上のトラックとマークをクリア
        /// </summary>
        public void Clear()
        {
            if (lineStringLayer != null)
            {
                mapControl.Map.Layers.Remove(lineStringLayer);
                lineStringLayer = null;
            }

            if (currentPointLayer != null)
            {
                mapControl.Map.Layers.Remove(currentPointLayer);
                currentPointLayer = null;
            }
            if (cutPointLayer != null)
            {
                mapControl.Map.Layers.Remove(cutPointLayer);
                cutPointLayer = null;
            }
        }
    }
}

