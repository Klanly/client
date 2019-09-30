using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.IO;

public class Rebuild_GameUI : Editor
{
    [MenuItem("Assets/husunren_UI优化/调整UI_提取所有的图片")]
    static public void RebuildGameUI_export_pics()
    {
        //获取在Project视图中选择的所有游戏对象
        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
        string out_path = null;
        float cur_count = 0f;
        foreach (Object obj in SelectedAsset)
        {
            out_path = AssetDatabase.GetAssetPath(obj);
            if (File.Exists(out_path))
            {
                int pos = out_path.LastIndexOf("/");
                string obj_name = out_path.Substring(pos + 1).Replace(".prefab", "");

                if (AssetDatabase.LoadAssetAtPath<Object>("Assets/AB_RELEASE/mother_package/ab_ui_shared/ui_pics/" + obj_name) == null)
                {
                    AssetDatabase.CreateFolder("Assets/AB_RELEASE/mother_package/ab_ui_shared/ui_pics", obj_name);
                }


                string assetRelativePath = GetRelativeAssetPath(out_path);
                try
                {
                    GameObject rtf = obj as GameObject;
                    Image[] image_pics = rtf.GetComponentsInChildren<Image>(true);
                    if (image_pics.Length > 0) Debug.Log("find name = " + assetRelativePath);

                    for (int i = 0; i < image_pics.Length; ++i)
                    {
                        GameObject link_obj = image_pics[i].gameObject;
                        bool raycastTarget = image_pics[i].raycastTarget;
                        Sprite sp_texture = image_pics[i].sprite;
                        //Color color = image_pics[i].color;

                        if (sp_texture != null)
                        {
                            string tex_path = AssetDatabase.GetAssetPath(sp_texture);
                            //Sprite sprite_tex = AssetDatabase.LoadAssetAtPath<Sprite>(tex_path);

                            pos = tex_path.LastIndexOf("/");
                            string target_copy_path = "Assets/AB_RELEASE/mother_package/ab_ui_shared/ui_pics/" + obj_name + "/" + tex_path.Substring(pos + 1);
                            //string new_name = out_path;
                            //new_name = new_name.Replace("Assets/A3/interface/resources/", "");
                            Sprite get_new_one = AssetDatabase.LoadAssetAtPath<Sprite>(target_copy_path);
                            if (get_new_one == null)
                            {
                                Debug.Log("src_path=" + tex_path + "   target_path=" + target_copy_path);
                                AssetDatabase.CopyAsset(tex_path, target_copy_path);
                                get_new_one = AssetDatabase.LoadAssetAtPath<Sprite>(target_copy_path);
                            }

                            image_pics[i].sprite = get_new_one;

                            EditorUtility.DisplayProgressBar("拷贝新的图片", target_copy_path, (float)i / image_pics.Length);
                        }
                    }
                }
                catch
                {
                    Debug.LogError("ClearUIRawImage failed: " + assetRelativePath);
                    return;
                }

                AssetDatabase.ImportAsset(assetRelativePath);
            }

            EditorUtility.DisplayProgressBar("调整UI_提取所有的图片 ", out_path, cur_count / SelectedAsset.Length);
            ++cur_count;
        }

        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();
    }


    [MenuItem("Assets/husunren_UI优化/清理不必要的CanvasRender")]
    static public void RebuildGameUI_remove_dirty_CanvasRender()
    {
        //获取在Project视图中选择的所有游戏对象
        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
        string out_path = null;
        float cur_count = 0f;
        int n_delcanvas_count = 0;
        int n_canvas_count = 0;
        foreach (Object obj in SelectedAsset)
        {
            out_path = AssetDatabase.GetAssetPath(obj);
            if (File.Exists(out_path))
            {
                string assetRelativePath = GetRelativeAssetPath(out_path);
                try
                {
                    GameObject rtf = obj as GameObject;
                    CanvasRenderer[] canvas_render = rtf.GetComponentsInChildren<CanvasRenderer>(true);
                    if (canvas_render.Length > 0) Debug.Log("find name = " + assetRelativePath);

                    for (int i = 0; i < canvas_render.Length; ++i)
                    {
                        //canvas_render[i].gameObject.GetComponents<UnityEngine.Object>().Length
                        GameObject check_obj = canvas_render[i].gameObject;
                        if (check_obj.GetComponent<Text>() == null && check_obj.GetComponent<Image>() == null)
                        {
                            n_delcanvas_count++;
                            Debug.LogError("error=" + check_obj.name);

                            //Destroy(check_obj.GetComponent<CanvasRenderer>());
                            DestroyImmediate(canvas_render[i], true);
                        }

                        n_canvas_count++;
                    }

                    canvas_render = null;
                }
                catch
                {
                    Debug.LogError("ClearUIRawImage failed: " + assetRelativePath);
                    return;
                }

                AssetDatabase.ImportAsset(assetRelativePath);
            }

            EditorUtility.DisplayProgressBar("检测不必要的CanvasRender ", out_path, cur_count / SelectedAsset.Length);
            ++cur_count;
        }

        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();

        string res_out = "canvas rendder数=" + n_canvas_count + "\n";
        res_out += "优化掉的canvas rendder数=" + n_delcanvas_count + "\n";

        EditorUtility.DisplayDialog("清理结束：", res_out, "确定");
    }


