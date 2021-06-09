using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILineRenderer : Graphic
{
    public Vector2Int gridSize;
    public float thickness;

    public List<Vector2> points;

    float width;
    float height;
    float unitWidth;
    float unitHeight;

    /// <summary>
    /// Overwrites the graphics function with which you can probably draw 
    /// </summary>
    /// <param name="vh"></param>
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        //Deletes the content of the VertexHelper, which later saves all points of the line of the graph and then connects them with polygons to form a line 
        vh.Clear();

        width = rectTransform.rect.width;
        height = rectTransform.rect.height;

        unitWidth = width / gridSize.x;
        unitHeight = height / gridSize.y;

        //The first point on the line is ignored 
        if (points.Count < 2)
        {
            return;
        }

        float angle = 0;        //The angle variable is created 

        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector2 point = points[i];
            Vector2 point2 = points[i + 1];

            if (i < points.Count - 1)
            {
                //The angle is determined based on the point and the future point of the line 
                angle = GetAngle(points[i], points[i + 1]) + 90f;
            }

            //The function is given the current point, the VertexHelper and the angle to generate the points with which the line will be drawn later
            DrawVerticesForPoint(point, point2, angle, vh);
        }

        //The points are worked through one after the other and then the saved corner points in the VertexHelper are used to draw the line with polygons 
        for (int i = 0; i < points.Count - 1; i++)
        {
            int index = i * 4;

            vh.AddTriangle(index + 0, index + 1, index + 2);
            vh.AddTriangle(index + 1, index + 2, index + 3);
        }
    }

    public float GetAngle(Vector2 me, Vector2 target)
    {
        //panel resolution go there in place of 9 and 16
        return (float)(Mathf.Atan2(9f * (target.y - me.y), 16f * (target.x - me.x)) * (180 / Mathf.PI));
    }

    void DrawVerticesForPoint(Vector2 point, Vector2 point2, float angle, VertexHelper vh)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        vertex.position = Quaternion.Euler(0, 0, angle) * new Vector3(-thickness / 2, 0);   //First, one of the corner points is set at the beginning of the line by including the thickness of the line and the angle 
        vertex.position += new Vector3(unitWidth * point.x, unitHeight * point.y);          //Then this corner point is led to the actually correct point on the line 
        vh.AddVert(vertex);     //I believe that the corner point is transmitted to the VertexHelper so that all corner points can finally be connected to the line. 

        vertex.position = Quaternion.Euler(0, 0, angle) * new Vector3(thickness / 2, 0);    //The next corner point is now also generated at the beginning and the thickness and the angle are also calculated, 
        vertex.position += new Vector3(unitWidth * point.x, unitHeight * point.y);          //but because of the missing - the corner point is then generated on the other side of the line's thickness. 
        vh.AddVert(vertex);     //So two corner points are created for each point in the line, which are then later connected to each other. 

        vertex.position = Quaternion.Euler(0, 0, angle) * new Vector3(-thickness / 2, 0);
        vertex.position += new Vector3(unitWidth * point2.x, unitHeight * point2.y);
        vh.AddVert(vertex);

        vertex.position = Quaternion.Euler(0, 0, angle) * new Vector3(thickness / 2, 0);
        vertex.position += new Vector3(unitWidth * point2.x, unitHeight * point2.y);
        vh.AddVert(vertex);
    }
}
