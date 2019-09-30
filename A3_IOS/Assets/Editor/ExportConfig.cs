// using UnityEngine;
// using UnityEditor;
// using System.Xml;
// using System.Collections;
// using System.Collections.Generic;

// public class ExportConfig : Editor 
// {
	// static XmlElement gameobj;
	// static GameObject obj;
	// static XmlElement position ;
	// static XmlElement rotation;
	// static XmlElement scale;
	// static XmlDocument xmlDoc;
	// static bool b_ani=false;

	// static void objtransform(GameObject obj)
	// {

		// gameobj.SetAttribute("id",obj.name);
		// // position 
		// position =xmlDoc.CreateElement("pos");
		// position.SetAttribute("x",obj.transform.position.x+"");
		// position.SetAttribute("y",obj.transform.position.y+"");
		// position.SetAttribute("z",obj.transform.position.z+"");
		
		// // rotation
		// rotation =xmlDoc.CreateElement("rot");
		// rotation.SetAttribute("x",obj.transform.eulerAngles.x+"");
		// rotation.SetAttribute("y",obj.transform.eulerAngles.y+"");
		// rotation.SetAttribute("z",obj.transform.eulerAngles.z+"");
		
		// // scale
		// scale = xmlDoc.CreateElement("scale");
		// scale.SetAttribute("x",obj.transform.lossyScale.x+"");
		// scale.SetAttribute("y",obj.transform.lossyScale.y+"");
		// scale.SetAttribute("z",obj.transform.lossyScale.z+"");
		
		// gameobj.AppendChild(position);
		// gameobj.AppendChild(rotation);
		// gameobj.AppendChild(scale);
	// }
	// static void ani(GameObject obj)
	// {
		// if(obj.transform.childCount>0)
		// {
			// for(int j=0;j<obj.transform.childCount;j++)
			// {
				// GameObject objchild=obj.transform.GetChild(j).gameObject;
				// SkinnedMeshRenderer skmeshchild=objchild.GetComponent<SkinnedMeshRenderer>();
				// if(skmeshchild!=null)
				// {
					// if(skmeshchild.rootBone!=null)
					// {
						// while(objchild.GetComponent<Animation>()==null)
						// {
							// objchild=objchild.transform.parent.gameObject;
						// }
						// while(skmeshchild.rootBone.GetComponent<Animation>()==null)
						// {
							// if(skmeshchild.rootBone.transform.parent!=null)
							// skmeshchild.rootBone=skmeshchild.rootBone.transform.parent;
							// else
								// break;
						// }
						// if(objchild==skmeshchild.rootBone.gameObject)
						// {
							// b_ani=true;
							// return;
						// }
					// }
				// }
				// ani (objchild);
			// }
		// }
	// }
	// static void ExportXml(object[] objs)
	// {
		// if (objs.Length == 0)
		// {
			// EditorUtility.DisplayDialog("Export SceneObj", "Please select one or more target objects", "OK");
			// return;
		// }
		// string path_xml =EditorUtility.SaveFilePanel("Export Sceneobj Config", "", "","xml");
		// if (path_xml == "")
			// return;
	    // xmlDoc = new XmlDocument ();
		// xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0","utf-8",""));
		// XmlElement map=xmlDoc.CreateElement("map");
		// map.SetAttribute ("id", "");
		// //env
		// XmlElement env=xmlDoc.CreateElement("env");
		// //fog
		// XmlElement fog=xmlDoc.CreateElement("fog");
		// XmlElement fogcor=xmlDoc.CreateElement("color");
		// fogcor.SetAttribute ("r", RenderSettings.fogColor.r + "");
		// fogcor.SetAttribute ("g", RenderSettings.fogColor.g + "");
		// fogcor.SetAttribute ("b", RenderSettings.fogColor.b + "");
		// fogcor.SetAttribute ("a", RenderSettings.fogColor.a + "");
		
		// fog.SetAttribute ("display", RenderSettings.fog+"");
		// fog.SetAttribute ("density", RenderSettings.fogDensity + "");
		// fog.SetAttribute ("mode", RenderSettings.fogMode + "");
		// fog.SetAttribute ("distBegin", RenderSettings.fogStartDistance + "");
		// fog.SetAttribute ("distEnd", RenderSettings.fogEndDistance + "");
		// fog.AppendChild (fogcor);

		// //ambientLight 
		// XmlElement amblight=xmlDoc.CreateElement("amblight");
		// XmlElement ambcor=xmlDoc.CreateElement("color");
		// ambcor.SetAttribute ("r", RenderSettings.ambientLight.r + "");
		// ambcor.SetAttribute ("g", RenderSettings.ambientLight.g + "");
		// ambcor.SetAttribute ("b", RenderSettings.ambientLight.b + "");
		// ambcor.SetAttribute ("a", RenderSettings.ambientLight.a + "");
		// amblight.AppendChild (ambcor);


		// env.AppendChild (fog);
		// env.AppendChild (amblight);
		// map.AppendChild (env);
		// for(int i=0;i<objs.Length;i++)
		// {
			// obj=objs[i]as GameObject;
			// if (objs[i] == null)
				// return;	
			// MeshRenderer obj_mesh=obj.GetComponent<MeshRenderer>();
			// Animation obj_ani=obj.GetComponent<Animation>();
			// MeshFilter obj_meshfilter = obj.GetComponent<MeshFilter> ();
			// Light obj_light = obj.GetComponent<Light> ();
			// ParticleSystem obj_particlesystem = obj.GetComponent<ParticleSystem> ();
			// Camera obj_camera=obj.GetComponent<Camera>();
			// if (obj_mesh!= null&&obj_meshfilter!=null) 
			// {
				// gameobj = xmlDoc.CreateElement ("Mesh");
				// XmlElement asset = xmlDoc.CreateElement ("asset");
				// string mesh_path = AssetDatabase.GetAssetPath (PrefabUtility.GetPrefabParent (obj));
				// int mesh_index = mesh_path.LastIndexOf (".");
				// string mesh_file = mesh_path.Substring (17, mesh_index - 17);
				// asset.SetAttribute ("file", mesh_file);
				// gameobj.AppendChild (asset);
				// objtransform(obj);
				// map.AppendChild(gameobj);
			// }
			// if (obj_light != null) 
			// {
				// switch (obj_light.type) {
				// case LightType.Directional:
					// gameobj = xmlDoc.CreateElement ("DctLight");
					// obj.name="LightDir";
					// break;
				// case LightType.Point:
					// gameobj = xmlDoc.CreateElement ("PtLight");
					// XmlElement range = xmlDoc.CreateElement ("range");
					// range.SetAttribute ("val", obj_light.range + "");
					// gameobj.AppendChild (range);
					// obj.name="LightPoint";
					// break;
				// case LightType.Spot:
					// gameobj = xmlDoc.CreateElement ("SpotLight");
					// XmlElement range1 = xmlDoc.CreateElement ("range");
					// range1.SetAttribute ("val", obj_light.range + "");
					// XmlElement spotangle = xmlDoc.CreateElement ("spotangle");
					// spotangle.SetAttribute ("val", obj_light.spotAngle + "");
					// gameobj.AppendChild (range1);
					// gameobj.AppendChild (spotangle);
					// obj.name="LightSpot";
					// break;
				// default:
					// break;
				// }
				// XmlElement color = xmlDoc.CreateElement ("color");
				// color.SetAttribute ("r", obj_light.color.r + "");
				// color.SetAttribute ("g", obj_light.color.g + "");
				// color.SetAttribute ("b", obj_light.color.b + "");
				// color.SetAttribute ("a", obj_light.color.a + "");
				// XmlElement intensity = xmlDoc.CreateElement ("intensity");
				// intensity.SetAttribute ("val", obj_light.intensity + "");
				// gameobj.AppendChild (color);
				// gameobj.AppendChild (intensity);
				// objtransform(obj);
				// map.AppendChild(gameobj);
			// }
			// if (obj_particlesystem != null) 
			// {
				// if(obj.transform.parent!=null)
				// {
					// if((obj.transform.parent.gameObject.GetComponent<ParticleSystem>())==null)
					// {
						// gameobj = xmlDoc.CreateElement ("Particles");

						// XmlElement asset = xmlDoc.CreateElement ("asset");
						// string par_path = AssetDatabase.GetAssetPath (PrefabUtility.GetPrefabParent (obj_particlesystem));
						// int par_index = par_path.LastIndexOf (".");
						// string par_file = par_path.Substring (17, par_index - 17);
						// asset.SetAttribute ("file", par_file);
						// gameobj.AppendChild (asset);
						// objtransform(obj);
						// gameobj.SetAttribute("loop",obj_particlesystem.loop+"");

						// map.AppendChild(gameobj);
					// }
				// }else
				// {
					// gameobj = xmlDoc.CreateElement ("Particles");

					// XmlElement asset = xmlDoc.CreateElement ("asset");
					// string par_path = AssetDatabase.GetAssetPath (PrefabUtility.GetPrefabParent (obj_particlesystem));
					// int par_index = par_path.LastIndexOf (".");
					// string par_file = par_path.Substring (17, par_index - 17);
					// asset.SetAttribute ("file", par_file);
					// gameobj.AppendChild (asset);
					// objtransform(obj);
					// gameobj.SetAttribute("loop",obj_particlesystem.loop+"");

					// map.AppendChild(gameobj);
				// }
			// }

			// if(obj_ani!=null)
			// {
				// ani (obj);
				// if(b_ani)
				// {
					// gameobj=xmlDoc.CreateElement("Ani");
					// XmlElement asset=xmlDoc.CreateElement("asset");
					// string sk_path=AssetDatabase.GetAssetPath(PrefabUtility.GetPrefabParent(obj));
					// int sk_index=sk_path.LastIndexOf(".");
					// string sk_file=sk_path.Substring(17,sk_index-17);
					// asset.SetAttribute("file",sk_file);
					// gameobj.AppendChild(asset);
					// XmlElement Anims=xmlDoc.CreateElement("Anims");
					// XmlElement asset1=xmlDoc.CreateElement("asset");
					// string ani_path=AssetDatabase.GetAssetPath(obj.GetComponent<Animation>().clip);
					// int ani_index=ani_path.LastIndexOf(".");
					// string ani_file=ani_path.Substring(17,ani_index-17);
					// asset1.SetAttribute("id",obj.GetComponent<Animation>().clip.name);
					// asset1.SetAttribute("file",ani_file);
					// Anims.AppendChild(asset1);
					// gameobj.AppendChild(Anims);
					// objtransform(obj);
					// map.AppendChild(gameobj);
				// }
			// }
			// if(obj_camera!=null)
			// {
				// gameobj=xmlDoc.CreateElement("Camera");
				// XmlElement fov=xmlDoc.CreateElement("fov");
				// fov.SetAttribute("val",obj_camera.fieldOfView+"");
				// gameobj.AppendChild(fov);
				// objtransform(obj);
				// map.AppendChild(gameobj);
			// }
			// xmlDoc.AppendChild(map);
			// xmlDoc.Save(path_xml);
		// }
		// AssetDatabase.Refresh();
	// }

	// [MenuItem("Assets/[CrossMono]ExportConfig_all")]
	// static void exportall()
	// {
		// object[] objs = GameObject.FindObjectsOfType (typeof(GameObject));
		// ExportXml (objs);
	// }
	// [MenuItem("Assets/[CrossMono]ExportConifg_one")]
	// static void exportone()
	// {
		// object [] objs = Selection.GetFiltered(typeof(GameObject),SelectionMode.Deep);
		// ExportXml (objs);
	// }

// }