    [MenuItem("Assets/husunren_UI优化/清理所有的小数点问题(细节还是要靠自己去调整)")]
    static public void RebuildGameUI_remove_dirty_FloatPos()
    {
        //获取在Project视图中选择的所有游戏对象
        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
        string out_path = null;
        float cur_count = 0f;
        int n_pos_count = 0;
        foreach (Object obj in SelectedAsset)
        {
            out_path = AssetDatabase.GetAssetPath(obj);
            if (File.Exists(out_path))
            {
                string assetRelativePath = GetRelativeAssetPath(out_path);
                try
                {
                    GameObject rtf = obj as GameObject;
                    RectTransform[] ui_rtfs = rtf.GetComponentsInChildren<RectTransform>(true);
                    if (ui_rtfs.Length > 0) Debug.Log("find name = " + assetRelativePath);

                    for (int i = 0; i < ui_rtfs.Length; ++i)
                    {
                        int x = (int)ui_rtfs[i].localPosition.x;
                        int y = (int)ui_rtfs[i].localPosition.y;
                        int z = (int)ui_rtfs[i].localPosition.z;

                        int w = (int)ui_rtfs[i].sizeDelta.x;
                        int h = (int)ui_rtfs[i].sizeDelta.y;

                        ui_rtfs[i].localPosition = new Vector3(x, y, z);
                        ui_rtfs[i].sizeDelta = new Vector2(w, h);
                        n_pos_count++;
                    }

                    ui_rtfs = null;
                }
                catch
                {
                    Debug.LogError("ClearUIRawImage failed: " + assetRelativePath);
                    return;
                }

                AssetDatabase.ImportAsset(assetRelativePath);
            }

            EditorUtility.DisplayProgressBar("清理所有的小数点问题 ", out_path, cur_count / SelectedAsset.Length);
            ++cur_count;
        }

        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();

        string res_out = "处理的节点数数=" + n_pos_count + "\n";

        EditorUtility.DisplayDialog("清理结束：", res_out, "确定");
    }


