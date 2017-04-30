using System;

namespace APIforEOSDIS
{
    public enum projection_type { geographic, arctic, antarctic };

    public class WorldviewQueryMaker
    {
        private string[] layers_list;
        private projection_type projection;
        private DateTime time;
        private RectArea coordinates;
        private const string default_layers = "VIIRS_SNPP_CorrectedReflectance_TrueColor,Reference_Labels,Reference_Features";

        public string[] Layers_list
        {
            get { return layers_list; }
            set { layers_list = value; }
        }
        public DateTime Time
        {
            get { return time; }
            set { time = value; }
        }
        public RectArea Coordinates
        {
            get { return coordinates; }
            set { coordinates = value; }
        }
        private projection_type Projections
        {
            get { return projection; }
            set { projection = value; }
        }

        public WorldviewQueryMaker(string[] Lyr_list, DateTime Tm, RectArea Coord, projection_type Prj_tp)
        {
            layers_list = Lyr_list; time = Tm; coordinates = Coord; projection = Prj_tp;
        }

        public string Gen_Query_Link()
        {
            string layers_str = "";

            if (layers_list != null)
                for (int i = 0; i < layers_list.Length; i++)
                    if (layers_list[i] != "")
                        layers_str += "," + layers_list[i];

            string projctn_name = "";
            switch (projection)
            {
                case projection_type.geographic:
                    projctn_name = "geographic";
                    break;
                case projection_type.arctic:
                    projctn_name = "arctic";
                    break;
                case projection_type.antarctic:
                    projctn_name = "antarctic";
                    break;
            }

            return "https://worldview.earthdata.nasa.gov/?p=" + projctn_name +
                "&l=" + default_layers + layers_str + "&t=" + time.ToString("yyyy-MM-dd") +
                "&z=3&v=" + coordinates.ToString();
        }
    }
}
