using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Delaunay;
using Delaunay.Geo;

namespace MonsterAdventure
{
    /*
     * \brief   The class in chage of the voronoi contructions
     *          and graphs construction for all the map.
     */
    public class VoronoiGenerator : MonoBehaviour
    {
        [Range(1, 1000)]
        public int numberOfCellulas;
        [Range(0, 5)]
        public uint numberOfIterations;

        public Voronoi voronoi;

        // Parameters which don't change
        private Rect _bounds;

        private List<LineSegment> _voronoiCellula;
        private RandomGenerator _random;

        void Awake()
        {
            // nothing
        }

        public void Construct(Rect bounds, RandomGenerator random)
        {
            _voronoiCellula = new List<LineSegment>();
            _random = random;

            _bounds = bounds;

            Generate();
        }

        /*!
        * \brief   TODO
        */
        void Start()
        {

        }       

        private void Generate()
        {
            List<Vector2> points = GeneratePoints();

            voronoi = new Voronoi(points, _bounds);

            DoSmoothIterations(numberOfIterations);
        }

        private List<Vector2> GeneratePoints()
        {
            List<Vector2> points = new List<Vector2>();

            for (int i = 0; i < numberOfCellulas; i++)
            {
                int x = _random.Next((int)_bounds.left, (int)_bounds.left + (int)_bounds.width);
                int y = _random.Next((int)_bounds.top, (int)_bounds.top + (int)_bounds.height);

                points.Add(new Vector2(x, y));
            }

            return points;
        }     

        /*!
         * \brief   Llyod Relaxation Algorithm applications to smooth
         *          the cellula graph. It replace all the points to the
         *          center (=centroid) of each polygon (=cellula).
         * \param   a_RegionsList a list of all Regions (=cellula) of the Voronoi Diagram
         * \param   a_Bounds Bounds where the algorithm will be apply
         * \return  The new points list centered
         */
        private List<Vector2> LloydRelaxation(List<List<Vector2>> a_RegionsList)
        {
            List<Vector2> newPointsList = new List<Vector2>();

            foreach (List<Vector2> t_Region in a_RegionsList)
            {
                // to provide error "index out of bounds"
                if (t_Region.Count < 1)
                {
                    continue;
                }

                Vector2 t_Centroid = calculateCentroid(t_Region);

                newPointsList.Add(t_Centroid);
            }

            return newPointsList;
        }

        /*!
         * \brief   Calculate the Centroid of a Polygon.
         * \param   a_Polygon a polygon defined by points.
         *          Points have to be numbered in order of their occurrence
         *          along the polygon's perimeter.
         *          It has to be a non-self-intersecting closed polygon.
         * \return  the centroid
         */
        private Vector2 calculateCentroid(List<Vector2> a_Polygon)
        {
            Vector2 t_Centroid = Vector2.zero;
            float t_Area = 0;
            float t_CoordStartX = 0;
            float t_CoordStartY = 0;
            float t_CoordEndX = 0;
            float t_CoordEndY = 0;
            float t_Determinant = 0;

            // calculate t_Centroid position until the (last - 1) vertex 
            for (int t_VertexIndex = 0; t_VertexIndex < a_Polygon.Count - 1; t_VertexIndex++)
            {
                t_CoordStartX = a_Polygon[t_VertexIndex].x;
                t_CoordStartY = a_Polygon[t_VertexIndex].y;
                t_CoordEndX = a_Polygon[t_VertexIndex + 1].x;
                t_CoordEndY = a_Polygon[t_VertexIndex + 1].y;

                t_Determinant = t_CoordStartX * t_CoordEndY - t_CoordEndX * t_CoordStartY;

                t_Area += t_Determinant;
                t_Centroid.x += (t_CoordStartX + t_CoordEndX) * t_Determinant;
                t_Centroid.y += (t_CoordStartY + t_CoordEndY) * t_Determinant;
            }

            // calculate t_Centroid position for the last vertex
            t_CoordStartX = a_Polygon[a_Polygon.Count - 1].x;
            t_CoordStartY = a_Polygon[a_Polygon.Count - 1].y;
            t_CoordEndX = a_Polygon[0].x;
            t_CoordEndY = a_Polygon[0].y;

            t_Determinant = t_CoordStartX * t_CoordEndY - t_CoordEndX * t_CoordStartY;

            t_Area += t_Determinant;
            t_Centroid.x += (t_CoordStartX + t_CoordEndX) * t_Determinant;
            t_Centroid.y += (t_CoordStartY + t_CoordEndY) * t_Determinant;

            // get the final centroid position
            t_Area *= 0.5f;
            t_Centroid.x /= (6 * t_Area);
            t_Centroid.y /= (6 * t_Area);

            return t_Centroid;
        }

        /*!
         * \brief   Llyod Relaxation Algorithm applications to smooth
         *          the cellula graph. For each iteration the Voronoi
         *          Diagram has to be regenerate.
         * \param   a_NumberIterations Number of application of the 
         *          Llyod Relaxation Algorithm
        */
        private void DoSmoothIterations(uint a_NumberIterations)
        {
            for (uint i = 0; i < a_NumberIterations; i++)
            {
                voronoi.pointsList = LloydRelaxation(voronoi.regions);
                voronoi.Generate(voronoi.pointsList);
            }
        }       
    }
}