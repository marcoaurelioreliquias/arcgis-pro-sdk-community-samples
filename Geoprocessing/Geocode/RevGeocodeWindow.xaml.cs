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
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ArcGIS.Desktop.Framework.Controls;
using ArcGIS.Desktop.Mapping;

namespace Geocode
{
    /// <summary>
    /// Interaction logic for GeocodeWindow.xaml
    /// </summary>
    public partial class RevGeocodeWindow : ProWindow
    {
        public RevGeocodeWindow()
        {
            InitializeComponent();
            // fill in a default address
            this.SearchTextLat.Text = "-19.8028221";
            this.SearchTextLong.Text = "-48.0018248";
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            // get the current module to access helper method
            GeocodeModule module = GeocodeModule.Current;

            // remove any existing graphics             
            MapView mapView = MapView.Active;
            GeocodeUtils.RemoveFromMapOverlay(mapView);

            try
            {
                // initiate the search
                CandidateResponse results = await GeocodeUtils.SearchFor(this.SearchTextLat.Text, 1);
                if (results.OrderedResults.Count > 0)
                {
                    // add a point graphic overlay
                    GeocodeUtils.UpdateMapOverlay(await results.OrderedResults[0].ToMapPoint(), mapView);
                    // zoom to the location
                    await GeocodeUtils.ZoomToLocation(results.OrderedResults[0].Extent);
                    // add the search results to the dialog window
                    this.LastSearch.Text = string.Format("Last Match: {0}", results.OrderedResults[0].CandidateDetails);
                }
                else
                {
                    ArcGIS.Desktop.Internal.Framework.DialogManager.ShowMessageBox(
                        string.Format("No results returned for {0}", this.SearchTextLat.Text), "GeocodeExample");
                }
            }
            catch (Exception ex)
            {
                string errorName = "Search Error";
                System.Diagnostics.Trace.WriteLine(string.Format("{0}: {1}", errorName, ex.ToString()));
                ArcGIS.Desktop.Internal.Framework.DialogManager.ShowMessageBox(errorName, this.SearchTextLat.Text);
            }
        }
    }
}
