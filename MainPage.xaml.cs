using Mapsui;
using Mapsui.Layers;
using Mapsui.Tiling;
using Mapsui.Tiling.Layers;
using Mapsui.UI.Maui;
using BruTile;
using BruTile.Predefined;
using BruTile.Web;
using Mapsui.Projections;
using Mapsui.Styles;

namespace estudo_maps
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();

            mapControl = new MapControl();
            Content = mapControl;

            mapControl.Map = CreateMap();

        }

        private static Mapsui.Map CreateMap()
        {
            var map = new Mapsui.Map
            {
                CRS = "EPSG:3857"
            };

            var tileSource = new HttpTileSource(
                new GlobalSphericalMercator(),
                "https://tile.openstreetmap.org/{z}/{x}/{y}.png",
                name: "OpenStreetMap",
                attribution: new BruTile.Attribution("© OpenStreetMap contributors")
            );

            var osmLayer = new TileLayer(tileSource)
            {
                Name = "OpenStreetMap"
            };

            map.Layers.Add(osmLayer);

            // Conversão de EPSG:4326 para EPSG:3857
            var point = SphericalMercator.FromLonLat(-42.705033, -21.374592);

            map.Home = n => n.CenterOn(point.x, point.y); // Ajuste o scale para visualizar melhor

            // Adicionar um pin no mapa
            MPoint pinPoint = new MPoint(point.x, point.y);
            var pinLayer = CreatePinLayer(pinPoint);
            map.Layers.Add(pinLayer);

            return map;
        }

        private static MemoryLayer CreatePinLayer(MPoint location)
        {
            var pinFeature = new PointFeature(location);

            // Criar um estilo para o pin (pode ser um ícone ou um círculo)
            var pinStyle = new SymbolStyle
            {
                SymbolScale = 0.4, // Tamanho do ícone
                Fill = new Mapsui.Styles.Brush(Mapsui.Styles.Color.Blue), // Cor do marcador
               // SymbolOffset = new Offset(0, 3), // Ajuste para não sobrepor o centro do ponto
                Opacity = 0.8f
            };

            pinFeature.Styles.Add(pinStyle);

            pinFeature.Styles.Add(pinStyle);
            pinFeature["Title"] = "Casa do Manel"; // Informação que será exibida no callout
            pinFeature["Description"] = "Apartamento 307"; // Descrição extra

            // Criar a camada para o pin
            var pinLayer = new MemoryLayer
            {
                Name = "Minha casa",
                Features = new List<IFeature> { pinFeature }
            };

            return pinLayer;
        }

     
    }
}