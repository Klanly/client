//using UnityEngine;
//using System.Collections;
//using Cross;
//using GameFramework;
//using MuGame;
//using System.Collections.Generic;
//public class drawline : MonoBehaviour
//{
//    int m_value = 1;
//    int index = 0;
//    float m_uint = 0.6f;
//    float m_pixel = 3; // hdt pixel transto grid

//    void Start()
//    {
//        //int w = (int)(localGrd.grayWidth / m_pixel );
//        //int h = (int)(localGrd.grayHeight / m_pixel );

//        //debug.Log( "grd.w ["+ w +"]  grd.h ["+h+"]" );

//        //for (float i = 0; i < w; i++)
//        //{
//        //    for (float j = 0; j < h; j++)
//        //    {
//        //        setLine( i * m_uint, j * m_uint, m_uint, m_uint, UIUtility.singleton.IsWalkAble( (int)i, (int)j ), index );
//        //        index++;
//        //    }

//        //    break;
//        //}
//    }



//    public ClientGrdConfig localGrd
//    {
//        get { return UILinksManager.singleton._uiClient.g_gameConfM.getObject(OBJECT_NAME.CONF_LOCAL_GRD) as ClientGrdConfig; }
//    }



//    void Update()
//    {

//    }

//    int xmin = 10;
//    int xmax = 10;
//    int ymin = 10;
//    int ymax = 10;
//    public void setLine(float x, float y, float w, float h,bool walkable, int index )
//    {

//        if( x <  this.gameObject.transform.position.x -xmin ||
//             x >  this.gameObject.transform.position.x +xmax ||
//              y <  this.gameObject.transform.position.z -ymin ||
//               y >  this.gameObject.transform.position.z +ymax
//        )
//        //return;

//        debug.Log(" >>>>>>>>>>> index[" + index + "]  x[" + x / m_uint + "] y[" + y / m_uint + "] val[" + walkable.ToString() + "] ");

//        List<Vec3> point = new List<Vec3>();
//        Vec3 left_top = new Vec3(x, this.gameObject.transform.position.y, y);
//        Vec3 right_top = new Vec3(x + w, this.gameObject.transform.position.y, y);
//        Vec3 left_down = new Vec3(x, this.gameObject.transform.position.y, y + h);
//        Vec3 right_down = new Vec3(x + w, this.gameObject.transform.position.y, y + h);
//        point.Add(left_top);
//        point.Add(right_top);
//        point.Add(right_down);
//        point.Add(left_down);
//        point.Add(left_top);
//        GraphScene2DImpl dr = new GraphScene2DImpl();

//        LineObject lineobj = dr.createLineObject() as LineObject;

//        if (!walkable)
//        {
//            lineobj.startColor = new Vec3(255, 0, 0);
//            lineobj.endColor = new Vec3(255, 0, 0);
//        }
//        else
//        {
//            lineobj.startColor = new Vec3(255, 255, 255);
//            lineobj.endColor = new Vec3(255, 255, 255);
//        }
//        lineobj.startWidth = 0.1f;
//        lineobj.endWidth = 0.1f;
//        lineobj.init(point); 
//    }
//}