    [MenuItem("Assets/husunren_UI优化/统计优化UI-清理所有Text的BestFit")]
    static public void RebuildGameUI_remove_dirty_TestBestFit()
    {
        //获取在Project视图中选择的所有游戏对象
        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
        string out_path = null;
        float cur_count = 0f;
        int n_text_count = 0;
        int n_use_best_fit = 0;
        int n_use_rich_text = 0;
        int n_font_size_over18 = 0;
        int n_font_size_low_18 = 0;

        int n_graphic_ray_count = 0;
        int n_disable_count = 0;

        foreach (Object obj in SelectedAsset)
        {
            out_path = AssetDatabase.GetAssetPath(obj);
            if (File.Exists(out_path))
            {
                string assetRelativePath = GetRelativeAssetPath(out_path);
                try
                {
                    GameObject rtf = obj as GameObject;
                    Text[] text_cpt = rtf.GetComponentsInChildren<Text>(true);
                    if (text_cpt.Length > 0) Debug.Log("find name = " + assetRelativePath);

                    for (int i = 0; i < text_cpt.Length; ++i)
                    {
                        Text cur_text = text_cpt[i];
                        if (cur_text.resizeTextForBestFit) n_use_best_fit++;
                        cur_text.resizeTextForBestFit = false;

                        if (cur_text.supportRichText) n_use_rich_text++;
                        cur_text.supportRichText = false;

                        if (cur_text.fontSize > 18) n_font_size_over18++;
                        if (cur_text.fontSize < 18) n_font_size_low_18++;
                        n_text_count++;
                    }

                    text_cpt = null;
                }
                catch
                {
                    Debug.LogError("ClearUIRawImage failed: " + assetRelativePath);
                    return;
                }

                try
                {
                    GameObject rtf = obj as GameObject;
                    Graphic[] graphic_com = rtf.GetComponentsInChildren<Graphic>(true);
                    if (graphic_com.Length > 0) Debug.Log("find name = " + assetRelativePath);

                    for (int i = 0; i < graphic_com.Length; ++i)
                    {
                        Graphic cur_graphic = graphic_com[i];

                        if (cur_graphic.raycastTarget) n_graphic_ray_count++;
                    }

                    graphic_com = null;
                }
                catch
                {
                    Debug.LogError("ClearUIRawImage failed: " + assetRelativePath);
                    return;
                }

                try
                {
                    GameObject rtf = obj as GameObject;
                    Transform[] tfs_obj = rtf.GetComponentsInChildren<Transform>(true);
                    if (tfs_obj.Length > 0) Debug.Log("find name = " + assetRelativePath);

                    for (int i = 0; i < tfs_obj.Length; ++i)
                    {
                        if (tfs_obj[i].gameObject.activeSelf == false) n_disable_count++;
                    }

                    tfs_obj = null;
                }
                catch
                {
                    Debug.LogError("ClearUIRawImage failed: " + assetRelativePath);
                    return;
                }

                AssetDatabase.ImportAsset(assetRelativePath);
            }

            EditorUtility.DisplayProgressBar("清理所有Text的BestFit ", out_path, cur_count / SelectedAsset.Length);
            ++cur_count;
        }

        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();

        string res_out = "总Text数量=" + n_text_count + "\n";
        res_out += "优化掉的BestFit数=" + n_use_best_fit + "\n";
        res_out += "优化掉的RichText数=" + n_use_rich_text + "\n";
        res_out += "大于18号字体的Text数=" + n_font_size_over18 + "\n";
        res_out += "小于18号字体的Text数=" + n_font_size_low_18 + "\n";
        res_out += "---------------------------------------------------\n";
        res_out += "可点击的图形数量=" + n_graphic_ray_count + "\n";
        res_out += "关闭无效的节点数量=" + n_disable_count + "\n";

        EditorUtility.DisplayDialog("清理结束：", res_out, "确定");
    }

    static bool IsActiveNow(Transform tf)
    {
        if (tf.gameObject.activeSelf == false)
        {
            return false;
        }

        if (tf.parent == null)
        {
            return true;
        }
        else
        {
            return IsActiveNow(tf.parent);
        }
    }


    //[MenuItem("Assets/husunren_UI优化/清理激活目标的raycastTarget")]
    //static public void RebuildGameUI_remove_dirty_raycastTarget()
    //{
    //    //获取在Project视图中选择的所有游戏对象
    //    Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
    //    string out_path = null;
    //    float cur_count = 0f;

    //    int n_graphic_ray_count = 0;

    //    foreach (Object obj in SelectedAsset)
    //    {
    //        out_path = AssetDatabase.GetAssetPath(obj);
    //        if (File.Exists(out_path))
    //        {
    //            string assetRelativePath = GetRelativeAssetPath(out_path);

    //            try
    //            {
    //                GameObject rtf = obj as GameObject;
    //                Graphic[] graphic_com = rtf.GetComponentsInChildren<Graphic>(true);
    //                if (graphic_com.Length > 0) Debug.Log("find name = " + assetRelativePath);

    //                for (int i = 0; i < graphic_com.Length; ++i)
    //                {
    //                    Graphic cur_graphic = graphic_com[i];

    //                    if (IsActiveNow(cur_graphic.transform) && cur_graphic.raycastTarget)
    //                    {
    //                        cur_graphic.raycastTarget = false;
    //                        n_graphic_ray_count++;
    //                    }
    //                }

    //                graphic_com = null;
    //            }
    //            catch
    //            {
    //                Debug.LogError("ClearUIRawImage failed: " + assetRelativePath);
    //                return;
    //            }

    //            AssetDatabase.ImportAsset(assetRelativePath);
    //        }

    //        EditorUtility.DisplayProgressBar("清理激活目标的raycastTarget ", out_path, cur_count / SelectedAsset.Length);
    //        ++cur_count;
    //    }

    //    AssetDatabase.Refresh();
    //    EditorUtility.ClearProgressBar();

    //    string res_out = "可点击的图形数量=" + n_graphic_ray_count + "\n";

    //    EditorUtility.DisplayDialog("清理结束：", res_out, "确定");
    //}


