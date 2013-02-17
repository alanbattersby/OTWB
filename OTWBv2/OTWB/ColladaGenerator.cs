using grendgine_collada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Shapes;

namespace Geometric_Chuck
{
    class SimpleColladaGenerator
    {
        public SimpleCollada Create(PolygonCollection paths)
        {
            SimpleCollada data = CreateSceneData();
            FillData(ref data,paths);

            return data;
        }

        private void FillData(ref SimpleCollada data, PolygonCollection paths)
        {

            Grendgine_Collada_Visual_Scene sc1 = CreateScene("ID1", 1);
            data.Library_Visual_Scenes.Visual_Scene = new Grendgine_Collada_Visual_Scene[1] { sc1 };
            foreach (Polygon poly in paths.Polygons)
            {

            }

            data.Scene.Visual_Scene = new Grendgine_Collada_Instance_Visual_Scene();
            data.Scene.Visual_Scene.URL = "#ID1";

        }

        Grendgine_Collada_Node CreateNode(string name)
        {
            Grendgine_Collada_Node node = new Grendgine_Collada_Node();
            node.Name = name;
            node.Type = Grendgine_Collada_Node_Type.NODE;
            node.Instance_Geometry = new Grendgine_Collada_Instance_Geometry[1];
            Grendgine_Collada_Instance_Geometry geom = new Grendgine_Collada_Instance_Geometry();
            geom.URL = "#ID2";
            geom.Bind_Material = new Grendgine_Collada_Bind_Material[1];
            geom.Bind_Material[0] = CreateBindMaterial(); 
            node.Instance_Geometry[0] = geom;
            return node;
        }

        Grendgine_Collada_Bind_Material CreateBindMaterial()
        {
            Grendgine_Collada_Bind_Material bm = new Grendgine_Collada_Bind_Material();
            bm.Technique_Common = new Grendgine_Collada_Technique_Common_Bind_Material();
            bm.Technique_Common.Instance_Material = new Grendgine_Collada_Instance_Material_Geometry[1];
            return bm;
        }

        Grendgine_Collada_Visual_Scene CreateScene(string id, int numnodes)
        {
            Grendgine_Collada_Visual_Scene sc = new Grendgine_Collada_Visual_Scene();
            sc.ID = "ID1";
            sc.Node = new Grendgine_Collada_Node[numnodes];
            sc.Node[0] = CreateNode("Sketchup");
            return sc;
        }

        SimpleCollada CreateSceneData()
        {
            SimpleCollada tst = new SimpleCollada();
            tst.Asset.Title = "Test 1";
            tst.Asset.Created = DateTime.Now;
            tst.Asset.Modified = tst.Asset.Created;
            tst.Asset.Up_Axis = "Y_UP";
            tst.Asset.Unit = new Grendgine_Collada_Asset_Unit();
            tst.Asset.Unit.Meter = 0.001;
            tst.Asset.Unit.Name = "MM";
            Grendgine_Collada_Asset_Contributor cont = new Grendgine_Collada_Asset_Contributor();
            cont.Author = "A. Battersby";
            cont.Authoring_Tool = "OTWorkbench";
            tst.Asset.Contributor = new Grendgine_Collada_Asset_Contributor[1] { cont };
            CreateMaterials(ref tst);
            CreateLibraryEffects(ref tst);
            return tst;
        }

        void CreateMaterials(ref SimpleCollada data)
        {
            data.Library_Materials.Material = new Grendgine_Collada_Material[1];
            Grendgine_Collada_Material m = new Grendgine_Collada_Material();
            m.ID = "ID3";
            m.Name = "edge_color000255";
            m.Instance_Effect = new Grendgine_Collada_Instance_Effect();
            m.Instance_Effect.URL = "#ID4";
            data.Library_Materials.Material[0] = m;
        }
        void CreateLibraryEffects(ref SimpleCollada data)
        {
            data.Library_Effects.Effect = new Grendgine_Collada_Effect[1];
            Grendgine_Collada_Effect efct = new Grendgine_Collada_Effect();
            efct.ID = "ID4";
            Grendgine_Collada_Profile_COMMON prof = new Grendgine_Collada_Profile_COMMON();
            prof.Technique = new Grendgine_Collada_Effect_Technique_COMMON();
            prof.Technique.sID = "COMMON";
            prof.Technique.Constant = new Grendgine_Collada_Constant();
            prof.Technique.Constant.Transparency = new Grendgine_Collada_FX_Common_Float_Or_Param_Type();
            prof.Technique.Constant.Transparency.Float = new Grendgine_Collada_SID_Float();
            prof.Technique.Constant.Transparency.Float.Value = 1;

            prof.Technique.Constant.Transparent = new Grendgine_Collada_FX_Common_Color_Or_Texture_Type();
            prof.Technique.Constant.Transparent.Opaque = Grendgine_Collada_FX_Opaque_Channel.A_ONE;
            prof.Technique.Constant.Transparent.Color = new Grendgine_Collada_Color();
            prof.Technique.Constant.Transparent.Color.Value_As_String = "0 0 0 1";

            efct.Profile_COMMON = new Grendgine_Collada_Profile_COMMON[] { prof };
            data.Library_Effects.Effect[0] = efct;
        }
    }
}
