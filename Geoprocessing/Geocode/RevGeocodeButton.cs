//Copyright 2019 Esri

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       https://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Extensions;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Internal.Mapping;
using ArcGIS.Desktop.Mapping;

namespace Geocode
{
    internal class RevGeocodeButton : Button
    {
        RevGeocodeWindow _revDlg = null;

        protected override void OnClick()
        {
            if (_revDlg != null)
                return;//already shown
            _revDlg = new RevGeocodeWindow();
            _revDlg.Closed += _revDlg_Closed;
            _revDlg.Topmost = true;
            _revDlg.Show();
        }

        void _revDlg_Closed(object sender, EventArgs e)
        {
            //try and clean up
            GeocodeUtils.RemoveFromMapOverlay(MapView.Active);
            //geocode dialog was closed
            _revDlg.Closed -= _revDlg_Closed;
            _revDlg = null;
        }
    }
}