    [MenuItem("Assets/husunren_UI优化/所有的raycastTarget")]
    static public void RebuildGameUI_removeall_dirty_raycastTarget()
    {
        //获取在Project视图中选择的所有游戏对象
        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
        string out_path = null;
        float cur_count = 0f;

        int n_graphic_ray_count = 0;

        foreach (Object obj in SelectedAsset)
        {
            out_path = AssetDatabase.GetAssetPath(obj);
            if (File.Exists(out_path))
            {
                string assetRelativePath = GetRelativeAssetPath(out_path);

                try
                {
                    GameObject rtf = obj as GameObject;
                    Graphic[] graphic_com = rtf.GetComponentsInChildren<Graphic>(true);
                    if (graphic_com.Length > 0) Debug.Log("find name = " + assetRelativePath);

                    for (int i = 0; i < graphic_com.Length; ++i)
                    {
                        Graphic cur_graphic = graphic_com[i];

                        cur_graphic.raycastTarget = false;
                        n_graphic_ray_count++;
                    }

                    graphic_com = null;
                }
                catch
                {
                    Debug.LogError("ClearUIRawImage failed: " + assetRelativePath);
                    return;
                }

                AssetDatabase.ImportAsset(assetRelativePath);
            }

            EditorUtility.DisplayProgressBar("所有的raycastTarget ", out_path, cur_count / SelectedAsset.Length);
            ++cur_count;
        }

        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();

        string res_out = "可点击的图形数量=" + n_graphic_ray_count + "\n";

        EditorUtility.DisplayDialog("清理结束：", res_out, "确定");
    }


    //    static void RebuildTex(Texture2D sourcetex, string assetRelativePath)
    //    {
    //        //分析像素
    //        int srcpic_w = sourcetex.width;
    //        int srcpic_h = sourcetex.height;

    //        int bottom = -128;
    //        for (int y = 0; y < sourcetex.height; ++y)
    //        {
    //            for (int x = 0; x < sourcetex.width; ++x)
    //            {
    //                if (sourcetex.GetPixel(x, y).a > 0f)
    //                {
    //                    bottom = y;
    //                }
    //            }

    //            if (bottom >= 0) break;
    //        }

    //        int top = -128;
    //        for (int y = sourcetex.height - 1; y >= 0; --y)
    //        {
    //            for (int x = 0; x < sourcetex.width; ++x)
    //            {
    //                if (sourcetex.GetPixel(x, y).a > 0f)
    //                {
    //                    top = y;
    //                }
    //            }

    //            if (top >= 0) break;
    //        }

    //        int left = -128;
    //        for (int x = 0; x < sourcetex.width; ++x)
    //        {
    //            for (int y = 0; y < sourcetex.height; ++y)
    //            {
    //                if (sourcetex.GetPixel(x, y).a > 0f)
    //                {
    //                    left = x;
    //                }
    //            }

    //            if (left >= 0) break;
    //        }

    //        int right = -128;
    //        for (int x = sourcetex.width - 1; x >= 0; --x)
    //        {
    //            for (int y = 0; y < sourcetex.height; ++y)
    //            {
    //                if (sourcetex.GetPixel(x, y).a > 0f)
    //                {
    //                    right = x;
    //                }
    //            }

    //            if (right >= 0) break;
    //        }

    //        int can_pot = 4;

    //        //Debug.Log("top= " + top + "   bottom=" + bottom);
    //        int out_h = top - bottom + 1;
    //        int out_w = right - left + 1;
    //        int h_pot8 = out_h % can_pot;
    //        Debug.Log("w= " + out_w + "   h=" + out_h);
    //        if (h_pot8 > 0)
    //        {
    //            out_h = out_h + can_pot - h_pot8;
    //            bottom = bottom - (can_pot - h_pot8);
    //        }

    //        int w_pot8 = out_w % can_pot;
    //        if (w_pot8 > 0)
    //        {
    //            out_w = out_w + can_pot - w_pot8;
    //            left = left - (can_pot - h_pot8);
    //        }
    //        Debug.Log("w= " + out_w + "   h=" + out_h);
    //        Debug.Log("left= " + left + "   bottom=" + bottom);


    //        Texture2D rgbTex = new Texture2D(out_w, out_h, TextureFormat.ARGB32, false);
    //        //rgbTex.SetPixels(sourcetex.GetPixels());

    //        for (int y = 0; y < out_h; ++y)
    //        {
    //            for (int x = 0; x < out_w; ++x)
    //            {
    //                rgbTex.SetPixel(x, y, sourcetex.GetPixel(x + left, y + bottom));
    //            }
    //        }
    //        rgbTex.Apply();

    //        //Texture2D rgbTex11 = Texture2D.CreateExternalTexture(512, 512, TextureFormat.ARGB32, false, true, rgbTex.GetNativeTexturePtr());

    //        byte[] bytes = rgbTex.EncodeToPNG();
    //        File.WriteAllBytes(assetRelativePath, bytes);
    //        //ReImportAsset(assetRelativePath, sourcetex.width, sourcetex.height);


