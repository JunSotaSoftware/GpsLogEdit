# GpsLogEdit

## 概要

GPSのログファイルの編集を行うソフトウエアです。

**出来る編集**  
+ トラックを分割  
+ トラックを結合（ファイル読み込み時に自動実行）  
+ トラック中の不要なポイントデータを削除  

**読み込めるファイル形式**  
+ GPX形式 / NMEA形式  
（複数のファイルを読み込むと、トラックを結合し時刻でソートします。統合されたトラックは編集で分割が可能です。）  

**書き込めるファイル形式**  
+ GPX形式 / KML形式  

**プロジェクトファイル**  
+ 編集の状態をプロジェクトファイルに保存できます。  

## 動作環境
Windows 11でのみ動作確認を行っています。  
その他のWindowsでも動作するかもしれませんが、確認は行っていません。  

## ご利用にあたり  
GpsLogEditはフリーソフトです。個人使用、業務使用のいずれでもご自由にお使いください。  

ソフトウエアは十分にテストをしていますが、お使いのパソコン環境や、プログラムの不具合などによって問題が生じる場合があります。それにより損害が生じても、損害に対する保証は出来かねますので、あらかじめご了承ください。  

詳細は、下記のライセンスをご覧ください。  

## スクリーンショット
**メインの画面**  
![メイン画面](https://www2.biglobe.ne.jp/~sota/gpslogedit_ss/ss_main.png "メイン画面")  

**保存確認の画面**  
![保存確認画面](https://www2.biglobe.ne.jp/~sota/gpslogedit_ss/ss_savenotify.png "保存確認画面")  

**ファイルメニュー**  
![ファイルメニュー](https://www2.biglobe.ne.jp/~sota/gpslogedit_ss/ss_filemenu.png "ファイルメニュー")  

**表示メニュー**  
![表示メニュー](https://www2.biglobe.ne.jp/~sota/gpslogedit_ss/ss_showmenu.png "表示メニュー")  

**設定メニュー**  
![設定メニュー](https://www2.biglobe.ne.jp/~sota/gpslogedit_ss/ss_settingmenu.png "設定メニュー")  

**ヘルプメニュー**  
![ヘルプメニュー](https://www2.biglobe.ne.jp/~sota/gpslogedit_ss/ss_helpmenu.png "ヘルプメニュー")  

## ライセンス

本ソフトウエアの扱いはMITライセンスに従う物とします。  

Copyright(c) 2024-2025 Sota.  

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:  

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.  

THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.  

**以下は日本語訳（参考）**  

Copyright (c) 2024-2025 Sota.  

本ソフトウェアおよび関連する文書のファイル（以下「ソフトウェア」）の複製を取得した全ての人物に対し、以下の条件に従うことを前提に、ソフトウェアを無制限に扱うことを無償で許可します。これには、ソフトウェアの複製を使用、複製、改変、結合、公開、頒布、再許諾、および/または販売する権利、およびソフトウェアを提供する人物に同様の行為を許可する権利が含まれますが、これらに限定されません。  

上記の著作権表示および本許諾表示を、ソフトウェアの全ての複製または実質的な部分に記載するものとします。  

ソフトウェアは「現状有姿」で提供され、商品性、特定目的への適合性、および権利の非侵害性に関する保証を含むがこれらに限定されず、明示的であるか黙示的であるかを問わず、いかなる種類の保証も行われません。著作者または著作権者は、契約、不法行為、またはその他の行為であるかを問わず、ソフトウェアまたはソフトウェアの使用もしくはその他に取り扱いに起因または関連して生じるいかなる請求、損害賠償、その他の責任について、一切の責任を負いません。 

## 第三者ソフトウエアなど

**Mapsuiライブラリ**  

GpsLogEditは地図の表示にMapsuiライブラリを使用します。  
GpsLogEdit開発時点では、Mapsuiライブラリのバージョンは 5.0.0-beta.8 です。  
https://mapsui.com/  
Copyright (c) 2022 The Mapsui authors  

**地図**  

表示する地図はOpenStreetMapを使用します。  
https://www.openstreetmap.org/  
(C) OpenStreetMap contributors (https://www.openstreetmap.org/copyright)  

**アイコン**  

GpsLogEditで使用しているアイコンは以下の物です。  

Designed by Freepik  
https://www.freepik.com/icons/gps  

## 開発用の情報

**Mapsuiライブラリインストール方法** 

Visual Studioの『ツール』→『NuGetパッケージマネージャ』→『パッケージマネージャコンソール』にて以下を実行。

       PM> NuGet\Install-Package Mapsui.WindowsForms -Version 5.0.0-beta.8

