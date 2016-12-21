using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Delaunay;
using Delaunay.Geo;


namespace MonsterAdventure
{   
    /*
     * \brief   Manage the voronoi contruction and the
     *          graphs generations.
     */
     [Serializable]
    public class Voronoi
    {
        public List<LineSegment> graph;
        public List<Vector2> pointsList;

        private Delaunay.Voronoi m_DelaunayVoronoy;
        private Rect m_Bounds;

        public List<List<Vector2>> regions
        {
            get { return m_DelaunayVoronoy.Regions(); }
        }

        /*!
         * \brief   Create a Voronoi Diagram and generate all graphs
         * \param   a_PointsList the points list which will be used to
         *          as vertices to construct graphs
         * \param   a_Bounds the bounds of the graph
         * \param   a_GenerateGraphs if graphes (voronoi graph, voronoi diagram, 
         *          MinST, MaxST, triangulation) has to be generated.
         * \param   a_AllGraphs if we generate MaxST and triangulation
         */
        public Voronoi(List<Vector2> a_PointsList, Rect a_Bounds)
        {
            m_Bounds = a_Bounds;

            Generate(a_PointsList);
        }

        public Voronoi(Voronoi a_VoronoiToCopy)
        {
            m_Bounds = new Rect(a_VoronoiToCopy.m_Bounds);
            pointsList = new List<Vector2>(a_VoronoiToCopy.pointsList);

            Generate(pointsList);
        }

        /*!
         * \brief   Construct a new Voronoi Diagram and generate all graphs
         * \param   a_PointsList the points list which will be used to
         *          as vertices to construct graphs
         * \param   a_GenerateGraphs if graphes (voronoi graph, voronoi diagram, 
         *          MinST, MaxST, triangulation) has to be generated.
         * \param   a_AllGraphs if we generate MaxST and triangulation
         */
        public void Generate(List<Vector2> a_PointsList)
        {
            pointsList = a_PointsList;

            m_DelaunayVoronoy = constructVoronoy(a_PointsList);

            graph = m_DelaunayVoronoy.VoronoiDiagram();
        }

        /*!
         * \brief   Construct a new Delaunay Voronoy Diagram
         * \param   a_PointsList the points list which will be used to
         *          as vertices to construct graphs
         * \return  the Delaunay Voronoi Diagram constructed
         */
        private Delaunay.Voronoi constructVoronoy(List<Vector2> a_PointsList)
        {
            return new Delaunay.Voronoi(a_PointsList, null, m_Bounds);
        }
    }
}