    //        TextureImporter textureImporter = AssetImporter.GetAtPath(assetRelativePath) as TextureImporter;

    //        float pivot_h = (float)(srcpic_h / 2 - bottom) / out_h;
    //        float pivot_w = (float)(srcpic_w / 2 - left) / out_w;

    //        textureImporter.spritePivot = new Vector2(pivot_w, pivot_h);
    //        textureImporter.isReadable = false;  //increase memory cost if readable is true  
    //        textureImporter.textureFormat = TextureImporterFormat.AutomaticCompressed;
    //        //importer.maxTextureSize = 128;
    //        //importer.anisoLevel = 0;
    //        AssetDatabase.ImportAsset(assetRelativePath, ImportAssetOptions.ForceUpdate);
    //    }

    //    static void SetTextureReadableEx(string _relativeAssetPath)    //set readable flag and set textureFormat TrueColor  
    //    {
    //        TextureImporter ti = null;
    //        try
    //        {
    //            ti = (TextureImporter)TextureImporter.GetAtPath(_relativeAssetPath);
    //        }
    //        catch
    //        {
    //            Debug.LogError("Load Texture failed: " + _relativeAssetPath);
    //            return;
    //        }
    //        if (ti == null)
    //        {
    //            return;
    //        }
    //        ti.isReadable = true;
    //        ti.textureFormat = TextureImporterFormat.AutomaticTruecolor;      //this is essential for departing Textures for ETC1. No compression format for following operation.  
    //        AssetDatabase.ImportAsset(_relativeAssetPath);
    //    }

    static string GetRightFormatPath(string _path)
    {
        return _path.Replace("\\", "/");
    }

    static string GetRelativeAssetPath(string _fullPath)
    {
        _fullPath = GetRightFormatPath(_fullPath);
        int idx = _fullPath.IndexOf("Assets");
        string assetRelativePath = _fullPath.Substring(idx);
        return assetRelativePath;
    }



    //    [MenuItem("Assets/图片整理/重新规范特效图片")]
    //    static public void RebuildFX_Picture()
    //    {
    //        //获取在Project视图中选择的所有游戏对象
    //        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
    //        string out_path = null;
    //        float cur_count = 0f;
    //        foreach (Object obj in SelectedAsset)
    //        {
    //            out_path = AssetDatabase.GetAssetPath(obj);
    //            if (File.Exists(out_path))
    //            {
    //                string assetRelativePath = GetRelativeAssetPath(out_path);

    //                TextureImporter ti = null;
    //                try
    //                {
    //                    ti = (TextureImporter)TextureImporter.GetAtPath(assetRelativePath);
    //                }
    //                catch
    //                {
    //                    Debug.LogError("Load Texture failed: " + assetRelativePath);
    //                    return;
    //                }
    //                if (ti == null)
    //                {
    //                    return;
    //                }

    //                //ti.isReadable = true;
    //                ti.mipmapEnabled = false;
    //                ti.maxTextureSize = 128;
    //                ti.textureFormat = TextureImporterFormat.AutomaticCompressed;
    //                AssetDatabase.ImportAsset(assetRelativePath);

    //                EditorUtility.DisplayProgressBar("重新规范图片", assetRelativePath, cur_count / SelectedAsset.Length);
    //            }

    //            ++cur_count;
    //        }

    //        AssetDatabase.Refresh();
    //        EditorUtility.ClearProgressBar();
    //    }


    //    [MenuItem("Assets/图片整理/重新规范场景贴图")]
    //    static public void RebuildScene_Picture()
    //    {
    //        //获取在Project视图中选择的所有游戏对象
    //        Object[] SelectedAsset = Selection.GetFiltered(typeof(Texture), SelectionMode.DeepAssets);
    //        string out_path = null;
    //        float cur_count = 0f;
    //        foreach (Object obj in SelectedAsset)
    //        {
    //            out_path = AssetDatabase.GetAssetPath(obj);
    //            //Debug.Log("out_path"+ out_path);
    //            if (File.Exists(out_path))
    //            {
    //                string assetRelativePath = GetRelativeAssetPath(out_path);

    //                TextureImporter ti = null;
    //                try
    //                {
    //                    ti = (TextureImporter)TextureImporter.GetAtPath(assetRelativePath);
    //                }
    //                catch
    //                {
    //                    Debug.LogError("Load Texture failed: " + assetRelativePath);
    //                    return;
    //                }

    //                if (ti == null)
    //                {
    //                    return;
    //                }

    //                ti.isReadable = false;
    //                ti.mipmapEnabled = false;
    //                ti.textureFormat = TextureImporterFormat.AutomaticCompressed;
    //                AssetDatabase.ImportAsset(assetRelativePath);

    //                EditorUtility.DisplayProgressBar("重新规范图片", assetRelativePath, cur_count / SelectedAsset.Length);
    //            }

    //            ++cur_count;
    //        }

    //        AssetDatabase.Refresh();
    //        EditorUtility.ClearProgressBar();
    //    }


    //    [MenuItem("Assets/图片整理/重新规范所有模型数据")]
    //    static public void RebuildGame_Model()
    //    {
    //        //获取在Project视图中选择的所有游戏对象
    //        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
    //        string out_path = null;
    //        float cur_count = 0f;
    //        foreach (Object obj in SelectedAsset)
    //        {
    //            out_path = AssetDatabase.GetAssetPath(obj);
    //            //Debug.Log("out_path"+ out_path);
    //            if (File.Exists(out_path))
    //            {
    //                string assetRelativePath = GetRelativeAssetPath(out_path);

    //                ModelImporter mi = null;
    //                try
    //                {
    //                    mi = (ModelImporter)ModelImporter.GetAtPath(assetRelativePath);
    //                }
    //                catch
    //                {
    //                    //Debug.LogError("Load Model failed: " + assetRelativePath);
    //                    continue;
    //                }

    //                if (mi == null)
    //                {
    //                    continue;
    //                }

    //                bool b_idle_anim = false;
    //                if (out_path.IndexOf("_idle") >= 0)
    //                {
    //                    b_idle_anim = true;
    //                }


    //                mi.isReadable = false;
    //                mi.importTangents = ModelImporterTangents.None;
    //                //mi.importNormals = ModelImporterNormals.None;
    //                mi.meshCompression = ModelImporterMeshCompression.Medium;
    //                //mi.mipmapEnabled = false;
    //                //mi.textureFormat = TextureImporterFormat.AutomaticCompressed;

    //                if (b_idle_anim)
    //                {
    //                    mi.animationCompression = ModelImporterAnimationCompression.Optimal;
    //                    mi.animationRotationError = 0f;
    //                    mi.animationPositionError = 4f;
    //                    mi.animationScaleError = 4f;
    //                    mi.resampleRotations = false;
    //                }
    //                else
    //                {
    //                    mi.animationCompression = ModelImporterAnimationCompression.Optimal;
    //                    mi.animationRotationError = 8f;
    //                    mi.animationPositionError = 8f;
    //                    mi.animationScaleError = 4f;
    //                    mi.resampleRotations = false;
    //                }



    //                AssetDatabase.ImportAsset(assetRelativePath);

    //                EditorUtility.DisplayProgressBar("重新模型", assetRelativePath, cur_count / SelectedAsset.Length);
    //            }

    //            ++cur_count;
    //        }

    //        AssetDatabase.Refresh();
    //        EditorUtility.ClearProgressBar();
    //    }

    //    static bool IsETC_Tex(TextureFormat format)
    //    {
    //        if (format == TextureFormat.ETC2_RGB ||
    //            format == TextureFormat.ETC2_RGBA1 ||
    //            format == TextureFormat.ETC2_RGBA8 ||
    //            format == TextureFormat.ETC_RGB4 ||
    //            format == TextureFormat.ETC_RGB4_3DS ||
    //            format == TextureFormat.ETC_RGBA8_3DS)
    //        {
    //            return true;
    //        }

    //        return false;
    //    }

    //    [MenuItem("Assets/图片整理/搜索非压缩图片")]
    //    static public void FindGame_ARGB_PIC()
    //    {
    //        string[] atlas_names = UnityEditor.Sprites.Packer.atlasNames;
    //        for (int i = 0; i < atlas_names.Length; ++i)
    //        {
    //            Texture2D[] altas_tex = UnityEditor.Sprites.Packer.GetTexturesForAtlas(atlas_names[i]);
    //            for (int j = 0; j < altas_tex.Length; ++j)
    //            {
    //                if (IsETC_Tex(altas_tex[j].format) == false)
    //                {
    //                    Debug.Log("图集的格式有问题：" + atlas_names[i] + "  format:" + altas_tex[j].format);
    //                    break;
    //                }
    //            }
    //        }

    //        Debug.Log("=================开始搜索===============================");
    //        //获取在Project视图中选择的所有游戏对象
    //        Object[] SelectedAsset = Selection.GetFiltered(typeof(Texture), SelectionMode.DeepAssets);
    //        string out_path = null;
    //        float cur_count = 0f;
    //        foreach (Object obj in SelectedAsset)
    //        {
    //            out_path = AssetDatabase.GetAssetPath(obj);
    //            //Debug.Log("out_path"+ out_path);
    //            if (File.Exists(out_path))
    //            {
    //                string assetRelativePath = GetRelativeAssetPath(out_path);

    //                TextureImporter ti = null;
    //                try
    //                {
    //                    ti = (TextureImporter)TextureImporter.GetAtPath(assetRelativePath);
    //                }
    //                catch
    //                {
    //                    Debug.LogError("Load Texture failed: " + assetRelativePath);
    //                    continue;
    //                }

    //                if (ti == null)
    //                {
    //                    continue;
    //                }

    //                if (ti.textureFormat != TextureImporterFormat.AutomaticCompressed)
    //                {
    //                    Debug.Log(out_path);
    //                }
    //                else if (ti.isReadable == true || ti.mipmapEnabled == true)
    //                {
    //                    Debug.Log(out_path);
    //                }
    //                else if (ti.spritePackingTag == "")
    //                {
    //                    Texture2D sourcetex = AssetDatabase.LoadAssetAtPath(assetRelativePath, typeof(Texture2D)) as Texture2D;  //not just the textures under Resources file
    //                    if (!sourcetex)
    //                    {
    //                        Debug.LogError("Load Texture Failed : " + assetRelativePath);
    //                        continue;
    //                    }

    //                    if (IsETC_Tex(sourcetex.format) == false)
    //                    {
    //                        Debug.Log(out_path);
    //                    }
    //                }


    //                ////Type t = tc.GetType();//获得该类的Type
    //                ////再用Type.GetProperties获得PropertyInfo[],然后就可以用foreach 遍历了
    //                //System.Type t = ti.GetType();
    //                //foreach( System.Reflection.PropertyInfo pi in t.GetProperties() )
    //                //{
    //                //    object value1 = pi.GetValue(ti, null);//用pi.GetValue获得值

    //                //    Debug.Log(pi.Name + " = " + value1);
    //                //}


    //                //Debug.Log("assetRelativePath = " + assetRelativePath);

    //                //ti.isReadable = false;
    //                //ti.mipmapEnabled = false;
    //                //ti.textureFormat = TextureImporterFormat.AutomaticCompressed;
    //                //AssetDatabase.ImportAsset(assetRelativePath);

    //                EditorUtility.DisplayProgressBar("非压缩图片", assetRelativePath, cur_count / SelectedAsset.Length);
    //            }

    //            ++cur_count;
    //        }

    //        EditorUtility.ClearProgressBar();
    //        Debug.Log("=================结束搜索===============================");
    //    }




    //    [MenuItem("Assets/AB整理/重新规范名字")]
    //    static public void RebuildAB_Name()
    //    {
    //        //获取在Project视图中选择的所有游戏对象
    //        Object[] SelectedAsset = Selection.GetFiltered(typeof(Texture), SelectionMode.DeepAssets);
    //        string out_path = null;
    //        float cur_count = 0f;
    //        foreach (Object obj in SelectedAsset)
    //        {
    //            out_path = AssetDatabase.GetAssetPath(obj);
    //            //Debug.Log("out_path"+ out_path);
    //            if (File.Exists(out_path))
    //            {
    //                if (out_path.EndsWith(".prefab") || true)
    //                {
    //                    //int pos = out_path.LastIndexOf("/");
    //                    //string new_name = "monster_" + out_path.Substring(pos + 1);
    //                    string new_name = out_path;
    //                    new_name = new_name.Replace("Assets/A3/interface/resources/", "");

    //                    int pos = new_name.LastIndexOf(".");
    //                    new_name = new_name.Substring(0, pos);

    //                    new_name = new_name.Replace("/", "_");



    //                    //Debug.Log(out_path + "  to  " + new_name);
    //                    AssetDatabase.RenameAsset(out_path, new_name);
    //                }
    //            }

    //            EditorUtility.DisplayProgressBar("重新规范名字", out_path, cur_count / SelectedAsset.Length);
    //            ++cur_count;
    //        }

    //        AssetDatabase.Refresh();
    //        EditorUtility.ClearProgressBar();
    //    }

    //    [MenuItem("Assets/Scene_AB名字加入")]
    //    static public void Add_AssetBundle_Name()
    //    {
    //        //获取在Project视图中选择的所有游戏对象
    //        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
    //        string out_path = null;
    //        float cur_count = 0f;
    //        foreach (Object obj in SelectedAsset)
    //        {
    //            out_path = AssetDatabase.GetAssetPath(obj);
    //            //Debug.Log("out_path"+ out_path);
    //            if (File.Exists(out_path))
    //            {
    //                string assetRelativePath = GetRelativeAssetPath(out_path);

    //                AssetImporter ai = null;
    //                try
    //                {
    //                    ai = (AssetImporter)AssetImporter.GetAtPath(assetRelativePath);
    //                }
    //                catch
    //                {
    //                    Debug.LogError("Load AssetImporter failed: " + assetRelativePath);
    //                    return;
    //                }

    //                if (ai == null)
    //                {
    //                    return;
    //                }

    //                ////Type t = tc.GetType();//获得该类的Type
    //                ////再用Type.GetProperties获得PropertyInfo[],然后就可以用foreach 遍历了
    //                //System.Type t = obj.GetType();
    //                //foreach (System.Reflection.PropertyInfo pi in t.GetProperties())
    //                //{
    //                //    object value1 = pi.GetValue(obj, null);//用pi.GetValue获得值
    //                //    Debug.Log(pi.Name + " = " + value1);
    //                //}

    //                ai.assetBundleName = obj.name + ".assetbundle";
    //                AssetDatabase.ImportAsset(assetRelativePath);
    //            }

    //            EditorUtility.DisplayProgressBar("AB名字加入", out_path, cur_count / SelectedAsset.Length);
    //            ++cur_count;
    //        }

    //        AssetDatabase.Refresh();
    //        EditorUtility.ClearProgressBar();
    //    }

    //    [MenuItem("Assets/清理UIRawImage")]
    //    static public void ClearUIRawImage()
    //    {
    //        //获取在Project视图中选择的所有游戏对象
    //        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
    //        string out_path = null;
    //        float cur_count = 0f;
    //        foreach (Object obj in SelectedAsset)
    //        {
    //            out_path = AssetDatabase.GetAssetPath(obj);
    //            if (File.Exists(out_path))
    //            {
    //                string assetRelativePath = GetRelativeAssetPath(out_path);
    //                try
    //                {
    //                    GameObject rtf = obj as GameObject;
    //                    RawImage[] ri_pics = rtf.GetComponentsInChildren<RawImage>(true);

    //                    if (ri_pics.Length > 0) Debug.Log("clear name = " + assetRelativePath);


    //                    for (int i = 0; i < ri_pics.Length; ++i)
    //                    {
    //                        GameObject link_obj = ri_pics[i].gameObject;
    //                        bool raycastTarget = ri_pics[i].raycastTarget;
    //                        Texture texture = ri_pics[i].texture;
    //                        Color color = ri_pics[i].color;

    //                        string tex_path = AssetDatabase.GetAssetPath(texture);
    //                        Sprite sprite_tex = AssetDatabase.LoadAssetAtPath<Sprite>(tex_path);

    //                        //Debug.Log("sprite = " + AssetDatabase.GetAssetPath(texture));
    //                        DestroyImmediate(ri_pics[i], true);

    //                        Image image = link_obj.AddComponent<Image>();
    //                        image.raycastTarget = raycastTarget;
    //                        image.sprite = sprite_tex;
    //                        image.color = color;

    //                        Debug.Log("raw image name = " + image.name);
    //                    }
    //                }
    //                catch
    //                {
    //                    Debug.LogError("ClearUIRawImage failed: " + assetRelativePath);
    //                    return;
    //                }
    //                AssetDatabase.ImportAsset(assetRelativePath);
    //            }

    //            EditorUtility.DisplayProgressBar("清理UIRawImage ", out_path, cur_count / SelectedAsset.Length);
    //            ++cur_count;
    //        }

    //        AssetDatabase.Refresh();
    //        EditorUtility.ClearProgressBar();
    //    }


    //    [MenuItem("Assets/获取asset的名字")]
    //    static public void Get_AssetNames()
    //    {
    //        //获取在Project视图中选择的所有游戏对象
    //        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
    //        //{ "dfasf", "fdsfsf", "fdsfaf" };
    //        string all_names = "{ ";
    //        string out_path = null;
    //        float cur_count = 0f;
    //        foreach (Object obj in SelectedAsset)
    //        {
    //            out_path = AssetDatabase.GetAssetPath(obj);
    //            //Debug.Log("out_path"+ out_path);
    //            if (File.Exists(out_path))
    //            {
    //                all_names += "\"" + obj.name + "\", ";
    //            }

    //            EditorUtility.DisplayProgressBar("获取asset的名字", out_path, cur_count / SelectedAsset.Length);
    //            ++cur_count;
    //        }
    //        Debug.LogError(all_names);

    //        EditorUtility.ClearProgressBar();
    //    }
}
